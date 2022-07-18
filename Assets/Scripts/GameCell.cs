using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public enum GameCellType
{
    Simple,InteractableOff,InteractableOn
}
public class GameCell : MonoBehaviour
{
    public GameCellType type;
    
    [SerializeField] private GameObject InteractableOff;
    [SerializeField] private GameObject InteractableOn;
    [SerializeField] private GameObject border;
     [SerializeField] private TextMeshProUGUI cordianteText;
    [SerializeField] private TextMeshProUGUI Height;
    
    
    public void Interact()
    {
        
        switch (type)
        {
            case GameCellType.Simple:
                GetComponent<IGameCellAnimation>().Interact();
                return;
                break;
            case GameCellType.InteractableOff:
                GetComponent<IGameCellAnimation>().Interact();
                type = GameCellType.InteractableOn;
                break;
            case GameCellType.InteractableOn:
                type = GameCellType.InteractableOff;
                GetComponent<IGameCellAnimation>().Interact();
                break;
            default:
                break;
        }
        UpdateView();
    }
    
    public void SetupDebugerPart(Vector2Int cordinate,int height,bool debuging)
    {
        cordianteText.text = $" x:{cordinate.x},y:{cordinate.y}";
        Height.text = height.ToString();
        ToggleDebugger(debuging);
    }

    public void ToggleDebugger(bool debugger)
    {
        
       
            cordianteText.gameObject.SetActive(debugger);
            Height.gameObject.SetActive(debugger);
            border.gameObject.SetActive(debugger);
       
    }

    public void Setup(GameCellType cellType)
    {
        type = cellType;

        UpdateView();
    }

    public void TurnOff()
    {
        type = GameCellType.InteractableOff;
        UpdateView();
    }

    private void UpdateView()
    {
        switch (type)
        {
            case GameCellType.Simple:
                InteractableOff.SetActive(false);
                InteractableOn.SetActive(false);
                break;
            case GameCellType.InteractableOff:
                InteractableOff.SetActive(true);
                InteractableOn.SetActive(false);

                break;
            case GameCellType.InteractableOn:
                InteractableOff.SetActive(false);
                InteractableOn.SetActive(true);

                break;
            default:
               break;
        }
    }

  
}

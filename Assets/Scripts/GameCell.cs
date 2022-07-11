using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IGameCellAnimation
{
    public void Interact();
}

public enum GameCellType
{
    Simple,InteractableOff,InteractableOn
}
public class GameCell : MonoBehaviour
{
    public GameCellType type;

    [SerializeField] private GameObject InteractableOff;
    [SerializeField] private GameObject InteractableOn;
    
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

    public void Setup(GameCellType cellType)
    {
        type = cellType;

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

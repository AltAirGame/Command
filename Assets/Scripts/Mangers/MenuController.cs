using System.Collections.Generic;
using DG.Tweening;
using MHamidi;
using TMPro;
using UnityEngine;
using Utils.Singlton;

public class MenuController:MonoBehaviour 
{
    
    [Header("--------------------------------Level SelectionPanel---")]
    [SerializeField]
    private RectTransform LevelSelctionParrent;
    
    
    [SerializeField]
    private RectTransform RaycastBlocker;
    [SerializeField]
    private RectTransform LevelSelctionMenu;
    
    [Header("--------------------------------Level Name---")] [SerializeField]
    private TextMeshProUGUI CurrentLevelName;
    
    
    
    
    
    
    public void ShowLevelMenu(List<Level>levels)
    {
        
        Util.ShowMessag($"Show Level menu",TextColor.Yellow);
        RaycastBlocker.gameObject.SetActive(true);
        AddLevelIcons(levels);
        UpdateLevelnameText(string.Empty);
    }

    public void UpdateLevelnameText(string name)
    {
        CurrentLevelName.text = name;
    }

    private void ClearLevelIcons()
    {
        foreach (var item in LevelSelctionParrent.GetComponentsInChildren<LevelSelectButton>())
        {
            item.transform.SetParent(Dipendency.Instance.Pool.GetGameObject().transform,false);
            item.gameObject.SetActive(false);
        }
    }

    private void AddLevelIcons(List<Level> gameDataLevels)
    {
        Util.ShowMessag($"Add Levels  and Levels Count is {gameDataLevels}");
        
        foreach (var item in gameDataLevels)
        {
            //Just for Simplify
            var passed = true;
            var icon = Resources.Load<Sprite>(passed?"Unlocked":"Lock");
            var number = item.number + 1;
            
            var levelButtonObject=Dipendency.Instance.Pool.Get("LevelButton");
            levelButtonObject.SetActive(true);
            levelButtonObject.transform.SetParent(LevelSelctionParrent,false);
            levelButtonObject.GetComponent<LevelSelectButton>().Setup(new LevelSelectButtonData(number,passed,icon,
                () =>
                {
                    Dipendency.Instance.GameManger.StartLevel(item);
                    Hide();
                    
                }));
        }
        Show();
    }

    private void Show()
    {
        LevelSelctionMenu.transform.position =new Vector3(Screen.width * 4, Screen.height / 2, 0);
        LevelSelctionMenu.gameObject.SetActive(true);
        LevelSelctionMenu.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), .5f).SetEase(Ease.InOutQuint);
    }

    private void Hide()
    {
       
        LevelSelctionMenu.transform.DOMove(new Vector3(Screen.width * 2, Screen.height / 2, 0), .5f).SetEase(Ease.InOutQuint).OnComplete(() =>
        {   ClearLevelIcons();
            RaycastBlocker.gameObject.SetActive(false);
        });
}

}
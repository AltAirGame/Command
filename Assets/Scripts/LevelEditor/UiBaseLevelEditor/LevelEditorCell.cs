
using System;
using DG.Tweening;
using MHamidi;
using UnityEngine;
using UnityEngine.UI;

namespace MHamidi
{
    

public enum CellType
{   Empty,
    Flat,
    FlatLight,
    OneStepHeight,
    OneStepLight,
    TwoStepHeight,
    TwoStepLight
}
public class LevelEditorCell : MonoBehaviour,ICellEditor
{
    

    [SerializeField]private Image Background;
    [SerializeField]Sprite[] icons;
    public void Interact()
    {
       ChangeValue();
    }

    private void Start()
    {
        UpdateView();
    }

    public CellType type { get; set; }
    public int cellType { get; set; } //Can Be Used In Replace of An Enmum 

    public void SetValue(CellType type)
    {
        this.type = type;
        UpdateView();
    }

    public void ChangeValue()
    {
        cellType += 1;
        
        if (cellType>=7)
        {
            cellType = 0;
        }
        UpdateView();
    }
    private void UpdateView()
    {
        if (icons==null)
        {
          
            Util.ShowMessag($"the Icon Array is Empty or Null");
            return;
        }
        Background.sprite = icons[cellType];
        transform.DOScale(.5f*Vector3.one,.2f).OnComplete(() =>
        {
            transform.localScale=Vector3.one;
        });
    }
}
}
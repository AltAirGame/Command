using System;
using System.Collections;
using System.Collections.Generic;
using MHamidi;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DiscreteSlider : MonoBehaviour
{
    public int CurrentValue=0;
    public int maxValue=8;
    public int minValue=0;
    public Image fill;

    public void SetCurrentValue(int value)
    {
        if (value>maxValue || value < minValue)
        {
            return;
        }

        CurrentValue = value;
        UpdateView();
    }

    private void Start()
    {
        CurrentValue = minValue;
        UpdateView();
        
    }

    public void IncreaseValue()
    {

        if (CurrentValue<maxValue)
        {
            CurrentValue++;
        }
        UpdateView();
        
    }

    public void DecreaseValue()
    {
        if (CurrentValue>minValue)
        {
            CurrentValue--;
        }
        UpdateView();
    }

    private void UpdateView()
    {
        var fillSize=fill.rectTransform.rect.width;
        Util.ShowMessag($"FillSize is {fillSize} ",TextColor.Red);
        var percentage = fillSize / 12;
        fill.fillAmount =CurrentValue *percentage*.002f;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    
    
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;


    public void Setup(string text, Action onClick)
    {

        this.text.text = text;
        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(() => {onClick?.Invoke();});

    }
}

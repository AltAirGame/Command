using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BufferUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textmeshPro;

    [SerializeField] private RectTransform Buffer;

    [SerializeField] private Button Button;




    public void SetText(string text)
    {
        textmeshPro.text=text;
    }

    public void SetSize(int size)
    {
        var SizeOfBuffer = Mathf.RoundToInt(size / 4);
        Buffer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,SizeOfBuffer); 
    }

    public void SetOnClick(Action onCLick)
    {
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => { onCLick?.Invoke();});
    }
}
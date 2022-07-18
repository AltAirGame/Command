using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Singlton;

public class BufferUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textmeshPro;

    [SerializeField] private RectTransform Buffer;

    [SerializeField] private Button Button;


    public void SetText(string text)
    {
        textmeshPro.text = text;
    }

    public void SetSize(int size)
    {
        Clear();
        int SizeOfBuffer;
        if (size > 4)
        {
            SizeOfBuffer = Mathf.RoundToInt(size / 4) * 100;
        }
        else
        {
            SizeOfBuffer = 100;
        }
        Buffer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SizeOfBuffer);
       
    }

    public void Clear()
    {
        foreach (var item in transform.GetComponentsInChildren<GameButton>())
        {
            
            item.gameObject.SetActive(false);
            item.transform.SetParent(Dipendency.Instance.Pool.GetGameObject().transform,false);
        }
    }

    public void SetOnClick(Action onCLick)
    {
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => { onCLick?.Invoke(); });
    }
}
using System.Collections;
using System.Collections.Generic;
using MHamidi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectButton : MonoBehaviour
{
    private LevelSelectButtonData data;


    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private Image Icon;

    public void Setup(LevelSelectButtonData data)
    {
        Util.ShowMessag($" Setueing Up Level SelectionButton was Happende");
        this.data = data;
        UpdateView();
    }

    private void UpdateView()
    {
        this.text.text = data.Number.ToString();
        this.Icon.sprite = data.icon;
        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(() => { data.onClick?.Invoke();});
        this.button.interactable = data.isUnlocked;
    }
}

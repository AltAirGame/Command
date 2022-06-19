using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
  [SerializeField] private Image Icon;
  [SerializeField] private Button Button;

  public void SetIcon(Sprite icon)
  {
    this.Icon.sprite = icon;

  }

  public void SetListener(Action onClick)
  {
    this.Button.onClick.RemoveAllListeners();
    Button.onClick.AddListener(() => { onClick?.Invoke();});
  }
}

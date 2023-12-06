using System;
using UnityEngine;

namespace GameSystems.Core
{
    public class SimpleHideAndShow:IUianimation
    {
        public void Show(GameObject Subject, Action OnShow)
        {
            Subject.SetActive(true);
            OnShow?.Invoke();
        }
        public void Hide(GameObject Subject, Action OnHide)
        {
            Subject.SetActive(false);
            OnHide?.Invoke();
        }
    }
}
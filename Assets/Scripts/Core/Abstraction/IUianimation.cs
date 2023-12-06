using System;
using UnityEngine;

namespace GameSystems.Core
{
    public interface IUianimation
    {
        public void Show(GameObject Subject, Action OnShow);
        public void Hide(GameObject Subject, Action OnHide);
    }
}
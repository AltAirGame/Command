using System;
using DG.Tweening;
using UnityEngine;

namespace GameSystems.Core
{
    public class SlidInOut:IUianimation
    {
        public void Show(GameObject Subject, Action OnShow)
        {   
            var startPos = new Vector3(.5f * Screen.width, -1.5f * Screen.height, 0);
            var targetPos = new Vector3(.5f * Screen.width, .5f * Screen.height, 0);
            Subject.transform.position = startPos;
           
            Subject.transform.DOMove(targetPos, 1.2f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                
                OnShow?.Invoke();
            });

       
        }
        public void Hide(GameObject Subject, Action OnHide)
        {
            
            var startPos = Subject.transform.position;
            var targetPos =new Vector3(.5f * Screen.width, -1.5f * Screen.height, 0);
            Subject.transform.DOMove(targetPos, .5f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
               
                OnHide?.Invoke();
                Subject.transform.parent.gameObject.SetActive(false);
            });
           
            
        }
    }
}
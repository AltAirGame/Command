
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace MHamidi
{
    public class OnPointerEnterAction : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField]
        Image DisplayBar;
        private float holdThreashold=2f;
        private float time=0;
        public IInteractable interactable;
        private bool TimerisOn;
        private void Start()
        {
            interactable = GetComponent<IInteractable>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable is null||time>.5f)
            {
                return;
            }
            interactable.Interact();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (DisplayBar is null)
            {
                return;
            }
            TimerisOn = true;
           StopCoroutine(Timer());
           StartCoroutine(Timer());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            TimerisOn = false;
        }

        IEnumerator Timer()
        { 
            time = 0f;
            while (TimerisOn)
            {
                time += Time.deltaTime;
                if (DisplayBar is not null)
                {
                    if (time>.2f)
                    {
                        DisplayBar.fillAmount=time / holdThreashold;
                    }
                   
                }
                yield return null;
            }

            if (time>holdThreashold)
            {
                interactable.InteractionTwo();
            }

            while (time > 0)
            {
                time -= Time.deltaTime * 2.5f;
                DisplayBar.fillAmount=time / holdThreashold;
                yield return null;
            }

            


        }
    }
}
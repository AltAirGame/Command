
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MHamidi
{
    public class OnMouseClick : MonoBehaviour,IPointerClickHandler
    {
        public IInteractable interactable;

        private void Start()
        {
            interactable = GetComponent<IInteractable>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable is null)
            {
                return;
            }
            interactable.Interact();
        }
    }
}
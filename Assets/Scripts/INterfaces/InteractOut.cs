using System;
using System.Collections;
using System.Collections.Generic;
using MHamidi;
using UnityEngine;
using UnityEngine.Events;

public class InteractOut : MonoBehaviour,IInteractable
{


    public UnityEvent Click;
    public UnityEvent HoldDown;
    public UnityEvent OnPointerEnter;
    
    public GameObject Interactable;
    private IInteractable _interactable;
    private void Start()
    {
        if (Interactable!=null)
        {
            _interactable = Interactable.GetComponent<IInteractable>();
            
        }
    }

    public void Interact()
    {
        Click?.Invoke();
    }

    public void InteractionTwo()
    {
     HoldDown?.Invoke();
    }

    public void InteractionThree()
    {
        OnPointerEnter?.Invoke();
    }
}

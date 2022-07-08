using System;
using System.Collections;
using System.Collections.Generic;
using MHamidi;
using UnityEngine;
using UnityEngine.Events;

public class InteractOut : MonoBehaviour,IInteractable
{


    public UnityEvent interaction;
    public UnityEvent interactionTwo;
    public UnityEvent interactionThree;
    
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
        interaction?.Invoke();
    }

    public void InteractionTwo()
    {
     interactionTwo?.Invoke();
    }

    public void InteractionThree()
    {
        interactionThree?.Invoke();
    }
}

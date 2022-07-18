using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IPlayerAnimation
{
    public void Walk();
    public void Jump();
    public void InterAct();
}

public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
{
    public void Walk()
    {
       
    }

    public void Jump()
        {
            transform.DOScale(new Vector3(1, .5f, 1), .1f).OnComplete(() => { transform.DOScale(Vector3.one, 0.1f); });
        }

        public void InterAct()
        {
           
        }
    }
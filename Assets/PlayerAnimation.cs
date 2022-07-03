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
        transform.DORotate(new Vector3(0, 0, 10f), .2f).OnComplete(() =>
        {
            {
                transform.DORotate(new Vector3(0, 0, -10f), .2f).OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0, 0, 0), .2f);
                });

            }
        });
    }

    public void Jump()
        {
            transform.DOScale(new Vector3(1, .5f, 1), .2f).OnComplete(() => { transform.DOScale(Vector3.one, 0.3f); });
        }

        public void InterAct()
        {
            transform.DOScale(new Vector3(.5f, .5f, .5f), .2f)
                .OnComplete(() => { transform.DOScale(Vector3.one, 0.3f); });
        }
    }
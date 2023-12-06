using DG.Tweening;
using GameSystems.Core;
using UnityEngine;


public class PlayerAnimationWrapper : MonoBehaviour, IPlayerAnimation
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
using DG.Tweening;
using GameSystems.Core;
using UnityEngine;

public class GameCellAnimation:MonoBehaviour,IGameCellAnimation
{
    public void Interact()
    {
        var StartPos = new Vector3(transform.position.x, transform.position.y , transform.position.z);
        transform.DOMove(new Vector3(StartPos.x, StartPos.y - .5f, StartPos.z), .1f).OnComplete(
            () =>
            {
                transform.DOMove(StartPos, 1f);
            });
    }
}
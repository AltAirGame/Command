using DG.Tweening;
using MHamidi;
using UnityEngine;


public class MoveCommand : ICommand
{
    public MoveCommand()
    {
    }

    public string name
    {
        get { return this.GetType().Name.ToLower(); }
        set { }
    }

    public void Execute(GameObject subject)
    {
     MoveForward(subject);
    }

    public void Undo(GameObject subject)
    {
        MoveBackWard(subject);
    }



    private void MoveForward(GameObject subject)
    {
        var target=subject.transform.position - subject.transform.forward;
        if (LevelManger3D.Instance.MoveValidation(new Vector3Int((int)target.x,(int)target.y,(int)target.z)))
        {
            subject.transform.DOMove(target, .2f);
        }
        subject.GetComponentInChildren<IPlayerAnimation>().Walk();
        
    }

    private void MoveBackWard(GameObject subject)
    {
        subject.transform.position = subject.transform.position + subject.transform.forward;
    }
}
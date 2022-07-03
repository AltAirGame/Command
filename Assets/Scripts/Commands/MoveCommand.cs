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

    public bool executeWasSuccessful { get; set; }

    public void Execute(GameObject subject)
    {
        executeWasSuccessful=MoveForward(subject);
    }

    public void Undo(GameObject subject)
    {
        MoveBackWard(subject);
    }


    private bool MoveForward(GameObject subject)
    {
        var target = subject.transform.position - subject.transform.forward;
        if (LevelManger3D.Instance.MoveValidation())
        {
            subject.transform.DOMove(target, .2f);
            subject.GetComponentInChildren<IPlayerAnimation>().Walk();
            return true;
        }
        subject.GetComponentInChildren<IPlayerAnimation>().Walk();
        return false;   


    }
    
    private void MoveBackWard(GameObject subject)
    {
        if (!executeWasSuccessful) return;
        subject.transform.position = subject.transform.position + subject.transform.forward;
        executeWasSuccessful = false;



    }
}
using System.Collections;
using DG.Tweening;
using MHamidi;
using UnityEngine;
using Utils.Singlton;


public class MoveCommand : ICommand
{
    public MoveCommand()
    {
    }

    public MoveCommand(GameObject subjectOfCommands)
    {
        this.SubjectOfCommands = subjectOfCommands;
    }

    public string name
    {
        get { return this.GetType().Name.ToLower(); }
        set { }
    }

    public GameObject SubjectOfCommands { get; set; }

    public bool Done { get; set; }

    public bool executeWasSuccessful { get; set; }
    
    public IEnumerator Execute(GameObject subject)
    {
        SubjectOfCommands = subject;
        executeWasSuccessful=MoveForward(subject);
        yield return null;
    }

    public IEnumerator Undo(GameObject subject)
    {
        MoveBackWard(subject);
        yield return null;
    }


    private bool MoveForward(GameObject subject)
    {
        var target = subject.transform.position + subject.transform.forward;
        
       
        if (RequestMoveValidation(1))
        {
            subject.transform.DOMove(target, .2f).OnComplete(() => {  Done = true;});
           
            subject.GetComponentInChildren<IPlayerAnimation>().Walk();
            return true;
        }
        subject.GetComponentInChildren<IPlayerAnimation>().Walk();
        return false;
       

    }
    
    public bool RequestMoveValidation(int direction)
    {
        Util.ShowMessag($" Entered Move Validation");
        if (direction > 0)
        {
            Util.ShowMessag($"Direction Was Forward");
            return MoveAble();
        }
        else
        {
        }

        return false;
    }
    private bool MoveAble()
    {
        if (IsForwardOutOfBound())
        {
            Util.ShowMessag($"Forward Was Out of Bound");
            return false;
        }

        if (IsForawardJumpable()) // if(mathf.abs(Playe Height-Forward Height)==0) //No Height Difrence
        {
            Util.ShowMessag($"Forward Was Jumpable");
            return false;
        }

        Util.ShowMessag($"Forward Was Jumpable");
        return true;
    }

    private bool IsForawardJumpable()
    {
        if (Mathf.Abs(Dipendency.Instance.LevelManger.GetPlayeCurrentHeight()-Dipendency.Instance.LevelManger.GetFrontOfPlayeHeight())>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsForwardOutOfBound()
    {
        
        Util.ShowMessag($" The PlayerPosition is {SubjectOfCommands.transform.position} and the PlayeForward is {SubjectOfCommands.transform.forward} and the target is {SubjectOfCommands.transform.position+SubjectOfCommands.transform.forward} ",TextColor.Yellow);
        var forward = SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward;
        if (forward.x>0 &&forward.x<Dipendency.Instance.LevelManger.currentLevel.width||forward.z>0 &&forward.z<Dipendency.Instance.LevelManger.currentLevel.height)
        {
            return false;
        }

        return true;
    }
    public bool IsForwardEmpty()
    {
        Util.ShowMessag($" The PlayerPosition is {SubjectOfCommands.transform.position} and the PlayeForward is {SubjectOfCommands.transform.forward} and the target is {SubjectOfCommands.transform.position+SubjectOfCommands.transform.forward} ",TextColor.Green);
        var forward = SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward;
        if (Dipendency.Instance.LevelManger.currentLevel.LevelLayout[(int)forward.x,(int)forward.z].cellHeight>0)
        {
            return false;
        }

        return true;
    }
  
    private void MoveBackWard(GameObject subject)
    {
        if (!executeWasSuccessful) return;
        subject.transform.position = subject.transform.position - subject.transform.forward;
        executeWasSuccessful = false;
    }
}
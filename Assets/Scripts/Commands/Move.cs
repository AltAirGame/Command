using System;
using System.Collections;
using DG.Tweening;
using MHamidi;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{


    public class Move : ICommand
    {
        public Move()
        {
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
            executeWasSuccessful = false;
            SubjectOfCommands = subject;
            executeWasSuccessful = MoveAble();
            yield return Dipendency.Instance.StartCoroutine(MoveForward(subject, executeWasSuccessful));
        }

        public IEnumerator Undo(GameObject subject)
        {
            SubjectOfCommands = subject;

            yield return Dipendency.Instance.StartCoroutine(MoveBackWard(subject));
        }


        private IEnumerator MoveForward(GameObject subject, bool moveable)
        {
            var target = subject.transform.position + subject.transform.forward;


            if (moveable)
            {
                subject.transform.DOMove(target, .2f, true).OnComplete(() => { Done = true; });

                subject.GetComponentInChildren<IPlayerAnimation>().Walk();

                yield return Util.GetWaitForSeconds(.2f);
                yield break;
            }
            else
            {
                subject.GetComponentInChildren<IPlayerAnimation>().Walk();
                yield return Util.GetWaitForSeconds(.1f);
            }


        }


        private bool MoveAble()
        {
            if (IsForwardOutOfBound())
            {
                Util.ShowMessag($"Forward Was Out of Bound");
                return false;
            }

            if (IsForawardJumpable())
            {
                Util.ShowMessag($"Forward Was Jumpable");
                return false;
            }

            Util.ShowMessag($"Forward Was Not Jumpable");
            return true;
        }

        private bool IsForawardJumpable()
        {
            if (Mathf.Abs(Dipendency.Instance.LevelManger.GetFrontOfPlayeHeight() -
                          Dipendency.Instance.LevelManger.GetPlayeCurrentHeight()) > 0)
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
            Util.ShowMessag(
                $" The PlayerPosition is {SubjectOfCommands.transform.position} and the PlayeForward is {SubjectOfCommands.transform.forward} and the target is {SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward} ",
                TextColor.Yellow);
            var forward = SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward;
            if (forward.x > 0 && forward.x <= Dipendency.Instance.LevelManger.currentLevel.height - 1 && forward.z > 0 &&
                forward.z <= Dipendency.Instance.LevelManger.currentLevel.width - 1)
            {
                return false;
            }

            return true;
        }

        public bool IsForwardEmpty()
        {
            Util.ShowMessag(
                $" The PlayerPosition is {SubjectOfCommands.transform.position} and the PlayeForward is {SubjectOfCommands.transform.forward} and the target is {SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward} ",
                TextColor.Green);
            var forward = SubjectOfCommands.transform.position + SubjectOfCommands.transform.forward;
            if (Dipendency.Instance.LevelManger.currentLevel.LevelLayout[(int)forward.x, (int)forward.z].cellHeight > 0)
            {
                return false;
            }

            return true;
        }

        private IEnumerator MoveBackWard(GameObject subject)
        {
            if (!executeWasSuccessful)
            {
                yield return Util.GetWaitForSeconds(.2f);
                yield break;
            }

            var target = subject.transform.position - subject.transform.forward;
            subject.transform.DOMove(target, .2f);
            yield return Util.GetWaitForSeconds(.2f);

        }
    }
}
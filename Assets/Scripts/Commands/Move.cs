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


        public string Name
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

            yield return Dipendency.Instance.StartCoroutine(MoveForward(subject, executeWasSuccessful));
        }

        public IEnumerator Undo(GameObject subject)
        {
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight, int forwardHeight)
        {
            //Check Out Of Bound
            if (playerPosition.x + playerForward.x < 0 && playerPosition.x + playerForward.x >= width)
            {
                return false;
            }

            if (playerPosition.z + playerForward.z < 0 && playerPosition.z + playerForward.z >= width)
            {
                return false;
            }

            if (Mathf.Abs(forwardHeight - playerHeight) > 0)
            {
                return false;
            }

            return true;
        }

        public bool ExecutionInstruction(GameObject subjectOfCommand, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight, int forwardHeight)
        {
            return false;
        }


        private IEnumerator MoveForward(GameObject subject, bool moveable)
        {
            var available = Dipendency.Instance.LevelManger.IsAvailable(
                this);
            if (available)
            {
                Dipendency.Instance.LevelManger.Submit(this);
            }

            var target = subject.transform.position + subject.transform.forward;
            subject.GetComponentInChildren<IPlayerAnimation>().Walk();
                
            // if (moveable)
            // {
            //     subject.transform.DOMove(target, .2f, true).OnComplete(() => { Done = true; });
            //
            //     subject.GetComponentInChildren<IPlayerAnimation>().Walk();
            //
            //     yield return Util.GetWaitForSeconds(.2f);
            //     yield break;
            // }
            // else
            // {
            //     subject.GetComponentInChildren<IPlayerAnimation>().Walk();

            // }
            yield return Util.GetWaitForSeconds(.1f);
        }
    }
    
}





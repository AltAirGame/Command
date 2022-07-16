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

        public IEnumerator Execute(GameObject subject)
        {
            yield return Dipendency.Instance.StartCoroutine(MoveForward(subject));
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


        private IEnumerator MoveForward(GameObject subject)
        {
            var available = Dipendency.Instance.LevelManger.IsAvailable(
                this);
            if (available)
            {
                var levelManger = Dipendency.Instance.LevelManger;
                levelManger.playerPos += levelManger.playerForward;
                levelManger.UpdatePlayer();
                subject.GetComponentInChildren<IPlayerAnimation>().Walk();
                yield return Util.GetWaitForSeconds(.2f);
            }
            else
            {
                subject.GetComponentInChildren<IPlayerAnimation>().Walk();
                yield return Util.GetWaitForSeconds(.2f);
            }

            
        }
    }
}
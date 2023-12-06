using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class Move : ICommand
    {
        public Move()
        {
        }


        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public GameObject SubjectOfCommands { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            yield return ServiceLocator.Instance.StartCoroutine(MoveForward(subject));
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
                Util.ShowMessage($"[{this.GetType().Name}] X was out of Bound", TextColor.Red);
                return false;
            }

            if (playerPosition.z + playerForward.z < 0 && playerPosition.z + playerForward.z >= width)
            {
                Util.ShowMessage($"[{this.GetType().Name}] X was out of Bound", TextColor.Red);
                return false;
            }

            if (Mathf.Abs(forwardHeight - playerHeight) > 0)
            {
                Util.ShowMessage(
                    $"[{this.GetType().Name}] Height Difference was incorrect {forwardHeight - playerHeight} ",
                    TextColor.Red);
                return false;
            }

            Util.ShowMessage($"[{this.GetType().Name}] Was Available", TextColor.Green);
            return true;
        }


        private IEnumerator MoveForward(GameObject subject)
        {
            var available = ServiceLocator.Instance.GetService<ILevelManagmentService>().IsAvailable(
                this);
            if (available)
            {
                var levelManger = ServiceLocator.Instance.GetService<ILevelManagmentService>();
                levelManger.PlayerPos += levelManger.PlayerForward;
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
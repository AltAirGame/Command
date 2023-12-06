using System.Collections;
using UnityEngine;


namespace GameSystems.Core
{
    public class Jump : ICommand
    {
        public string Name
        {
            get => GetType().Name.ToLower();
        }

        public GameObject SubjectOfCommands { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            SubjectOfCommands = subject;
            Util.ShowMessage("Jump");

            yield return ServiceLocator.Instance.RunCoroutine(JumpForward(subject));
        }

        public IEnumerator Undo(GameObject subject)
        {
            Util.ShowMessage("Jump Undo");
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward,
            int playerHeight, int forwardHeight)
        {
            // Refactor conditions into separate, descriptive methods to improve readability
            if (IsOutOfBound(playerPosition, playerForward, width))
            {
                Util.ShowMessage($"[{Name}] Out of Bound", TextColor.Red);
                return false;
            }

            if (IsHeightDifferenceIncorrect(playerHeight, forwardHeight))
            {
                Util.ShowMessage($"[{Name}] Height Difference Incorrect", TextColor.Red);
                return false;
            }

            Util.ShowMessage($"[{Name}] Was Available", TextColor.Green);
            return true;
        }

        private IEnumerator JumpForward(GameObject subject)
        {
            ILevelManagmentService levelManagmentService = ServiceLocator.Instance.GetService<ILevelManagmentService>();
            if (levelManagmentService == null)
            {
                yield break;
            }

            var available = levelManagmentService.IsAvailable(this);
            if (available)
            {
                var jumpHeight = levelManagmentService.GetFrontOfPlayerHeight() -
                                 levelManagmentService.GetPlayerCurrentHeight();
                var start = levelManagmentService.PlayerPos;
                levelManagmentService.PlayerPos = new Vector3Int(levelManagmentService.PlayerPos.x,
                    levelManagmentService.PlayerPos.y + jumpHeight, levelManagmentService.PlayerPos.z);
                levelManagmentService.PlayerPos += levelManagmentService.PlayerForward;
                var end = levelManagmentService.PlayerPos;

                yield return ServiceLocator.Instance.RunCoroutine(JumpForwardAction(start, end));
            }
            else
            {
                subject.GetComponentInChildren<IPlayerAnimation>().Jump();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator JumpForwardAction(Vector3 start, Vector3 end)
        {
            Util.ShowMessage($"[{Name}] Happened", TextColor.Yellow);
            float timeFrame = 0;
            var positions = GetBezierPositions(start, end);

            while (Vector3.Distance(SubjectOfCommands.transform.position, positions.position3) > 0)
            {
                SubjectOfCommands.transform.position = CalculateCubicBezierCurve(timeFrame, positions);
                timeFrame += Time.deltaTime * 3;
                if (timeFrame > 0.99f)
                {
                    timeFrame = 1;
                }

                yield return null;
            }

            SubjectOfCommands.transform.position = positions.position3;
            Util.ShowMessage("Jump Ended");
        }

        private (Vector3 position0, Vector3 position1, Vector3 position2, Vector3 position3) GetBezierPositions(
            Vector3 start, Vector3 end)
        {
            var position0 = new Vector3(start.x, 1 + ((start.y - 1) * 0.4f), start.z);
            var position1 = new Vector3(start.x, start.y + 2, start.z);
            var position2 = new Vector3(end.x, end.y + 2, end.z);
            var position3 = new Vector3(end.x, (1 + (end.y - 1) * 0.4f), end.z);

            return (position0, position1, position2, position3);
        }

        private Vector3 CalculateCubicBezierCurve(float t,
            (Vector3 position0, Vector3 position1, Vector3 position2, Vector3 position3) positions)
        {
            var u = 1 - t;
            var uu = u * u;
            var tt = t * t;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * positions.position0;
            p += 3 * uu * t * positions.position1;
            p += 3 * u * tt * positions.position2;
            p += ttt * positions.position3;

            return p;
        }

        private bool IsOutOfBound(Vector3Int playerPosition, Vector3Int playerForward, int width)
        {
            return playerPosition.x + playerForward.x < 0 || playerPosition.x + playerForward.x >= width ||
                   playerPosition.z + playerForward.z < 0 || playerPosition.z + playerForward.z >= width;
        }

        private bool IsHeightDifferenceIncorrect(int playerHeight, int forwardHeight)
        {
            return Mathf.Abs(forwardHeight - playerHeight) == 0 || Mathf.Abs(forwardHeight - playerHeight) >= 2;
        }
    }
}
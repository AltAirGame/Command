using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class Jump : ICommand
    {
        public string Name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public Jump()
        {
            
        }

        public GameObject SubjectOfCommands { get; set; }

      

        public IEnumerator Execute(GameObject subject)
        {
            SubjectOfCommands = subject;
            Util.ShowMessag($" Jump");
            yield return Dipendency.Instance.StartCoroutine(JumpForward(subject));
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            Util.ShowMessag($"Jump Undo");
          
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            yield return null;
        }

        public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
            int forwardHeight)
        {
            
            if (playerPosition.x + playerForward.x < 0 && playerPosition.x + playerForward.x >= width)
            {
                Util.ShowMessag($"[{this.GetType().Name}] X was out of Bound",TextColor.Red);
                return false;
            }

            if (playerPosition.z + playerForward.z < 0 && playerPosition.z + playerForward.z >= width)
            {
                Util.ShowMessag($"[{this.GetType().Name}] z was out of Bound",TextColor.Green);
                return false;
            }

            if (Mathf.Abs(forwardHeight - playerHeight) ==0 ||Mathf.Abs(forwardHeight - playerHeight) >=2 )
            {
                Util.ShowMessag($"[{this.GetType().Name}] Height Difference was incorrect {forwardHeight-playerHeight} ",TextColor.Red);
                return false;
            }
            Util.ShowMessag($"[{this.GetType().Name}] Was Available",TextColor.Green);
            return true;
        }

        public void ExecutionInstruction(ILevelManger levelManger)
        {
           
        }

       

        public IEnumerator JumpForward(GameObject subject)
        {

            yield return null;
            var available = Dipendency.Instance.LevelManger.IsAvailable(
                this);
            if (available)
            {
                var levelManger = Dipendency.Instance.LevelManger;
                var jumpHeight = levelManger.GetFrontOfPlayerHeight() - levelManger.GetPlayerCurrentHeight();
                var start = levelManger.playerPos;
                  levelManger.playerPos = new Vector3Int(levelManger.playerPos.x, levelManger.playerPos.y+jumpHeight,
                    levelManger.playerPos.z);
                levelManger.playerPos += levelManger.playerForward;
                var end = levelManger.playerPos;

                yield return Dipendency.Instance.StartCoroutine(JumpForwardAction(start,end));
                subject.GetComponentInChildren<IPlayerAnimation>().Jump();
                yield return Util.GetWaitForSeconds(.2f);
            }
            else
            {
                subject.GetComponentInChildren<IPlayerAnimation>().Jump();
                yield return Util.GetWaitForSeconds(.2f);
            }

            

        }

        private bool IsForwardEmpty()
        {
            if (Dipendency.Instance.LevelManger.currentLevel.LevelLayout[(int)SubjectOfCommands.transform.position.x,(int)SubjectOfCommands.transform.position.z].cellHeight==0)
            {
                return true;
            }

            return false;
        }

     

      

        private int GetForwardJump()
        {
            var jumpHeight = Dipendency.Instance.LevelManger.GetFrontOfPlayerHeight() -
                             Dipendency.Instance.LevelManger.GetPlayerCurrentHeight();
            return jumpHeight;
        }

    
        //there is A Bug Here 
        public IEnumerator JumpForwardAction(Vector3 start,Vector3 end)
        {

            yield return Util.GetWaitForSeconds(.5f);
            Util.ShowMessag($"[{this.GetType().Name}] Happened",TextColor.Yellow);
            float timeFrame = 0;
            var position0 = new Vector3(start.x, start.y+(start.y-1*.4f), start.z);
            var position1 = new Vector3(start.x, start.y*.4f+1, start.z);
            var position2 = new Vector3(end.x, end.y*.4f+1, end.z);
            var position3 = new Vector3(end.x, end.y+(end.y-1*.4f), end.z);
            // Vector3 p1 = new Vector3(p0.x, p0.y + 1, p0.z);
            // Vector3 p2 = new Vector3(p0.x + 1, p0.y + 1, p0.z);
            // Vector3 p3 = p0;
            //
            // switch (target)
            // {
            //     case 0:
            //         p2 = p1;
            //         p3 = p0;
            //         break;
            //     case 1:
            //
            //         p3 = (new Vector3(p0.x, p0.y + .4f, p0.z) + (SubjectOfCommands.transform.forward));
            //         Util.ShowMessag($"JUMP-UP");
            //         break;
            //     case -1:
            //         p3 = (new Vector3(p0.x, p0.y - .4f, p0.z) + (SubjectOfCommands.transform.forward));
            //         Util.ShowMessag($"JUMP-DOWN");
            //         break;
            // }
            //
            SubjectOfCommands = Dipendency.Instance.LevelManger.Player;
            while (Vector3.Distance(SubjectOfCommands.transform.position, position3) > 0)
            {
                SubjectOfCommands.transform.position = CalculateCubicBezierCurve(timeFrame, position0, position1, position2,position3);
                timeFrame += Time.deltaTime * 3; // we Can Add ease Here
                if (timeFrame > .99f)
                {
                    timeFrame = 1;
                }
            
               
                yield return null;
            }
            SubjectOfCommands.transform.position = position3;
            Util.ShowMessag($"jump Ended");
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

        private Vector3 CalculateCubicBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // (1-t) p0 + 3(1-t)^2 t*p1 +3(1-t)t*p2+t^3 *p3 

            var u = 1 - t;
            var uu = u * u;
            var tt = t * t;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
            var c = p0 + t * (p1 - p0);
            return c;
        }
    }
}
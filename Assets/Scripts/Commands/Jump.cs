using System.Collections;
using UnityEngine;
using Utils.Singlton;

namespace MHamidi
{
    public class Jump : ICommand
    {
        public string name
        {
            get { return this.GetType().Name.ToLower(); }
            set { }
        }

        public Jump()
        {
            
        }

        public GameObject SubjectOfCommands { get; set; }

        public bool Done { get; set; }

        public bool executeWasSuccessful { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            SubjectOfCommands = subject;
            Util.ShowMessag($" Jump");
            yield return Dipendency.Instance.StartCoroutine(JumpForward());
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            Util.ShowMessag($"Jump Undo");
          
            subject.GetComponentInChildren<IPlayerAnimation>().Jump();
            yield return null;
        }

      

        public IEnumerator JumpForward()
        {
            if (IsForwardOutOfBound())
            {
                yield return null;
                yield break;
            }

            if (IsForwardEmpty())
            {
                yield return null;
                yield break;
            }

            var jumpHeight = GetForwardJump();
            yield return Dipendency.Instance.StartCoroutine(JumpForwardAction(jumpHeight));
            yield return null;

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
            var jumpHeight = Dipendency.Instance.LevelManger.GetFrontOfPlayeHeight() -
                             Dipendency.Instance.LevelManger.GetPlayeCurrentHeight();
            return jumpHeight;
        }

 

        public IEnumerator JumpForwardAction(int target)
        {
            float t = 0;

            Vector3 p0 = (SubjectOfCommands.transform.position);
            Vector3 p1 = new Vector3(p0.x, p0.y + 1, p0.z);
            Vector3 p2 = new Vector3(p0.x + 1, p0.y + 1, p0.z);
            Vector3 p3 = p0;

            switch (target)
            {
                case 0:
                    p2 = p1;
                    p3 = p0;
                    break;
                case 1:

                    p3 = (new Vector3(p0.x, p0.y + .4f, p0.z) + (SubjectOfCommands.transform.forward));
                    Util.ShowMessag($"JUMP-UP");
                    break;
                case -1:
                    p3 = (new Vector3(p0.x, p0.y - .4f, p0.z) + (SubjectOfCommands.transform.forward));
                    Util.ShowMessag($"JUMP-DOWN");
                    break;
            }

            while (Vector3.Distance(SubjectOfCommands.transform.position, p3) > 0)
            {
                SubjectOfCommands.transform.position = CalculateCubicBezierCurve(t, p0, p1, p2, p3);
                t += Time.deltaTime * 3; // we Can Add ease Here
                if (t > .99f)
                {
                    t = 1;
                }

               
                yield return null;
            }
            SubjectOfCommands.transform.position = p3;
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
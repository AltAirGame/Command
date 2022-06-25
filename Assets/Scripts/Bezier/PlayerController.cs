using System.Collections;
using MHamidi;
using UnityEngine;

public class PlayerController:MonoBehaviour,IMover
{
    public void Move()
    {
        var newPos = transform.position+transform.forward;
        Debug.Log($" Move to from {transform.position } the {newPos}");
        transform.position = newPos;
    }

    public void jump()
    {
        Util.ShowMessag($"Jump");
        StartCoroutine(Jump(new Vector3(transform.position.x + 1, transform.position.y + 1, 0)));
    }

    public void RightRotate()
    {
        Debug.Log($" Rotate right");
    }

    public void LeftRotate()
    {
        Debug.Log($" Rotate Left");
    }

    public IEnumerator Jump(Vector3 target)
    {

        float t = 0;

        var p0 = transform.position;
        var p1 = new Vector3(p0.x, p0.y + 3, p0.z);
        var p2 = new Vector3(p0.x + 1, p0.y + 3, p0.z);
        var p3 = target;
        while (Vector3.Distance(transform.position,target)>0)
        {


            transform.position=CalculateCubicBezierCurve(t, p0, p1, p2, p3);
            t += Time.deltaTime;// we Can Add ease Here
            if (t>.99f)
            {
                t = 1;
            }
            yield return null;
        }
        Util.ShowMessag($"jump Ended");         
        
     
    }

    private Vector3 CalculateLinerBezierCurve(float t,Vector3 p0,Vector3 p1)
    {
        var c = p0 + t * (p1 - p0);
        return c;
        
    }
    private Vector3 CalculateQuadraticBezierCurve(float t,Vector3 p0,Vector3 p1,Vector3 p2)
    {
        
        
        var u = 1 - t;
        var uu = u * u;
        var tt = t * t;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
        
    }
    private Vector3 CalculateCubicBezierCurve(float t,Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3)
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
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
        
        Debug.Log($" Move to from {transform.position } the {transform.position+transform.forward+transform.up}");
        var newPos = transform.position+transform.forward + transform.up;
        transform.position = newPos;
    }

    public void RightRotate()
    {
        Debug.Log($" Rotate right");
    }

    public void LeftRotate()
    {
        Debug.Log($" Rotate Left");
    }
}
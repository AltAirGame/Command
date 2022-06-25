using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IMover
{
    public void Move();
    public void jump();
    public void RightRotate();
    public void LeftRotate();
}

public class Jump : MonoBehaviour
{
    [SerializeField] public IMover mover;
    private void Start()
    {
        mover = GetComponent<IMover>();
        if (mover is null)
        {
            Debug.Log($" No Mover is Founded on the Object");

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mover.jump();
        }
        if (Input.GetKeyDown(KeyCode.M))
        { 
            mover.Move();
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            mover.RightRotate();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            mover.LeftRotate();
        }
        
    }
}

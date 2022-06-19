using System;
using System.Collections;
using System.Collections.Generic;
using MHamidi.Helper;
using UnityEngine;

namespace MHamidi
{
    

public class GridManger : MonoBehaviour
{
    private float width=3;
    private float height=3;
    [SerializeField] private GameObject gamePrefab;

    private void Start()
    {
        CreatGrid();
    }

    public void CreatGrid(int[,] cells)
    {

        for (int x = 0; x <width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (cells[x,y]==0)
                {
                    
                    return;
                }
                else
                {
                    var cell=Pool.Instance.Get("Cell");
                    cell.gameObject.SetActive(true);
                    cell.transform.position = new Vector3(x, 0, y);
                }
               
            }
        }
        
    }
    public void CreatGrid()
    {

        for (int x = 0; x <width; x++)
        {
            for (int y = 0; y < height; y++) 
            {

              
               
                var cell=Pool.Instance.Get("Cell");
                cell.gameObject.SetActive(true);
                cell.transform.position = new Vector3(x, 0, y);
               
               
            }
        }
        
    }



}
}
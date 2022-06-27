using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using MHamidi;
using MHamidi.Helper;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGrid : MonoBehaviour
{
    [SerializeField] private Toggle[] _toggles;
    [SerializeField] private DiscreteSlider BufferSize;
    [SerializeField] private DiscreteSlider P1Size;
    [SerializeField] private DiscreteSlider P2Size;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    
    [SerializeField] private LevelData currrentLevel;
    [SerializeField] private List<int> commands = new List<int>();
        
    public DataManger dataManger;
    public ICellEditor[,] Grid;
    public int[,] grid;
  
    [SerializeField]
    private RectTransform parrent;

    private void Start()
    {
        // var json = SavingSystem.Load("PlayerData");
        // if (string.IsNullOrWhiteSpace(json))
        // {
        //     _dropdown.ClearOptions();
        //     _playerData = null;
        // }
        // else
        // {
        //     
        //     PopulateDropDown(json);
        //     //We Had Saved levels
        //     //We Load the last Level
        // }
        //
        // parrent = GetComponent<RectTransform>();
         dataManger ??= GetComponent<DataManger>();
         CreatGrid();
    }

    private void CreatGrid()
    {
        grid = new int[width, height];
        Grid = new ICellEditor[width, height];
        var parentSize = new Vector2(parrent.rect.width, parrent.rect.height);
        var cellSizeX = parentSize.x / (float)width;
        var cellSizeY = parentSize.y / (float)height;

        var hafCellX = (float)cellSizeX / 2f;
        var hafCellY = (float)cellSizeY / 2f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var newCell = Pool.Instance.Get("EditorCell");
                newCell.gameObject.name = $" [{i}] ,[{j}]";
                newCell.SetActive(true);
                newCell.transform.SetParent(transform, false);
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(cellSizeX, cellSizeY);
                rectTransform.anchoredPosition = new Vector2((i * cellSizeX) + hafCellX, (j * cellSizeY) + hafCellY);
                Grid[i, j] = newCell.GetComponent<ICellEditor>();
            }
        }
    }

    private void HandleTogglesData()
    {
        commands.Clear();
        if (_toggles is null || _toggles.Length == 0)
        {
            Util.ShowMessag($"Toggles is Empty Or Null", TextColor.Yellow);
            return;
        }

        for (int i = 0; i < _toggles.Length; i++)
        {
            if (_toggles[i].isOn)
            {
                commands.Add(i);
            }
            else
            {
                if (commands.Contains(i))
                {
                    commands.Remove(i);
                }
            }
        }
    }



    public void SaveLevel()
    {
        HandleTogglesData();
        UpdateGrid(grid);
        dataManger ??= GetComponent<DataManger>();
        DataManger.instance.AddTOLevels(new Level(DataManger.instance.gameData.levels.Count.ToString(),commands,grid,BufferSize.CurrentValue,P1Size.CurrentValue,P2Size.CurrentValue));
        

    }
    
    private void  UpdateGrid(int [,] grid)
    {
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = Grid[i, j].cellType;
            }
        }
            
       
    }


    public void LoadLevel()
    {
        
        //Some How Must Get the Value In Drop Down And Load the Grid 
        
    }

    private void PopulateDropDown()
    {
        
        List<string> options = new List<string>();
        foreach (var level in DataManger.instance.gameData.levels)
        {
            options.Add(level.name);
        }
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
    }

  
}
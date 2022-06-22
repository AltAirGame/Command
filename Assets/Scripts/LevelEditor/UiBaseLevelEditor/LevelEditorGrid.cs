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
    [SerializeField] private TMP_InputField LevelName;
    [SerializeField] private TMP_InputField BufferSize;
    [SerializeField] private TMP_InputField P1Size;
    [SerializeField] private TMP_InputField P2Size;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    
    [SerializeField] private Level currrentLevel;
    [SerializeField] private List<int> commands = new List<int>();
    [Range(1, 8)] [SerializeField] private int mainBufferSize;
    [Range(0, 8)] [SerializeField] private int p1BufferSize;
    [Range(0, 8)] [SerializeField] private int p2BufferSize;
    private PlayerData _playerData;
    public ICellEditor[,] Grid;
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
        // CreatGrid();
    }

    private void CreatGrid()
    {
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

    private void HandleBufferSizeData()
    {
        if (string.IsNullOrWhiteSpace(BufferSize.text))
        {
            mainBufferSize = 1;
        }
        else
        {
            mainBufferSize = Int32.Parse(BufferSize.text);
            mainBufferSize = (int)Mathf.Clamp(mainBufferSize, 1, 8);
        }

        if (string.IsNullOrWhiteSpace(P1Size.text))
        {
            p1BufferSize = 0;
        }
        else
        {
            p1BufferSize = Int32.Parse(P1Size.text);
            p1BufferSize = (int)Mathf.Clamp(p1BufferSize, 1, 8);
        }

        if (string.IsNullOrWhiteSpace(P2Size.text))
        {
            p2BufferSize = 0;
        }
        else
        {
            p2BufferSize = Int32.Parse(P2Size.text);
            p2BufferSize = (int)Mathf.Clamp(p2BufferSize, 1, 8);
        }
    }

    public void SaveLevel()
    {
        HandleTogglesData();
        HandleBufferSizeData();
                              
        if (_playerData is not null) return;
        //First Time We want to Save level
        _playerData = new PlayerData();
        _playerData.levels.levels.Add(new Level(LevelName.text,commands,HandelGridData(),mainBufferSize,p1BufferSize,p1BufferSize));
        // SavingSystem.Save("PlayerData",JsonConvert.SerializeObject(_playerData));
        //SaveData.current.SaveInResource(json,name);
        
    }
    
    private int[,] HandelGridData()
    {
        var array=new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                array[i, j] = Grid[i, j].cellType;
            }
        }

        return array;
    }


    public void LoadLevel()
    {
        
        //Some How Must Get the Value In Drop Down And Load the Grid 
        
    }

    private void PopulateDropDown(string playerData)
    {
        var PlayerData = JsonConvert.DeserializeObject<PlayerData>(playerData);
        List<string> options = new List<string>();
        foreach (var level in PlayerData.levels.levels)
        {
            options.Add(level.name);
        }
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
    }

  
}
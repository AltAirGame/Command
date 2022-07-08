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
    [SerializeField] private float margin=5;
    [SerializeField] private Level currrentLevel;
    [SerializeField] private List<int> commands = new List<int>();

    public DataManger dataManger;
    public ICellEditor[,] Grid;
    public CellLayout[,] grid;
    public Vector2Int StartPos=Vector2Int.zero;
    [SerializeField] private RectTransform parrent;

    private void Start()
    {
        dataManger ??= DataManger.Instance;

        PopulateDropDown();
        CreatGrid();
    }

    private void CreatGrid()
    {
        grid = new CellLayout[width, height];
        Grid = new ICellEditor[width, height];
        var parentSize = new Vector2(parrent.rect.width, parrent.rect.height);
        var cellSizeX = parentSize.x / (float)width ;
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
                rectTransform.sizeDelta = new Vector2(cellSizeX-margin/2, cellSizeY-margin/2);
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
        dataManger.AddTOLevels(new Level(_dropdown.value, commands, grid, BufferSize.CurrentValue, P1Size.CurrentValue,
            P2Size.CurrentValue,StartPos.x,StartPos.y));
        PopulateDropDown();
    }

    public void SaveNewLevel()
    {
        HandleTogglesData();
        UpdateGrid(grid);
        dataManger.AddTOLevels(new Level(_dropdown.options.Count, commands, grid, BufferSize.CurrentValue,
            P1Size.CurrentValue, P2Size.CurrentValue,StartPos.x,StartPos.y));
        PopulateDropDown();
    }

    private void UpdateGrid(CellLayout[,] grid)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = new CellLayout(Grid[i,j].CellLevelHeight,Grid[i,j].type);
                if (Grid[i,j].IsStart)
                {
                    StartPos = new Vector2Int(i, j);
                }
            }
        }
    }

    private IEnumerator UpdateBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Grid[i, j].SetValue(currrentLevel.LevelLayout[i,j].cellHeight,currrentLevel.LevelLayout[i,j].Type);
                if (currrentLevel.startX==i&&currrentLevel.startY==j)
                {
                    Grid[i,j].SetAsStart();
                }
                yield return new WaitForSeconds(.05f);
            }
        }
    }


    public void LoadLevel()
    {
        currrentLevel = dataManger.gameData.GetLevel(_dropdown.value);

        UpdateUi(currrentLevel);
        StartCoroutine(UpdateBoard());
    }

    private void UpdateUi(Level level)
    {
        UpdateToggles(level);
        UpdateBuffers(level);
    }

    private void UpdateBuffers(Level currentLevel)
    {
        BufferSize.SetCurrentValue(currentLevel.maxBufferSize);
        P1Size.SetCurrentValue(currentLevel.maxP1Size);
        P2Size.SetCurrentValue(currentLevel.maxP2Size);
    }
    
    private void UpdateToggles(Level currentLevel)
    {
        TurnOffAllToggles();
        for (int i = 0; i < currentLevel.AvailableCommand.Count; i++)
        {
            TurnOnTogglesByIndex(currentLevel.AvailableCommand[i]);
        }
    }

    private void TurnOnTogglesByIndex( int index)
    {
        _toggles[index].isOn = true;
    }

    private void TurnOffAllToggles()
    {
        for (int i = 0; i < _toggles.Length; i++)
        {
            _toggles[i].isOn = false;
        }
    }

    private void PopulateDropDown()
    {
        List<string> options = new List<string>();

        foreach (var level in dataManger.gameData.levels)
        {
            options.Add(level.number.ToString());
        }

        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
    }
}
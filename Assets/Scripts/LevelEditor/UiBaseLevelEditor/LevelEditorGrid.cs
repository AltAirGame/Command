using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using MHamidi;
using MHamidi.Helper;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.Singlton;

public class LevelEditorGrid : MonoBehaviour
{

    [SerializeField] private LevelEditorToggle togglePrefab;
    [SerializeField] private RectTransform ToggleParrent;
    
    [SerializeField] private List<LevelEditorToggle> _toggles;
    [SerializeField] private DiscreteSlider BufferSize;
    [SerializeField] private DiscreteSlider P1Size;
    [SerializeField] private DiscreteSlider P2Size;
    [SerializeField] private TMP_Dropdown _levelNameDropDown;
    [SerializeField] private TMP_Dropdown _playerDirectionDropdown;
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    [SerializeField] private float margin=5;
    [SerializeField] private Level currrentLevel;
    [SerializeField] private List<string> commands = new List<string>();
   
        
    
    public ICellEditor[,] Grid;
    public CellLayout[,] grid;
    public Vector2Int StartPos=Vector2Int.zero;
    [SerializeField] private RectTransform parrent;

    private void Awake()
    {
       
        var listofCommands = CommandFactory.GetAllCommandInFactory();
        _toggles = new List<LevelEditorToggle>();
        foreach (var item in listofCommands)
        {
            var tempToggle=Instantiate(togglePrefab);
            tempToggle.SetNameOfToggle(item.Name);
            _toggles.Add(tempToggle);
            tempToggle.transform.SetParent(ToggleParrent,false);
            tempToggle.gameObject.SetActive(false);
            tempToggle.gameObject.SetActive(true);
        }


    }
    
    
    
    
    
  

    private void Start()
    {
       

        PopulateLevelNameDropDown();
        PopulatePlayerDirectionDropDOwn();
        CreatGrid();
        
    }

    
    //Creat Grid___________________________________________
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
                var newCell = Dipendency.Instance.Pool.Get("EditorCell");
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


    #region Collect Data

    private void HandleTogglesData()
    {
        commands.Clear();
        if (_toggles is null || _toggles.Count == 0)
        {
            Util.ShowMessag($"Toggles is Empty Or Null", TextColor.Yellow);
            return;
        }

        for (int i = 0; i < _toggles.Count; i++)
        {
            if (_toggles[i].toggle.isOn)
            {
                commands.Add(_toggles[i].Name);
            }
            else
            {
                if (commands.Contains(_toggles[i].Name))
                {
                    commands.Remove(_toggles[i].Name);
                }
            }
        }
    }

    #endregion
   


    public void SaveLevel()
    {
        HandleTogglesData();
        ClearlevelLayOutGrid();
        UpdateGrid(grid);
        
        Util.ShowMessag($"We Are Updaing Level Number ({_levelNameDropDown.value})");
        Dipendency.Instance.DataManger.AddTOLevels(new Level(currrentLevel.number,commands,(PlayerDirection)_playerDirectionDropdown.value, grid, BufferSize.CurrentValue, P1Size.CurrentValue,
            P2Size.CurrentValue,StartPos.x,StartPos.y));
        
    }

    public void SaveNewLevel()
    {
        HandleTogglesData();
        ClearlevelLayOutGrid();
        UpdateGrid(grid);
        var levelNumber = Dipendency.Instance.DataManger.gameData.levels.Count > 0
            ? Dipendency.Instance.DataManger.gameData.levels.Count + 1
            : 0;
        Dipendency.Instance.DataManger.AddTOLevels(new Level(levelNumber, commands,(PlayerDirection)_playerDirectionDropdown.value, grid, BufferSize.CurrentValue,
            P1Size.CurrentValue, P2Size.CurrentValue,StartPos.x,StartPos.y));
        PopulateLevelNameDropDown();
    }

    private void ClearlevelLayOutGrid()
    {
        for (int i = 0; i <8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                grid[i, j] = null;
            }
        }
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
        yield return StartCoroutine(ClearBoard());
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
        
        currrentLevel = Dipendency.Instance.DataManger.gameData.GetLevel(_levelNameDropDown.value);
        Util.ShowMessag($"We Are Loading Level Number ({_levelNameDropDown.value})");
        UpdateUi(currrentLevel);
        StartCoroutine(UpdateBoard());
    }

    
    private IEnumerator ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Grid[i, j].Clear();
            
                yield return new WaitForSeconds(.05f);
            }
        }
    }
    private void UpdateUi(Level level)
    {
        UpdateToggles(level);
        UpdateBuffers(level);
        UpdateDirectionDropDown(level.direction);
    }

    private void UpdateDirectionDropDown(PlayerDirection levelDirection)
    {
        _playerDirectionDropdown.value = (int)levelDirection;
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
            TurnOnTogglesByName(currentLevel.AvailableCommand[i]);
        }
    }

    private void TurnOnTogglesByName( string name)
    {
        var toggles = _toggles.Where(t => t.Name == name).First().toggle.isOn=true;
    }

    private void TurnOffAllToggles()
    {
        for (int i = 0; i < _toggles.Count; i++)
        {
            _toggles[i].toggle.isOn = false;
        }
    }

    private void PopulatePlayerDirectionDropDOwn()
    {

        var names = Enum.GetNames(typeof(PlayerDirection)).ToList();
        _playerDirectionDropdown.ClearOptions();
        _playerDirectionDropdown.AddOptions(names);
        
        
    }

    private void PopulateLevelNameDropDown()
    {
        List<string> options = new List<string>();

        foreach (var level in Dipendency.Instance.DataManger.gameData.levels)
        {
            options.Add(level.number.ToString());
        }

        _levelNameDropDown.ClearOptions();
        _levelNameDropDown.AddOptions(options);
        _levelNameDropDown.value = _levelNameDropDown.options.Count - 1;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystems.Core
{
    public class LevelEditor : MonoBehaviour
    {
        public LevelData levelData;
        private IPoolService poolService;


        //#layers
        private ILevelEditorGridManager gridManager;
        private ILevelEditorDataManager dataManager;
        private ILevelEditorUIManager uiManager;
        private ILevelEditorCoroutineManager coroutineManager;
        private RectTransform parent;


        // cashed
        [SerializeField] private RectTransform ToggleParrent;


        [SerializeField] private List<LevelEditorToggle> _toggles;

        //Buffer is the amount of commands that we could add in a Level
        [SerializeField] private DiscreteSlider BufferSize;
        [SerializeField] private DiscreteSlider P1Size;
        [SerializeField] private DiscreteSlider P2Size;
        [SerializeField] private TMP_Dropdown _levelNameDropDown;
        [SerializeField] private TMP_Dropdown _playerDirectionDropdown;
        [SerializeField] private int width = 3;
        [SerializeField] private int height = 3;
        [SerializeField] private float margin = 5;
        [SerializeField] private Level currentLevel;
        [SerializeField] private List<string> commands = new List<string>();


        private ICellEditService[,] Grid;
        private CellLayout[,] grid;
        public Vector2Int StartPos = Vector2Int.zero;
        [SerializeField] private RectTransform parrent;
        private IDataManagementService dataMangerService;

        private IEnumerator Start()
        {
            poolService = ServiceLocator.Instance.GetService<IPoolService>();
            dataMangerService = ServiceLocator.Instance.GetService<IDataManagementService>();
            parent = GetComponent<RectTransform>();

            yield return new WaitForSeconds(1);

            gridManager = new LevelEditorGridManager(poolService, parent);
            dataManager = new LevelEditorDataManager(levelData, dataMangerService);
            uiManager = new LevelEditorUIManager();
            coroutineManager = new LevelEditorCoroutineManager();


            var listCommands = CommandFactory.GetAllCommandInFactory();
            _toggles = new List<LevelEditorToggle>();
            Debug.Log($"<color=blue> We got {listCommands.Count} command from the factory </color>");
            foreach (var item in listCommands)
            {
                var tempToggle = poolService.Get("Toggle").GetComponent<LevelEditorToggle>();
                tempToggle.SetNameOfToggle(item.Name);
                _toggles.Add(tempToggle);
                tempToggle.transform.SetParent(ToggleParrent, false);
                tempToggle.gameObject.SetActive(false);
                tempToggle.gameObject.SetActive(true);
            }

            Debug.Log($" creat grid A {height} * {width}");
            PopulateLevelNameDropDown();
            PopulatePlayerDirectionDropDOwn();
            CreateGrid();
        }


        #region Grid

        // private void CreatGrid()
        // {
        //     Debug.Log($" creat grid {height} * {width}");
        //     grid = new CellLayout[width, height];
        //     Grid = new ICellEditService[width, height];
        //     var parentSize = new Vector2(parrent.rect.width, parrent.rect.height);
        //     var cellSizeX = parentSize.x / (float) width;
        //     var cellSizeY = parentSize.y / (float) height;
        //
        //     var hafCellX = (float) cellSizeX / 2f;
        //     var hafCellY = (float) cellSizeY / 2f;
        //     for (int i = 0; i < width; i++)
        //     {
        //         for (int j = 0; j < height; j++)
        //         {
        //             var newCell = poolService.Get("EditorCell");
        //             newCell.gameObject.name = $" [{i}] ,[{j}]";
        //             newCell.SetActive(true);
        //             newCell.transform.SetParent(transform, false);
        //             RectTransform rectTransform = newCell.GetComponent<RectTransform>();
        //             rectTransform.sizeDelta = new Vector2(cellSizeX - margin / 2, cellSizeY - margin / 2);
        //             rectTransform.anchoredPosition =
        //                 new Vector2((i * cellSizeX) + hafCellX, (j * cellSizeY) + hafCellY);
        //             Grid[i, j] = newCell.GetComponent<ICellEditService>();
        //         }
        //     }
        // }

        private void CreateGrid()
        {
            gridManager.CreateGrid(width, height, margin);
        }

        private void UpdateGrid(CellLayout[,] grid)
        {
            gridManager.UpdateGrid(grid);
        }

        private void CLearLevel()
        {
            gridManager.ClearGrid();
        }

        private void UpdateBoard()
        {
            coroutineManager.UpdateBoard(currentLevel, gridManager.grid, width, height);
        }

        // private void UpdateGrid(CellLayout[,] grid)
        // {
        //     for (int i = 0; i < width; i++)
        //     {
        //         for (int j = 0; j < height; j++)
        //         {
        //             grid[i, j] = new CellLayout(Grid[i, j].CellLevelHeight, Grid[i, j].type);
        //             if (Grid[i, j].IsStart)
        //             {
        //                 StartPos = new Vector2Int(i, j);
        //             }
        //         }
        //     }
        // }
        // private void ClearlevelLayOutGrid()
        // {
        //     for (int i = 0; i < 8; i++)
        //     {
        //         for (int j = 0; j < 8; j++)
        //         {
        //             grid[i, j] = null;
        //         }
        //     }
        // }
        // private IEnumerator UpdateBoard()
        // {
        //     yield return StartCoroutine(ClearBoard());
        //     for (int i = 0; i < width; i++)
        //     {
        //         for (int j = 0; j < height; j++)
        //         {
        //             Grid[i, j].SetValue(currrentLevel.LevelLayout[i, j].cellHeight,
        //                 currrentLevel.LevelLayout[i, j].Type);
        //
        //             if (currrentLevel.startX == i && currrentLevel.startY == j)
        //             {
        //                 Grid[i, j].SetAsStart();
        //             }
        //
        //             yield return new WaitForSeconds(.05f);
        //         }
        //     }
        // }

        #endregion

        #region Collect Data

        private void HandleTogglesData()
        {
            commands.Clear();
            if (_toggles is null || _toggles.Count == 0)
            {
                Util.ShowMessage($"Toggles is Empty Or Null", TextColor.Yellow);
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
            // ClearlevelLayOutGrid();
            CLearLevel();
            UpdateGrid(grid);

            // Assuming you have a method to create a new Level object from the editor data
            Level newLevel = CreateLevelFromEditorData();

            // Add or update the level in the ScriptableObject
            UpdateLevelInScriptableObject(newLevel);

            // Inform Unity that the ScriptableObject has changed
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(levelData);
#endif
        }

        private void UpdateLevelInScriptableObject(Level level)
        {
            int index = FindLevelIndex(level.number);
            if (index >= 0)
            {
                // Update existing level
                levelData.levels[index] = level;
            }
            else
            {
                // Add new level
                List<Level> levelsList = levelData.levels.ToList();
                levelsList.Add(level);
                levelData.levels = levelsList.ToList();
            }
        }

        private Level CreateLevelFromEditorData()
        {
            Level newLevel = new Level();
            // Populate newLevel fields based on the editor data
            // Example: newLevel.width = width; (and so on for other fields)
            return newLevel;
        }

        private int FindLevelIndex(int levelNumber)
        {
            for (int i = 0; i < levelData.levels.Count; i++)
            {
                if (levelData.levels[i].number == levelNumber)
                {
                    return i;
                }
            }

            return -1; // Not found
        }


        public void SaveNewLevel()
        {
            HandleTogglesData();
            // ClearlevelLayOutGrid();
            CLearLevel();
            UpdateGrid(grid);
            var levelNumber = dataMangerService.LevelData.levels.Count > 0
                ? dataMangerService.LevelData.levels.Count + 1
                : 0;
            dataMangerService.AddLevel(new Level(levelNumber, commands,
                (PlayerDirection) _playerDirectionDropdown.value, grid, BufferSize.CurrentValue,
                P1Size.CurrentValue, P2Size.CurrentValue, StartPos.x, StartPos.y));
            PopulateLevelNameDropDown();
        }

        public void LoadLevel()
        {
            currentLevel = dataMangerService.LevelData.GetLevel(_levelNameDropDown.value);
            Util.ShowMessage($"We Are Loading Level Number ({_levelNameDropDown.value})");
            UpdateUi(currentLevel);
            UpdateBoard();
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
            _playerDirectionDropdown.value = (int) levelDirection;
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

        private void TurnOnTogglesByName(string name)
        {
            var toggles = _toggles.Where(t => t.Name == name).First().toggle.isOn = true;
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
            if (dataMangerService is null)
            {
                Debug.Log($"Data Manager is Null ");
                return;
            }

            if (dataMangerService.LevelData.levels == null)
            {
                Debug.Log($" Game Data Levels is Null ");
                return;
            }

            List<string> options = dataMangerService.LevelData.levels.Select(level => level.number.ToString()).ToList();

            _levelNameDropDown.ClearOptions();
            _levelNameDropDown.AddOptions(options);
            _levelNameDropDown.value = _levelNameDropDown.options.Count - 1;
        }
    }
}
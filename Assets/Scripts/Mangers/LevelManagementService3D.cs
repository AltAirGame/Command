using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;


namespace GameSystems.Core.Game
{
    

public class LevelManagementService3D : MonoBehaviour, ILevelManagmentService
{
    public static event Action CurrentLevelEnded;

    public Ease ease;
    public Level CurrentLevel { get; set; }
    public Vector3Int PlayerPos { get; set; }
    public Vector3Int PlayerForward { get; set; }

    private IPoolService poolService;


    public static event Action<List<ICommand>> AddAvilableCommand;
    public static event Action<int> AddBufferSize;
    public static event Action<int> AddP1Size;
    public static event Action<int> AddP2Size;
    [SerializeField] private bool DebugingIsOn = false;
    private List<GameObject> Cells;
    private GameCell[,] gameCells;
    private List<GameCell> currentLevelInteractable;
    private Vector3 playerStartPosition;
    public GameObject Player { get; set; }
    [SerializeField] private GameObject PlayerPrefab;

    public static ILevelManagmentService Instance;


    #region Unity

    private void Awake()
    {
        Instance = this;
        Cells = new List<GameObject>();
        currentLevelInteractable = new List<GameCell>();
    }

    private void Start()
    {
        poolService = ServiceLocator.Instance.GetService<IPoolService>();
    }

    #endregion

    public void CreatLevel(Level level, Action<GameObject> Playerreference)
    {
        currentLevelInteractable.Clear();

        if (gameCells is null)
        {
            gameCells = new GameCell[level.width, level.height];
        }

        CurrentLevel = level;

        StartCoroutine(CreateLevelWithDelay(level, () => { Playerreference?.Invoke(Player); }));
    }

    public void ResetLevel()
    {
        Player.transform.position = playerStartPosition;
        PlayerPos = new Vector3Int(CurrentLevel.startX,
            CurrentLevel.LevelLayout[CurrentLevel.startX][CurrentLevel.startY].cellHeight, 
            CurrentLevel.startY);
        PlayerForward = CurrentLevel.direction switch
        {
            PlayerDirection.Down => new Vector3Int(0, 0, 1),
            PlayerDirection.Up => new Vector3Int(0, 0, -1),
            PlayerDirection.Left => new Vector3Int(1, 0, 0),
            _ => new Vector3Int(-1, 0, 0),
        };
        Player.transform.rotation =
            Quaternion.LookRotation(new Vector3(PlayerForward.x, PlayerForward.y, PlayerForward.z));
        foreach (var item in currentLevelInteractable)
        {
            item.TurnOff();
        }
    }

    private IEnumerator CreateLevelWithDelay(Level level, Action onComplete)
    {
        yield return StartCoroutine(ClearLevelWithDelay());

        for (int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                var layout = level.LevelLayout[i][j];
                if (layout.cellHeight > 0)
                {
                    var cell = poolService.Get("Cell" + layout.cellHeight);
                    cell.gameObject.SetActive(true);
                    var cellType = layout.Type == CellType.Interactable
                        ? GameCellType.InteractableOff
                        : GameCellType.Simple;
                    SetupCell(cell, i, j, layout.cellHeight, cellType);

                    if (level.startX == i && level.startY == j)
                    {
                        // Setting up Player position and orientation
                        PlayerPos = new Vector3Int(i, layout.cellHeight, j);
                        PlayerForward = level.direction switch
                        {
                            PlayerDirection.Down => new Vector3Int(0, 0, 1),
                            PlayerDirection.Up => new Vector3Int(0, 0, -1),
                            PlayerDirection.Left => new Vector3Int(1, 0, 0),
                            _ => new Vector3Int(-1, 0, 0),
                        };
                        Player = poolService.Get("Player");
                        Player.gameObject.SetActive(true);
                        Player.transform.position = new Vector3(i, -10, j);
                        Player.transform.rotation = Quaternion.LookRotation(new Vector3(PlayerForward.x, PlayerForward.y, PlayerForward.z));
                        playerStartPosition = new Vector3(i, 1 + ((layout.cellHeight - 1) * .4f), j);
                        Player.transform.DOMove(playerStartPosition, .8f).SetEase(ease);
                        Cells.Add(Player);
                    }
                }

                yield return new WaitForSeconds(.03f);
            }
        }

        onComplete?.Invoke();
    }


    private IEnumerator ClearLevelWithDelay()
    {
        foreach (var item in Cells)
        {
            item.transform.DOMove(item.transform.position - (item.transform.up * 10), .8f).SetEase(ease).OnComplete(
                () =>
                {
                    item.transform.SetParent(poolService.GetGameObject().transform, false);
                    item.gameObject.SetActive(false);
                });
            yield return new WaitForSeconds(.02f);
        }

        Cells.Clear();
    }

    private void SetupCell(GameObject cell, int i, int j, int height, GameCellType cellType)
    {
        Cells.Add(cell);
        gameCells[i, j] = cell.GetComponent<GameCell>();
        gameCells[i, j].Setup(cellType);
        gameCells[i, j].SetupDebugerPart(new Vector2Int(i, j), height, DebugingIsOn);
        if (cellType is GameCellType.InteractableOff)
        {
            currentLevelInteractable.Add(gameCells[i, j]);
        }

        cell.transform.position = new Vector3(i, -10, j);
        cell.transform.DOMove(new Vector3(i, 0, j), .8f).SetEase(ease);
    }

    public bool CheckIfGameEnded()
    {
        foreach (var interactableCell in currentLevelInteractable)
        {
            if (interactableCell.type == GameCellType.InteractableOff)
            {
                Util.ShowMessage($" Game Not Ended", TextColor.Red);
                return false;
            }
        }

        Util.ShowMessage($" Game Is Ended", TextColor.Green);
        CurrentLevelEnded?.Invoke();
        return true;
    }

    public void Intereact()
    {
        var pos = GetPlayerPosition();
        //gameCells[(int)Player.transform.position.x, (int)Player.transform.position.z].Interact();
        gameCells[pos.x, pos.z].Interact();
        CheckIfGameEnded();
    }


    public Vector3Int GetFrontOfPlayerPosition()
    {
        var fp = GetPlayerPosition() + GetLocalForwardOfPlayer();
        return fp;
    }

    public Vector3Int GetPlayerPosition()
    {
        return PlayerPos;
    }

    //
    //there was a Problem with multiplying Vector.Forward in the Player Rotation So I Used This f
    public void Rotate(bool isRight)
    {
        Util.ShowMessage($" Rotate is Trigerd {(isRight ? " Rotating Toward Right" : " Rotating Toward Left")} ");
        if (isRight)
        {
            switch (PlayerForward.x)
            {
                case 0 when PlayerForward.z == 1:
                    PlayerForward = new Vector3Int(1, 0, 0);
                    break;
                case 0 when PlayerForward.z == -1:
                    PlayerForward = new Vector3Int(-1, 0, 0);
                    break;
                case 1 when PlayerForward.z == 0:
                    PlayerForward = new Vector3Int(0, 0, -1);
                    break;
                case -1 when PlayerForward.z == 0:
                    PlayerForward = new Vector3Int(0, 0, 1);
                    break;
            }
        }
        else
        {
            switch (PlayerForward.x)
            {
                case 0 when PlayerForward.z == 1:
                    PlayerForward = new Vector3Int(-1, 0, 0);
                    break;
                case 0 when PlayerForward.z == -1:
                    PlayerForward = new Vector3Int(1, 0, 0);
                    break;
                case 1 when PlayerForward.z == 0:
                    PlayerForward = new Vector3Int(0, 0, 1);
                    break;
                case -1 when PlayerForward.z == 0:
                    PlayerForward = new Vector3Int(0, 0, -1);
                    break;
            }
        }

        Player.transform.rotation =
            Quaternion.LookRotation(new Vector3(PlayerForward.x, PlayerForward.y, PlayerForward.z));
    }

    public Vector3Int GetLocalForwardOfPlayer()
    {
        return PlayerForward;
    }


    public int GetFrontOfPlayerHeight()
    {
        var frontPosition = GetFrontOfPlayerPosition();
        // Ensure that the indices are within the bounds of the list
        if (frontPosition.x < CurrentLevel.LevelLayout.Count &&
            frontPosition.z < CurrentLevel.LevelLayout[frontPosition.x].Count)
        {
            var cellLayout = CurrentLevel.LevelLayout[frontPosition.x][frontPosition.z];
            return cellLayout.cellHeight;
        }
        else
        {
            // Handle the case where the front position is out of bounds
            // This could be a return of a default value or handling an error
            return 0; // Example default value
        }
    }


    public int GetPlayerCurrentHeight()
    {
        //var position = GetPlayerPosition();
        //var ch = currentLevel.LevelLayout[position.x, position.z].cellHeight;

        return GetPlayerPosition().y;
    }


    public bool IsAvailable(ICommand command)
    {
        return command.Requirement(CurrentLevel.height, CurrentLevel.width, GetPlayerPosition(),
            GetLocalForwardOfPlayer(),
            GetPlayerCurrentHeight(), GetFrontOfPlayerHeight());
    }

    public void UpdatePlayer()
    {
        Player.transform.DOMove(new Vector3(PlayerPos.x, 1 + ((PlayerPos.y - 1) * .4f), PlayerPos.z), .2f);
    }
}
}
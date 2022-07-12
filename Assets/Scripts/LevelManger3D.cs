using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MHamidi;
using MHamidi.Helper;
using UnityEngine;


public interface ILevelManger
{
    public Level currentLevel { get; set; }
    void UpdateCellInteraction();
    public Vector3Int GetFrontOfPlayerPosition();
    public Vector3Int GetBackOfPlayerPosition();
    public int GetFrontOfPlayeHeight();
    public int GetPlayeCurrentHeight();
    public int GetBackofPlayerHeight();
    public void CreatLevel(Level level, Action<GameObject> setSubjectOfCommand);
    public void ResetLevel();
    public bool CheckIfGameEnded();
}


public class PlayeController
{
}

public class LevelManger3D : MonoBehaviour, ILevelManger
{

    public static event Action CurrentLevelEnded;
    
    public Ease ease;
    public Level currentLevel { get; set; }

    public static event Action<List<ICommand>> AddAvilableCommand;
    public static event Action<int> AddBufferSize;
    public static event Action<int> AddP1Size;
    public static event Action<int> AddP2Size;
    private List<GameObject> Cells;
    private GameCell[,] gameCells;
    private List<GameCell> currentLevelInteractable;
    public GameObject Player;
    public Quaternion PlayeInitalRotaion=new Quaternion(0,0,0,0);
    [SerializeField] private GameObject PlayerPrefab;
    public static ILevelManger Instance;

    private void Awake()
    {
        Instance = this;
        Cells = new List<GameObject>();
        currentLevelInteractable = new List<GameCell>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void CreatLevel(Level level, Action<GameObject> Playerreference)
    {
        currentLevelInteractable.Clear();

        if (gameCells is null)
        {
            gameCells = new GameCell[level.width, level.height];
        }

        currentLevel = level;

        StartCoroutine(CreatLevelWithDelay(level, () => { Playerreference?.Invoke(Player); }));
    }

    public void ResetLevel()
    {
        Player.transform.position = new Vector3(currentLevel.startX,1f+((currentLevel.LevelLayout[currentLevel.startX,currentLevel.startY].cellHeight-1)*.2f),currentLevel.startY);
        Player.transform.rotation = PlayeInitalRotaion;
       
        foreach (var item in currentLevelInteractable)
        {
            item.TurnOff();
        }
    }

    private IEnumerator CreatLevelWithDelay(Level level, Action OnComplet)
    {
        yield return StartCoroutine(ClearLevelWithDelay());

        for (int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                if (level.LevelLayout[i, j].cellHeight > 0)
                {
                    var cell = Pool.Instance.Get("Cell" + level.LevelLayout[i, j].cellHeight);
                    cell.gameObject.SetActive(true);
                    var cellType = level.LevelLayout[i, j].Type == CellType.Interactable
                        ? GameCellType.InteractableOff
                        : GameCellType.Simple;
                    SetupCell(cell, i, j, cellType);


                    if (level.startX == i && level.startY == j)
                    {
                        Player = Instantiate(PlayerPrefab, new Vector3(i, -10, j), PlayeInitalRotaion);
                        Player.transform.DOMove(new Vector3(i, 1, j), .8f).SetEase(ease);
                        Cells.Add(Player);
                    }
                }

                yield return new WaitForSeconds(.03f);
            }

            OnComplet?.Invoke();
        }

        yield return null;
    }

    private IEnumerator ClearLevelWithDelay()
    {
        foreach (var item in Cells)
        {
            item.transform.DOMove(item.transform.position - (item.transform.up * 10), .8f).SetEase(ease).OnComplete(
                () =>
                {
                    item.transform.SetParent(Pool.Instance.gameObject.transform, false);
                    item.gameObject.SetActive(false);
                });
            yield return new WaitForSeconds(.02f);
        }

        Cells.Clear();
    }

    private void SetupCell(GameObject cell, int i, int j, GameCellType cellType)
    {
        Cells.Add(cell);
        gameCells[i, j] = cell.GetComponent<GameCell>();
        gameCells[i, j].Setup(cellType);
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
                Util.ShowMessag($" Game Not Ended", TextColor.Red);
                return false;
            }
        }

        Util.ShowMessag($" Game Is Ended", TextColor.Green);
        CurrentLevelEnded?.Invoke();
        return true;
    }

    public void UpdateCellInteraction()
    {
        gameCells[(int)Player.transform.position.x, (int)Player.transform.position.z].Interact();
        CheckIfGameEnded();
    }


    public Vector3Int GetFrontOfPlayerPosition()
    {
        return Vector3Int.FloorToInt(Player.transform.position + Player.transform.forward);
    }

    public Vector3Int GetBackOfPlayerPosition()
    {
        return Vector3Int.FloorToInt(Player.transform.position - Player.transform.forward);
    }

    public int GetFrontOfPlayeHeight()
    {
        var front = GetFrontOfPlayerPosition();
        return currentLevel.LevelLayout[front.x, front.z].cellHeight;
    }

    public int GetBackofPlayerHeight()
    {
        var back = GetBackOfPlayerPosition();
        return currentLevel.LevelLayout[back.x, back.z].cellHeight;
    }

    public int GetPlayeCurrentHeight()
    {
        var position = Player.transform.position;
        return currentLevel.LevelLayout[(int)position.x, (int)position.z].cellHeight;
    }
}
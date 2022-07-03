using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MHamidi;
using MHamidi.Helper;
using UnityEngine;

public interface ILevelManger
{
    void Interact();
    void CreatLevel(Level level, Action<GameObject> Playerreference);
    bool MoveValidation(Vector3Int target);
    public void Jump();
    public void JumpBack();
}

public class LevelManger3D : MonoBehaviour, ILevelManger
{
    public Ease ease;
    private Level currentLevel;
    public static event Action<List<ICommand>> AddAvilableCommand;
    public static event Action<int> AddBufferSize;
    public static event Action<int> AddP1Size;
    public static event Action<int> AddP2Size;
    private List<GameObject> Cells;
    private GameCell[,] gameCells;
    public GameObject Player;
    [SerializeField] private GameObject PlayerPrefab;

    public static ILevelManger Instance;

    private void Awake()
    {
        Instance = this;
        Cells = new List<GameObject>();
    }

    private void OnEnable()
    {
        CommandManger.ResetPlayerPosition += ResetPlayer;
    }


    private void OnDisable()
    {
        CommandManger.ResetPlayerPosition -= ResetPlayer;
    }

    public void CreatLevel(Level level, Action<GameObject> Playerreference)
    {
        if (gameCells is null)
        {
            gameCells = new GameCell[level.width, level.height];
        }

        currentLevel = level;

        StartCoroutine(CreatLevelWithDelay(level, () => { Playerreference?.Invoke(Player); }));
    }

    private IEnumerator CreatLevelWithDelay(Level level, Action OnComplet)
    {
        yield return StartCoroutine(CleraLevelWithDelay());

        for (int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                if (level.LevelLayout[i, j] > 0)
                {
                    switch (level.LevelLayout[i, j])
                    {
                        case 1:
                            var cell = Pool.Instance.Get("Cell");
                            cell.gameObject.SetActive(true);

                            SetupCell(cell, i, j, GameCellType.Simple);

                            break;
                        case 2:
                            cell = Pool.Instance.Get("Cell");
                            cell.gameObject.SetActive(true);
                            SetupCell(cell, i, j, GameCellType.InteractableOff);
                            break;
                        case 3:
                            cell = Pool.Instance.Get("Cell2");
                            cell.gameObject.SetActive(true);
                            SetupCell(cell, i, j, GameCellType.Simple);

                            break;
                        case 4:
                            cell = Pool.Instance.Get("Cell2");

                            cell.gameObject.SetActive(true);
                            SetupCell(cell, i, j, GameCellType.InteractableOff);
                            break;
                        case 5:
                            cell = Pool.Instance.Get("Cell3");
                            cell.gameObject.SetActive(true);
                            SetupCell(cell, i, j, GameCellType.Simple);
                            break;

                        case 6:
                            cell = Pool.Instance.Get("Cell3");
                            cell.gameObject.SetActive(true);
                            SetupCell(cell, i, j, GameCellType.InteractableOff);

                            break;
                    }

                    if (level.startX == i && level.startY == j)
                    {
                        Player = Instantiate(PlayerPrefab, new Vector3(i, -10, j), Quaternion.identity);
                        Player.transform.DOMove(new Vector3(i, 1, j), .8f).SetEase(ease);
                        Cells.Add(Player);
                    }
                }

                yield return new WaitForSeconds(.03f);
            }

            OnComplet?.Invoke();
        }
    }

    private IEnumerator CleraLevelWithDelay()
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
        cell.transform.position = new Vector3(i, -10, j);
        cell.transform.DOMove(new Vector3(i, 0, j), .8f).SetEase(ease);
    }


    private void ResetPlayer()
    {
        Player.transform.position = new Vector3(currentLevel.startX, 0, currentLevel.startY);
    }


    public bool MoveValidation(Vector3Int targetPosition)
    {
        return NotEmpty(targetPosition);
        //
    }


    public void Jump()
    {
        var j = GetJump();
        StartCoroutine(JumpForwardAction(j));
    }

    public void JumpBack()
    {
        var j = GetJumpBack();
        StartCoroutine(JumpBackAction(j));
    }

    public int GetJump()
    {
        var JumpDirectionPosition = Vector3Int.FloorToInt(Player.transform.position - Player.transform.forward);
        var JumpDirectionValue = currentLevel.LevelLayout[JumpDirectionPosition.x, JumpDirectionPosition.z];
        var PlayePositionValue =
            currentLevel.LevelLayout[(int)Player.transform.position.x, (int)Player.transform.position.z];

        switch (PlayePositionValue)
        {
            //Can be Improved if We had More time
            case 1:
                //h=0
                if (JumpDirectionValue - PlayePositionValue == 2 || JumpDirectionValue - PlayePositionValue == 3)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                break;
            case 2:
                if (JumpDirectionValue - PlayePositionValue == 1 || JumpDirectionValue - PlayePositionValue == 2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                break;
            case 3:

                if (JumpDirectionValue - PlayePositionValue == -2 || JumpDirectionValue - PlayePositionValue == -1)
                {
                    return -1;
                }
                else if (JumpDirectionValue - PlayePositionValue == 2 || JumpDirectionValue - PlayePositionValue == 3)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                //h=1
                break;
            case 4:
                if (JumpDirectionValue - PlayePositionValue == -3 || JumpDirectionValue - PlayePositionValue == -2)
                {
                    return -1;
                }
                else if (JumpDirectionValue - PlayePositionValue == 1 || JumpDirectionValue - PlayePositionValue == 2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                //h=1
                break;
            case 5:
                if (JumpDirectionValue - PlayePositionValue == -2 || JumpDirectionValue - PlayePositionValue == -1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

                break;
            case 6:
                if (JumpDirectionValue - PlayePositionValue == -3 || JumpDirectionValue - PlayePositionValue == -2)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

                //h=2
                break;
        }

        return 0;
    }
     public int GetJumpBack()
    {
        var FrontOfPlayer = Vector3Int.FloorToInt(Player.transform.position + Player.transform.forward);
        var FronOfPlayerPosition = currentLevel.LevelLayout[FrontOfPlayer.x, FrontOfPlayer.z];
        var PlayerPositiion =
            currentLevel.LevelLayout[(int)Player.transform.position.x, (int)Player.transform.position.z];

        switch (PlayerPositiion)
        {
            //Can be Improved if We had More time
            case 1:
                //h=0
                if (FronOfPlayerPosition - PlayerPositiion == 2 || FronOfPlayerPosition - PlayerPositiion == 3)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                break;
            case 2:
                if (FronOfPlayerPosition - PlayerPositiion == 1 || FronOfPlayerPosition - PlayerPositiion == 2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                break;
            case 3:

                if (FronOfPlayerPosition - PlayerPositiion == -2 || FronOfPlayerPosition - PlayerPositiion == -1)
                {
                    return -1;
                }
                else if (FronOfPlayerPosition - PlayerPositiion == 2 || FronOfPlayerPosition - PlayerPositiion == 3)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                //h=1
                break;
            case 4:
                if (FronOfPlayerPosition - PlayerPositiion == -3 || FronOfPlayerPosition - PlayerPositiion == -2)
                {
                    return -1;
                }
                else if (FronOfPlayerPosition - PlayerPositiion == 1 || FronOfPlayerPosition - PlayerPositiion == 2)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

                //h=1
                break;
            case 5:
                if (FronOfPlayerPosition - PlayerPositiion == -2 || FronOfPlayerPosition - PlayerPositiion == -1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

                break;
            case 6:
                if (FronOfPlayerPosition - PlayerPositiion == -3 || FronOfPlayerPosition - PlayerPositiion == -2)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }

                //h=2
                break;
        }

        return 0;
    }


    public void Interact()
    {
        if (currentLevel.LevelLayout[(int)Player.transform.position.x, (int)Player.transform.position.z] % 2 == 0)
        {
            gameCells[(int)Player.transform.position.x, (int)Player.transform.position.z].Interact();
        }
    }

    public IEnumerator JumpForwardAction(int target)
    {
        float t = 0;

        Vector3 p0 = Vector3Int.FloorToInt(Player.transform.position);
        Vector3 p1 = Vector3Int.FloorToInt( new Vector3(p0.x, p0.y + 1, p0.z));
        Vector3 p2 = Vector3Int.FloorToInt( new Vector3(p0.x + 1, p0.y +1, p0.z));
        Vector3 p3 = p0;

        switch (target)
        {
            case 0:
                p2 = p1;
                p3 = p0;
                break;
            case 1:

                p3 = new Vector3(p0.x, p0.y + .15f, p0.z - 1);
                break;
            case -1:
                p3 = new Vector3(p0.x, p0.y - .15f, p0.z- 1);
                break;
        }

        while (Vector3.Distance(Player.transform.position, p3) > 0)
        {
            Player.transform.position = CalculateCubicBezierCurve(t, p0, p1, p2, p3);
            t += Time.deltaTime*3; // we Can Add ease Here
            if (t > .99f)
            {
                t = 1;
            }

            yield return null;
        }

        Util.ShowMessag($"jump Ended");
    }
    public IEnumerator JumpBackAction(int target)
    {
        float t = 0;

        var p0 = Player.transform.position;
        var p1 = new Vector3(p0.x, p0.y + 1, p0.z);
        var p2 = new Vector3(p0.x + 1, p0.y +1, p0.z);
        var p3 = p0;

        switch (target)
        {
            case 0:
                p2 = p1;
                p3 = p0;
                break;
            case 1:

                p3 = new Vector3(p0.x, p0.y + .15f, p0.z +1);
                break;
            case -1:
                p3 = new Vector3(p0.x, p0.y - .15f, p0.z+ 1);
                break;
        }

        while (Vector3.Distance(Player.transform.position, p3) > 0)
        {
            Player.transform.position = CalculateCubicBezierCurve(t, p0, p1, p2, p3);
            t += Time.deltaTime*3; // we Can Add ease Here
            if (t > .99f)
            {
                t = 1;
            }

            yield return null;
        }

        Util.ShowMessag($"jump Ended");
    }

    private Vector3 CalculateCubicBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // (1-t) p0 + 3(1-t)^2 t*p1 +3(1-t)t*p2+t^3 *p3 

        var u = 1 - t;
        var uu = u * u;
        var tt = t * t;
        var uuu = uu * u;
        var ttt = tt * t;

        var p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
        var c = p0 + t * (p1 - p0);
        return c;
    }

    private bool NotEmpty(Vector3Int targetPosition)
    {
        if (currentLevel.LevelLayout[targetPosition.x, targetPosition.z] == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
using System;
using UnityEngine;

public interface ILevelManger
{
    public GameObject Player { get; set; }
    public Level currentLevel { get; set; }
    public Vector3Int playerPos { get; set; }
    public Vector3Int playerForward { get; set; }
    void Intereact();
    public Vector3Int GetFrontOfPlayerPosition();

    public int GetFrontOfPlayerHeight();
    public int GetPlayerCurrentHeight();

    public void CreatLevel(Level level, Action<GameObject> setSubjectOfCommand);
    public void ResetLevel();
    public bool CheckIfGameEnded();
    bool IsAvailable(ICommand command);
    public void Rotate(bool isRight);
    public void UpdatePlayer();
}
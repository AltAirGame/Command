using System;
using System.Collections;
using UnityEngine;


public interface ICommand
{
    public string Name { get; set; }
    public GameObject SubjectOfCommands { get; set; }
    public IEnumerator Execute(GameObject subject);
    public IEnumerator  Undo(GameObject subject);

    public bool Requirement(int height, int width, Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
        int forwardHeight);

    public bool ExecutionInstruction(GameObject subjectOfCommand,Vector3Int playerPosition, Vector3Int playerForward, int playerHeight,
        int forwardHeight);
}
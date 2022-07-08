using System;
using System.Collections;
using UnityEngine;


public interface ICommand
{
    public string name { get; set; }
    public GameObject SubjectOfCommands { get; set; }
    public bool executeWasSuccessful { get; set; }
    IEnumerator Execute(GameObject subject);
    IEnumerator  Undo(GameObject subject);
}
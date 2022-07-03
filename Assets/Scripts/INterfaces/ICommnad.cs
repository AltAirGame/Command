using System;
using UnityEngine;


public interface ICommand
{
    public string name { get; set; }
    public bool executeWaseSuccesful { get; set; }
    void Execute(GameObject subject);
    void Undo(GameObject subject);
}
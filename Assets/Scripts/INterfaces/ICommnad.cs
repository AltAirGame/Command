using System;

public interface ICommand
{
    public string name { get; set; }
  
    void Execute();
    void Undo();
}
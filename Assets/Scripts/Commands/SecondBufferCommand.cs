﻿using System.Collections;
using UnityEngine;

namespace MHamidi
{
    public class SecondBufferCommand : ICommand
    {
       
        public string name {   get { return this.GetType().Name.ToLower(); }
            set { }}

        public SecondBufferCommand()
        {
           
        }

        public GameObject SubjectOfCommands { get; set; }

        public bool Done { get; set; }
        public bool executeWasSuccessful { get; set; }

        public IEnumerator Execute(GameObject subject)
        {
            throw new System.NotImplementedException();
            yield return null;
        }

        public IEnumerator Undo(GameObject subject)
        {
            throw new System.NotImplementedException();
            yield return null;
        }

        public void Execute()
        {
           
        }

        public void Undo()
        {
            
        }
    }
}
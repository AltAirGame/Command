using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Core
{
   public class LevelEditorToggle : MonoBehaviour
   {
      [SerializeField] private Toggle _toggle;
      public string Name;

      public Toggle toggle
      {
         get { return _toggle; }
      }

      private void Start()
      {
         _toggle = GetComponent<Toggle>();
      }

      public void SetNameOfToggle(string itemName)
      {
         Name = itemName;
         GetComponent<ITextRename>().RenameTOText(itemName);
      }
   }
}
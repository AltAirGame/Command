using System;
using TMPro;

namespace GameSystems.Core
{
    public interface ITextRename
    {

        public TextMeshProUGUI text { get; set; }
        public void RenameTOText(String name);

    }
}
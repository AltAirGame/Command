using System;
using TMPro;

public interface ITextRename
{

    public TextMeshProUGUI text { get; set; }
    public void RenameTOText(String name);

}
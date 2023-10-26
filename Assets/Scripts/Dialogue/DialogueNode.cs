using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string UniqueID;
    public string Text;
    public string[] Children;
    public Rect position;
}

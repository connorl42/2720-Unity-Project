using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
 [SerializeField] private List<DialogueNode> _nodes = new List<DialogueNode>();
 
 #if UNITY_EDITOR
 void Awake()
    {
     if (_nodes.Count == 0)
     {
      _nodes.Add((new DialogueNode()));
     }
    }
 #endif

 public IEnumerable<DialogueNode> GetAllNodes()
 {
  return _nodes;
 }
}

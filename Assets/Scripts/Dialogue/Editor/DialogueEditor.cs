using System;
using UniGLTF.Extensions.VRMC_node_constraint;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRMShaders;

public class DialogueEditor : EditorWindow
{
   private Dialogue _selectedDialogue = null;
   private GUIStyle _nodeStyle;
   private GUIStyle _playerStyle;
   private DialogueNode _draggingNode = null;
   private DialogueNode _creatingNode = null;
   private DialogueNode _deletingNode = null;
   private DialogueNode _linkingNode = null;
   private Vector2 _draggingOffset;
   private Vector2 _scrollPosition;
   private bool _draggingCanvas = false;
   private Vector2 _draggingCanvasOffset;
   
   [MenuItem("Window/Dialogue Editor")]
   public static void ShowEditorWindow()
   {
      GetWindow(typeof(DialogueEditor), false, "DialogueEditor");
   }

   [OnOpenAsset(1)]
   public static bool OpenDialogue(int instanceID, int line)
   {
      Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
      if (dialogue != null)
      {
         ShowEditorWindow();
         return true;
      }
      return false;
   }

   private void OnEnable()
   {
      Selection.selectionChanged += OnSelectionChanged;
      _nodeStyle = new GUIStyle();
      _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
      _nodeStyle.normal.textColor = Color.white;
      _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
      _nodeStyle.border = new RectOffset(12, 12, 12, 12);
      
      _playerStyle = new GUIStyle();
      _playerStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
      _playerStyle.normal.textColor = Color.white;
      _playerStyle.padding = new RectOffset(20, 20, 20, 20);
      _playerStyle.border = new RectOffset(12, 12, 12, 12);
   }

   private void OnSelectionChanged()
   {
      Dialogue newDialogue = Selection.activeObject as Dialogue;
      if (newDialogue != null)
      {
         _selectedDialogue = newDialogue;
         Repaint();
      }
   }

   void OnGUI()
   {
      if (_selectedDialogue == null)
      {
         EditorGUILayout.LabelField("No dialogue selected");
      }
      else
      {
         ProcessEvents();

         _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

         GUILayoutUtility.GetRect(4000, 4000);
         foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
         {
            DrawConnections(node);
         }
         foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
         {
            DrawNode(node);
         }
         
         EditorGUILayout.EndScrollView();

         if (_creatingNode != null)
         {
            _selectedDialogue.CreateNode(_creatingNode);
            _creatingNode = null;
         }
         
         if (_deletingNode != null)
         {
            _selectedDialogue.DeleteNode(_deletingNode);
            _deletingNode = null;
         }
      }
   }

   void ProcessEvents()
   {
      if (Event.current.type == EventType.MouseDown && _draggingNode == null)
      {
         _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
         if (_draggingNode != null)
         {
            _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
            Selection.activeObject = _draggingNode;
         }
         else
         {
            _draggingCanvas = true;
            _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
            Selection.activeObject = _selectedDialogue;
         }
      }
      else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
      {
         Undo.RecordObject(_selectedDialogue, "Move Dialogue Node");
         _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
         GUI.changed = true;
      }
      else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
      {
         _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
         GUI.changed = true;
      }
      else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
      {
         _draggingNode = null;
      }
      else if (Event.current.type == EventType.MouseUp && _draggingCanvas)
      {
         _draggingCanvas = false;
      }
   }

   private DialogueNode GetNodeAtPoint(Vector2 point)
   {
      DialogueNode foundNode = null;
      foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
      {
         if (node.GetRect().Contains(point))
         {
            foundNode = node;
         }
      }

      return foundNode;
   }

   private void DrawNode(DialogueNode node)
   {
      GUIStyle style = _nodeStyle;
      if (node.IsPlayer())
      {
         style = _playerStyle;
      }
      
      GUILayout.BeginArea(node.GetRect(), style);
      
      node.SetText(EditorGUILayout.TextField(node.GetText()));
      
      
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("+"))
      {
         _creatingNode = node;
      }

      DrawLinkButtons(node);
      if (GUILayout.Button("-"))
      {
         _deletingNode = node;
      }
      GUILayout.EndHorizontal();
      
      GUILayout.EndArea();
   }

   private void DrawLinkButtons(DialogueNode node)
   {
      if (_linkingNode == null)
      {
         if (GUILayout.Button("Link"))
         {
            _linkingNode = node;
         }
      }
      else if (_linkingNode == node)
      {
         if (GUILayout.Button("Cancel"))
         {
            _linkingNode = null;
         }
      }
      else if (_linkingNode.GetChildren().Contains(node.name))
      {
         if (GUILayout.Button("Unlink"))
         {
            Undo.RecordObject(_selectedDialogue, "Remove Dialogue Link");
            _linkingNode.RemoveChild(node.name);
            _linkingNode = null;
         }
      }
      else
      {
         if (GUILayout.Button("Child"))
         {
            Undo.RecordObject(_selectedDialogue, "Add Dialogue Link");
            _linkingNode.AddChild(node.name);
            _linkingNode = null;
         }
      }
   }

   void DrawConnections(DialogueNode node)
   {
      Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
      foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
      {
         Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
         Vector3 controlPointOffset = endPosition - startPosition;
         controlPointOffset.y = 0;
         controlPointOffset.x *= 0.8f;
         Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset
         , Color.white, null, 4f);
      }
   }
}


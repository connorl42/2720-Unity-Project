using System;
using UniGLTF.Extensions.VRMC_node_constraint;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


public class DialogueEditor : EditorWindow
{
   private Dialogue _selectedDialogue = null;
   private GUIStyle _nodeStyle;
   
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
         
         foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
         {
            OnGUINode(node);
         }
      }
   }

   private void OnGUINode(DialogueNode node)
   {
      GUILayout.BeginArea(node.position, _nodeStyle);
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.LabelField("Node:");
      string newText = EditorGUILayout.TextField(node.Text);
      string newUniqueID = EditorGUILayout.TextField(node.UniqueID);
      if (EditorGUI.EndChangeCheck())
      {
         Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");
         node.Text = newText;
         node.UniqueID = newUniqueID;
      }
      GUILayout.EndArea();
   }
}


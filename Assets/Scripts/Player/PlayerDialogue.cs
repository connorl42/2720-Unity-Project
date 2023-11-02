using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private string _name;
    private PlayerInputSystemController _playerInputController;
    Dialogue _currentDialogue;
    private DialogueNode _currentNode = null;
    private bool _choosing = false;
    private AIDialogue _currentAI = null;

    public event Action onConversationUpdated;

    private void Start()
    {
        _playerInputController = GetComponent<PlayerInputSystemController>();
    }

    public void StartDialogue(Dialogue newDialogue, AIDialogue newAIDialogue)
    {
        _currentDialogue = newDialogue;
        _currentAI = newAIDialogue;
        _currentNode = _currentDialogue.GetRootNode();
        onConversationUpdated();
        _playerInputController.Pause();

    }

    public bool IsActive()
    {
        return _currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return _choosing;
    }
    public string GetText()
    {
        if (_currentDialogue == null)
        {
            return "";
        }

        return _currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return _currentDialogue.GetPlayerChildren(_currentNode);
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        _currentNode = chosenNode;
        _choosing = false;
        Next();
    }

    public string GetCurrentName()
    {
        if (IsChoosing())
        {
            return _name;
        }
        else
        {
            return _currentAI.GetName();
        }
    }

    public void Next()
    {
        int numPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();

        if (numPlayerResponses > 0)
        {
            _choosing = true;
            onConversationUpdated();
            return;
        }

        DialogueNode[] children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
        int randomIndex = Random.Range(0, children.Length); //makes NPC response random
        _currentNode = children[randomIndex];
        onConversationUpdated();
    }

    public bool HasNext()
    {
        return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
    }

    public void Quit()
    {
        _currentDialogue = null;
        _currentNode = null;
        _currentAI = null;
        _choosing = false;
        onConversationUpdated();
        _playerInputController.Unpause();
    }
    
}

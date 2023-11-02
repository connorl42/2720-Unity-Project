using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _AItext;
    [SerializeField] private Button _nextButton;
    [SerializeField] private GameObject _AIresponse;
    [SerializeField] private Transform _choiceRoot;
    [SerializeField] private GameObject _choicePrefab;
    [SerializeField] private Button _quitButton;
    private PlayerDialogue _playerDialogue;
    void Start()
    {
        _playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();
        _playerDialogue.onConversationUpdated += UpdateUI;
        _nextButton.onClick.AddListener(() => _playerDialogue.Next());
        _quitButton.onClick.AddListener(() => _playerDialogue.Quit());
        UpdateUI();

    }
    

    private void UpdateUI()
    {
        gameObject.SetActive(_playerDialogue.IsActive());
        if (!_playerDialogue.IsActive())
        {
            return;
        }

        _nameText.text = _playerDialogue.GetCurrentName();
        _AIresponse.SetActive(!_playerDialogue.IsChoosing());
        _choiceRoot.gameObject.SetActive(_playerDialogue.IsChoosing());
        if (_playerDialogue.IsChoosing())
        {
            BuildChoiceList();
        }
        else
        {
            _AItext.text = _playerDialogue.GetText();
            _nextButton.gameObject.SetActive(_playerDialogue.HasNext());
        }
    }

    private void BuildChoiceList()
    {
        _choiceRoot.DetachChildren();
        foreach (DialogueNode choice in _playerDialogue.GetChoices())
        {
            GameObject choiceInstance = Instantiate(_choicePrefab, _choiceRoot);
            var textComponent = choiceInstance.GetComponentInChildren<TMP_Text>();
            textComponent.text = choice.GetText();
            Button button = choiceInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                _playerDialogue.SelectChoice(choice);
            });
        }
    }
}

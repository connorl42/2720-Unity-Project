using UniGLTF;
using UnityEngine;

public class AIDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue = null;
    [SerializeField] private string _name;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartDialogue()
    {
        if (_dialogue == null)
            return;
        _player.GetComponent<PlayerDialogue>().StartDialogue(_dialogue, this);
    }

    public string GetName()
    {
        return _name;
    }
    
}

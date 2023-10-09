using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public AudioSource _BGMAudioSource;
    public AudioClip[] audioClipArray;

    private GameObject _playerObj;
    private Vector3 _playerPosition;

    private void Start()
    {
        _playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        
    }

    void DetermineLocation()
    {
        
    }
}

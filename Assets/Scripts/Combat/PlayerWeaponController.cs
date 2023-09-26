using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _weaponTriggerVolumes = new List<GameObject>();

    private Animator _animator;

    private int _currentIndex = 0;


    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        var animatorEvents = _animator.gameObject.GetComponent<RPGCharacterAnimatorEvents>();
        animatorEvents.OnWeaponSwitch.AddListener(WeaponSwitch);

        for (int i = 1; i < _weaponTriggerVolumes.Count; i++)
        {
            _weaponTriggerVolumes[i].SetActive(false);
        }

    }

    private void WeaponSwitch()
    {
        _weaponTriggerVolumes[_currentIndex].SetActive(false);
        if (_currentIndex == _weaponTriggerVolumes.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }

        _weaponTriggerVolumes[_currentIndex].SetActive(true);
    }
}

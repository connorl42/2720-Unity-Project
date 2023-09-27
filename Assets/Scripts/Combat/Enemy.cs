using System;
using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims;
using RPGCharacterAnims.Lookups;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(RPGCharacterController))]
[RequireComponent(typeof(RPGCharacterNavigationController))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 15f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _rotationSpeed = 1f;

    private RPGCharacterController _rpgCharacterController;
    private RPGCharacterNavigationController _rpgNavigationController;
    private NavMeshAgent _navMeshAgent;
    private GameObject _player;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;
    private bool _aggro = false;

    private void Awake()
    {
        _rpgCharacterController = GetComponent<RPGCharacterController>();
        _rpgNavigationController = GetComponent<RPGCharacterNavigationController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rpgCharacterController.target = _player.transform;
        _originalPosition = transform.position;
    }

   
    void Update()
    {
        if (InDetectionRange())
        {
            _aggro = true;
            _targetPosition = _rpgCharacterController.target.transform.position;
            if (!InAttackRange())
            {
                _rpgCharacterController.StartAction(HandlerTypes.Navigation, _targetPosition);
            }
            else
            {
                _rpgNavigationController.StopNavigating();
                _rpgNavigationController.StopAnimation();
                RotateTowardsTarget();
                
            }
        }
        else if (_aggro)
        {
            _aggro = false;
            Reset();
        }
    }

    bool InDetectionRange()
    {
        float playerDistanceSqr = (_player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= _detectionRange * _detectionRange;
    }

    bool InAttackRange()
    {
        float playerDistanceSqr = (_player.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= _attackRange * _attackRange;
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
            Time.deltaTime * _navMeshAgent.angularSpeed * _rotationSpeed);
    }

    public void Reset()
    {
        _rpgCharacterController.StartAction(HandlerTypes.Navigation, _originalPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}

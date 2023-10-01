using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPGCharacterAnims;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField] private RPGCharacterNavigationController _rpgNavigationController;
    [SerializeField] private float _waitTime = 3f;
    [SerializeField] private float _gizmoRadius = 0.3f;

     List<Transform> _waypoints = new List<Transform>();
     private Vector3 _targetPosition;
     private int _currentIndex = 0;
     private bool _waiting = false;

     private void Awake()
     {
         _waypoints = GetComponentsInChildren<Transform>().Skip(1).ToList();
     }

   
    void Start()
    {
        if (_waypoints.Count > 0)
        {
            _targetPosition = _waypoints[_currentIndex].position;
            _rpgNavigationController.MeshNavToPoint(_targetPosition);
        }
    }
    
    void Update()
    {
        if (_waypoints.Count == 0) return;

        Patrol();
    }

     void Patrol()
    {
        if (!_rpgNavigationController.navMeshAgent.hasPath && !_waiting)
        {
            StartCoroutine(PausePatrol());
        }
    }

     IEnumerator PausePatrol()
     {
         _targetPosition = NextWayPoint();
         _waiting = true;
         yield return new WaitForSeconds(_waitTime);
         _rpgNavigationController.MeshNavToPoint(_targetPosition);
         _waiting = false;
     }

    Vector3 NextWayPoint()
    {
        _currentIndex = GetNextIndex(_currentIndex);
        return _waypoints[_currentIndex].position;
    }

    private int GetNextIndex(int i)
    {
        if (i + 1 == _waypoints.Count)
            return 0;
        return i + 1;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _waypoints.Count; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_waypoints[i].position, _gizmoRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(_waypoints[i].position, _waypoints[j].position);
        }
    }
}

using System;
using System.Collections.Generic;
using Cinemachine;
using Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private CinemachineVirtualCamera _targetingCamera;
    [SerializeField] private GameObject _target;


    public Target CurrentTarget { get; private set; }
    
    List<Target> _targets = new List<Target>();
    
    private Camera _mainCamera;
    private PlayerInputSystemController _playerInputController;
    
    
    private bool _activeCursor = false;
    
    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputSystemController>();
        
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        

    }

    private void Update()
    {

        if (_activeCursor)
        {
            /*//move crosshair to target
            GameObject _crossHair = GameObject.FindGameObjectWithTag("Crosshair"); //finds and labels crosshair component
            _crossHair.transform.position = CurrentTarget.transform.position; */
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        
        _targets.Add(target);
        target.OnDestroyed += RemoveTarget;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        
        RemoveTarget(target);
        
    }

    void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            _targetingCamera.Priority = 9;
            _targetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
            _playerInputController.CancelInputAim();
        }

        /*_activeCursor = false;
        GameObject _crossHair = GameObject.FindGameObjectWithTag("Crosshair"); //finds and labels crosshair component
        _crossHair.transform.position = new Vector3(1000, 1, 1000);*/




        target.OnDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) return false;

        Target closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Target target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestDistance)
            {
                closestTarget = target;
                closestDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
        _targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        _targetingCamera.Priority = 11;

        _activeCursor = true;
        GameObject _crossHair = GameObject.FindGameObjectWithTag("Crosshair"); //finds and labels crosshair component
        _crossHair.transform.position = CurrentTarget.transform.position;
       

       return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;

        _targetingCamera.Priority = 9;
        _targetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

   
}

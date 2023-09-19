using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private CinemachineVirtualCamera _targetingCamera;
    List<Target> _targets = new List<Target>();
    
    public Target CurrentTarget { get; private set; }

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
        }

        target.OnDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (_targets.Count == 0) return false;
        
        CurrentTarget = _targets[0];
        _targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        _targetingCamera.Priority = 11;

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

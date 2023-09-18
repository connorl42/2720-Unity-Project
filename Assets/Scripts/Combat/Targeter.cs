using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> _targets = new List<Target>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        
        _targets.Add(target);
       
       /*if (other.GetComponent<Target>())
           _targets.Add(other.GetComponent<Target>())*/;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        
        _targets.Remove(target);
    }

    public bool HasTarget()
    {
        if (_targets.Count == 0)
            return false;
        return true;
    }

    public Target GetTarget()
    {
        return _targets[0];
    }
}

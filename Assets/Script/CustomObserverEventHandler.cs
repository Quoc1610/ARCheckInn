using System;
using UnityEngine;
using UnityEngine.Serialization;
using Vuforia;


public class CustomObserverEventHandler : DefaultObserverEventHandler
{ 
    public event Action<bool> OnTargetTrackingStatusChanged;
    protected override void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
            
        bool isTracked = targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED;
        
        OnTargetTrackingStatusChanged?.Invoke(isTracked);

        base.OnObserverStatusChanged(behaviour, targetStatus);
    }
        
}

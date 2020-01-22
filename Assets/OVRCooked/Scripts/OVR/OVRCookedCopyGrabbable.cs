using System;
using UnityEngine;

/// <summary>
/// An object that can be grabbed and thrown by OVRGrabber.
/// </summary>
public class OVRCookedCopyGrabbable : OVRCookedGrabbable
{
    [SerializeField]
    OVRCookedGrabbable grabbablePrefab;

    public override OVRCookedGrabbable GetGrabbable()
    {
        OVRCookedGrabbable copiedGrabbable = GameObject.Instantiate(grabbablePrefab, transform.position, Quaternion.identity);
        copiedGrabbable.GrabbedKinematic = grabbablePrefab.GetComponent<Rigidbody>().isKinematic;
        Debug.Log($"Created new copied grabbable. Kinematic: {copiedGrabbable.GrabbedKinematic}");
        return copiedGrabbable;
    }

    protected override void Start() 
    {
        base.Start();
    }

}

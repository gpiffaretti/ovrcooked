using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCookedGrabbable : OVRGrabbable
{

    [SerializeField]
    bool fixedPosition;

    public bool FixedPosition { get { return fixedPosition; } }

    protected override void Start()
    {
        // need empty method so that kinematic variable is not set automatically
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
        base.GrabBegin(hand, grabPoint);
    }

    public virtual OVRCookedGrabbable GetGrabbable()
    {
        return this;
    }

    public bool GrabbedKinematic
    {
        get { return m_grabbedKinematic; }
        set { m_grabbedKinematic = value; }
    }
}

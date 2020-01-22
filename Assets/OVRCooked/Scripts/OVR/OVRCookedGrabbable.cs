using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCookedGrabbable : OVRGrabbable
{

    protected override void Start()
    {
        // need empty method so that kinematic variable is not set automatically
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

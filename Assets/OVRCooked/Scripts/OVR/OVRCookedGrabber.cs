using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OVRCookedGrabber : OVRGrabber
{

    OVRCookedGrabbable m_grabbedOVRCookedObj;
    AudioSource audioSource;

    [SerializeField]
    AudioClip pickUpSound;

    protected override void Start() 
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void GrabBegin()
    {
        float closestMagSq = float.MaxValue;
        OVRCookedGrabbable closestGrabbable = null;
        Collider closestGrabbableCollider = null;

        // clean grab candidates in case it has missing references
        var itemsToRemove = m_grabCandidates.Where(f => f.Key == null).ToArray();
        foreach (var item in itemsToRemove)
            m_grabCandidates.Remove(item.Key);

        // Iterate grab candidates and find the closest grabbable candidate
        foreach (OVRGrabbable grabbable in m_grabCandidates.Keys)
        {
            if (grabbable == null) continue;
            bool canGrab = !(grabbable.isGrabbed && !grabbable.allowOffhandGrab);
            if (!canGrab)
            {
                continue;
            }

            for (int j = 0; j < grabbable.grabPoints.Length; ++j)
            {
                Collider grabbableCollider = grabbable.grabPoints[j];
                // Store the closest grabbable
                Vector3 closestPointOnBounds = Vector3.zero;
                closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                
                float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq)
                {
                    closestMagSq = grabbableMagSq;
                    closestGrabbable = grabbable as OVRCookedGrabbable;
                    closestGrabbableCollider = grabbableCollider;
                }
            }
        }

        // Disable grab volumes to prevent overlaps
        GrabVolumeEnable(false);

        if (closestGrabbable != null)
        {
            if (closestGrabbable.isGrabbed)
            {
                (closestGrabbable.grabbedBy as OVRCookedGrabber).OffhandGrabbed(closestGrabbable);
            }

            m_grabbedObj = closestGrabbable.GetGrabbable();
            m_grabbedOVRCookedObj = m_grabbedObj as OVRCookedGrabbable;

            // get closest grabbableCollider for this Grabbable 
            // (we can get an instance of another grabbable because of OVRCopyGrabbable)
            closestMagSq = float.MaxValue;
            for (int j = 0; j < m_grabbedObj.grabPoints.Length; ++j)
            {
                Collider grabbableCollider = m_grabbedObj.grabPoints[j];
                // Store the closest grabbable
                Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq)
                {
                    closestGrabbableCollider = grabbableCollider;
                }
            }
            m_grabbedObj.GrabBegin(this, closestGrabbableCollider);

            m_lastPos = transform.position;
            m_lastRot = transform.rotation;

            // Set up offsets for grabbed object desired position relative to hand.
            if (m_grabbedObj.snapPosition)
            {
                m_grabbedObjectPosOff = m_gripTransform.localPosition;
                if (m_grabbedObj.snapOffset)
                {
                    Vector3 snapOffset = m_grabbedObj.snapOffset.position;
                    if (m_controller == OVRInput.Controller.LTouch) snapOffset.x = -snapOffset.x;
                    m_grabbedObjectPosOff += snapOffset;
                }
            }
            else
            {
                Vector3 relPos = m_grabbedObj.transform.position - transform.position;
                relPos = Quaternion.Inverse(transform.rotation) * relPos;
                m_grabbedObjectPosOff = relPos;
            }

            if (m_grabbedObj.snapOrientation)
            {
                m_grabbedObjectRotOff = m_gripTransform.localRotation;
                if (m_grabbedObj.snapOffset)
                {
                    m_grabbedObjectRotOff = m_grabbedObj.snapOffset.rotation * m_grabbedObjectRotOff;
                }
            }
            else
            {
                Quaternion relOri = Quaternion.Inverse(transform.rotation) * m_grabbedObj.transform.rotation;
                m_grabbedObjectRotOff = relOri;
            }

            // Note: force teleport on grab, to avoid high-speed travel to dest which hits a lot of other objects at high
            // speed and sends them flying. The grabbed object may still teleport inside of other objects, but fixing that
            // is beyond the scope of this demo.
            MoveGrabbedObject(m_lastPos, m_lastRot, true);
            SetPlayerIgnoreCollision(m_grabbedObj.gameObject, true);
            if (m_parentHeldObject)
            {
                m_grabbedObj.transform.parent = transform;
            }

            HapticsHelper.Vibration(
                this, 
                m_controller, 
                HapticsHelper.Duration.Short, 
                HapticsHelper.Frequency.Medium, 
                HapticsHelper.Amplitude.Low);

            if (m_grabbedOVRCookedObj.PlayPickUpSound) 
            {
                audioSource.PlayOneShot(pickUpSound);
            }
        }
    }

    protected override void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_grabbedObj == null)
        {
            return;
        }

        Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
        Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
        Quaternion grabbableRotation = rot * m_grabbedObjectRotOff;

        // account for rigidbody rotation freezes
        Vector3 newRotationEuler = grabbableRotation.eulerAngles;
        Vector3 originalRotationEuler = grabbedRigidbody.transform.rotation.eulerAngles;
        RigidbodyConstraints constraints = grabbedRigidbody.constraints;
        grabbableRotation = Quaternion.Euler(
            constraints.HasFlag(RigidbodyConstraints.FreezeRotationX) ? originalRotationEuler.x : newRotationEuler.x,
            constraints.HasFlag(RigidbodyConstraints.FreezeRotationY) ? originalRotationEuler.y : newRotationEuler.y,
            constraints.HasFlag(RigidbodyConstraints.FreezeRotationZ) ? originalRotationEuler.z : newRotationEuler.z);
        
        if (forceTeleport)
        {
            if(!m_grabbedOVRCookedObj.FixedPosition) grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
        }
        else
        {
            if (!m_grabbedOVRCookedObj.FixedPosition) grabbedRigidbody.MovePosition(grabbablePosition);
            grabbedRigidbody.MoveRotation(grabbableRotation);
        }
    }

}

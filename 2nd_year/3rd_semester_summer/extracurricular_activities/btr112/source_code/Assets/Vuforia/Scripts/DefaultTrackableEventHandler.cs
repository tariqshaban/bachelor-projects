/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.Events;
using Vuforia;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour
{

    private GameObject Fire;

    Animator Anim;
    Animator Anim1;
    Animator Anim2;
    Animator Anim3;
    GameObject[] Dummies;
    bool played = false;
    bool Reached = false;
    bool Left = true;

    public enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    }

    /// <summary>
    /// A filter that can be set to either:
    /// - Only consider a target if it's in view (TRACKED)
    /// - Also consider the target if's outside of the view, but the environment is tracked (EXTENDED_TRACKED)
    /// - Even consider the target if tracking is in LIMITED mode, e.g. the environment is just 3dof tracked.
    /// </summary>
    public TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;
    public UnityEvent OnTargetFound;
    public UnityEvent OnTargetLost;


    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;
    protected bool m_CallbackReceivedOnce = false;

    protected virtual void Start()
    {
        Dummies = new GameObject[] { GameObject.Find("Dummy1L"), GameObject.Find("Dummy1R"), GameObject.Find("Dummy2L"), GameObject.Find("Dummy2R"),
                                    GameObject.Find("Dummy3L"), GameObject.Find("Dummy3R"),GameObject.Find("BTR112L"), GameObject.Find("BTR112R"),};
        Fire = GameObject.Find("Fire");
        Anim = GameObject.Find("BTR112").GetComponent<Animator>();
        Anim1 = GameObject.Find("Dummy1").GetComponent<Animator>();
        Anim2 = GameObject.Find("Dummy2").GetComponent<Animator>();
        Anim3 = GameObject.Find("Dummy3").GetComponent<Animator>();

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.UnregisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        m_PreviousStatus = statusChangeResult.PreviousStatus;
        m_NewStatus = statusChangeResult.NewStatus;

        if (m_NewStatus == TrackableBehaviour.Status.DETECTED ||
            m_NewStatus == TrackableBehaviour.Status.TRACKED ||
            m_NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (!played)
            {
                Anim.Play("Entrance");
                Anim1.Play("Dummy");
                Anim2.Play("Dummy2");
                Anim3.Play("Dummy3");
                Destroy(GameObject.Find("Dummy1"), 15);
                Destroy(GameObject.Find("Dummy2"), 15);
                Destroy(GameObject.Find("Dummy3"), 15);
                Destroy(GameObject.Find("Dump"), 10);
                Destroy(GameObject.Find("Dump1"), 10);
                InvokeRepeating("Stop_Particle", 1, 2);
                InvokeRepeating("Firing", 0.1f, 0.1f);

                GameObject.Find("Dummy1").GetComponent<AudioSource>().Play();
                GameObject.Find("Dummy2").GetComponent<AudioSource>().Play();

                Debug.Log("============----------------------");

                played = true;
            }

            OnTrackingFound();
        }

        Debug.LogFormat("Trackable {0} {1} -- {2}",
            mTrackableBehaviour.TrackableName,
            mTrackableBehaviour.CurrentStatus,
            mTrackableBehaviour.CurrentStatusInfo);

        HandleTrackableStatusChanged();
    }

    int i = 0;
    private void Stop_Particle()
    {
        i++;
        if (i == 3)
        {
            GameObject.Find("Dump").GetComponent<ParticleSystem>().Stop();
            GameObject.Find("Dump1").GetComponent<ParticleSystem>().Stop();
            Reached = true;
        }
        if (i == 8)
        {
            CancelInvoke("Stop_Particle");
            CancelInvoke("Firing");
        }
    }

    private void Firing()
    {
        if (!Reached)
        {
            GameObject clone;
            clone = Instantiate(Fire);
            if (Left)
                clone.transform.parent = Dummies[6].transform;
            else
                clone.transform.parent = Dummies[7].transform;
            clone.transform.localScale = new Vector3(30, 30, 30);
            clone.transform.rotation = GameObject.Find("cannon").transform.rotation * Quaternion.Euler(0, 90, 0);
            if (Left)
                clone.transform.position = Dummies[6].transform.position;
            else
                clone.transform.position = Dummies[7].transform.position;

            Vector3 forwardVel = transform.forward * 600;
            Vector3 horizontalVel = -transform.right * 600;
            Vector3 upVel = transform.up * 600;
            clone.GetComponent<Rigidbody>().velocity = forwardVel + horizontalVel + upVel;
            Destroy(clone, 5);
        }
        for (int i = 0; i < 5; i += 2)
        {
            GameObject clone;
            clone = Instantiate(Fire);
            if (Left)
                clone.transform.parent = Dummies[i].transform;
            else
                clone.transform.parent = Dummies[i + 1].transform;
            clone.transform.localScale = new Vector3(30, 30, 30);
            clone.transform.rotation = GameObject.Find("cannon").transform.rotation * Quaternion.Euler(0, 90, 0);
            if (Left)
                clone.transform.position = Dummies[i].transform.position;
            else
                clone.transform.position = Dummies[i + 1].transform.position;

            Vector3 forwardVel = transform.forward * 600;
            Vector3 horizontalVel = -transform.right * 600;
            Vector3 upVel = transform.up * 600;
            clone.GetComponent<Rigidbody>().velocity = forwardVel + horizontalVel + upVel;
            Destroy(clone, 5);
        }
        Left = (!Left);
    }

    protected virtual void HandleTrackableStatusChanged()
    {
        if (!ShouldBeRendered(m_PreviousStatus) &&
            ShouldBeRendered(m_NewStatus))
        {
            OnTrackingFound();
        }
        else if (ShouldBeRendered(m_PreviousStatus) &&
                 !ShouldBeRendered(m_NewStatus))
        {
            OnTrackingLost();
        }
        else
        {
            if (!m_CallbackReceivedOnce && !ShouldBeRendered(m_NewStatus))
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        m_CallbackReceivedOnce = true;
    }

    protected bool ShouldBeRendered(TrackableBehaviour.Status status)
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED)
        {
            // always render the augmentation when status is DETECTED or TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                // also return true if the target is extended tracked
                return true;
            }
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED ||
                status == TrackableBehaviour.Status.LIMITED)
            {
                // in this mode, render the augmentation even if the target's tracking status is LIMITED.
                // this is mainly recommended for Anchors.
                return true;
            }
        }

        return false;
    }

    protected virtual void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
        }

        if (OnTargetFound != null)
            OnTargetFound.Invoke();
    }

    protected virtual void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }

        if (OnTargetLost != null)
            OnTargetLost.Invoke();
    }
}

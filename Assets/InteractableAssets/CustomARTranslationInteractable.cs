using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

[RequireComponent(typeof(CustomARSelectionInteractable))]
public class CustomARTranslationInteractable : ARBaseGestureInteractable
{
    [SerializeField]
    [Tooltip("Controls whether the object will be constrained vertically, horizontally, or free to move in all axis.")]
    GestureTransformationUtility.GestureTranslationMode m_ObjectGestureTranslationMode;
    /// <summary>
    /// The translation mode of this object.
    /// </summary>
    public GestureTransformationUtility.GestureTranslationMode objectGestureTranslationMode { get { return m_ObjectGestureTranslationMode; } set { m_ObjectGestureTranslationMode = value; } }

    [SerializeField]
    [Tooltip("The maximum translation distance of this object.")]
    float m_MaxTranslationDistance = 10.0f;
    /// <summary>
    /// The maximum translation distance of this object.
    /// </summary>
    public float maxTranslationDistance { get { return m_MaxTranslationDistance; } set { m_MaxTranslationDistance = value; } }

    const float k_PositionSpeed = 12.0f;
    const float k_DiffThreshold = 0.0001f;

    bool m_IsActive = false;

    Vector3 m_DesiredLocalPosition;
    float m_GroundingPlaneHeight;
    Vector3 m_DesiredAnchorPosition;
    Quaternion m_DesiredRotation;
    GestureTransformationUtility.Placement m_LastPlacement;
   
    /// <summary>
    /// The Unity's Start method.
    /// </summary>
    protected void Start()
    {
        // m_DesiredLocalPosition = new Vector3(0, 0, 0);
        m_DesiredLocalPosition = this.transform.localPosition;
    }

    /// <summary>
    /// The Unity's Update method.
    /// </summary>
    void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// Returns true if the manipulation can be started for the given gesture.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    /// <returns>True if the manipulation can be started.</returns>
    protected override bool CanStartManipulationForGesture(DragGesture gesture)
    {
        if (gesture.TargetObject == null)
        {
            return false;
        }

        // If the gesture isn't targeting this item, don't start manipulating.
        if (gesture.TargetObject != gameObject)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Function called when the manipulation is started.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnStartManipulation(DragGesture gesture)
    {
        m_GroundingPlaneHeight = transform.parent.position.y;
    }
    List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    /// <summary>
    /// Continues the translation.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnContinueManipulation(DragGesture gesture)
    {
      
        Debug.Assert(transform.parent != null, "Translate interactable needs a parent object.");
        m_IsActive = true;

        GestureTransformationUtility.Placement desiredPlacement =
            GestureTransformationUtility.GetBestPlacementPosition(
                transform.parent.position, gesture.Position, m_GroundingPlaneHeight, 0.03f,
                maxTranslationDistance, objectGestureTranslationMode);
#if UNITY_IOS
       
        // raycast to gesture.position. 
        // prarse all points/hits
        // check if its ground plane 
        // set desiredPlacement.HoveringPosition to ground plane point
        // desiredPlacement.UpdatedGroundingPlaneHeight = ground plane height Y;
        if (GestureTransformationUtility.Raycast(gesture.Position,  s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("UNITY_IOS hit position " + s_Hits[0].pose.position);
          //  var hit = s_Hits[0];
            foreach (var hit in s_Hits)
            {
               
                if (hit.trackable.GetComponent<ARPlane>().classification == PlaneClassification.Floor)
                {
                    desiredPlacement.HoveringPosition = hit.pose.position;
                    desiredPlacement.UpdatedGroundingPlaneHeight = hit.pose.position.y;
                    desiredPlacement.PlacementPosition = hit.pose.position;
                    desiredPlacement.HasPlane = true;
                    m_LastPlacement = desiredPlacement;
                    break;
                }
            }
        }
#endif     

            if (desiredPlacement.HasHoveringPosition && desiredPlacement.HasPlacementPosition)
        {
            // If desired position is lower than current position, don't drop it until it's finished.
            m_DesiredLocalPosition = transform.parent.InverseTransformPoint(desiredPlacement.HoveringPosition);
            m_DesiredAnchorPosition = desiredPlacement.PlacementPosition;

            m_GroundingPlaneHeight = desiredPlacement.UpdatedGroundingPlaneHeight;

            // Rotate if the plane direction has changed.
            if (((desiredPlacement.PlacementRotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
                m_DesiredRotation = desiredPlacement.PlacementRotation;
            else
                m_DesiredRotation = transform.rotation;

            if (desiredPlacement.HasPlane)
                m_LastPlacement = desiredPlacement;
        }
    }

    /// <summary>
    /// Finishes the translation.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(DragGesture gesture)
    {
        if (!m_LastPlacement.HasPlacementPosition)
            return;

        GameObject oldAnchor = transform.parent.gameObject;
        Pose desiredPose = new Pose(m_DesiredAnchorPosition, m_LastPlacement.PlacementRotation);

        Vector3 desiredLocalPosition = transform.parent.InverseTransformPoint(desiredPose.position);

        if (desiredLocalPosition.magnitude > maxTranslationDistance)
            desiredLocalPosition = desiredLocalPosition.normalized * maxTranslationDistance;
        desiredPose.position = transform.parent.TransformPoint(desiredLocalPosition);

        //Anchor newAnchor = m_LastPlacement.Trackable.CreateAnchor(desiredPose);
        var anchorGO = new GameObject("PlacementAnchor");
        anchorGO.transform.position = m_LastPlacement.PlacementPosition;
        anchorGO.transform.rotation = m_LastPlacement.PlacementRotation;
        transform.parent = anchorGO.transform;

        Destroy(oldAnchor);

        m_DesiredLocalPosition = Vector3.zero;

        // Rotate if the plane direction has changed.
        if (((desiredPose.rotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
            m_DesiredRotation = desiredPose.rotation;
        else
            m_DesiredRotation = transform.rotation;

        // Make sure position is updated one last time.
        m_IsActive = true;
    }

    void UpdatePosition()
    {
        if (!m_IsActive)
            return;

        // Lerp position.
        Vector3 oldLocalPosition = transform.localPosition;
        Vector3 newLocalPosition = Vector3.Lerp(
            oldLocalPosition, m_DesiredLocalPosition, Time.deltaTime * k_PositionSpeed);

        float diffLenght = (m_DesiredLocalPosition - newLocalPosition).magnitude;
        if (diffLenght < k_DiffThreshold)
        {
            newLocalPosition = m_DesiredLocalPosition;
            m_IsActive = false;
        }

        transform.localPosition = newLocalPosition;

        // Lerp rotation.
        Quaternion oldRotation = transform.rotation;
        Quaternion newRotation =
            Quaternion.Lerp(oldRotation, m_DesiredRotation, Time.deltaTime * k_PositionSpeed);
        transform.rotation = newRotation;
    }

}

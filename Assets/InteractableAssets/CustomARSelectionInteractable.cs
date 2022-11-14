using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;
using System.Collections;

public class CustomARSelectionInteractable : ARBaseGestureInteractable
{
    bool m_GestureSelected;

    /// <summary>
    /// The visualization game object that will become active when the object is selected.
    /// </summary>        
    [SerializeField, Tooltip("The GameObject that will become active when the object is selected.")]
    GameObject m_SelectionVisualization;

    public GameObject selectionVisualization { get { return m_SelectionVisualization; } set { m_SelectionVisualization = value; } }

    /// <summary>
    /// Determines if this interactable can be selected by a given interactor.
    /// </summary>
    /// <param name="interactor">Interactor to check for a valid selection with.</param>
    /// <returns>True if selection is valid this frame, False if not.</returns>
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        /*if (!(interactor is ARGestureInteractor))
            return false;*/

        return m_GestureSelected;
    }

    /// <summary>
    /// Is Point Over UI Object..
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Returns true if the manipulation can be started for the given gesture.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    /// <returns>True if the manipulation can be started.</returns>
    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        return true;
    }

    /// <summary>
    /// Function called when the manipulation is ended.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture == null)
            return;

        if (gesture.WasCancelled)
            return;

        if (gestureInteractor == null)
            return;

        if (gesture.FingerId > 0 || IsPointOverUIObject(gesture.StartPosition))
            return;

        if (gesture.TargetObject == gameObject)
        {
            Debug.Log("[CustomARSelectionInteractable] ==> gesture.TargetObject(" + gesture.TargetObject.name + ") == gameObject(" + gameObject.name + ")");
            m_GestureSelected = !m_GestureSelected;
        }
        else
        {
            Debug.Log("SELECTED OBJECT IS NULL");
            m_GestureSelected = false;
        }
    }

    private void ToggleUI(GameObject targetObject)
    {
        if (GameObject.Find("CommonARState") != null)
        {
            GameObject g = GameObject.Find("CommonARState");
            object[] tempStorage = new object[2];
            tempStorage[0] = targetObject;
            tempStorage[1] = m_GestureSelected;
            g.SendMessage("GetSelectedModelToDelete", tempStorage);
        }
        else if (GameObject.Find("BrochureARState") != null)
        {
            GameObject g = GameObject.Find("BrochureARState");
            object[] tempStorage = new object[2];
            tempStorage[0] = targetObject;
            tempStorage[1] = m_GestureSelected;
            g.SendMessage("GetSelectedModelToDelete", tempStorage);
        }
    }

    /// <summary>This method is called by the interaction manager 
    /// when the interactor first initiates selection of an interactable.</summary>
    /// <param name="interactor">Interactor that is initiating the selection.</param>
    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        Debug.Log("[" + gameObject.name + "]OnSelectEnter called......!! --> " + interactor.selectTarget.name);

        base.OnSelectEnter(interactor);

        // sending message to AR state to toggle ADD/Delete Button...
        ToggleUI(interactor.selectTarget.gameObject);

        if (m_SelectionVisualization != null)
        {
            Debug.Log("m_SelectionVisualization" + m_SelectionVisualization.gameObject.name);
            m_SelectionVisualization.SetActive(true);

            // Activate UI Canvas for Product Customization ( Color & Mesh )
            //  ShowCustomizationPanel(gameObject, true);
        }

        // CODE COMMENTED.....
        #region Scale up models if model contains "ScaleTweenModels" component
        //if (gameObject.HasComponent<ScaleTweenModels>())
        //{
        //    ScaleTweenModels scaleTweenModels = gameObject.GetComponent<ScaleTweenModels>();

        //    if (!scaleTweenModels.isModelScaledUp)
        //        scaleTweenModels.ScaleUp(gameObject);
        //}
        #endregion
    }

    /// <summary>This method is called by the interaction manager 
    /// when the interactor ends selection of an interactable.</summary>
    /// <param name="interactor">Interactor that is ending the selection.</param>
    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        Debug.Log("[" + gameObject.name + "]OnSelectExit called......!!");

        if (interactor.selectTarget == null)
            Debug.Log("OnSelectExit interactor selectTarget is NULL......!!");
        else
            Debug.Log("OnSelectExit interactor selectTarget is " + interactor.selectTarget.name);

        base.OnSelectExit(interactor);

        // sending message to AR state to toggle ADD/Delete Button...
        ToggleUI(null);

        if (m_SelectionVisualization != null)
        {
            Debug.Log("m_SelectionVisualization" + m_SelectionVisualization.gameObject.name);
            m_SelectionVisualization.SetActive(false);
            // Activate UI Canvas for Product Customization ( Color & Mesh )
            //ShowCustomizationPanel(gameObject, true);
        }
    }

    public void ShowCustomizationPanel(GameObject selectedAsset, bool status)
    {
        GameObject customizationParent = getChildGameObject(selectedAsset, "ColorSwitch");
        //if (customizationParent != null)
        //{
        //    Debug.Log("ShowCustomizationPanel +++++++++ " + customizationParent.name + "Parent " + selectedAsset.name);
        //    customizationParent.GetComponent<ColorSwitcher>().ToggleUi(status);
        //}

        // TODO: This is for Mesh Customization....

        //GameObject meshCustomizationParent = getChildGameObject(selectedAsset, "MeshCustomizer");
        //if (meshCustomizationParent != null)
        //{
        //    Debug.Log("meshCustomizationParent +++++++++ " + meshCustomizationParent.name + "Parent " + selectedAsset.name);
        //    meshCustomizationParent.GetComponent<MeshCustomizer>().DropDownUi(status);
        //}
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
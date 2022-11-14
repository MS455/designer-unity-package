using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class InteractableAssets : MonoBehaviour
{
    private GameObject currentGameObject;

    // Start is called before the first frame update
    void Awake()
    {
        currentGameObject = this.gameObject;
    }
       
    void Start()
    {
        //Debug.Log("InteractableAssets Start Called");
        currentGameObject.AddComponent<Rigidbody>();
        ARRotationInteractable ARRotationInteractable = currentGameObject.AddComponent<ARRotationInteractable>();
        CustomARTranslationInteractable ARTranslationInteractable = currentGameObject.AddComponent<CustomARTranslationInteractable>();
        CustomARSelectionInteractable ARSelectionInteractable = currentGameObject.GetComponent<CustomARSelectionInteractable>();

        ARSelectionInteractable.selectionVisualization = getChildGameObject(currentGameObject, currentGameObject.name + "_SGO");
        ARSelectionInteractable.interactionLayerMask = ~(1 << LayerMask.NameToLayer("ReflectionLayer"));
        ARTranslationInteractable.interactionLayerMask = ~(1 << LayerMask.NameToLayer("ReflectionLayer"));

        ARRotationInteractable.interactionLayerMask = ~(1 << LayerMask.NameToLayer("ReflectionLayer"));
        currentGameObject.tag = "Model";
        currentGameObject.layer = LayerMask.NameToLayer("ARAsset");
        currentGameObject.AddComponent<ObjectEffect>();
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
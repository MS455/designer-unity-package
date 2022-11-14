using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class ObjectEffect : MonoBehaviour
{
    [SerializeField]
    ARSessionHandler aRSessionHandler;

    [SerializeField]
    private CustomARPlacementInteractable arPlacementInteractable;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ObjectEffect Start Called");

        aRSessionHandler = FindObjectOfType<ARSessionHandler>();
        arPlacementInteractable = FindObjectOfType<CustomARPlacementInteractable>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Name: " + collision.gameObject.name + "Game Object Name: " + gameObject.name + "Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag != "GroundPlane")
            return;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb) Destroy(rb);

        //if (arPlacementInteractable) arPlacementInteractable.ResetPlacementPrefab();

       /* if (aRSessionHandler != null)
            aRSessionHandler.StopPlaneTrackingOnObjectPlaced(false);
*/
        StartCoroutine("AddAnchor");
    }

    IEnumerator AddAnchor()
    {
        yield return new WaitForEndOfFrame();

        //adding AR Anchor
        if (gameObject.GetComponent<ARAnchor>() == null)
            gameObject.AddComponent<ARAnchor>();

        Destroy(gameObject.GetComponent<ObjectEffect>());
        Debug.Log("ObjectEffect removed");
    }
}
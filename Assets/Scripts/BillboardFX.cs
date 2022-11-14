using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    public Transform camTransform;

    Quaternion originalRotation;

    void Start()
    {
        camTransform = Camera.main.transform;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (camTransform == null || !gameObject.activeInHierarchy)
            return;

        transform.rotation = camTransform.rotation * originalRotation;
    }
}
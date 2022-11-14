using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;

public class ToastMessage : MonoBehaviour
{

    [SerializeField]
    float timeToDisable = 2.0f;
    [SerializeField]
    StringEvent E_ToastMessage;
    [SerializeField]
    TMP_Text Message;
    [SerializeField]
    GameObject ToastPanel;

    private string _M;
    void OnEnable()
    {
        StartCoroutine("DisableObject");
       //_M =  E_ToastMessage.
    }

    void OnDisable()
    {
        StopCoroutine("DisableObject");
    }

    public void SetMessage(string message)
    {
        ToastPanel.SetActive(true);
        Message.text = message;
        StartCoroutine(DisableObject());

    }

    IEnumerator DisableObject()
    {
        Debug.Log("Starting Timer");
        yield return new WaitForSeconds(timeToDisable);
        ToastPanel.SetActive(false);
    }
   
}

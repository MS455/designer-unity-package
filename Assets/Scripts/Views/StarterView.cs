using DapperDino.DapperTools.StateMachines;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterView : BaseView
{
    /*[SerializeField]
    GameObject loadingPanel;
    [SerializeField]
    string contactusURL = "https://hexaware.com/";*/

    

    public override void OnEnter()
    {
        base.OnEnter();
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

   /* public void ShowLoadingScreen(bool flag)
    {
        loadingPanel.SetActive(flag);
    }

    public void ContactUs()
    {
        Application.OpenURL(contactusURL);
    }

    public void ReloadApplication()
    {
        StartCoroutine(CallReloadApplication());
    }

    IEnumerator CallReloadApplication()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }*/

    /*public void ClickSound()
    {
        AudioManager.PlaySound(Sounds.ButtonClick);
    }*/
}
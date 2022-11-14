using RestSharp;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class API_GetAllCategories : MonoBehaviour
{
    [SerializeField]
    StringVariable V_getAllCategoriesURL;

    [SerializeField]
    StringEvent getAllCategoriesSuccessEvent;
    [SerializeField]
    StringEvent getAllCategoriesFailedEvent;
    [Space]
    [SerializeField]
    BoolVariable V_IsConnectedToInternet;

    public void Begin()
    {
        if (V_IsConnectedToInternet.Value == true)
            GetAllCategory();
        else
            getAllCategoriesFailedEvent.Raise("No Internet Connection");
    }

    private async void GetAllCategory()
    {
        Debug.Log("GetAllCategory called\n" + V_getAllCategoriesURL.Value);

        var client = new RestClient(V_getAllCategoriesURL.Value);
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);

        IRestResponse response = await client.ExecuteAsync(request);

        Debug.Log("StatusCode : " + response.StatusCode);

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            getAllCategoriesFailedEvent.Raise(response.ErrorMessage);
            Debug.Log("API_GetAllCategories -> Failed Response : " + response.ErrorMessage);
        }
        else
        {
            if (response.IsSuccessful)
            {
                getAllCategoriesSuccessEvent.Raise(response.Content);
                Debug.Log("API_GetAllCategories -> Success Response : " + response.Content);
            }
            else
            {
                getAllCategoriesFailedEvent.Raise(response.ErrorMessage);
                Debug.Log("API_GetAllCategories -> Failed Response : " + response.ErrorMessage);
            }
        }
    }
}
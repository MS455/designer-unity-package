using RestSharp;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class API_GetAllProducts : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    BoolVariable v_IsConnectedToInternet;

    [SerializeField]
    StringEvent E_getAllProductsFailed;
    [SerializeField]
    StringEvent E_getAllProductsSuccess;

    [SerializeField]
    StringVariable V_getAllProductsURL;
    public void Begin()
    {
        if (v_IsConnectedToInternet.Value)
            GetAllCategory();
        else
            E_getAllProductsFailed.Raise("No Connection");
    }
    private async void GetAllCategory()
    {
        var client = new RestClient(V_getAllProductsURL.Value);
        client.Timeout = -1;
        var request = new RestRequest(Method.GET);

        IRestResponse response = await client.ExecuteAsync(request);

        if(response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            E_getAllProductsFailed.Raise(response.ErrorMessage);
            //Debug.Log(response.ErrorMessage);
        }
        else if(response.IsSuccessful)
        {
            E_getAllProductsSuccess.Raise(response.Content);
            //Debug.Log(response.Content);
        }
        else
        {
            E_getAllProductsFailed.Raise(response.ErrorMessage);
            //Debug.Log(response.ErrorMessage);
        }
    }
}

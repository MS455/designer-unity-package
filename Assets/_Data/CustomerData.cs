using UnityEngine;
using System;

[Serializable]
public class CustomerData 
{
    public string PhNo;
    public string Email;
    public string Address;
    public string OrgName;
    public string BillingName;



    
    public static CustomerData CreateFromJSON(string jsonString)
    {
        if(string.IsNullOrEmpty(PlayerPrefs.GetString("CustomerData")))
        {
            return JsonUtility.FromJson<CustomerData>(jsonString);
        }
        else
        {
            return JsonUtility.FromJson<CustomerData>(PlayerPrefs.GetString("CustomerData"));
        }
        
    }

    public string SaveToString()
    {
        string customerDataString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("CustomerData", customerDataString);
        return customerDataString;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Collectible"))
        {
            Destroy(collision.gameObject);
            UpdateCurrency("ArgentTest", 1);
            Debug.Log(PlayerPrefs.GetInt("ArgentTest").ToString());
        }
    }

    private void UpdateCurrency(string currencyName, int currencyAmount)
    {
        if (!PlayerPrefs.HasKey(currencyName))
        {
            PlayerPrefs.SetInt(currencyName, currencyAmount);
        } else
        {
            PlayerPrefs.SetInt(currencyName, PlayerPrefs.GetInt(currencyName) + currencyAmount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Collectible"))
        {
            //Détruit l'objet
            Destroy(collision.gameObject);

            //ajouter a l'inventaire quand collision avec Player
            Inventory.instance.AddCoins(1);

            Debug.Log(PlayerPrefs.GetInt("ArgentTest").ToString());
        }
    }

}

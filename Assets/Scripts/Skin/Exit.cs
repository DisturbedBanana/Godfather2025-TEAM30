using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void OnExitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu"); // Retour au menu
    }
}
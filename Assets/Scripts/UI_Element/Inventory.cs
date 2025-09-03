using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI coinsCountText; // L'affichage TextMeshPro pour le compte de pièces

    public static Inventory instance; // Singleton pour un accès facile depuis d'autres scripts

    // Clé PlayerPrefs unique pour toutes les pièces du jeu
    private const string COINS_PLAYERPREFS_KEY = "PlayerCoins";

    private void Awake()
    {
        // Implémentation du Singleton
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène. Destruction de la nouvelle.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        UpdateTextUI(GetCoinsCount()); // Initialise l'UI avec les pièces chargées
    }

    // Ajoute des pièces et les sauvegarde
    public void AddCoins(int count)
    {
        int currentCoins = GetCoinsCount(); // Charge le montant actuel via la méthode GetCoinsCount
        currentCoins += count; // Ajoute les nouvelles pièces
        PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, currentCoins); // Sauvegarde le nouveau total
        PlayerPrefs.Save(); // Assure que la sauvegarde est écrite sur le disque
        UpdateTextUI(currentCoins); // Met à jour l'affichage UI
        Debug.Log("Pièces ajoutées : " + count + ". Total : " + currentCoins);
    }

    public bool RemoveCoins(int count)
    {
        int currentCoins = GetCoinsCount(); // Charge le montant actuel

        if (currentCoins >= count)
        {
            currentCoins -= count; // Soustrait 'count'
            PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, currentCoins); // Sauvegarde le nouveau total
            PlayerPrefs.Save(); // Assure que la sauvegarde est écrite sur le disque
            UpdateTextUI(currentCoins); // Met à jour l'affichage UI
            Debug.Log("Pièces retirées : " + count + ". Reste : " + currentCoins);
            return true; // Succès
        }
        else
        {
            Debug.Log("Tentative de retirer " + count + " pièces, mais seulement " + currentCoins + " disponibles.");
            return false; // Échec (pas assez de pièces)
        }
    }

    public int GetCoinsCount()
    {
        return PlayerPrefs.GetInt(COINS_PLAYERPREFS_KEY, 30); // Récupère le total actuel des pièces
    }

    // Met à jour l'affichage du nombre de pièces
    private void UpdateTextUI(int currentCoins)
    {
        if (coinsCountText != null)
        {
            coinsCountText.text = currentCoins.ToString();
        }
    }

    // Fonction de test/réinitialisation pour l'inventaire
    public void ResetInventoryCoins()
    {
        PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, 0);
        PlayerPrefs.Save();
        UpdateTextUI(0);
        Debug.Log("Pièces de l'inventaire réinitialisées à 0.");
    }
}
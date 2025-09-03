using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI coinsCountText; // L'affichage TextMeshPro pour le compte de pi�ces

    public static Inventory instance; // Singleton pour un acc�s facile depuis d'autres scripts

    // Cl� PlayerPrefs unique pour toutes les pi�ces du jeu
    private const string COINS_PLAYERPREFS_KEY = "PlayerCoins";

    private void Awake()
    {
        // Impl�mentation du Singleton
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la sc�ne. Destruction de la nouvelle.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        UpdateTextUI(GetCoinsCount()); // Initialise l'UI avec les pi�ces charg�es
    }

    // Ajoute des pi�ces et les sauvegarde
    public void AddCoins(int count)
    {
        int currentCoins = GetCoinsCount(); // Charge le montant actuel via la m�thode GetCoinsCount
        currentCoins += count; // Ajoute les nouvelles pi�ces
        PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, currentCoins); // Sauvegarde le nouveau total
        PlayerPrefs.Save(); // Assure que la sauvegarde est �crite sur le disque
        UpdateTextUI(currentCoins); // Met � jour l'affichage UI
        Debug.Log("Pi�ces ajout�es : " + count + ". Total : " + currentCoins);
    }

    public bool RemoveCoins(int count)
    {
        int currentCoins = GetCoinsCount(); // Charge le montant actuel

        if (currentCoins >= count)
        {
            currentCoins -= count; // Soustrait 'count'
            PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, currentCoins); // Sauvegarde le nouveau total
            PlayerPrefs.Save(); // Assure que la sauvegarde est �crite sur le disque
            UpdateTextUI(currentCoins); // Met � jour l'affichage UI
            Debug.Log("Pi�ces retir�es : " + count + ". Reste : " + currentCoins);
            return true; // Succ�s
        }
        else
        {
            Debug.Log("Tentative de retirer " + count + " pi�ces, mais seulement " + currentCoins + " disponibles.");
            return false; // �chec (pas assez de pi�ces)
        }
    }

    public int GetCoinsCount()
    {
        return PlayerPrefs.GetInt(COINS_PLAYERPREFS_KEY, 30); // R�cup�re le total actuel des pi�ces
    }

    // Met � jour l'affichage du nombre de pi�ces
    private void UpdateTextUI(int currentCoins)
    {
        if (coinsCountText != null)
        {
            coinsCountText.text = currentCoins.ToString();
        }
    }

    // Fonction de test/r�initialisation pour l'inventaire
    public void ResetInventoryCoins()
    {
        PlayerPrefs.SetInt(COINS_PLAYERPREFS_KEY, 0);
        PlayerPrefs.Save();
        UpdateTextUI(0);
        Debug.Log("Pi�ces de l'inventaire r�initialis�es � 0.");
    }
}
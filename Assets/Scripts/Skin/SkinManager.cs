using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinManager : MonoBehaviour
{
    public Skin[] allSkins; // Liste de tous les skins disponibles
    public Image characterDisplay; // Image de prévisualisation du skin sélectionné

    public GameObject skinButtonPrefab; // Prefab du bouton d'achat/sélection de skin
    public Transform skinButtonContainer; // Parent des boutons de skin (conteneur dans le Canvas)

    // Variables pour les noms des objets enfants dans le prefab du bouton de skin
    public string priceTextName = "PriceText"; // Nom de l'objet TextMeshPro enfant
    public string coinIconName = "CoinIcon"; // Nom de l'objet Image enfant (icône de pièce)

    private int currentSkinIndex = 0; // Index du skin actuellement sélectionné

    // Clé PlayerPrefs pour le skin sélectionné
    private const string SELECTED_SKIN_PLAYERPREFS_KEY = "SelectedSkin";

    void Start()
    {
        // currentSkinIndex est chargé ici, PAS les pièces (Inventory.cs s'en charge)
        currentSkinIndex = PlayerPrefs.GetInt(SELECTED_SKIN_PLAYERPREFS_KEY, 0);

        LoadSkins(); // Crée les boutons et charge les skins débloqués
        DisplayCurrentSkin(); // Affiche le skin actuellement sélectionné
    }

    void LoadSkins()
    {
        // Assurez-vous que le conteneur est vide avant de charger les nouveaux boutons
        foreach (Transform child in skinButtonContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < allSkins.Length; i++)
        {
            // Récupère si le skin est débloqué dans les préférences
            allSkins[i].unlocked = PlayerPrefs.GetInt("SkinUnlocked" + i, i == 0 ? 1 : 0) == 1;

            // Crée un bouton pour chaque skin
            GameObject btn = Instantiate(skinButtonPrefab, skinButtonContainer);
            int index = i; // Capture de la variable pour la lambda

            // Ajout du listener
            btn.GetComponent<Button>().onClick.AddListener(() => TrySelectSkin(index));

            // Récupérer les références au texte et à l'icône de pièce
            TMP_Text buttonText = btn.GetComponentInChildren<TMP_Text>();
            Image coinIcon = btn.transform.Find(coinIconName)?.GetComponent<Image>();

            // Met à jour l'affichage initial du bouton
            UpdateSkinButtonUI(buttonText, coinIcon, allSkins[i], i == currentSkinIndex);
        }
    }

    void DisplayCurrentSkin()
    {
        // Affiche le sprite principal du skin sélectionné
        if (currentSkinIndex >= 0 && currentSkinIndex < allSkins.Length && allSkins[currentSkinIndex].sprites != null && allSkins[currentSkinIndex].sprites.Length > 0)
        {
            characterDisplay.sprite = allSkins[currentSkinIndex].sprites[0];
        }
        else
        {
            Debug.LogWarning("Skin sélectionné invalide ou sprite manquant pour l'affichage : " + currentSkinIndex);
        }
    }

    public void TrySelectSkin(int index)
    {

        if (allSkins[index].unlocked)
        {
            // Si déjà débloqué, on sélectionne simplement
            currentSkinIndex = index;
            SaveSelectedSkin(); // Nouvelle fonction pour sauvegarder le skin sélectionné
            DisplayCurrentSkin();

            //if (PlayerSkinApplier.instance != null)
            //{
            //    PlayerSkinApplier.instance.ApplySkin(currentSkinIndex);
            //}

            Debug.Log("Skin " + allSkins[index].name + " sélectionné.");
        }
        else // Skin est verrouillé, tentative d'achat
        {
            // Vérifie les pièces via Inventory.instance
            if (Inventory.instance.GetCoinsCount() >= allSkins[index].price)
            {
                // Tente de retirer les pièces via Inventory.instance (qui gère la sauvegarde)
                if (Inventory.instance.RemoveCoins(allSkins[index].price))
                {
                    // Si le retrait des pièces a réussi, on débloque le skin
                    allSkins[index].unlocked = true;
                    PlayerPrefs.SetInt("SkinUnlocked" + index, 1); // Enregistre le skin comme débloqué

                    // Sélectionne automatiquement le skin après achat
                    currentSkinIndex = index;
                    SaveSelectedSkin(); // Sauvegarde le skin sélectionné
                    DisplayCurrentSkin();

                    Debug.Log("Skin " + allSkins[index].name + " acheté et sélectionné. Pièces restantes : " + Inventory.instance.GetCoinsCount());
                }
            }
            else
            {
                // Le joueur n'a pas assez de pièces
                Debug.Log("Pas assez de pièces pour acheter " + allSkins[index].name + ". Nécessite : " + allSkins[index].price + ", Possède : " + Inventory.instance.GetCoinsCount());
            }
        }

        // Met à jour le texte de tous les boutons après achat ou sélection (cela doit toujours se faire)
        for (int i = 0; i < skinButtonContainer.childCount; i++)
        {
            GameObject buttonObject = skinButtonContainer.GetChild(i).gameObject;
            TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
            Image coinIcon = buttonObject.transform.Find(coinIconName)?.GetComponent<Image>();

            UpdateSkinButtonUI(buttonText, coinIcon, allSkins[i], i == currentSkinIndex);
        }
    }

    // Fonction pour gérer la logique d'affichage du bouton
    void UpdateSkinButtonUI(TMP_Text buttonText, Image coinIcon, Skin skinData, bool isCurrentSkin)
    {
        if (skinData.unlocked)
        {
            if (isCurrentSkin)
            {
                buttonText.text = "E Q U I P ";
            }
            else
            {
                buttonText.text = "U S E";
            }
            if (coinIcon != null) coinIcon.gameObject.SetActive(false); // Cache l'icône de pièce si déverrouillé
        }
        else
        {
            buttonText.text = skinData.name + " - " + skinData.price;
            if (coinIcon != null) coinIcon.gameObject.SetActive(true); // Affiche l'icône de pièce si verrouillé
        }
    }

    // Nouvelle fonction pour sauvegarder le skin sélectionné
    void SaveSelectedSkin()
    {
        PlayerPrefs.SetInt(SELECTED_SKIN_PLAYERPREFS_KEY, currentSkinIndex);
        PlayerPrefs.Save(); // Sauvegarde explicite
    }

    public Sprite GetCurrentSkinSprite()
    {
        if (currentSkinIndex >= 0 && currentSkinIndex < allSkins.Length && allSkins[currentSkinIndex].sprites != null && allSkins[currentSkinIndex].sprites.Length > 0)
        {
            return allSkins[currentSkinIndex].sprites[0];
        }
        return null; // Retourne null si l'index est invalide ou le sprite manquant
    }
}
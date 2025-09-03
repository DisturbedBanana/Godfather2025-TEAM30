using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinManager : MonoBehaviour
{
    public Skin[] allSkins; // Liste de tous les skins disponibles
    public Image characterDisplay; // Image de pr�visualisation du skin s�lectionn�

    public GameObject skinButtonPrefab; // Prefab du bouton d'achat/s�lection de skin
    public Transform skinButtonContainer; // Parent des boutons de skin (conteneur dans le Canvas)

    // Variables pour les noms des objets enfants dans le prefab du bouton de skin
    public string priceTextName = "PriceText"; // Nom de l'objet TextMeshPro enfant
    public string coinIconName = "CoinIcon"; // Nom de l'objet Image enfant (ic�ne de pi�ce)

    private int currentSkinIndex = 0; // Index du skin actuellement s�lectionn�

    // Cl� PlayerPrefs pour le skin s�lectionn�
    private const string SELECTED_SKIN_PLAYERPREFS_KEY = "SelectedSkin";

    void Start()
    {
        // currentSkinIndex est charg� ici, PAS les pi�ces (Inventory.cs s'en charge)
        currentSkinIndex = PlayerPrefs.GetInt(SELECTED_SKIN_PLAYERPREFS_KEY, 0);

        LoadSkins(); // Cr�e les boutons et charge les skins d�bloqu�s
        DisplayCurrentSkin(); // Affiche le skin actuellement s�lectionn�
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
            // R�cup�re si le skin est d�bloqu� dans les pr�f�rences
            allSkins[i].unlocked = PlayerPrefs.GetInt("SkinUnlocked" + i, i == 0 ? 1 : 0) == 1;

            // Cr�e un bouton pour chaque skin
            GameObject btn = Instantiate(skinButtonPrefab, skinButtonContainer);
            int index = i; // Capture de la variable pour la lambda

            // Ajout du listener
            btn.GetComponent<Button>().onClick.AddListener(() => TrySelectSkin(index));

            // R�cup�rer les r�f�rences au texte et � l'ic�ne de pi�ce
            TMP_Text buttonText = btn.GetComponentInChildren<TMP_Text>();
            Image coinIcon = btn.transform.Find(coinIconName)?.GetComponent<Image>();

            // Met � jour l'affichage initial du bouton
            UpdateSkinButtonUI(buttonText, coinIcon, allSkins[i], i == currentSkinIndex);
        }
    }

    void DisplayCurrentSkin()
    {
        // Affiche le sprite principal du skin s�lectionn�
        if (currentSkinIndex >= 0 && currentSkinIndex < allSkins.Length && allSkins[currentSkinIndex].sprites != null && allSkins[currentSkinIndex].sprites.Length > 0)
        {
            characterDisplay.sprite = allSkins[currentSkinIndex].sprites[0];
        }
        else
        {
            Debug.LogWarning("Skin s�lectionn� invalide ou sprite manquant pour l'affichage : " + currentSkinIndex);
        }
    }

    public void TrySelectSkin(int index)
    {

        if (allSkins[index].unlocked)
        {
            // Si d�j� d�bloqu�, on s�lectionne simplement
            currentSkinIndex = index;
            SaveSelectedSkin(); // Nouvelle fonction pour sauvegarder le skin s�lectionn�
            DisplayCurrentSkin();

            //if (PlayerSkinApplier.instance != null)
            //{
            //    PlayerSkinApplier.instance.ApplySkin(currentSkinIndex);
            //}

            Debug.Log("Skin " + allSkins[index].name + " s�lectionn�.");
        }
        else // Skin est verrouill�, tentative d'achat
        {
            // V�rifie les pi�ces via Inventory.instance
            if (Inventory.instance.GetCoinsCount() >= allSkins[index].price)
            {
                // Tente de retirer les pi�ces via Inventory.instance (qui g�re la sauvegarde)
                if (Inventory.instance.RemoveCoins(allSkins[index].price))
                {
                    // Si le retrait des pi�ces a r�ussi, on d�bloque le skin
                    allSkins[index].unlocked = true;
                    PlayerPrefs.SetInt("SkinUnlocked" + index, 1); // Enregistre le skin comme d�bloqu�

                    // S�lectionne automatiquement le skin apr�s achat
                    currentSkinIndex = index;
                    SaveSelectedSkin(); // Sauvegarde le skin s�lectionn�
                    DisplayCurrentSkin();

                    Debug.Log("Skin " + allSkins[index].name + " achet� et s�lectionn�. Pi�ces restantes : " + Inventory.instance.GetCoinsCount());
                }
            }
            else
            {
                // Le joueur n'a pas assez de pi�ces
                Debug.Log("Pas assez de pi�ces pour acheter " + allSkins[index].name + ". N�cessite : " + allSkins[index].price + ", Poss�de : " + Inventory.instance.GetCoinsCount());
            }
        }

        // Met � jour le texte de tous les boutons apr�s achat ou s�lection (cela doit toujours se faire)
        for (int i = 0; i < skinButtonContainer.childCount; i++)
        {
            GameObject buttonObject = skinButtonContainer.GetChild(i).gameObject;
            TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
            Image coinIcon = buttonObject.transform.Find(coinIconName)?.GetComponent<Image>();

            UpdateSkinButtonUI(buttonText, coinIcon, allSkins[i], i == currentSkinIndex);
        }
    }

    // Fonction pour g�rer la logique d'affichage du bouton
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
            if (coinIcon != null) coinIcon.gameObject.SetActive(false); // Cache l'ic�ne de pi�ce si d�verrouill�
        }
        else
        {
            buttonText.text = skinData.name + " - " + skinData.price;
            if (coinIcon != null) coinIcon.gameObject.SetActive(true); // Affiche l'ic�ne de pi�ce si verrouill�
        }
    }

    // Nouvelle fonction pour sauvegarder le skin s�lectionn�
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
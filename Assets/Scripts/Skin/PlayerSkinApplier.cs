using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkinApplier : MonoBehaviour
{
    public Skin[] allSkins;   // La m�me liste de skins que celle utilis�e dans SkinManager

    public SpriteRenderer playerSpriteRenderer; // Le SpriteRenderer du joueur

    public static PlayerSkinApplier instance; // Singleton si d'autres scripts ont besoin d'acc�der facilement � l'appliqueur

    private void Awake()
    {
        // Impl�mentation du Singleton
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerSkinApplier dans la sc�ne.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        // Charge l'index du skin s�lectionn� depuis PlayerPrefs
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0); // "SelectedSkin" est la cl� utilis�e par SkinManager

        // Applique le skin charg� au d�marrage
        ApplySkin(selectedSkinIndex);
    }

    // Nouvelle m�thode publique pour appliquer un skin par son index
    public void ApplySkin(int skinIndex)
    {

        // V�rification du SpriteRenderer
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("PlayerSkinApplier: Le SpriteRenderer du joueur n'est pas assign� dans l'Inspecteur !");
            return;
        }

        if (skinIndex < 0 || skinIndex >= allSkins.Length)
        {
            Debug.LogWarning("PlayerSkinApplier: Index de skin invalide : " + skinIndex + ". Application du skin par d�faut (index 0).");
            skinIndex = 0; // Revenir au skin par d�faut si l'index est invalide
        }

        // Application du Sprite
        if (allSkins[skinIndex].sprites != null && allSkins[skinIndex].sprites.Length > 0)
        {
            playerSpriteRenderer.sprite = allSkins[skinIndex].sprites[0]; // Prend le premier sprite du tableau
            ApplyLightColor(skinIndex);
            Debug.Log("Sprite du skin '" + allSkins[skinIndex].name + "' appliqu� au SpriteRenderer.");
        }
        else
        {
            Debug.LogWarning("PlayerSkinApplier: Le skin '" + allSkins[skinIndex].name + "' � l'index " + skinIndex + " n'a pas de sprites assign�s ! Le SpriteRenderer pourrait �tre vide.");
            playerSpriteRenderer.sprite = null;
        }
    }

    private void ApplyLightColor(int skinIndex)
    {
        Color newLightColor = Color.white;
        
        switch (skinIndex)
        {
            case 0:
                newLightColor = Color.red;
                break;
            case 1:
                newLightColor = Color.blue;
                break;
            case 2:
                newLightColor = new Color(255, 128, 0);
                break;
            case 3:
                newLightColor = Color.yellow;
                break;
            case 4:
                newLightColor = Color.magenta;
                break;
        }
        
        GetComponentInChildren<Light2D>().color = newLightColor;
        
    }
}
using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    public Skin[] allSkins;   // La même liste de skins que celle utilisée dans SkinManager

    public SpriteRenderer playerSpriteRenderer; // Le SpriteRenderer du joueur

    public static PlayerSkinApplier instance; // Singleton si d'autres scripts ont besoin d'accéder facilement à l'appliqueur

    private void Awake()
    {
        // Implémentation du Singleton
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerSkinApplier dans la scène.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        // Charge l'index du skin sélectionné depuis PlayerPrefs
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0); // "SelectedSkin" est la clé utilisée par SkinManager

        // Applique le skin chargé au démarrage
        ApplySkin(selectedSkinIndex);
    }

    // Nouvelle méthode publique pour appliquer un skin par son index
    public void ApplySkin(int skinIndex)
    {

        // Vérification du SpriteRenderer
        if (playerSpriteRenderer == null)
        {
            Debug.LogError("PlayerSkinApplier: Le SpriteRenderer du joueur n'est pas assigné dans l'Inspecteur !");
            return;
        }

        if (skinIndex < 0 || skinIndex >= allSkins.Length)
        {
            Debug.LogWarning("PlayerSkinApplier: Index de skin invalide : " + skinIndex + ". Application du skin par défaut (index 0).");
            skinIndex = 0; // Revenir au skin par défaut si l'index est invalide
        }

        // Application du Sprite
        if (allSkins[skinIndex].sprites != null && allSkins[skinIndex].sprites.Length > 0)
        {
            playerSpriteRenderer.sprite = allSkins[skinIndex].sprites[0]; // Prend le premier sprite du tableau
            Debug.Log("Sprite du skin '" + allSkins[skinIndex].name + "' appliqué au SpriteRenderer.");
        }
        else
        {
            Debug.LogWarning("PlayerSkinApplier: Le skin '" + allSkins[skinIndex].name + "' à l'index " + skinIndex + " n'a pas de sprites assignés ! Le SpriteRenderer pourrait être vide.");
            playerSpriteRenderer.sprite = null;
        }
    }
}
using UnityEngine;

[System.Serializable]
public class Skin
{
    public string name; //Nom du skin
    public int price; //Prix du skin
    public bool unlocked; //Skin débloqué
    public RuntimeAnimatorController animatorController; // Le contrôleur d'animation spécifique à ce skin
    public Sprite[] sprites; // Un tableau de sprites représentant le visuel du skin
}

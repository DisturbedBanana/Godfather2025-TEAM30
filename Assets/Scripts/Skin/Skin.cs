using UnityEngine;

[System.Serializable]
public class Skin
{
    public string name; //Nom du skin
    public int price; //Prix du skin
    public bool unlocked; //Skin d�bloqu�
    public RuntimeAnimatorController animatorController; // Le contr�leur d'animation sp�cifique � ce skin
    public Sprite[] sprites; // Un tableau de sprites repr�sentant le visuel du skin
}

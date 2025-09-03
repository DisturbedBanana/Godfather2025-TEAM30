using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, IPointerClickHandler
{
    public DialogDatabase dialogDatabase;
    public TMP_Text speakerText;
    public TMP_Text dialogText;
    public Image playerImg;
    public Image strangerImg;

    private int counter = 0;
    private bool isWriting = false;
    private bool isClickable => counter < dialogDatabase.dialogData.Count && !isWriting;
    
    private void Start()
    {
        DisplaySentence();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(isClickable)
        {
            isWriting = true;
            DisplaySentence();
        }
    }

    //Fonction qui va afficher les nouvelles phrases contenue dans la database du dialogue
    private void DisplaySentence()
    {
        //Affiche l'image de la personne qui parle (player ou stranger)
        Color playerColor = playerImg.color;
        Color strangerColor = strangerImg.color;
        switch (dialogDatabase.dialogData[counter].speakerType)
        {
            case DIALOG_SPEAKER.PLAYER :
                playerColor.a = 100;
                strangerColor.a = 0;
                break;
            case DIALOG_SPEAKER.STRANGER :
                strangerColor.a = 100;
                playerColor.a = 0;
                break;
        }
        playerImg.color = playerColor;
        strangerImg.color = strangerColor;

        //Remplace le texte du dialogue
        speakerText.text = dialogDatabase.dialogData[counter].speakerName;
        StartCoroutine(WriteSentence());
        counter++;
    }

    IEnumerator WriteSentence()
    {
        dialogText.text = "";

        foreach (char letter in dialogDatabase.dialogData[counter].textContent.ToCharArray())
        {
            dialogText.text += letter;

            yield return null;
        }
        isWriting = false;
    }


}

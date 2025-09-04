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
    public Sprite vilainSprite;
    public Sprite devSprite;

    private int counter = 0;
    private bool isWriting = false;
    private bool isClickable => counter < dialogDatabase.dialogData.Count + 1 && !isWriting;
    
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
        if (counter >= dialogDatabase.dialogData.Count)
        {
            // No more sentences to display, call DisplayMenu from UIManager
            UIManager.Instance.HideDialog();
            return;
        }
        
        //Change le nom de la personne qui parle, change le sprite si c'est un stranger
        switch (dialogDatabase.dialogData[counter].speaker)
        {
            case DIALOG_SPEAKER.NAUFRAGE:
                speakerText.text = "Le Naufrage";
                break;
            case DIALOG_SPEAKER.MECHANT:
                speakerText.text = "Monsieur Mechant";
                strangerImg.sprite = vilainSprite;
                break;
            case DIALOG_SPEAKER.DEV:
                speakerText.text = "Un dev";
                strangerImg.sprite = devSprite;
                break;
        }

        //Change les alphas pour afficher l'image du joueur ou du stranger
        Color playerColor = playerImg.color;
        Color strangerColor = strangerImg.color;
        if (dialogDatabase.dialogData[counter].speaker == DIALOG_SPEAKER.NAUFRAGE)
        {
            playerColor.a = 100;
            strangerColor.a = 0;
        }
        else
        {
            strangerColor.a = 100;
            playerColor.a = 0;
        }
        playerImg.color = playerColor;
        strangerImg.color = strangerColor;

        //Remplace le texte du dialogue
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

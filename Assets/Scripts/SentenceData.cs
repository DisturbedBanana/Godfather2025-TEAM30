using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple structure serializable qui décrit une phrase qui sera ajoutable dans la database de dialog
[Serializable]
public struct SentenceData
{
    public DIALOG_SPEAKER speaker;
    public string textContent;

    public SentenceData(DIALOG_SPEAKER speaker, string textContent)
    {
        this.speaker = speaker;
        this.textContent = textContent;
    }
}

//Enum pour savoir qui est la personne qui parle actuellement
public enum DIALOG_SPEAKER
{
    PLAYER,
    STRANGER
}


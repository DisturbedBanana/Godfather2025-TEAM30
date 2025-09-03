using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple structure serializable qui décrit une phrase qui sera ajoutable dans la database de dialog
[Serializable]
public struct SentenceData
{
    public DIALOG_SPEAKER speakerType;
    public string speakerName;
    public string textContent;

    public SentenceData(DIALOG_SPEAKER speakerType, string speakerName, string textContent)
    {
        this.speakerType = speakerType;
        this.speakerName = speakerName;
        this.textContent = textContent;
    }
}

//Enum pour savoir quelle type de personne parle
public enum DIALOG_SPEAKER
{
    PLAYER,
    STRANGER
}


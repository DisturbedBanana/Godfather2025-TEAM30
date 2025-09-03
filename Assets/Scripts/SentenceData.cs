using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public struct SentenceData
{
    public string SpeakerName;
    public string TextContent;

    public SentenceData(string SpeakerName, string TextContent)
    {
        this.SpeakerName = SpeakerName;
        this.TextContent = TextContent;
    }
}


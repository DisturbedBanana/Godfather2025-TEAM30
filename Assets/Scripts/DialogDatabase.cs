using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogDatabase", menuName = "Database/Dialog", order = 0)]
public class SentencesDatabase : ScriptableObject
{
    public List<SentenceData> DialogData = new();
}


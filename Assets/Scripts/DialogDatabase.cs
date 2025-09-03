using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Un simple scriptable object qui fait office de database, juste une liste de SentenceData
[CreateAssetMenu(fileName = "NewDialogDatabase", menuName = "Database/Dialog", order = 0)]
public class DialogDatabase : ScriptableObject
{
    public List<SentenceData> dialogData = new();
}


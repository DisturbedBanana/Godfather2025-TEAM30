using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class LevelNameMovements : MonoBehaviour
{
    [SerializeField] private List<Transform> positions = new List<Transform>();

    private void Awake()
    {
        transform.position = positions[0].position;
    }

    // Moves the object through a series of predefined positions while updating its text to display the level name.
    public void MoveObjectAndSetText(string levelName, Action onComplete) 
    {
        if (positions.Count < 4) return;
        
        GetComponent<TextMeshPro>().text = levelName;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(positions[1].position, 0.5f));

        seq.Append(transform.DOMove(positions[2].position, 2f));
        
        seq.Append(transform.DOMove(positions[3].position, 0.5f));

        seq.OnComplete(() => onComplete?.Invoke());
    }
    
    public void ResetPosition() // Resets the object's position to the starting point.
    {
        transform.position = positions[0].position;
    }
}
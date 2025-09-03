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

    public void MoveObjectAndSetText(string levelName, Action onComplete)
    {
        if (positions.Count < 4) return;
        
        GetComponent<TextMeshPro>().text = levelName;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(positions[1].position, 0.5f)
            .OnStart(() => Debug.Log("Moving fast from 1 to 2")));

        seq.Append(transform.DOMove(positions[2].position, 2f)
            .OnStart(() => Debug.Log("Moving slow from 2 to 3")));
        
        seq.Append(transform.DOMove(positions[3].position, 0.5f)
            .OnStart(() => Debug.Log("Moving fast from 3 to 4"))
            .OnComplete(ResetPosition));
        
        seq.OnComplete(() => onComplete?.Invoke());
    }
    
    public void ResetPosition()
    {
        transform.position = positions[0].position;
    }
}
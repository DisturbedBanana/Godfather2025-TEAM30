using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Level : MonoBehaviour
{
    [SerializeField] private string _levelName;
    public string LevelName => _levelName;
    
    [SerializeField] private int _par;
    public int Par => _par;
    
    [SerializeField] private List<SpriteRenderer> _wallSpriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        ScoreManager.Instance?.ResetScore(Par);
    }

    [Button("Collect Wall SpriteRenderers")]
    public void CollectWallSpriteRenderers()
    {
        _wallSpriteRenderers.Clear();

        foreach (Transform child in transform)
        {
            if (child.name == "Wall")
            {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    _wallSpriteRenderers.Add(spriteRenderer);
                }
            }
        }
    }
}
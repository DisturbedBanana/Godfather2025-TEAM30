using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D
using DG.Tweening; // For DOTween
using NaughtyAttributes;

public class Lightning : MonoBehaviour
{
    public void StartBlinking()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private void BlinkLightning()
    {
        // Select a random child for each lightning flash
        int childCount = transform.childCount;
        if (childCount <= 0)
        {
            Debug.LogError("Lightning effect requires at least one child with a Light2D component");
            return;
        }
        
        int randomIndex = Random.Range(0, childCount);
        Light2D lightningLight = transform.GetChild(randomIndex).GetComponent<Light2D>();
        
        if (lightningLight == null)
        {
            Debug.LogError($"Child at index {randomIndex} does not have a Light2D component");
            return;
        }

        // Ensure the light is initially off
        lightningLight.intensity = 0;

        // Create a sequence for a more realistic lightning effect
        Sequence lightningSequence = DOTween.Sequence();

        // First flash
        lightningSequence.Append(DOTween.To(
            () => lightningLight.intensity,
            x => lightningLight.intensity = x,
            3f,
            0.05f
        ).SetEase(Ease.OutQuad));

        // Dim slightly
        lightningSequence.Append(DOTween.To(
            () => lightningLight.intensity,
            x => lightningLight.intensity = x,
            0.5f,
            0.07f
        ).SetEase(Ease.InOutQuad));

        // Main bright flash
        lightningSequence.Append(DOTween.To(
            () => lightningLight.intensity,
            x => lightningLight.intensity = x,
            5f,
            0.1f
        ).SetEase(Ease.OutExpo));

        // Fade out
        lightningSequence.Append(DOTween.To(
            () => lightningLight.intensity,
            x => lightningLight.intensity = x,
            0f,
            0.2f
        ).SetEase(Ease.InExpo));
    }
    
    private IEnumerator BlinkCoroutine()
    {
        yield return new WaitForSeconds(5f);
        InvokeRepeating(nameof(BlinkLightning), 0f, 10f);
    }
}
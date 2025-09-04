using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class SunAndMoon : MonoBehaviour
{
    //Make a method that rotates the object for 180° each time the method is triggered
    [Button]
    public void Rotate()
    {
        transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z + 180), 1f).SetEase(Ease.InOutElastic);
    }
}

using UnityEngine;

public class ButtonTween : MonoBehaviour
{
    public void OnClick()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.5f)
                 .setEaseInBack();
    }
}
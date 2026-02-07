using UnityEngine;

public class ActivationRadiusSprite : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float activationRadius = 3f;
    [SerializeField] private GameObject activationSprite;

    private void Start()
    {
        if (activationSprite != null)
            activationSprite.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (activationSprite != null)
            activationSprite.SetActive(distance <= activationRadius);
    }
}
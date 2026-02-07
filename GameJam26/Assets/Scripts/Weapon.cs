using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
    }
}

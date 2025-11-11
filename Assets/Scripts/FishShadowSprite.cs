using UnityEngine;

public class FishShadowSprite : MonoBehaviour
{
    void Update()
    {
        Transform player = PlayerMovement.Instance.transform;
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }
}

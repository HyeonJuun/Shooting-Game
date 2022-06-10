using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bullet_power;
    private float total_power;
    private bool is_move = false;
    private float player_attack_power;
    private void FixedUpdate()
    {
        MoveBullet();
    }
    public void Init(float attack_power, Vector3 muzzle_pos)
    {
        player_attack_power = attack_power;
        transform.position = muzzle_pos;
        total_power = bullet_power + attack_power;
    }

    public void Fire()
    {
        is_move = true;
    }
    void MoveBullet()
    {
        if (is_move == false) return;

        transform.Translate(Vector3.forward, Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) // Enemy
        {
            collision.gameObject.GetComponent<Enemy>().Damage(total_power);
            Destroy(gameObject);
        }

        else if (collision.gameObject.layer == 8) // Bullet Map Boundary
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet_prefab;
    [SerializeField]
    private Transform gun_muzzle;
    [SerializeField]
    private float attack_power;
    [SerializeField]
    private int max_bullet_cnt;
    private int now_bullet_cnt;

    private void Start()
    {
        now_bullet_cnt = max_bullet_cnt;
    }
    private void Update()
    {
        GetFireInput();
    }

    void GetFireInput()
    {
        if (Input.GetMouseButton(0) == false) return;
        if (now_bullet_cnt <= 0)
        {
            Debug.Log("ÃÑ¾Ë ºÎÁ·");
            StartCoroutine(ReLoadBullet());
        }

        Bullet bullet = Instantiate(bullet_prefab, gun_muzzle.position, gun_muzzle.rotation).GetComponent<Bullet>();

        bullet.Init(attack_power, gun_muzzle.position);
        bullet.Fire();

       // max_bullet_cnt--;
        //bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    }

    IEnumerator ReLoadBullet()
    {
        yield return new WaitForSeconds(1f);
    }
}

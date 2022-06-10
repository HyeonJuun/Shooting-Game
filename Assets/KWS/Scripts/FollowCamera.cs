using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Vector3 origin_cam_pos;
    private bool is_hit = false;
    private void Start()
    {
        origin_cam_pos = transform.position;
    }

    private void Update()
    {
        if (is_hit == true) return;

        Vector3 next_pos = new Vector3(player.position.x, 0f, player.position.z);
        transform.position = origin_cam_pos + next_pos;
    }

    public IEnumerator ShakeCamera()
    {
        is_hit = true;

        Vector3 origin_pos = transform.position;

        for(int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.05f);

            Vector3 temp = transform.position + Random.insideUnitSphere * 1f;
            temp.y = origin_pos.y;

            transform.position = temp;
        }

        is_hit = false;
    }
}

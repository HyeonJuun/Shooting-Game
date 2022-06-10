using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region º¯¼ö
    [SerializeField]
    private float move_power;
    [SerializeField]
    private float rotate_power;

    Vector3 movement_vector;
    #endregion

    private Rigidbody rigid_body;
    private void Start()
    {
        rigid_body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetMoveInput();
    }

    void GetMoveInput()
    {
        movement_vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        Move();
        //Rotate();
        Rotate2();
    }

    void Move()
    {
        if (movement_vector.magnitude == 0)
        {
            rigid_body.velocity = Vector3.zero;
            return;
        }

        //transform.Translate(movement_vector.normalized * Time.deltaTime * move_power, Space.World);
        //rigid_body.AddForce(movement_vector.normalized * move_power, ForceMode.Force);
        rigid_body.velocity = movement_vector * move_power * Time.deltaTime;
    }

    void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            Vector3 look = new Vector3(hit.point.x, 0f, hit.point.z);
            transform.LookAt(look);
        }
    }
    
    void Rotate2()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 3f);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 look = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            transform.forward = look;
        }
    }
}

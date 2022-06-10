using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int attack_power = 50;
    private float hp;
    private Material material;
    private Color origin_color;
    private NavMeshAgent nav_mesh_agent;
    private GameObject player;
    private Rigidbody rigid;
    private int enemy_speed;

    public delegate void Report_Death_Callback();
    public Report_Death_Callback report_death_callback;

    [SerializeField]
    private GameObject weapon;
    private Coroutine is_doing_hit_cor = null;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        nav_mesh_agent = GetComponent<NavMeshAgent>();
        origin_color = material.color;
        player = GameObject.Find("Player");
        rigid = this.GetComponent<Rigidbody>();
        //StartCoroutine(FollowingPlayer());
    }

    private void Update()
    {
        rigid.velocity = (player.transform.position - this.transform.position).normalized * enemy_speed;
    }
    IEnumerator FollowingPlayer()
    {
        while(true)
        {
            yield return null;
            nav_mesh_agent.SetDestination(player.transform.position);
        }
    }

    public void Damage(float damage)
    {
        StartCoroutine(DamageEffect());
        DownHp(damage);
    }

    public void Init(Report_Death_Callback report_death_callback, Vector3 spwan_pos, int hp, int speed)
    {
        this.hp = hp;
        this.report_death_callback = report_death_callback;
        //nav_mesh_agent.speed = speed;
        enemy_speed = speed;
        transform.position = spwan_pos;
    }
    void DownHp(float damage)
    {
        hp -= damage;

        Debug.Log("HP: " + hp);

        if(hp <= 0)
        {
            Debug.Log("Àû Á×À½.");
            Death();
        }
    }

    void Death()
    {
        report_death_callback();
        Destroy(this.gameObject);
    }
    IEnumerator DamageEffect()
    {
        for(int i = 0; i < 2; i++)
        {
            material.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            material.color = origin_color;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9)// Player
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 9) // Player
        {
            if (is_doing_hit_cor == null)
            {
                PlayerState player_state = other.gameObject.GetComponent<PlayerState>();
                is_doing_hit_cor = StartCoroutine(Hit(player_state));
            }
        }
    }

    IEnumerator Hit(PlayerState player_state)
    {
        player_state.Damage(attack_power);
        
        yield return new WaitForSeconds(1f);

        is_doing_hit_cor = null;
    }
}

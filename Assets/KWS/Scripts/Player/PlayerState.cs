using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    private GameObject m_goHpBar;

    private bool isDead = false;
    [SerializeField]
    private GameManager gamemanager;

    [SerializeField]
    private float hp;
    [SerializeField]
    private UIManager ui_manager;
    private Renderer renderer;
    private Color origin_color;
    private float max_hp;
    [SerializeField]
    private FollowCamera follow_camera;

    private void Start()
    {
        max_hp = hp;
        renderer = GetComponent<Renderer>();
        m_goHpBar = GameObject.Find("Canvas/Slider");
        m_goHpBar.transform.GetComponent<Slider>().maxValue = max_hp;
        m_goHpBar.transform.GetComponent <Slider>().value = hp;
        origin_color = renderer.material.color;
    }
    private void Update()
    {
        //m_goHpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 5f, 0));
    }

    public void Damage(int damage)
    {
        LoseHp(damage);
        StartCoroutine(EffectDamaged());
        CheckDeath();

        StartCoroutine(follow_camera.ShakeCamera());
    }

    void LoseHp(int damage)
    {
        hp -= damage;
        //ui_manager.UpdateHp(hp / max_hp);
        m_goHpBar.transform.GetComponent<Slider>().value = hp;
    }

    private void CheckDeath()
    {
        if (hp > 0) return;
        gamemanager.PlayerDeadState();
        
        Debug.Log("Death");
    }


    IEnumerator EffectDamaged()
    {
        for (int i = 0; i < 2; i++)
        {
            renderer.material.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            renderer.material.color = origin_color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

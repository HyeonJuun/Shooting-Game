using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy_prefab;
    [SerializeField]
    private Transform[] spwan_point;
    [SerializeField]
    private StageManager stage_manager;
    [SerializeField]
    private UIManager ui_manager;
    [SerializeField]
    private Text score_text;
    [SerializeField]
    private Text max_score_text;
    [SerializeField]
    private GameObject gameover_pannel;
    public static int score;
    private int max_score;
    public static bool isdead;

    private int now_stage_idx;
    private int now_stage_max_enemy;
    private int now_stage_killed_enemy;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("max_score"))
            PlayerPrefs.SetInt("max_score", 0);

        score = 0;
        score_text.text = score.ToString();
        max_score_text.text = PlayerPrefs.GetInt("max_score").ToString();

    }


    private void Start()
    {
        now_stage_idx = 0;
        now_stage_max_enemy = 0;
        now_stage_killed_enemy = 0;

        int now_stage_num = stage_manager.stage_info[now_stage_idx].stage_num;
        //StartCoroutine(ui_manager.ActiveClearUIDuringTime(3f, now_stage_num.ToString()));
        StartCoroutine(ui_manager.ActiveStartUIDuringTime(3f, now_stage_num.ToString()));
        Invoke("NextStage", 3f);
    }
    private void LateUpdate()
    {
        score_text.text = score.ToString();

    }
    public void PlayerDeadState()
    {
        int maxScore = Mathf.Max(PlayerPrefs.GetInt("max_score"), score);
        PlayerPrefs.SetInt("max_score", maxScore);
        gameover_pannel.transform.GetChild(2).GetComponent<Text>().text = score_text.text;
        gameover_pannel.SetActive(true);
    }
    void NextStage()
    {
        now_stage_max_enemy = stage_manager.stage_info[now_stage_idx].enemy_cnt;

        //StartCoroutine(CreateEnemy(obj, stage_enemy_cnt, create_speed));
        StartCoroutine(CreateEnemy(stage_manager.stage_info[now_stage_idx]));
    }
    void FinalStage()
    {
        StartCoroutine(CreateEnemy(stage_manager.stage_info[now_stage_idx]));
    }

    IEnumerator CreateEnemy(StageManager.StageInfo now_stage_info)
    {
        if (now_stage_info.stage_num == 6)
        {
            while (true)
            {
                int spwan_ind = Random.Range(0, spwan_point.Length - 1);
                Debug.Log("Final Round , spawn_ind = " + spwan_ind);

                Enemy enemy = Instantiate(now_stage_info.enemy_prefab).GetComponent<Enemy>();
                enemy.Init(ReportDeath, spwan_point[spwan_ind].position, now_stage_info.enemy_hp, now_stage_info.enemy_move_speed, now_stage_info.stage_num);

                yield return new WaitForSeconds(now_stage_info.create_speed);
            }
        }
        else
        {
            for (int i = 0; i < now_stage_info.enemy_cnt; i++)
            {
                int spwan_idx = Random.Range(0, spwan_point.Length - 1);

                Enemy enemy = Instantiate(now_stage_info.enemy_prefab).GetComponent<Enemy>();
                enemy.Init(ReportDeath, spwan_point[spwan_idx].position, now_stage_info.enemy_hp, now_stage_info.enemy_move_speed, now_stage_info.stage_num);

                yield return new WaitForSeconds(now_stage_info.create_speed);
            }
        }
    }
    void ClearStage()
    {
        Debug.Log("Clear!");

        now_stage_idx = (++now_stage_idx);
        now_stage_killed_enemy = 0;
        score += 100;

        if (now_stage_idx == 5)
        {
            StartCoroutine(ui_manager.ActiveClearUIDuringTime(3f, "Final"));
            Invoke("FinalStage", 3f);
            return;
        }

        int now_stage_num = stage_manager.stage_info[now_stage_idx].stage_num;

        StartCoroutine(ui_manager.ActiveClearUIDuringTime(3f, now_stage_num.ToString()));
        Invoke("NextStage", 3f);
    }

    public void ReportDeath()
    {
        now_stage_killed_enemy++;

        if (now_stage_killed_enemy == now_stage_max_enemy && now_stage_idx < 5)
            ClearStage();
        else if (now_stage_idx == 5)
            return;
    }
}

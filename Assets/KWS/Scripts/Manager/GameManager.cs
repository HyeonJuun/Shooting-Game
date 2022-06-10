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

    private int now_stage_idx;
    private int now_stage_max_enemy;
    private int now_stage_killed_enemy;


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
        //if(now_stage_info.stage_num == 5)
        //{
        //    while(true)
        //    {
        //        int spwan_ind = Random.Range(0, spwan_point.Length - 1);
        //        Debug.Log("Final Round , spawn_ind = " + spwan_ind);

        //        Enemy enemy = Instantiate(now_stage_info.enemy_prefab).GetComponent<Enemy>();
        //        enemy.Init(ReportDeath, spwan_point[spwan_ind].position, now_stage_info.enemy_hp, now_stage_info.enemy_move_speed);

        //        yield return new WaitForSeconds(now_stage_info.create_speed);
        //    }
        //}
        //else
        //{
            for (int i = 0; i < now_stage_info.enemy_cnt; i++)
            {
                int spwan_idx = Random.Range(0, spwan_point.Length - 1);

                Enemy enemy = Instantiate(now_stage_info.enemy_prefab).GetComponent<Enemy>();
                enemy.Init(ReportDeath, spwan_point[spwan_idx].position, now_stage_info.enemy_hp, now_stage_info.enemy_move_speed);

                yield return new WaitForSeconds(now_stage_info.create_speed);
            }
        //}

    }
    void ClearStage()
    {
        Debug.Log("Clear!");

        now_stage_idx = (++now_stage_idx);
        now_stage_killed_enemy = 0;
        //if(now_stage_idx >= 5)
        //{
        //    now_stage_idx--;
        //    StartCoroutine(ui_manager.ActiveClearUIDuringTime(3f, "Final"));
        //    StartCoroutine(CreateEnemy(stage_manager.stage_info[now_stage_idx]));
        //    //Invoke("CreateEnemy", 3f);
        //}

        int now_stage_num = stage_manager.stage_info[now_stage_idx].stage_num;

        StartCoroutine(ui_manager.ActiveClearUIDuringTime(3f, now_stage_num.ToString()));
        Invoke("NextStage", 3f);
    }

    public void ReportDeath()
    {
        now_stage_killed_enemy++;

        if (now_stage_killed_enemy == now_stage_max_enemy /*&& now_stage_idx < 5*/)
            ClearStage();
    }
}

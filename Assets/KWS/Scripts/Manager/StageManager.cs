using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Scriptable Object/Data")]
public class StageManager : ScriptableObject
{
    [Serializable]
    public class StageInfo
    {
        public GameObject enemy_prefab;
        public int stage_num;
        public int enemy_cnt;
        public float create_speed;
        public int enemy_hp;
        public int enemy_move_speed;
    }

    public StageInfo[] stage_info;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    [SerializeField] GameObject Monster;
    [SerializeField] GameObject SpawnPos;

    [HideInInspector] public static bool Stage_Start = false;
    int MaxSpawnNum = 20;

    float SpawnTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (0 < GlobalValue.Game_Stage)
        {
            if (Stage_Start)
            {
                SpawnTime -= Time.deltaTime * GlobalValue.Game_Speed;

                if (SpawnTime <= 0.0f)
                {
                    Instantiate(Monster, SpawnPos.transform.position, SpawnPos.transform.rotation, this.transform);
                    GlobalValue.Spawn_Mon_Cnt++;
                    SpawnTime = 1.0f;
                }
            }
        }

        if (GlobalValue.Spawn_Mon_Cnt >= MaxSpawnNum)
        {
            Stage_Start = false;
        }
    }
}

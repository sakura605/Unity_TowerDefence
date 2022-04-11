using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [HideInInspector] public Transform Target;

    AudioSource AS;
    bool TakeDmgOn = false;
    float CheckTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        AS = this.GetComponent<AudioSource>();
        AS.time = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            this.transform.position = this.transform.position;
            return;
        }

        else
        {
            this.transform.position = new Vector3(Target.position.x, 2.0f, Target.position.z);
            if (!TakeDmgOn)
            {
                CheckTime += Time.deltaTime * GlobalValue.Game_Speed;
                if (CheckTime >= 0.2f)
                {
                    AS.Play();
                    Target.GetComponent<MonsterCtrl>().Mon_HP -= GlobalValue.Knight_TW_LV * 20;
                    CheckTime = 0.0f;
                    TakeDmgOn = true;
                }
            }
        }
    }
}

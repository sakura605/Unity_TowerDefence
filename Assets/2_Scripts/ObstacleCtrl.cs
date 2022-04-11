using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCtrl : MonoBehaviour
{
    TowerCtrl TC;
    GameObject Target;
    
    // Start is called before the first frame update
    void Start()
    {
        TC = this.GetComponentInParent<TowerCtrl>();
        Target = TC.Target;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Vector3 TargetPos = Target.transform.position + Vector3.up * 0.5f;
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, GlobalValue.Game_Speed * 0.2f);
        }
        this.transform.Rotate(Vector3.forward * Time.deltaTime * GlobalValue.Game_Speed * 1800.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            TC.TakeDamage();
            //데미지를 준 후 새로운 적을 찾는 함수 넣기
            FindNewTarget();
        }
    }

    void FindNewTarget()
    {
        GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster");
        float shortestDistance = Mathf.Infinity;    //가장 짧은 거리
        GameObject nearestMonster = null;           //가장 가까운 몬스터

        foreach (GameObject Monster in Monsters)     //좀비의 수만큼 반복문을 돌린다.
        {
            float DistanceToMonsters = Vector3.Distance(this.transform.position, Monster.transform.position);
            //좀비와 플레이어간의 거리를 받아온다.

            //현재 위치에서 찾아온 몬스터가 0.5보단 멀리있고 shortestDistance보단 가까이 있다면
            if (0.5f < DistanceToMonsters && DistanceToMonsters < shortestDistance)   //위에서 받아온거리가 전에 받아왔던 거리보다 짧다면
            {
                shortestDistance = DistanceToMonsters;  //거리를 새로 업데이트 해준 후
                nearestMonster = Monster;               //그 몬스터를 가장 가까운 적으로 인식시킨다.
            }
        }

        if (nearestMonster != null && shortestDistance <= 3.0f)    //타겟팅할 적이 있고 거리가 사정거리 안쪽이라면
        {
            Target = nearestMonster;            //타겟팅할 적을 담아 준다.
        }

        else  //타겟팅할 적도 없고 사정거리 바깥이라면
        {
            Target = null;  //타겟을 null로 초기화시킨다.
        }
    }
}

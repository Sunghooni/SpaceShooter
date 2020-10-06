using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    //몬스터의 상태 정보가 있는 Eumerable 변수 선언
    public enum MonsterState { idle, trace, attack, die };
    //몬스터의 현제 상태정보를 저장할 enum변수
    public MonsterState monsterState = MonsterState.idle;

    //속도 향상을 위해 각종 컴포넌트를 변수에 할당
    private Transform monsterTr;
    private Transform playerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;
    private Animator animator;

    //추적 사거리
    public float traceDist = 10.0f;
    //공격 사거리
    public float attackDist = 2.0f;

    //몬스터의 사망여부
    private bool isDie = false;

    //혈흔 효과 프리펩
    public GameObject bloodEffest;
    //혈흔 데칼 효과 프리펩
    public GameObject bloodDecal;

    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        animator = this.gameObject.GetComponent<Animator>();

        //nvAgent.destination = playerTr.position;

        StartCoroutine(this.CheckMonsterState());

        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            //0,2초 동안 기다렸다가 다음으로 넘어감
            yield return new WaitForSeconds(0.2f);
            //몬스터와 플레이어 사이의 거리 측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
    }

    //몬스터의 상태값에 따라 적절한 동작을 수행하는 함수
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                //idle 상태
                case MonsterState.idle:
                    nvAgent.Stop();
                    //Animator의 IsTrace 변수를 false로 설정
                    animator.SetBool("IsTrace", false);
                    break;
                //추적 상태
                case MonsterState.trace:
                    //추적 대상의 위치를 넘겨줌
                    nvAgent.destination = playerTr.position;
                    //추적을 재시작
                    nvAgent.Resume();
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;
                //공격 상태
                case MonsterState.attack:
                    nvAgent.Stop();
                    animator.SetBool("IsAttack", true);
                    break;
            }
            yield return null;
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BULLET")
        {
            //혈흔 효과 함수 호출
            CreateBloodEffect(coll.transform.position);

            Destroy(coll.gameObject);
            animator.SetTrigger("IsHit");
        }
    }
    void CreateBloodEffect(Vector3 pos)
    {
        //혈흔 효과 생성
        GameObject blood1 = (GameObject)Instantiate(bloodEffest, pos, Quaternion.identity);
        Destroy(blood1, 2.0f);

		//데칼 생성위치 - 바닥에서 조금 올린 위치 산출
		Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);
		//데칼의 회전값을 무작위로 설정
		Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0,360));

		//데칼 프켑 생성
		GameObject blood2 = (GameObject) Instantiate(bloodDecal, decalPos,decalRot);
		//데칼의 크기도 불칙적으로 나타나게끔 스케일 조정
		float scale = Random.Range(1.5f, 3.5f);
		blood2.transform.localScale = Vector3.one * scale;

		//5초 후에 혈흔효과 프리펩을 삭제
		Destroy(blood2, 5.0f);
    }
}

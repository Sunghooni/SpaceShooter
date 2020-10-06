using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    public Transform targetTr;
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;

    private Transform tr;

	void Start () {
        tr = GetComponent<Transform>();
	}
	
    //LateUpdate를 쓰는 이유는 Player의 Update 실행 이후에 따라 움직이는 역할이기 때문
	void LateUpdate () {
        //Lerp는 선형보간. 즉 부드럽게 따라 움직이는 역할
        //Target의 좌표에서 forward로 dist만큼 떨어져서, up로  height만큼 더 떠서 따라간다.
        tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);

        //다시 Target의 위치정보 받아오기
        tr.LookAt(targetTr.position);
    }
}

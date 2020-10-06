using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour {

    //WASD 사용을 위해 선언
    private float h = 0.0f;
    private float v = 0.0f;

    private Transform tr;

    //이동 속도
    public float moveSpeed = 10.0f;


    //회전속도
    public float rotSpeed = 100.0f;

    public Anim anim;

    public Animation _animation;

	void Start () {
        tr = GetComponent<Transform>();
        _animation = GetComponentInChildren<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
	}
	

	void Update () {
        //WASD 입력 판단
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //Debug에서 출력, 확인
        Debug.Log("H=" + h.ToString());
        Debug.Log("V=" + v.ToString());

        //방향 = 앞뒤 * v(WS) + 양옆 * h(AD)
        //Vector에서 앞뒤(Vector3.forward * v) + 좌우 좌표(Vector3.right * h) 방식은 피타고라스로 대각선(최종 방향) 계산 가능
        //https://hoy.kr/TF6Ny
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //normalized는 Vector 계산에서 대각선은 루트(1 + 1) = 루트 2 = 1.414...로 속도가 빨라지는 것을 막음
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.Self);

        //회전 = up축 기준, 회전속도로 마우스 X좌표로 회전
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        if(v >= 0.1f)
        {
            _animation.CrossFade(anim.runForward.name, 0.3f);
        }
        else if(v <= -0.1f)
        {
            _animation.CrossFade(anim.runBackward.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            _animation.CrossFade(anim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            _animation.CrossFade(anim.runLeft.name, 0.3f);
        }
        else
        {
            _animation.CrossFade(anim.idle.name, 0.3f);
        }
    }
}

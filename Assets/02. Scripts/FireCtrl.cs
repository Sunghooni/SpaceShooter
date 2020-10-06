using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour {
    //총알 프리팹
    public GameObject bullet;
    //총알 발사좌표 
    public Transform firePos;
    //총알 발사 사운드
    public AudioClip fireSfx;
    //AudioSource 컴포넌트를 저장할 번수
    private AudioSource source = null;
    //MuzzleFlash의 MeshRenderer컴포넌트 연결 변수
    public MeshRenderer muzzleFlash;

    void Start()
    {
        //AudioSource 컴포넌트를 추출한 후 변수에 할당
        source = GetComponent<AudioSource>();
        //최초에 MuzzleFlash MeshRenderer를 비활성화
        muzzleFlash.enabled = false;
    }

    void Update () {
		//마우스 왼쪽 버튼을 클릭했을 때 Fire 함수 호출 
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
	}

    void Fire()
    {
        //동적으로 총알을 생성하는 횟수
        CreateBullet();
        //사운드 발생 함수
        source.PlayOneShot(fireSfx, 0.9f);
        //잠시 기다리는 루틴을 위해 코루틴 함수로 호출
        StartCoroutine(this.ShowMuzzleFlash());
    }

    void CreateBullet()
    {
        //Bullet 프리팹을 동적으로 생성
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash 스케일을 불규칙하게 변경
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //MuzzleFlash를 z축 기준으로 불규칙 회전
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;

        //활성화해서 보이게 함
        muzzleFlash.enabled = true;

        //불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));

        //비활성화해서 보이지 않게 함
        muzzleFlash.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour {

    public GameObject expEffect;

    public Texture[] textures;

    private Transform tr;

    private int hitCount = 0;

	void Start () {
        tr = GetComponent<Transform>();

        int idx = Random.Range(0, textures.Length);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
	}

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);

            if(++hitCount >= 3)
            {
                ExpBarrel();
            }
        }
    }

    //드럼통 폭발 함수
    void ExpBarrel () {
        //폭발 효과 파티틀 생성
        Instantiate(expEffect, tr.position, Quaternion.identity);

        //지정 원점 기준으로 10.0f 반경 내에 들어와 있는 Collider 객체 추출
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);

        //추출한 Collider객체에 폭발력 전달
        foreach (Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if(rbody != null)
            {
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000.0f, tr.position, 10.0f, 300.0f);
            }
        }
        Destroy(gameObject, 5.0f);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour {

    public GameObject sparkEffect;

    //충돌이 시작할 때 발생하는 이벤트
    private void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임오브젝트의 태그값 비교
        if(coll.collider.tag == "BULLET")
        {
            GameObject spark = (GameObject)Instantiate(sparkEffect, coll.transform.position, Quaternion.identity);

            Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 0.2f);

            //충돌한 게임오브젝트 삭제
            Destroy(coll.gameObject);
        }
    }
}

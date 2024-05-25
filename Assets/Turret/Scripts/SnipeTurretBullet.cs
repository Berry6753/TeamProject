using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeTurretBullet : MonoBehaviour
{
    [SerializeField]
    private Turret turret;

    private Rigidbody rb;
    private float power = 1000f;

    private Vector3 targetDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        transform.position = turret.snipeTurretFirePos.position;
        targetDir = turret.fireEfect.transform.position - turret.snipeTurretFirePos.position;
        rb.velocity = Vector3.zero;


        rb.AddForce(targetDir * power);
    }

    private void Update()
    {
        if ( Vector3.Distance(transform.position, turret.fireEfect.transform.position) >= turret.turretAttackRange)
        {
            gameObject.SetActive(false);
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.gameObject != turret.gameObject)
            {
                if (other.gameObject.layer != LayerMask.NameToLayer("Monster") && !other.CompareTag("Barrel"))
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    if (other.gameObject.CompareTag("Boss"))
                    {
                        //데미지
                        other.gameObject.GetComponent<BossMonster>().Hurt(turret.turretAttackDamge);
                    }
                    else if (other.gameObject.CompareTag("Monster"))
                    {
                        //이펙트 생성
                        //몬스터 데미지 주는 부분
                        other.gameObject.GetComponent<Monster>().Hurt(turret.turretAttackDamge);
                        //몬스터 함수 불러온단 소리
                    }
                    else if (other.CompareTag("Barrel"))//드럼통일경우
                    {
                        //드럼통 폭발시키기도 있어야함
                        other.gameObject.GetComponent<Barrel>().Hurt();
                    }
                }
            }
            
        }
    }
}

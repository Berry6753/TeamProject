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
                        //������
                    }
                    else if (other.gameObject.CompareTag("Monster"))
                    {
                        //����Ʈ ����
                        //���� ������ �ִ� �κ�
                        other.gameObject.GetComponent<Monster>().Hurt(turret.turretAttackDamge);
                        //���� �Լ� �ҷ��´� �Ҹ�
                    }
                    else if (other.CompareTag("Barrel"))//�巳���ϰ��
                    {
                        //�巳�� ���߽�Ű�⵵ �־����
                        other.gameObject.GetComponent<Barrel>().Hurt();
                    }
                }
            }
            
        }
    }
}

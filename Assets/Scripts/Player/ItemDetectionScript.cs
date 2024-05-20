using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetectionScript : MonoBehaviour
{

    List<Transform> Items = new List<Transform>();

    private void FixedUpdate()
    {
        for(int i = Items.Count - 1; i >= 0; i--)
        {
            Transform item = Items[i];
            if (!item.gameObject.activeSelf)
            {
                Items.Remove(item);
                continue;
            }
            item.position = Vector3.Lerp(item.position, transform.position, 0.1f);
            //item.AddForce((transform.position - body.position) * addMoveFore * Time.fixedDeltaTime);
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Gear"))
        {
            Items.Add(other.GetComponent<Transform>());
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
    //    {
    //        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
    //        Items.Remove(otherRigid);
    //        otherRigid.velocity = Vector3.zero;
    //    }
    //}
}

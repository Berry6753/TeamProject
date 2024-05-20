using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseSkill_Item : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform AttackPoint;
    //[SerializeField]
    //private GameObject objectToThrow;

    [Header("Setting")]
    [SerializeField]
    private int totalThrows;
    [SerializeField]
    private float throwCoolDown;

    [Header("Throwing")]
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float throwUpwardForce;

    private bool readyToThrow;

    private Dictionary<Skill_Item_Info, int> skill_Inven;
    private List<Skill_Item_Info> skill_Item;

    private List<GameObject> targetItem;
    private Skill_Item_Info UseItem;

    private int SelectGetIndex;
    private int SelectUseIndex;

    private int preIndex = -1;

    private Animator animator;

    private readonly int hashThrow = Animator.StringToHash("Throw");
    private readonly int hashHeal = Animator.StringToHash("Heal");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        skill_Inven = new Dictionary<Skill_Item_Info, int>();
        skill_Item = new List<Skill_Item_Info>();
        targetItem = new List<GameObject>();
        SelectGetIndex = 0;
        SelectUseIndex = 0;
        readyToThrow = true;
    }

    public void OnSelectUseItem_1(InputAction.CallbackContext context)
    {
        SelectUseIndex = 0;
    }
    public void OnSelectUseItem_2(InputAction.CallbackContext context)
    {
        SelectUseIndex = 1;
    }
    public void OnSelectUseItem_3(InputAction.CallbackContext context)
    {
        SelectUseIndex = 2;
    }
    public void OnSelectUseItem_4(InputAction.CallbackContext context)
    {
        SelectUseIndex = 3;
    }

    private void Update()
    {
        if (SelectUseIndex != preIndex)
        {
            SelectUseItem();
        }

    }

    private void SelectUseItem()
    {
        if (skill_Inven.Count > 0)
        {
            if (SelectUseIndex >= skill_Inven.Count) return;
            UseItem = skill_Item[SelectUseIndex];
            Item_UI.Instance.ChangeItemIcon(UseItem.ItemIcon);
            Item_UI.Instance.ChangeItemCount(skill_Inven[UseItem]);
            preIndex = SelectUseIndex;
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.started) return;
        if (context.performed)
        {
            if (targetItem.Count <= 0) return;

            Skill_Item_Info PickUpItem = targetItem[SelectGetIndex].GetComponent<Skill_Item_Info>();

            if (PickUpItem == null) return;
            if (skill_Inven.ContainsKey(PickUpItem))
            {
                skill_Inven[PickUpItem]++;
            }
            else
            {
                skill_Inven.Add(PickUpItem, 1);
                skill_Item.Add(PickUpItem);                
            }            

            if(skill_Item.Count <= 1)
            {
                UseItem = PickUpItem;
                Item_UI.Instance.ChangeItemIcon(skill_Item[SelectUseIndex].ItemIcon);
            }

            Item_UI.Instance.ChangeItemCount(skill_Inven[PickUpItem]);

            //setActive(false)
            targetItem[SelectGetIndex].SetActive(false);
            targetItem.RemoveAt(SelectGetIndex);
        }
    }

    public void OnUseItem(InputAction.CallbackContext callback)
    {        
        if(callback.started) return;
        if (callback.performed)
        {
            if (UseItem == null) return;
            if (skill_Inven.ContainsKey(UseItem))
            {
                switch (UseItem.getType) 
                {
                    case ItemType.Attack:
                        if (!readyToThrow) return;
                        readyToThrow = false;
                        animator.SetBool(hashThrow, true);
                        break;
                    case ItemType.Heal:
                        animator.SetBool(hashHeal, true);
                        HealingAnimation();
                        break;
                    case ItemType.Defence:
                        if (!readyToThrow) return;
                        animator.SetBool(hashThrow, true);
                        break;

                }                            
            }
        }
    }

    public void HealingAnimation()
    {
        GameObject healObject = ItemObjectPool.SpawnFromPool(UseItem.EffectPrefab.name, transform.position, Quaternion.Euler(0, transform.rotation.y, 0));
        ConsumeItem();
    }

    public void ThrowAnimation()
    {
        GameObject throwObject = ItemObjectPool.SpawnFromPool(UseItem.EffectPrefab.name, AttackPoint.position, cam.rotation);
        Rigidbody rigidbody = throwObject.GetComponent<Rigidbody>();

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        Vector3 forceToAdd = (ray.GetPoint(20f) - AttackPoint.position).normalized * throwForce + transform.up * throwUpwardForce;

        rigidbody.AddForce(forceToAdd, ForceMode.Impulse);

        ConsumeItem();
    }

    private void ConsumeItem()
    {
        skill_Inven[UseItem]--;
        if (skill_Inven[UseItem] <= 0)
        {
            skill_Inven.Remove(UseItem);
            skill_Item.Remove(UseItem);
        }

        if (SelectUseIndex >= skill_Inven.Count)
        {
            SelectUseIndex = Mathf.Clamp(skill_Inven.Count - 1, 0, skill_Inven.Count - 1);
        }

        if (skill_Item.Count <= 0)
        {
            Item_UI.Instance.ChangeItemIcon();
            Item_UI.Instance.ItemCountNull();
        }
        else
        {
            UseItem = skill_Item[SelectUseIndex];
            Item_UI.Instance.ChangeItemCount(skill_Inven[UseItem]);
            Item_UI.Instance.ChangeItemIcon(UseItem.ItemIcon);
        }
    }

    public void ThrowEnd()
    {
        animator.SetBool(hashThrow, false);
        readyToThrow = true;
    }

    public void HashhealEnd()
    {
        animator.SetBool(hashHeal, false);
    }

    public void HealEnd()
    {
        animator.SetBool(hashHeal, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;
        if (other.CompareTag("Gear")) return;

        targetItem.Add(other.gameObject);        
    }

    private void OnTriggerExit(Collider other)
    {
        if (targetItem.Contains(other.gameObject))
        {
            targetItem.Remove(other.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Item"))
    //    {           
    //        targetItem.Add(collision.gameObject);
    //    }
    //}
    
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (targetItem.Contains(collision.gameObject))
    //    {
    //        targetItem.Remove(collision.gameObject);
    //    }
    //}
}

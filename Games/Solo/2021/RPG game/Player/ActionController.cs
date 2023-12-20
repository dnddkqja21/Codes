using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionController : MonoBehaviour
{
    public Conversation talkManager;

    [SerializeField]
    float range = 3;    // ���� ���� �Ÿ�

    bool pickupActivated = false;   // ���� �����ϸ� true�� ����
    bool talkActivated = false;

    RaycastHit hitInfo;

    RaycastHit hitInfo2;

    [SerializeField]
    LayerMask layerMask;    // ������ ���̾�� ����, �÷��̾� ĳ������ ���� ��ȭ�� ���ؼ� �������� ���̾� ������.

    [SerializeField]
    LayerMask layerMaskNPC;

    [SerializeField]
    TextMeshProUGUI actionText;

    [SerializeField]
    TextMeshProUGUI actionTextNPC;

    Vector3 offSet = new Vector3(0, 0.1f, 0);

    [SerializeField]
    InventoryUI inventory;

    public Animator ani;

    public GameObject[] weapons;    // ���� �迭
    public bool[] hasWeapon;    // �ش� ���⸦ �������� �Ǵ�.
    Weapon equipedWeapon;   // ���� ���� ����
    int weaponIndex = -1;   // �ƹ� ���⵵ ���� �� ������ �⺻ �� ����

    float attackDelay;
    bool isAttackReady;
    bool isComboReady;

    public GameObject[] inactivatedWeapon;  // ������ ���� ����

    public ToolTip tooltip; // �������� ������ ���.

    Joystick joystick;

    public Image attackIcon;

    public GameObject Fist;

    public Player_PF player;

    public AudioSource effectSoundManager;


    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        
        equipedWeapon = Fist.GetComponent<Weapon>();    // �ʱ� ���� ������ ���ָ��� ������
    }

    void Update()
    {
        CheckNPC();
        CheckItem();
        TryAction();
        Debug.DrawRay(transform.position + offSet, transform.TransformDirection(Vector3.forward), Color.red, range);
        Swap();

        AttackDelay();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
        }              
    }

    void AttackDelay()  // ���� �����̴� �׻� ����ؾ� �Ѵ�.
    {
        attackDelay += Time.deltaTime;

        // ������ ���Ӻ��� ��� �ð��� ũ�� ���ݰ���
        isAttackReady = equipedWeapon.attackRate < attackDelay;       
    }   

    public void Attack()    // ui��ư���ε� ������ �� �־�� �ϹǷ� �����̿� ���� �Լ��� ������ ��.
    {
        if (isAttackReady && player.curMP > 15f)
        {
            equipedWeapon.Attack();            

            if (equipedWeapon.attackType == Weapon.Type.Melee)
            {
                // Į�� ���⼭ �����Ἥ ���� ���
                int rand = Random.Range(0, 5);

                switch(rand)
                {
                    case 0:
                        PlayClips(16);
                        ani.SetTrigger("toAttack");
                        break;

                    case 1:
                        PlayClips(17);
                        ani.SetTrigger("toAttack2");
                        break;

                    case 2:
                        PlayClips(23);
                        ani.SetTrigger("toAttack3");
                        break;

                    case 3:
                        PlayClips(16);
                        ani.SetTrigger("toAttack");
                        break;

                    case 4:
                        PlayClips(17);
                        ani.SetTrigger("toAttack2");
                        break;
                }
                
                //ani.SetTrigger("toAttack");
                
            }

            else if (equipedWeapon.attackType == Weapon.Type.Range)
            {
                // ���Ÿ� ����� ����� ���� ������ �������� �б�
                int rand = Random.Range(0, 2);

                switch (rand)
                {
                    case 0:
                        PlayClips(18);
                        break;

                    case 1:
                        PlayClips(19);
                        break;
                }
                ani.SetTrigger("toShot");
            }

            else if (equipedWeapon.attackType == Weapon.Type.Wand)
            {
                int rand = Random.Range(0, 2);

                switch (rand)
                {
                    case 0:
                        PlayClips(20);
                        break;

                    case 1:
                        PlayClips(21);
                        break;
                }
                ani.SetTrigger("toMagicArrow");
            }

            else if (equipedWeapon.attackType == Weapon.Type.Fist)
            {
                ani.SetTrigger("toFist");
            }
            attackDelay = 0;
        }  
    }

    void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {            
            //CheckItem(); ���� �� üũ�������� �ߴ��� �𸣰��� �Ǽ��ΰ�?
            CanPickup();
            StartTalk();
        }
    }

    // ������ üũ
    void CheckItem()
    {
        // lossyScale : �������� ������ �б� ����
        if (Physics.SphereCast(transform.position + offSet, transform.lossyScale.x / 1.7f,
            transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {            
            if(hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }   
            
        }
        
        else
        {
            InfoDisappear();
        }
        
    }

    void CheckNPC()
    {
        if (Physics.SphereCast(transform.position + offSet, transform.lossyScale.x / 1.7f,
            transform.TransformDirection(Vector3.forward), out hitInfo2, range, layerMaskNPC))
        {
            if (hitInfo2.transform.tag == "NPC")
            {                
                TryTalk();
            }
        }
        else
        {
            InfoDisappearNPC();
        }
    }

    void TryTalk()
    {
        actionTextNPC.gameObject.SetActive(true);
        // �� ���Ǿ��� �ִ� ��ũ��Ʈ�� ��������Ʈ�Ͽ� ��ȭ�Ѵ�
        actionTextNPC.text = " \"" + hitInfo2.transform.name + "\" " + "\n��(��) ��ȣ �ۿ��ϱ�(E)Ű";
        talkActivated = true;
    }

    void StartTalk()
    {
        if(talkActivated)
        {
            if(hitInfo2.transform != null)
            {
                // ��ȭâ ����
                talkManager.Talk(hitInfo2.transform);
                //InfoDisappearNPC(); ���Ǿ��� ������ó�� ������� �� �ƴϱ� ������ �𽺾��Ǿ� �ʿ���� ��.
            }
        }
    }

    // ȹ�� ����â ���
    void ItemInfoAppear()
    {        
        pickupActivated = true; // �Ⱦ�����

        // ������ �̸��� �ؽ�Ʈ�� ���
        actionText.gameObject.SetActive(true);
        if(hitInfo.transform.GetComponent<ItemPickup>().item.itemType == Item.ItemType.Equipment)
        {
            actionText.text = " \"" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "\" " + "\n����(E)Ű";
        }
        else
        actionText.text = " \"" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "\" " + "\nȹ��(E)Ű";
    }

    // ���� �����
    void InfoDisappear()
    {
        pickupActivated = false;
        
        actionText.gameObject.SetActive(false);
    }

    void InfoDisappearNPC()
    {
        talkActivated = false;

        actionTextNPC.gameObject.SetActive(false);
    }

    // �Ⱦ�
    void CanPickup()
    {
        if(pickupActivated) // �Ⱦ� ������ ��
        {
            if(hitInfo.transform != null)   // �浹 ���� Ʈ���� ���� �ִٸ�, �������� ��
            {
                if (hitInfo.transform.GetComponent<ItemPickup>().item.itemType == Item.ItemType.Gold && 
                    hitInfo.transform.GetComponent<ItemPickup>().item.itemType != Item.ItemType.Equipment)
                {   // ���� ��� +
                    player.gold += hitInfo.transform.GetComponent<ItemPickup>().item.gold;
                }
                // ��� �������� �ƴϸ�
                else if (hitInfo.transform.GetComponent<ItemPickup>().item.itemType != Item.ItemType.Equipment)
                {
                    // �κ��丮 ���Կ� ������ �߰�
                    inventory.AddSlotItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                }                

                else
                {
                    // ��� �������̸� ������ ��ȣ�� �ش� ���⸦ ������ �ִٰ� �Ǵ�.
                    Item item = hitInfo.transform.GetComponent<ItemPickup>().item;
                    weaponIndex = item.value;
                    hasWeapon[weaponIndex] = true;
                                        
                    // �������� ������ ���� ��ȯ
                    Invoke("InactivatedWeapon", 0.5f);
                }

                Invoke("DestroyItem", 0.5f);

                InfoDisappear();

                effectSoundManager.clip = effectSoundManager.GetComponent<SoundList>().clips[1];
                effectSoundManager.Play();

                ani.SetTrigger("toPickup");
            }
        }    
    }

    void DestroyItem()
    {
        // ������ �±׸� ���� �������� �����ؾ� �÷��̾� ĳ���Ͱ� ����ִ� ���⸦ �������� ����.
        // �׷��� �����ɽ�Ʈ�� �տ� �� Ȱ�� ��� �ִ�.
        // �տ� �� �������� ���̾�� �±׸� �� �� �����ؾ� ��.
        if (hitInfo.transform.tag == "Item")
        {
            //Destroy(hitInfo.transform.gameObject);  // �� ������Ʈ �ı�

            ObjectPool_PF.objectPoolInstance.AddPoolObject(hitInfo.transform.gameObject);
        }
    }
       

    void Swap()
    {       

        if(Input.GetKeyDown(KeyCode.F1))    
        {
            weaponIndex = 0;

            if (inactivatedWeapon != null && hasWeapon[0])   // �ش� ������ ���� �� �������� �ش� �������� �������� ����.
            {
                inactivatedWeapon[weaponIndex].SetActive(false);
                attackIcon.sprite = Resources.Load<Sprite>("Image/UI/set_icon_role_sorcerer");  // ���� ��ư ��������Ʈ ����
            }

            // �ϵ�� ���� ���� ���� ���� ����
            if (hasWeapon[0] != false && hasWeapon[1] != false)
            {
                inactivatedWeapon[1].SetActive(true);
            }

            // �ϵ�� Ȱ�� ���� ���� Ȱ�� ����
            if (hasWeapon[0] != false && hasWeapon[2] != false)
            {
                inactivatedWeapon[2].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            weaponIndex = 1;

            if (inactivatedWeapon != null && hasWeapon[1])
            {
                inactivatedWeapon[weaponIndex].SetActive(false);
                attackIcon.sprite = Resources.Load<Sprite>("Image/UI/set_icon_role_assassin");
            }

            // �˰� �ϵ尡 ���� ���� �ϵ带 ����
            if (hasWeapon[1] != false && hasWeapon[0] != false)
            {
                inactivatedWeapon[0].SetActive(true);
            }

            // �˰� Ȱ�� ���� ���� Ȱ�� ����
            if (hasWeapon[1] != false && hasWeapon[2] != false)
            {
                inactivatedWeapon[2].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            weaponIndex = 2;

            if (inactivatedWeapon != null && hasWeapon[2])
            {
                inactivatedWeapon[weaponIndex].SetActive(false);
                attackIcon.sprite = Resources.Load<Sprite>("Image/UI/set_icon_role_archer");
            }

            // Ȱ�� ���� ���� ���� ���� ����
            if (hasWeapon[2] != false && hasWeapon[1] != false)
            {
                inactivatedWeapon[1].SetActive(true);
            }

            // Ȱ�� �ϵ尡 ���� ���� �ϵ带 ����
            if (hasWeapon[2] != false && hasWeapon[0] != false)
            {
                inactivatedWeapon[0].SetActive(true);
            }
        }

        if ((Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3))
            && hasWeapon[weaponIndex])
        {
            if(equipedWeapon != null)   // �������� ���� ���̶��
            {               
                // ���� ���� �������� ��Ȱ��ȭ
                equipedWeapon.gameObject.SetActive(false);
            }

            Fist.SetActive(false);  // ��� ������ ���� �� �ָ��� ��Ȱ��ȭ

            equipedWeapon = weapons[weaponIndex].GetComponent<Weapon>();    // ���ָ��̾��� ���빫�⸦ �ε��� ����� ����
            equipedWeapon.gameObject.SetActive(true);            
        }        
    }    

    void InactivatedWeapon()
    {
        inactivatedWeapon[weaponIndex].SetActive(true);
    }

    public void ShowTooltip(Item _item, Vector3 _pos)
    {
        tooltip.ItemTooltip(_item, _pos);
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
    }

    public void PlayClips(int _num)
    {
        //effectSoundManager.clip = effectSoundManager.GetComponent<SoundList>().clips[_num];
        effectSoundManager.PlayOneShot(effectSoundManager.GetComponent<SoundList>().clips[_num]);
    }
}

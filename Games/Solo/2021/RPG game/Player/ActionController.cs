using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionController : MonoBehaviour
{
    public Conversation talkManager;

    [SerializeField]
    float range = 3;    // 습득 가능 거리

    bool pickupActivated = false;   // 습득 가능하면 true로 변경
    bool talkActivated = false;

    RaycastHit hitInfo;

    RaycastHit hitInfo2;

    [SerializeField]
    LayerMask layerMask;    // 아이템 레이어에만 반응, 플레이어 캐릭터의 외형 변화를 위해서 착용템은 레이어 미지정.

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

    public GameObject[] weapons;    // 무기 배열
    public bool[] hasWeapon;    // 해당 무기를 가졌는지 판단.
    Weapon equipedWeapon;   // 착용 중인 무기
    int weaponIndex = -1;   // 아무 무기도 없을 때 음수로 기본 값 지정

    float attackDelay;
    bool isAttackReady;
    bool isComboReady;

    public GameObject[] inactivatedWeapon;  // 미착용 중인 무기

    public ToolTip tooltip; // 아이템의 툴팁을 담당.

    Joystick joystick;

    public Image attackIcon;

    public GameObject Fist;

    public Player_PF player;

    public AudioSource effectSoundManager;


    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        
        equipedWeapon = Fist.GetComponent<Weapon>();    // 초기 무기 값으로 맨주먹을 가져옴
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

    void AttackDelay()  // 공격 딜레이는 항상 계산해야 한다.
    {
        attackDelay += Time.deltaTime;

        // 무기의 공속보다 대기 시간이 크면 공격가능
        isAttackReady = equipedWeapon.attackRate < attackDelay;       
    }   

    public void Attack()    // ui버튼으로도 공격할 수 있어야 하므로 딜레이와 어택 함수를 나눠야 함.
    {
        if (isAttackReady && player.curMP > 15f)
        {
            equipedWeapon.Attack();            

            if (equipedWeapon.attackType == Weapon.Type.Melee)
            {
                // 칼은 여기서 랜덤써서 여러 모션
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
                // 원거리 무기는 모션이 같기 때문에 웨폰에서 분기
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
            //CheckItem(); 여기 왜 체크아이템을 했는지 모르겠음 실수인가?
            CanPickup();
            StartTalk();
        }
    }

    // 아이템 체크
    void CheckItem()
    {
        // lossyScale : 절대적인 스케일 읽기 전용
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
        // 각 엔피씨에 있는 스크립트를 겟컴포넌트하여 대화한다
        actionTextNPC.text = " \"" + hitInfo2.transform.name + "\" " + "\n과(와) 상호 작용하기(E)키";
        talkActivated = true;
    }

    void StartTalk()
    {
        if(talkActivated)
        {
            if(hitInfo2.transform != null)
            {
                // 대화창 띄우기
                talkManager.Talk(hitInfo2.transform);
                //InfoDisappearNPC(); 엔피씨는 아이템처럼 사라지는 게 아니기 때문에 디스어피어 필요없을 듯.
            }
        }
    }

    // 획득 인포창 띄움
    void ItemInfoAppear()
    {        
        pickupActivated = true; // 픽업가능

        // 아이템 이름을 텍스트로 출력
        actionText.gameObject.SetActive(true);
        if(hitInfo.transform.GetComponent<ItemPickup>().item.itemType == Item.ItemType.Equipment)
        {
            actionText.text = " \"" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "\" " + "\n착용(E)키";
        }
        else
        actionText.text = " \"" + hitInfo.transform.GetComponent<ItemPickup>().item.itemName + "\" " + "\n획득(E)키";
    }

    // 인포 사라짐
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

    // 픽업
    void CanPickup()
    {
        if(pickupActivated) // 픽업 가능할 때
        {
            if(hitInfo.transform != null)   // 충돌 점에 트랜스 폼이 있다면, 멈춰있을 때
            {
                if (hitInfo.transform.GetComponent<ItemPickup>().item.itemType == Item.ItemType.Gold && 
                    hitInfo.transform.GetComponent<ItemPickup>().item.itemType != Item.ItemType.Equipment)
                {   // 골드면 골드 +
                    player.gold += hitInfo.transform.GetComponent<ItemPickup>().item.gold;
                }
                // 장비 아이템이 아니면
                else if (hitInfo.transform.GetComponent<ItemPickup>().item.itemType != Item.ItemType.Equipment)
                {
                    // 인벤토리 슬롯에 아이템 추가
                    inventory.AddSlotItem(hitInfo.transform.GetComponent<ItemPickup>().item);
                }                

                else
                {
                    // 장비 아이템이면 아이템 번호로 해당 무기를 가지고 있다고 판단.
                    Item item = hitInfo.transform.GetComponent<ItemPickup>().item;
                    weaponIndex = item.value;
                    hasWeapon[weaponIndex] = true;
                                        
                    // 아이템을 먹으면 외형 변환
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
        // 아이템 태그를 가진 아이템을 삭제해야 플레이어 캐릭터가 들고있는 무기를 삭제하지 않음.
        // 그래도 레이케스트가 손에 쥔 활을 쏘고 있다.
        // 손에 쥔 아이템의 레이어와 태그를 둘 다 삭제해야 함.
        if (hitInfo.transform.tag == "Item")
        {
            //Destroy(hitInfo.transform.gameObject);  // 그 오브젝트 파괴

            ObjectPool_PF.objectPoolInstance.AddPoolObject(hitInfo.transform.gameObject);
        }
    }
       

    void Swap()
    {       

        if(Input.GetKeyDown(KeyCode.F1))    
        {
            weaponIndex = 0;

            if (inactivatedWeapon != null && hasWeapon[0])   // 해당 아이템 있을 시 등착에서 해당 아이템을 손착으로 변경.
            {
                inactivatedWeapon[weaponIndex].SetActive(false);
                attackIcon.sprite = Resources.Load<Sprite>("Image/UI/set_icon_role_sorcerer");  // 공격 버튼 스프라이트 변경
            }

            // 완드와 검이 있을 때는 검을 등착
            if (hasWeapon[0] != false && hasWeapon[1] != false)
            {
                inactivatedWeapon[1].SetActive(true);
            }

            // 완드와 활이 있을 때는 활을 등착
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

            // 검과 완드가 있을 때는 완드를 등착
            if (hasWeapon[1] != false && hasWeapon[0] != false)
            {
                inactivatedWeapon[0].SetActive(true);
            }

            // 검과 활이 있을 때는 활을 등착
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

            // 활과 검이 있을 때는 검을 등착
            if (hasWeapon[2] != false && hasWeapon[1] != false)
            {
                inactivatedWeapon[1].SetActive(true);
            }

            // 활과 완드가 있을 때는 완드를 등착
            if (hasWeapon[2] != false && hasWeapon[0] != false)
            {
                inactivatedWeapon[0].SetActive(true);
            }
        }

        if ((Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.F3))
            && hasWeapon[weaponIndex])
        {
            if(equipedWeapon != null)   // 아이템을 착용 중이라면
            {               
                // 착용 중인 아이템을 비활성화
                equipedWeapon.gameObject.SetActive(false);
            }

            Fist.SetActive(false);  // 장비 아이템 착용 시 주먹은 비활성화

            equipedWeapon = weapons[weaponIndex].GetComponent<Weapon>();    // 맨주먹이었던 착용무기를 인덱스 무기로 변경
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

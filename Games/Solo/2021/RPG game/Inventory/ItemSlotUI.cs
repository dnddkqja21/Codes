using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class ItemSlotUI : MonoBehaviour, 
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // 획득한 아이템
    public int itemCount;   // 갯수
    public Image itemImage; // 이미지

    public TextMeshProUGUI countText;   // 아이템 카운트
        
    public GameObject CountImage;   // 갯수가 있는 아이템의 경우 카운트 이미지

    public LoadingBar loading;
    
    ActionController ac;    // 툴팁을 담당하는 액션컨트롤러    

    Player_PF player;    

    Vector3 respawnPos = new Vector3(0, 0, -3.5f);

    NavMeshAgent playerNav;

    void Start()
    {
        // 액션 컨트롤러를 찾아줌.
        ac = FindObjectOfType<ActionController>();
        
        player = FindObjectOfType<Player_PF>();        

        playerNav = player.gameObject.GetComponent<NavMeshAgent>();
    }

    // 아이템 이미지 알파 조정
    void SetColor(float _a)
    {
        Color color = itemImage.color;
        color.a = _a;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment)    // 장비 아닌 경우
        {
            CountImage.SetActive(true);
            countText.text = itemCount.ToString();                        
        }
        else
        {
            countText.text = "0";
            CountImage.SetActive(false);
        }
        SetColor(1);
    }

    // 아이템 갯수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        countText.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        countText.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 시 
        Debug.Log("인벤토리 우클릭");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment)   // 장비일 경우 
                {
                    Debug.Log("착용 미구현");
                }
                else if(item.itemType == Item.ItemType.Quest)   // 퀘스트일 경우
                {
                    Debug.Log("퀘템");
                }
                else
                {
                    switch(item.itemName)
                    {
                        case "체력 포션":
                            Debug.Log("체력 회복");
                            ac.PlayClips(4);
                            player.curHP += item.recoveryValue;
                            break;

                        case "마나 포션":
                            ac.PlayClips(4);
                            player.curMP += item.recoveryValue;
                            break;

                        case "귀환 주문서":
                            Debug.Log("귀환");
                            loading.gameObject.SetActive(true);
                            playerNav.enabled = false;
                            player.transform.position = respawnPos;
                            playerNav.enabled = true;
                            break;

                        case "드래곤 이빨":
                            playerNav.enabled = false;
                            player.transform.position = respawnPos;
                            playerNav.enabled = true;                            
                            break;
                    }
                    SetSlotCount(-1);   // 소모 아이템일 경우 카운트 감소                    
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작");
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;  // 드래그 슬롯에 자신(해당 슬롯)을 대입
            DragSlot.instance.DragSetImage(itemImage);  // 아이템 이미지도 넣어준다.
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 슬롯을 초기화
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;        
    }

    // 다른 슬롯 위에서 드래그가 끝났을 경우 호출
    public void OnDrop(PointerEventData eventData)
    {
        // 빈 슬롯을 드래그할 경우 체인지 슬롯을 호출하지 못하기 위함.
        if(DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    void ChangeSlot()
    {
        // 드래그 앤 드랍이 끝나는 곳의 슬롯의 아이템을 임시 변수에 복사함.
        Item tmp = item;
        int tmpCount = itemCount;

        // 드래그 중인 슬롯의 아이템을 넣어 줌
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        
        if(tmp != null)
        {
            // 드랍이 되는 곳에 아이템이 있다면 교체
            DragSlot.instance.dragSlot.AddItem(tmp, tmpCount);
        }
        else
        {
            // 빈 슬롯이면 드래그 슬롯 클리어
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    // 포인터가 영역 안에 들어왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ac.ShowTooltip(item, transform.position);
        }
    }

    // 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        ac.HideTooltip();
    }    
}

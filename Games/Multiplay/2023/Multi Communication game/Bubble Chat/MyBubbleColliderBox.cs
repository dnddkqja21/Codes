using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 충돌 유지 시 메시지 전달 담당
/// 2초 이상 충돌 시 포톤에 던지는 역할
/// </summary>

public class MyBubbleColliderBox : MonoBehaviourPunCallbacks
{    
    Vector3 createPos = Vector3.zero;
    bool isReady;

    PlayerAttributes myAttribute;
    PlayerAttributes otherAttribute;
    MyBubbleColliderBox otherColliderBox;
    GameObject upAndDownButton;
    Collider other;
    Transform layout;
        

    void Start()
    {
        myAttribute = GetComponentInParent<PlayerAttributes>();
        upAndDownButton = UIManager.Instance.bCUpAndDown;
        upAndDownButton.SetActive(false);
        layout = GameObject.Find("Bubble Layout").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //버블일때 체크

        // 플레이어 접촉 시 유저 속성을 추가
        if(other.isTrigger && other.CompareTag("BubbleCollider"))
        {
            otherAttribute = other.GetComponentInParent<PlayerAttributes>();
            otherColliderBox = other.GetComponent<MyBubbleColliderBox>();
            if (GetComponentInParent<PhotonView>().IsMine)
            {
                this.other = other;
                isReady = ReadyState(transform.position , otherColliderBox.transform.position);

                // test
                ExitGames.Client.Photon.Hashtable myProperties = GetComponentInParent<PhotonView>().Owner.CustomProperties;
                ExitGames.Client.Photon.Hashtable yourProperties = other.GetComponentInParent<PhotonView>().Owner.CustomProperties;
                bool isAccept = false;
                if (Util.GetStr(myProperties, "bubbleAccept").Equals("true") && Util.GetStr(yourProperties, "bubbleAccept").Equals("true"))
                {
                    isAccept = true;
                }

                if (isReady && isAccept)
                {
                    StartCoroutine(CreateMessage());
                }
            }
        }
    }
    public IEnumerator CreateMessage()
    {
        yield return new WaitForSeconds(2f);

        if (this.other == null)
        {
            yield break;
        }
        if (BubbleChat.Instance.isJoining())
        {
            yield break;
        }
        createPos = new Vector3(transform.position.x + other.transform.position.x,
                                                transform.position.y + other.transform.position.y,
                                                transform.position.z + other.transform.position.z) * 0.5f;
        int myCode = ExtractIntFromString(UserData.Instance.avatarData.userSeq);
        int yourCode = ExtractIntFromString(other.GetComponentInParent<PlayerAttributes>().seq); 

        if(myCode > yourCode)
        {
            PhotonManagerWorld.Instance.collisionUser(createPos ,
                UserData.Instance.avatarData.userSeq ,
                other.GetComponentInParent<PlayerAttributes>().seq);
        }
    }
    private int ExtractIntFromString(string input)
    {
        string numericPart = input.Substring(input.LastIndexOf('_') + 1);
        int extractedInt;
        if (int.TryParse(numericPart, out extractedInt))
        {
            return extractedInt;
        }
        else
        {
            return 0; 
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.isTrigger && other.CompareTag("BubbleCollider"))
        {
            if (GetComponentInParent<PhotonView>().IsMine)
            {
                this.other = other;
            }
        }        
    }
   
    bool ReadyState(Vector3 mypos , Vector3 yourPos)
    {
        bool isReady = false;
        bool isMyPos = IsBubbleAble(mypos);
        bool isYourPos = IsBubbleAble(yourPos);

        if (isMyPos && isYourPos)
        {
            isReady = true;
        }

        return isReady;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.isTrigger && other.CompareTag("BubbleCollider"))
        {
            this.other = null;
            otherAttribute = null;
            isReady = false;
        }
    }

    public void Join(int channelName)
    {
        BubbleChat.Instance.Join(channelName);
        SoundManager.Instance.PlaySFX(SFX.BubbleIn);
        
        upAndDownButton.SetActive(true);
    }

    public void Leave()
    {
        BubbleChat.Instance.Leave();
        upAndDownButton.SetActive(false);

        if(layout.childCount > 1)
        {
            for (int i = 0; i < layout.childCount; i++)
            {
                Destroy(layout.GetChild(i).gameObject);
            }
        }
    }
 
    public bool IsBubbleAble(Vector3 position)
    {
        GameObject rectangularObject = GameObject.Find("Start Point");
        Vector3 positionA = rectangularObject.transform.position;
        Vector3 positionB = position;

        Bounds objectRenderer = rectangularObject.GetComponent<Collider>().bounds;

        float halfWidthA = objectRenderer.size.x / 2;
        float halfHeightA = objectRenderer.size.z / 2;

        float minX = positionA.x - halfWidthA;
        float maxX = positionA.x + halfWidthA;
        float minZ = positionA.z - halfHeightA;
        float maxZ = positionA.z + halfHeightA;

        if (positionB.x >= minX && positionB.x <= maxX && positionB.z >= minZ && positionB.z <= maxZ)//안에 있으면
        {
            return false;
        }
        else // 밖이면..
        {
            return true;
        }
    }
}

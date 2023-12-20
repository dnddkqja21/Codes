using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 버블 오브젝트 
/// 특수 효과 및 버블챗 입장, 퇴장
/// </summary>

public class BubbleBox : MonoBehaviourPunCallbacks
{
    [SerializeField] AnimationCurve displacementCurve;
    [SerializeField] float displacementMagnitude;
    [SerializeField] float lerpSpeed;    

    Renderer renderers;
    public int channelName;
    public List<string> joinUserList = new List<string>();
    Collider myCollider;

    public string bubbleMaster;
    public string subMaster;

    int joinUserCount;
    bool enterBubble;

    void Start()
    {
        renderers = GetComponent<Renderer>();
        channelName = GetComponent<PhotonView>().ViewID;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application is quitting");
        // sendUserRPC(false);
        // 잠시 고려중...
    }
    void OnTriggerEnter(Collider other)
    {
        PhotonView view = other.transform.GetComponent<PhotonView>();
        if (view != null && view.IsMine && other.CompareTag("Player"))
        {
            Hashtable bubbleProperties = photonView.Owner.CustomProperties;
            joinUserCount = (int)(bubbleProperties["joinUserCount"] ?? 0);

            // 뒤늦게 들어온 유저는 joinUserList.Count가 0이다.
            //if (joinUserList.Count >= 5)
            //{
            //    //view.RPC("fullRoom",RpcTarget.Others);
            //}
            if(joinUserCount >= 5)
            {
                Debug.Log("버블 챗 인원 제한");
            }
            else
            {
                StopAllCoroutines();
                // 타겟 스케일 (+), 몇초 동안 실행
                StartCoroutine(ScaleUpAndDown(0.15f, 0.3f));
                SoundManager.Instance.PlaySFX(SFX.BubbleIn);

                other.transform.GetComponentInChildren<MyBubbleColliderBox>().Join(channelName);
                SendUserRPC(true, UserData.Instance.avatarData.userSeq);
                enterBubble = true;

                if (myCollider == null)
                {
                    myCollider = other;
                }
            }            
        }
    }

    void OnTriggerExit(Collider other)
    {
        PhotonView view = other.transform.GetComponent<PhotonView>();
        if (view != null && view.IsMine && other.CompareTag("Player") && enterBubble)
        {
            other.transform.GetComponentInChildren<MyBubbleColliderBox>().Leave();
            SendUserRPC(false, UserData.Instance.avatarData.userSeq);
            enterBubble = false;
        }
    }

    public void ConnectOut(string userSeq)
    {
        photonView.RPC("OutMe", RpcTarget.All, channelName.ToString(), userSeq);
    }

    public void SendUserRPC(bool isJoin , string userSeq)
    {
        photonView.RPC(isJoin == true? "JoinMe" : "OutMe", RpcTarget.All, channelName.ToString() , userSeq);
    }

    
    [PunRPC]
    public void JoinMe(string channel , string userSeq)
    {
        if (channelName.ToString().Equals(channel))
        {
            joinUserList.Add(userSeq);
            joinUserCount = joinUserList.Count;
            Hashtable bubbleProperties = new Hashtable();
            bubbleProperties["joinUserCount"] = joinUserCount;
            photonView.Owner.SetCustomProperties(bubbleProperties);
        }
    }

    [PunRPC]
    public void OutMe(string channel, string userSeq)
    {
        if (channelName.ToString().Equals(channel))
        {
            joinUserList.Remove(userSeq);
            joinUserCount = joinUserList.Count;
            Hashtable bubbleProperties = new Hashtable();
            bubbleProperties["joinUserCount"] = joinUserCount;
            photonView.Owner.SetCustomProperties(bubbleProperties);

            if (joinUserList.Count <= 1)
            {
                //남은게 나면
                if (UserData.Instance.avatarData.userSeq.Equals(joinUserList[0]))
                {
                    myCollider.transform.GetComponentInChildren<MyBubbleColliderBox>().Leave();
                }
                RemoveRoom();
            }   
        }
    }

    //private void Update()
    //{
    //    //onny test
    //    if (Input.GetKeyUp(KeyCode.P))
    //    {
    //        Debug.Log(channelName.ToString());
    //        Debug.Log(joinUserList.Count);
    //        foreach(object s in joinUserList)
    //        {
    //            Debug.Log(s);
    //        }
    //    }
    //}

    public void RemoveRoom()
    {
        StartCoroutine(ScaleUpAndDown(-0.15f, 0.3f));
        SoundManager.Instance.PlaySFX(SFX.BubbleOut);
        PhotonNetwork.Destroy(photonView);
    }


    public void HitShield(Vector3 hitPos)
    {        
        renderers.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();        
        StartCoroutine(CoroutineHitDisplacement());

        SoundManager.Instance.PlaySFX(SFX.BubbleIn);
    }
    
    IEnumerator CoroutineHitDisplacement()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            renderers.material.SetFloat("_DisplacementStrength", displacementCurve.Evaluate(lerp) * displacementMagnitude);
            lerp += Time.deltaTime * lerpSpeed;
            yield return null;
        }
    }    

    IEnumerator ScaleUpAndDown(float scaleFactor, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        // Growing phase
        while (elapsedTime < duration / 2f)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / (duration / 2f));
            transform.localScale = Vector3.Lerp(startScale, startScale + scaleFactor * Vector3.one, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Middle phase
        

        // Shrinking phase
        elapsedTime = 0f;
        while (elapsedTime < duration / 2f)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / (duration / 2f));
            transform.localScale = Vector3.Lerp(startScale + scaleFactor * Vector3.one, startScale + scaleFactor * Vector3.one * 0.5f,  t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to original scale
        transform.localScale = startScale + scaleFactor * Vector3.one * 0.5f;        
    }

    public void RoomCreater(string bubbleMaster , string subMaster)
    {
        this.bubbleMaster = bubbleMaster;
        this.subMaster = subMaster;
    }
}

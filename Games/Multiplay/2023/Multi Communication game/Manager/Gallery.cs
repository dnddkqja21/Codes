using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryData
{
    public string title;
    public string description;
}

public class Gallery : MonoBehaviour
{
    List<GalleryData> galleryDatas = new List<GalleryData>();

    // api
    [Header("API 관련")]
    [SerializeField]
    GameObject[] photos;

    // 캔버스
    [Header("캔버스")]
    [SerializeField]
    GameObject photoFrameBG;
    RectTransform[] galleryContents = new RectTransform[3];
    Image image;
    // 0 : 제목, 1 : 설명
    TextMeshProUGUI[] texts = new TextMeshProUGUI[2];
    // 3가지 레이아웃

    void Start()
    {
        // 갤러리 오브젝트 세팅
        image = photoFrameBG.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        galleryContents[(int)GalleryContents.Frame] = photoFrameBG.transform.GetChild(0).GetComponent<RectTransform>();
        galleryContents[(int)GalleryContents.Title] = photoFrameBG.transform.GetChild(1).GetComponent<RectTransform>();
        galleryContents[(int)GalleryContents.Desc] = photoFrameBG.transform.GetChild(2).GetComponent<RectTransform>();
        texts[(int)GalleryContents.Title -1] = galleryContents[(int)GalleryContents.Title].GetComponent<TextMeshProUGUI>();
        texts[(int)GalleryContents.Desc -1] = galleryContents[(int)GalleryContents.Desc].GetComponent<TextMeshProUGUI>();
        photoFrameBG.SetActive(false);

        // 갤러리 api
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.GALLERY, (data) =>
        {
            if (data != null)
            {
                List<Dictionary<string, object>> dataArray = (List<Dictionary<string, object>>)data;
                foreach (var item in dataArray)
                {
                    long index = (long)item["spotKey"];
                    string url = URL_CONFIG.MAIN_BACK + (string)item["imgUrl"];
                    //Debug.Log("인덱스와 주소 : " + index + " / " + url);
                    DebugCustom.Log("인덱스와 주소 : " + index + " / " + url);

                    GalleryData galleryData = new GalleryData();
                    galleryData.title = (string)item["sj"];
                    galleryData.description = (string)item["cn"];
                    galleryDatas.Add(galleryData);

                    StartCoroutine(LoadImage(url, index));
                }
            }
        }));
    }

    IEnumerator LoadImage(string url, long index)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(www.downloadHandler.data);
                //texture.Compress(true); 텍스쳐 깨짐
                photos[index].GetComponent<Renderer>().material.mainTexture = texture;
            }
            else
            {
                //Debug.LogError($"Failed to load image. Error: {www.error}");
                DebugCustom.Log("이미지 파일이 없습니다.");
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            Vector3 touchPosition;            

            if (Input.touchCount > 0)
            {
                //Touch touch = Input.GetTouch(0);
                //if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                //    return;

                touchPosition = Input.GetTouch(0).position;
            }
            else
            {
                //if (EventSystem.current.IsPointerOverGameObject())
                //    return;

                touchPosition = Input.mousePosition;
            }
            if (EventSystem.current.IsPointerOverGameObject() || (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                // Handle UI touch (optional)
                DebugCustom.Log("Touched UI element");
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("PhotoFrame"))
                    {
                        //Debug.Log("클릭한 오브젝트 명 : " + hit.collider.name);
                        DebugCustom.Log("로그 테스트입니다. 클릭한 오브젝트 명 " + hit.collider.name);
                        // 오브젝트의 텍스쳐 검출
                        Texture2D texture = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
                        // 스프라이트를 캔버스에 그려주기 (마지막 파라미터는 피벗이다.)
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                        photoFrameBG.SetActive(true);

                        FrameRatio frameRatio;
                        if (Enum.TryParse(hit.collider.name, out frameRatio))
                        {
                            switch (frameRatio)
                            {
                                case FrameRatio.OneByOne:
                                    SetGalleryContentsProperties(new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                                        new Vector3(150, 0, 0), new Vector2(800, 800), new Vector3(190, 250, 0), new Vector3(500, -100, 0));
                                    break;

                                case FrameRatio.WideLandscape:
                                    SetGalleryContentsProperties(new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                                        new Vector3(150, 0, 0), new Vector2(800, 450), new Vector3(190, 250, 0), new Vector3(500, -100, 0));
                                    break;

                                case FrameRatio.WidePortrait:
                                    SetGalleryContentsProperties(new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                                        new Vector3(300, 0, 0), new Vector2(450, 800), new Vector3(190, 250, 0), new Vector3(500, -100, 0));
                                    break;

                                case FrameRatio.UltraWide:
                                    SetGalleryContentsProperties(new Vector2(0, 0.5f), new Vector2(0, 0.5f),
                                        new Vector3(150, 200, 0), new Vector2(1716, 286), new Vector3(-735, -70, 0), new Vector3(-435, -420, 0));
                                    break;
                            }
                        }
                        image.sprite = sprite;
                        texts[(int)GalleryContents.Title - 1].text = galleryDatas[int.Parse(hit.transform.parent.name) - 1].title;
                        texts[(int)GalleryContents.Desc - 1].text = galleryDatas[int.Parse(hit.transform.parent.name) - 1].description;
                    }
                }
            }
        }
    }

    void SetGalleryContentsProperties(Vector2 framePivot, Vector2 frameAnchor, Vector3 framePosition, Vector2 frameSize, Vector3 titlePosition, Vector3 descPosition)
    {
        galleryContents[(int)GalleryContents.Frame].pivot = framePivot;
        galleryContents[(int)GalleryContents.Frame].anchorMin = frameAnchor;
        galleryContents[(int)GalleryContents.Frame].anchorMax = frameAnchor;
        galleryContents[(int)GalleryContents.Frame].anchoredPosition = framePosition;
        galleryContents[(int)GalleryContents.Frame].sizeDelta = frameSize;

        galleryContents[(int)GalleryContents.Title].anchoredPosition = titlePosition;
        galleryContents[(int)GalleryContents.Desc].anchoredPosition = descPosition;
    }
}

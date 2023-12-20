using UnityEngine;
using UnityEngine.UI;

public class LoadingChangeGreen : MonoBehaviour
{
    [Header("로딩 이미지")]
    public Image loadingImage;

    public float rotateSpeed = 100f;

    void Start()
    {
        loadingImage.rectTransform.eulerAngles = new Vector2(0, 0);
    }

    
    void Update()
    {
        Vector3 temp = loadingImage.rectTransform.eulerAngles;
        temp.z += Time.deltaTime * rotateSpeed;
        loadingImage.rectTransform.eulerAngles = temp;
    }
}

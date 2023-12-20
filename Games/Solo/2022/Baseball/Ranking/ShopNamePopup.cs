using UnityEngine;

/// <summary>
/// Class : ShopNamePopup
/// Desc  : 전국 랭킹에 필요한 매장명 입력기능
/// Date  : 2022-08-25
/// Autor : Kang Cheol Woong

public class ShopNamePopup : MonoBehaviour {     
    
    public UIInput inputName;  

    public void OnShopNamePopup()
    {        
        gameObject.SetActive(true);
    }

    public void OffShopNamePopup()
    {
        gameObject.SetActive(false);
    }

    public void SaveShopName()
    {
        SaveGameSystem.homerunDerbyPlayerInfo.SetShopName(inputName.value); 
        Debug.Log("입력한 매장명 : " + inputName.value);
        gameObject.SetActive(false);
    }
}

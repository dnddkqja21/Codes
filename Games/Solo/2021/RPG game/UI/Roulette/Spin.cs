using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spin : MonoBehaviour
{
    [SerializeField]
    Roulette roulette;

    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI msg;

    Player_PF player;

    ActionController sound;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            roulette.Spin(EndSpin);
        });

        player = FindObjectOfType<Player_PF>();
        sound = FindObjectOfType<ActionController>();
    }

    void EndSpin(PieceData _selectedItem)
    {
        button.interactable = true;
        Debug.Log(_selectedItem.desc);

        sound.PlayClips(26);

        if (_selectedItem.desc == "100골드")
        {
            player.gold += 100;
            msg.gameObject.SetActive(true);
            msg.text = "100골드를 얻었습니다.";
            Invoke("TurnOff", 1.5f);
        }
        else if(_selectedItem.desc == "500골드")
        {
            player.gold += 500;
            msg.gameObject.SetActive(true);
            msg.text = "100골드를 얻었습니다.";
            Invoke("TurnOff", 1.5f);
        }

        else
        {            
            InventoryUI.instance.AddSlotItem(_selectedItem.reward, 1);
            
            msg.gameObject.SetActive(true);
            msg.text = _selectedItem.reward.itemName + "을(를) 얻었습니다.";
            Invoke("TurnOff", 1.5f);
        }
    }

    void TurnOff()
    {
        msg.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빌딩 이름 제어
/// </summary>

public class SetBuildingName : MonoBehaviour
{
    [SerializeField]
    BuildingName buildingName;

    public void OnSetBuildingName()
    {
        GameManager.Instance.buildingName = EnumToData.Instance.BuildingNameToString(buildingName);
    }
}

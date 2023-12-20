using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour
{
    // ������ ��
    [SerializeField]
    Transform piecePrefab;

    [SerializeField]
    Transform linePrefab;

    [SerializeField]
    Transform pieceParent;

    [SerializeField]
    Transform lineParent;

    // ���� ������
    [SerializeField]
    PieceData[] pieceDatas;

    // ȸ��
    [SerializeField]
    int spinTime;

    [SerializeField]
    Transform roulettePanel;

    [SerializeField]
    AnimationCurve speed;

    ActionController sound;

    float pieceAngle;   // ���� �ϳ��� ��ġ�Ǵ� ����
    float halfPieceAngle;   // ���� ����
    float halfPieceAnglePadding; // ���� ���� �е��� ���Ե� ������ ���� 

    int countWeight;
    bool isSpin = false;
    int selectedItem = 0;

    private void Awake()
    {
        pieceAngle = 360 / pieceDatas.Length;
        halfPieceAngle = pieceAngle * 0.5f;
        halfPieceAnglePadding = halfPieceAngle - (halfPieceAngle * 0.25f);

        CreateItem();

        CountWeight();

        sound = FindObjectOfType<ActionController>();
    }

    void CreateItem()
    {
        for (int i = 0; i < pieceDatas.Length; ++i)
        {
            // �귿 ������ ���� �� ���� ����
            Transform piece = Instantiate(piecePrefab, pieceParent.position, Quaternion.identity, pieceParent);

            piece.GetComponent<Piece>().SetPieceData(pieceDatas[i]);

            // ���� ������ŭ ȸ��
            piece.RotateAround(pieceParent.position, Vector3.back, (pieceAngle * i));

            // �� ����
            Transform line = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);
            line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
        }
    }

    void CountWeight()
    {
        for (int i = 0; i < pieceDatas.Length; ++i)
        {
            pieceDatas[i].index = i;

            if (pieceDatas[i].chance <= 0)
                pieceDatas[i].chance = 1;

            countWeight += pieceDatas[i].chance;
            pieceDatas[i].weight = countWeight;

        }
    }

    int GetRandomItem()
    {
        int weight = Random.Range(0, countWeight);

        for (int i = 0; i < pieceDatas.Length; ++i)
        {
            if(pieceDatas[i].weight > weight)
            {
                return i;
            }
        }
        return 0;
    }

    public void Spin(UnityAction<PieceData> action = null)
    {
        if (isSpin == true)
            return;

        selectedItem = GetRandomItem();

        float angle = pieceAngle * selectedItem;

        float leftOffset = (angle - halfPieceAnglePadding) % 360;
        float rightOffset = (angle + halfPieceAnglePadding) % 360;
        float randomAngle = Random.Range(leftOffset, rightOffset);

        int rotateSpeed = 2;
        float targetAngle = (randomAngle + 360 * spinTime * rotateSpeed);

        isSpin = true;

        StartCoroutine(StartSpin(targetAngle, action));
    }

    IEnumerator StartSpin(float _targetAngle, UnityAction<PieceData> _action)
    {
        sound.PlayClips(25);

        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / spinTime;

            float axisZ = Mathf.Lerp(0, _targetAngle, speed.Evaluate(percent));
            roulettePanel.rotation = Quaternion.Euler(0, 0, axisZ);

            yield return null;
        }
        isSpin = false;

        if (_action != null)
            _action.Invoke(pieceDatas[selectedItem]);
    }
}

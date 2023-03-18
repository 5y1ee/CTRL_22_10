using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Collection : MonoBehaviour
{
    [SerializeField] public GameObject Battle_Unit_Ref; // �������� ���� ���Ž� �����ϴ� ��

    [SerializeField] public GameObject Collection_Beast; // �߼� ���� ���
    [SerializeField] public GameObject Collection_Undead; // �𵥵� ���� ���
    [SerializeField] public GameObject Collection_Demon; // ���� ���� ���
    [SerializeField] public GameObject Collection_Spirit; // ���� ���� ���
    [SerializeField] public GameObject Collection_Goblin; // ��� ���� ���
    [SerializeField] public GameObject Collection_Dwarp; // ����� ���� ���
    [SerializeField] public GameObject Collection_DarkElf; // ��ũ���� ���� ���
    [SerializeField] public GameObject Collection_HighElf; // ���̿��� ���� ���
    [SerializeField] public GameObject Collection_Angel; // õ�� ���� ���
    [SerializeField] public GameObject Collection_Human; // �ΰ� ���� ���

    [SerializeField] public GameObject UnlockCountTexts; // �ر� Ƚ�� �ؽ�Ʈ
    [SerializeField] public GameObject CollectionInfo; // ���� ����â
    [SerializeField] public bool[] collectionID; // �÷��� ID ����

    private int unitCnt = 0; // ���� ���� �� ���� �� ���� �����ϴ� ����
    private int unlockUnitStarOne = 0; // 1�� ���� �ر� ��
    private int unlockUnitStarTwo = 0; // 2�� ���� �ر� ��
    private int unlockUnitStarThree = 0; // 3�� ���� �ر� ��

    // ���� ������
    private List<Dictionary<string, object>> unitDatas;

    void Start()
    {
        collectionID = new bool[100];
        unitDatas = CSVReader.Read("DataBase/DB_Using_Character_1");
    }

    void Update()
    {
        // WŰ ������ Ȱ��ȭ/��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(this.transform.GetChild(0).gameObject.activeSelf==true) this.transform.GetChild(0).gameObject.SetActive(false);
            else this.transform.GetChild(0).gameObject.SetActive(true);
            isMouseOvered = false;
        }

        // Unit_parent�� ���� �ö󰡸� ���� üũ (���� ���Ž� ���� üũ)
        if(unitCnt != Battle_Unit_Ref.transform.childCount)
        {
            unitCnt = Battle_Unit_Ref.transform.childCount;
            Character CollectionCharacter;
            for (int i = 0; i < unitCnt; i++)
            {
                CollectionCharacter = Battle_Unit_Ref.transform.GetChild(i).gameObject.GetComponent<Character>();

                Debug.Log(CollectionCharacter.Character_Status_ID);
                // �̹� ���� Ȱ��ȭ���¸� ���� ����X
                if (collectionID[CollectionCharacter.Character_Status_ID] == true) continue;

                // ���� ����
                collectionID[CollectionCharacter.Character_Status_ID] = true;

                GameObject target = getCollectionUnitByID(CollectionCharacter.Character_Status_ID);

                // �̹��� ����
                target.transform.GetChild(0).gameObject.SetActive(true);
                // �� �̹��� (1/2/3��)
                target.transform.GetChild(1).GetChild((CollectionCharacter.Character_Status_ID-1) % 3).GetChild(0).gameObject.SetActive(true);
                // �ر� �� ����
                switch((CollectionCharacter.Character_Status_ID - 1) % 3){
                    case 0:
                        unlockUnitStarOne++;
                        UnlockCountTexts.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1�� �ر� : " +unlockUnitStarOne.ToString()+"��";
                        break;
                    case 1:
                        unlockUnitStarTwo++;
                        UnlockCountTexts.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "2�� �ر� : " + unlockUnitStarTwo.ToString() + "��";
                        break;
                    case 2:
                        unlockUnitStarThree++;
                        UnlockCountTexts.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "3�� �ر� : " + unlockUnitStarThree.ToString() + "��";
                        break;
                }
            }
        }

        if (isMouseOvered) // ���� ������ ���콺 �÷��� ������
        {
            GameObject target = getCollectionUnitByID(mouseOverUnitID);
            if (target == null) return; // ���� ã�� ���� �� ������ null �̸� ���� X

            // ���� infoâ ��ġ���� - x,y�� ���콺 ��ġ��, z�� Ÿ����ġ(���� �� ����)��
            Vector3 newVector = new Vector3(Input.mousePosition.x + CollectionInfo.GetComponent<RectTransform>().rect.width / 2, Input.mousePosition.y - CollectionInfo.GetComponent<RectTransform>().rect.height / 2, target.transform.position.z);
            CollectionInfo.transform.position = newVector;
        }
        else CollectionInfo.transform.position = new Vector3(-999f, -999f); // ���콺 �÷������� ������ �ٸ����� ������

    }
}

#region METHOD
partial class Collection
{
    private int mouseOverUnitID; // ���콺 �ø� ���� ID
    private bool isMouseOvered; // ���콺 �÷ȴ��� �ȿ÷ȴ��� üũ

    // ���� ���� ���콺�ø��� ����â ����
    [SerializeField]
    public void CollectionUnit_OnMouseEnter(int unitID)
    {
        // �Ű������� ���� ID���� �� ������ 1�� ID��
        mouseOverUnitID = unitID;
        isMouseOvered = true;

        if (unitID == 0)
        {
            CollectionInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "???";
            CollectionInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "����";
            CollectionInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Ư��";
            CollectionInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ";
            return;
        }

        if (collectionID[unitID]) // ���� �ر��� �����̸�
        {
            CollectionInfo.transform.GetChild(0).GetChild(unitID/3).gameObject.SetActive(true); // ���� �ʻ�ȭ on
            CollectionInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (string)unitDatas[unitID / 3]["NAME"]; // �̸�
            CollectionInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (string)unitDatas[unitID / 3]["RACE"]; // ����
            CollectionInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (string)unitDatas[unitID / 3]["JOB"]; // Ư��
            CollectionInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "���� ����";
        }

        else // �ر����� ���� �����̸�
        {
            CollectionInfo.transform.GetChild(0).GetChild(unitID / 3).gameObject.SetActive(false); // ���� �ʻ�ȭ off
            CollectionInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "???";
            CollectionInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "����";
            CollectionInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Ư��";
            CollectionInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ���丮�����̳� ��ų���� ";
        }
    }

    // ���� ���� ���콺 ����� ����â ����
    [SerializeField]
    public void CollectionUnit_OnMouseExit()
    {
        isMouseOvered = false;
        CollectionInfo.transform.GetChild(0).GetChild(mouseOverUnitID / 3).gameObject.SetActive(false); // ���� �ʻ�ȭ off
    }

    // ���� ���� �˻�
    private GameObject getCollectionUnitByID(int unitID)
    {
        GameObject target = null;
        switch (unitID)
        {
            case 1:
            case 2:
            case 3:
                // �߼� 005
                target = Collection_Beast.transform.GetChild(1).GetChild(0).gameObject;
                break;
            case 4:
            case 5:
            case 6:
                // ��� 002
                target = Collection_Goblin.transform.GetChild(1).GetChild(0).gameObject;
                break;
            case 7:
            case 8:
            case 9:
                // ���� 002
                target = Collection_Spirit.transform.GetChild(1).GetChild(0).gameObject;
                break;
            case 10:
            case 11:
            case 12:
                // ��� 004
                target = Collection_Goblin.transform.GetChild(1).GetChild(1).gameObject;
                break;
            case 13:
            case 14:
            case 15:
                // ��� 003
                target = Collection_Goblin.transform.GetChild(1).GetChild(2).gameObject;
                break;
            case 16:
            case 17:
            case 18:
                // �߼� 003
                target = Collection_Beast.transform.GetChild(1).GetChild(1).gameObject;
                break;
        }
        return target;
    }
}
#endregion
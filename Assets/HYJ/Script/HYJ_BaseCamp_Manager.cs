using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public partial class HYJ_BaseCamp_Manager : MonoBehaviour
{
    [SerializeField] int Basic_initialize;

    [SerializeField] private int actionCntMax = 10; // �ִ��ൿ����
    [SerializeField] private int actionCnt = 0; // ���� �ൿ����

    [SerializeField] private Text gaugeText; // �ൿ���� ���� �ؽ�Ʈ
    [SerializeField] private Transform gaugeImgs; // ������ ������ �̹�����

    [SerializeField] private int restStack = 0; // �޽� �� ���� �޽Ŀ� �ʿ��� �ൿ���� �����ϱ� ���� ����
    [SerializeField] private int removeStack = 0; // ���ֻ��� �� ���� �޽Ŀ� �ʿ��� �ൿ���� �����ϱ� ���� ����

    int randomSelectUnitNumber1; // ���õ� ���� �ε��� ��ȣ1
    int randomSelectUnitNumber2; // ���õ� ���� �ε��� ��ȣ2
    int randomSelectUnitNumber3; // ���õ� ���� �ε��� ��ȣ3

    GameObject unitList1; // ���ָ���Ʈ 1 (�̹���)
    GameObject unitList2; // ���ָ���Ʈ 2 (�̹���)
    GameObject unitList3; // ���ָ���Ʈ 3 (�̹���)

    Dictionary<string, object> selectedUnit1; // �����ϰ� ���õ� ���� ������1
    Dictionary<string, object> selectedUnit2; // �����ϰ� ���õ� ���� ������1
    Dictionary<string, object> selectedUnit3; // �����ϰ� ���õ� ���� ������1

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    object HYJ_ActiveOn(params object[] _args)
    {
        bool aa = (bool)_args[0];

        //
        this.gameObject.SetActive(aa);

        //
        return null;
    }

    public void HYJ_SetActive(bool _isActive)
    {
        this.gameObject.SetActive(_isActive);

        //
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.MAP___ACTIVE__ACTIVE_ON, !_isActive);
    }

    //////////  Default Method  //////////
    // Start is called before the first frame update
    void Start()
    {
        Basic_initialize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(Basic_initialize)
        {
            case -1:    break;
            //
            case 0:
                {
                    gaugeText = GameObject.Find("GaugeText").transform.GetChild(0).GetComponent<Text>(); // ������ �ؽ�Ʈ
                    gaugeImgs = GameObject.Find("Gauge").transform.GetChild(1); // ������ �̹��� ������
                    this.actionCnt = this.actionCntMax; // �ൿ������ �ִ� �ൿ�����μ���
                    ChangeGaugeUI(); // UI ����

                    Camera camera
                        = (Camera)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(
                            HYJ_ScriptBridge_EVENT_TYPE.MASTER___UI__GET_CAMERA,
                            0);

                    if (camera != null)
                    {
                        this.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = camera;

                        Basic_initialize = 1;
                    }
                }
                break;
            case 1:
                {
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.BASE_CAMP___ACTIVE__ACTIVE_ON, HYJ_ActiveOn);

                    this.HYJ_SetActive(true);

                    Basic_initialize = -1;
                }
                break;
        }
    }
}

// �޼���
partial class HYJ_BaseCamp_Manager {
    // ����
    public void JHW_BaseCamp_Maintenance()
    {
        GameObject.Find("BaseCamp").transform.GetChild(0).transform.GetChild(1).transform.gameObject.SetActive(false); // ���̽�ķ�� ������ �ߴ� �̹��� ��Ȱ��ȭ
        GameObject.Find("UnitList").transform.GetChild(0).transform.gameObject.SetActive(true); // ���̽�ķ�� ���� �����ϴ� �̹��� Ȱ��ȭ

        List<Dictionary<string, object>> unitDatas = CSVReader.Read("DataBase/UnitDataBase");

        // ���� ���� �ε��� �ߺ����� �̱�
        bool[] dataFlag = new bool[unitDatas.Count];
        for (int i = 0; i < unitDatas.Count; i++) dataFlag[i] = false;
        randomSelectUnitNumber1 = Random.Range(0, unitDatas.Count);
        dataFlag[randomSelectUnitNumber1] = true;
        do { randomSelectUnitNumber2 = Random.Range(0, unitDatas.Count); } while (dataFlag[randomSelectUnitNumber2] == true);
        dataFlag[randomSelectUnitNumber2] = true;
        do { randomSelectUnitNumber3 = Random.Range(0, unitDatas.Count); } while (dataFlag[randomSelectUnitNumber3] == true);

        // �����ϰ� ���õ� ����
        selectedUnit1 = unitDatas[randomSelectUnitNumber1];
        selectedUnit2 = unitDatas[randomSelectUnitNumber2];
        selectedUnit3 = unitDatas[randomSelectUnitNumber3];

        GameObject unitList = GameObject.Find("UnitList").transform.GetChild(0).transform.gameObject;
        unitList1 = unitList.transform.GetChild(0).gameObject;
        unitList2 = unitList.transform.GetChild(1).gameObject;
        unitList3 = unitList.transform.GetChild(2).gameObject;

        // ����ī�忡 �������� �ؽ�Ʈ(���� ��ġ��) ����
        ChangeCardUI();
    }

    // �޽�
    public void JHW_BaseCamp_Rest()
    {
        int minusActionCnt = 2 + this.restStack; // ���ҽ�ų �ൿ����
        if (this.actionCnt < minusActionCnt) return; // �ൿ���� �����ϸ� ����X 

        actionCnt -= (2 + this.restStack++); // �޽� ��� �� ���� �޽Ŀ� �ʿ��� �ൿ���� ���� �� �ൿ���� ����

        // 10��ŭ ü��ȸ��
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BASIC__HP_INCREASE, 10);

        ChangeGaugeUI(); // Gauge UI ����
    }

    // ������ư on/off
    public void OnOffDeleteButton(GameObject gameObject)
    {
        unitList1.transform.GetChild(1).gameObject.SetActive(false);
        unitList2.transform.GetChild(1).gameObject.SetActive(false);
        unitList3.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // Gauge UI ����
    public void ChangeGaugeUI()
    {
        this.gaugeText.text = this.actionCnt + "/" + this.actionCntMax;
        for (int i = 0; i < this.actionCntMax; i++)
        {
            this.gaugeImgs.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < this.actionCnt; i++)
        {
            this.gaugeImgs.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }


    // ����
    public void Reroll()
    {
        int minusActionCnt = 1; // ���ҽ�ų �ൿ����
        if (this.actionCnt < minusActionCnt) return; // �ൿ���� �����ϸ� ����X 

        actionCnt -= minusActionCnt; // �޽� ��� �� ���� �޽Ŀ� �ʿ��� �ൿ���� ���� �� �ൿ���� ����

        ChangeGaugeUI(); // ������ UI ����

        List<Dictionary<string, object>> unitDatas = CSVReader.Read("DataBase/UnitDataBase");

        // ���� ���� �ε��� �ߺ����� �̱�
        bool[] dataFlag = new bool[unitDatas.Count];
        for (int i = 0; i < unitDatas.Count; i++) dataFlag[i] = false;
        randomSelectUnitNumber1 = Random.Range(0, unitDatas.Count);
        dataFlag[randomSelectUnitNumber1] = true;
        do { randomSelectUnitNumber2 = Random.Range(0, unitDatas.Count); } while (dataFlag[randomSelectUnitNumber2] == true);
        dataFlag[randomSelectUnitNumber2] = true;
        do { randomSelectUnitNumber3 = Random.Range(0, unitDatas.Count); } while (dataFlag[randomSelectUnitNumber3] == true);
        dataFlag[randomSelectUnitNumber3] = true;

        // �����ϰ� ���õ� ����
        selectedUnit1 = unitDatas[randomSelectUnitNumber1];
        selectedUnit2 = unitDatas[randomSelectUnitNumber2];
        selectedUnit3 = unitDatas[randomSelectUnitNumber3];

        GameObject unitList = GameObject.Find("UnitList").transform.GetChild(0).transform.gameObject;
        unitList1 = unitList.transform.GetChild(0).gameObject;
        unitList2 = unitList.transform.GetChild(1).gameObject;
        unitList3 = unitList.transform.GetChild(2).gameObject;

        // ī�� ���� ��ġ �� �̸� ����
        ChangeCardUI();

        //������ư �Ⱥ��̰�
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.gameObject.SetActive(false);
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform.gameObject.SetActive(false);
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.gameObject.SetActive(false);
    }

    // ī�� ���� ��ġ �� �̸� ����
    public void ChangeCardUI()
    {
        //ī��1
        unitList1.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["NameKor"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["MaxHP"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["MaxMP"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["AttackPower"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(4).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["SpellPower"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(5).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["Defence"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(6).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["SpellResistance"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(7).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["CriticalChance"].ToString();
        unitList1.transform.GetChild(0).transform.GetChild(2).transform.GetChild(8).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit1["CriticalMultiplier"].ToString();

        //ī��2
        unitList2.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["NameKor"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["MaxHP"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["MaxMP"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["AttackPower"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(4).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["SpellPower"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(5).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["Defence"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(6).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["SpellResistance"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(7).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["CriticalChance"].ToString();
        unitList2.transform.GetChild(0).transform.GetChild(2).transform.GetChild(8).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit2["CriticalMultiplier"].ToString();

        //ī��3
        unitList3.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["NameKor"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["MaxHP"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["MaxMP"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(3).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["AttackPower"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(4).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["SpellPower"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(5).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["Defence"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(6).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["SpellResistance"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(7).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["CriticalChance"].ToString();
        unitList3.transform.GetChild(0).transform.GetChild(2).transform.GetChild(8).transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = selectedUnit3["CriticalMultiplier"].ToString();
    }

    public void DeleteUnit(int number)
    {
        int deleteLineNumber=0; //������ UnitDataBase ���� �ε���
        if (number == 1) deleteLineNumber = randomSelectUnitNumber1+3;
        if (number == 2) deleteLineNumber = randomSelectUnitNumber2+3;
        if (number == 3) deleteLineNumber = randomSelectUnitNumber3+3;

        string[] lines;

        // ���� ������ �о����
        lines = File.ReadAllLines("Assets/Resources/DataBase/UnitDataBase.csv");
        // ���� ������ ����
        StreamWriter outStream = System.IO.File.CreateText("Assets/Resources/DataBase/UnitDataBase1.csv");
        // �����Ǵ� ���� ������ ���� ������ �������� ��ü
        lines[deleteLineNumber] = lines[lines.Length-1];
        for (int i = 0; i < lines.Length-1; i++) {outStream.WriteLine(lines[i].ToString());}
        outStream.Close();
    }

    public void DeleteCard(int deleteNumber)
    {
        int minusActionCnt = 1+(this.removeStack++); // ���ҽ�ų �ൿ����
        if (this.actionCnt < minusActionCnt) return; // �ൿ���� �����ϸ� ����X 

        // ���� ����
        DeleteUnit(deleteNumber);

        actionCnt -= minusActionCnt; // �ൿ���� ����

        GameObject.Find("BaseCamp").transform.GetChild(0).transform.GetChild(1).transform.gameObject.SetActive(true); // ���̽�ķ�� UI Ȱ��ȭ
        GameObject.Find("UnitList").transform.GetChild(0).transform.gameObject.SetActive(false); // ���� �� UI ��Ȱ��ȭ

        //������ư �Ⱥ��̰�
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.gameObject.SetActive(false);
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(1).transform.GetChild(1).transform.gameObject.SetActive(false);
        GameObject.Find("UnitList").transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).transform.gameObject.SetActive(false);

        // ������ �̹��� on/off
        ChangeGaugeUI();
    }
}

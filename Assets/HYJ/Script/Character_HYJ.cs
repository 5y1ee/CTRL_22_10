using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class Character
{
    [Header("======================================= STATUS =======================================")]
    [SerializeField] protected CTRL_Character_Data Status_saveData; // ����� Ŭ����

    // �̸�
    [SerializeField] protected string Status_name;
    [SerializeField] protected string Status_name_kor;
    [SerializeField] protected string Status_name_eng;

    // �Ӽ�
    [SerializeField] protected string Status_script;    // ���丮 ��ȣ
    [SerializeField] protected string Status_race;      // ����
    [SerializeField] protected string Status_job;       // Ư��

    // ����
    [SerializeField] protected int Status_mix;  // ���
    [SerializeField] protected int Status_Cost; // �ڽ�Ʈ

    // 
    [SerializeField] protected string Status_atkType; // ���ݹ��
    [SerializeField] protected float Status_MaxHP;  // �ִ�ü��
    [SerializeField] protected float Status_MaxMP;  // �ִ븶��
    [SerializeField] protected float Status_startMp;// ���۸���


    [SerializeField] protected float Status_atkPhysics;  // ����
    [SerializeField] protected float Status_atkSpell;    // ����
    [SerializeField] protected float Status_atkSpeed;    // ����

    [SerializeField] protected float Status_defence;            // ����
    [SerializeField] protected float Status_spellRegistance;    // ����

    // ġ��Ÿ
    [SerializeField] protected int Status_critValue;  // ġ��Ÿ ��ġ
    [SerializeField] protected int Status_critPer;    // ġ��Ÿ Ȯ��

    // ��ų
    [SerializeField] protected string Data_spell0; // �Ϲ� ���� ��ȣ
    [SerializeField] protected string Data_spell1; // ��ų ��ȣ

    //////////  Getter & Setter //////////
    public CTRL_Character_Data HYJ_Status_saveData
    {
        get { return Status_saveData;   }
        set { Status_saveData = value;  }
    }
    virtual public string Character_Status_name_eng { get { return Status_name_eng; } }


    //////////  Method          //////////

    public void HYJ_Status_SettingData(Dictionary<string, object> _data)
    {
        // �̸�
        Status_name = (string)_data["NAME"];
        Status_name_kor = (string)_data["NAME_KOR"];
        Status_name_eng = (string)_data["NAME_ENG"];

        // �Ӽ�
        Status_script = (string)_data["SCRIPT_KOR"];
        Status_race = (string)_data["RACE"];
        Status_job = (string)_data["JOB"];

        // ����
        Status_mix = (int)_data["MIX"];

        // �ڽ�Ʈ
        Status_Cost = (int)_data["COST"];

        // �ɷ�ġ
        Status_atkType = (string)_data["ATK_TYPE"];
        Status_MaxHP = (float)_data["MAX_HP"];
        Status_MaxMP = (float)_data["MAX_MP"];
        Status_startMp = (float)_data["START_MP"];

        // ����
        Status_atkPhysics = (float)_data["ATK_PHYSICS"];
        Status_atkSpell = (float)_data["ATK_SPELL"];
        Status_atkSpeed = (float)_data["ATK_SPEED"];

        // ���
        Status_defence = (float)_data["DEFENCE"];
        Status_spellRegistance = (float)_data["SPELL_REGISTANCE"];

        // ġ��Ÿ
        Status_critPer = (int)_data["CRIT_PERCENT"];
        Status_critValue = (int)_data["CRIT_VALUE"];

        // ��ų
        Data_spell0 = (string)_data["SPELL_0"];
        Data_spell1 = (string)_data["SPELL_1"];


    }

    //////////  Default Method  //////////
}

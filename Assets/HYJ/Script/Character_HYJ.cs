using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class Character
{
    [Header("======================================= HYJ_STATUS =======================================")]
    [SerializeField] protected CTRL_Character_Data Status_saveData; // ����� Ŭ����

    [SerializeField] protected string Status_name;
    // �Ӽ�
    [SerializeField] protected string Status_race;
    [SerializeField] protected string Status_job;
    [SerializeField] protected string Status_affiliation;
    // ����
    [SerializeField] protected int Status_mix;

    [SerializeField] protected int Status_startMp;

    [SerializeField] protected int Status_atkRange;
    [SerializeField] protected int Status_atkPhysics;
    [SerializeField] protected int Status_atkSpell;
    [SerializeField] protected int Status_defence;
    [SerializeField] protected int Status_spellRegistance;

    // ��ų
    [SerializeField] protected int Data_spell0;
    [SerializeField] protected int Data_spell1;

    //////////  Getter & Setter //////////
    public CTRL_Character_Data HYJ_Status_saveData
    {
        get { return Status_saveData;   }
        set { Status_saveData = value;  }
    }

    //////////  Method          //////////

    public void HYJ_Status_SettingData(Dictionary<string, object> _data)
    {
        Status_name = (string)_data["NAME"];
        // �Ӽ�
        Status_race = (string)_data["RACE"];
        Status_job = (string)_data["JOB"];
        Status_affiliation = (string)_data["AFFILIATION"];
        // ����
        Status_mix = (int)_data["MIX"];
        // �ɷ�ġ
        Status_MaxHP = (int)_data["MAX_HP"];
        Status_MaxMP = (int)_data["MAX_MP"];
        Status_startMp = (int)_data["START_MP"];
        Status_atkRange = (int)_data["ATK_RANGE"];
        Status_atkPhysics = (int)_data["ATK_PHYSICS"];
        Status_atkSpell = (int)_data["ATK_SPELL"];
        Status_atkSpeed = (int)((float)_data["ATK_SPEED"]);
        Status_defence = (int)_data["DEFENCE"];
        Status_spellRegistance = (int)_data["SPELL_REGISTANCE"];
        Status_critPer = (int)((float)_data["CRIT_PERCENT"]);
        Status_critValue = (int)((float)_data["CRIT_VALUE"]);
        // ��ų
        Data_spell0 = (int)_data["SPELL_0"];
        Data_spell1 = (int)_data["SPELL_1"];
        // �ڽ�Ʈ
        Status_Cost = (int)_data["COST"];
    }

    //////////  Default Method  //////////
}

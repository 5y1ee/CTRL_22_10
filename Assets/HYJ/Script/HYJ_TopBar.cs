using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using UnityEngine.UI;

// 상단바를 관리하는 클래스
public partial class HYJ_TopBar : MonoBehaviour
{
    //////////  Getter & Setter //////////

    //////////  Method          //////////

    //////////  Default Method  //////////
    // Start is called before the first frame update
    void Start()
    {
        HP_Start();
        HYJ_Level_Start();
        HYJ_Gold_Start();
        HYJ_Power_Start();
        HYJ_Buff_Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

partial class HYJ_TopBar
{
    //////////  Getter & Setter //////////

    //////////  Method          //////////

    //////////  Default Method  //////////
}
# region HP
partial class HYJ_TopBar
{
    [SerializeField] public Slider HP_bar;
    [SerializeField] public Text HP_text;

    object HP_ViewHP(params object[] _args)
    {
        int curHP = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BASIC__CURRENT_HP);
        int maxHP = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BASIC__MAX_HP);

        //
        HP_text.text = curHP +" / "+ maxHP;
        HP_bar.value = (float)curHP / maxHP;

        //
        return true;
    }

    void HP_Start()
    {
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.TOPBAR___HP__VIEW_HP, HP_ViewHP);
    }
}
#endregion HP

#region LEVEL
partial class HYJ_TopBar
{
    [SerializeField] Text Level_text;

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    object HYJ_Level_ViewLevel(params object[] _args)
    {
        int level = (int)_args[0];

        //
        Level_text.text = "Lv. " + level;

        //
        return true;
    }

    //////////  Default Method  //////////
    void HYJ_Level_Start()
    {
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.TOPBAR___LEVEL__VIEW_LEVEL, HYJ_Level_ViewLevel);
    }
}
# endregion

#region GOLD

partial class HYJ_TopBar
{
    [SerializeField] Text Gold_text;

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    object HYJ_Gold_ViewGold(params object[] _args)
    {
        int gold = (int)_args[0];

        //
        Gold_text.text = gold + "G";

        //
        return true;
    }

    //////////  Default Method  //////////
    void HYJ_Gold_Start()
    {
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.TOPBAR___GOLD__VIEW_GOLD, HYJ_Gold_ViewGold);
    }
}

#endregion

#region BATTLE

partial class HYJ_TopBar
{
    [SerializeField] Text Battle_powerText;

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    object HYJ_Battle_ViewPower(params object[] _args)
    {
        int power = (int)_args[0];

        //
        Battle_powerText.text = power + "";

        //
        return true;
    }

    //////////  Default Method  //////////
    void HYJ_Power_Start()
    {
        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set( HYJ_ScriptBridge_EVENT_TYPE.TOPBAR___BATTLE__VIEW_POWER,    HYJ_Battle_ViewPower    );
    }
}

#endregion

#region BUFF

partial class HYJ_TopBar
{
    [Header("==================================================")]
    [Header("BUFF")]
    [SerializeField] Transform      Buff_parent;
    [SerializeField] List<Image>    Buff_buffs;

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    object HYJ_Buff_View(params object[] _args)
    {
        //
        //
        int count = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BUFF__GET_BUFF_COUNT);

        for(int i = 0; i < count; i++)
        {
            CTRL_Buff_Save element = (CTRL_Buff_Save)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BUFF__GET_BUFF_FROM_COUNT, i);

            Buff_buffs[Buff_buffs.Count - 1 - i].gameObject.SetActive(true);
        }

        //
        count = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BUFF__GET_DEBUFF_COUNT);

        for (int i = 0; i < count; i++)
        {
            CTRL_Buff_Save element = (CTRL_Buff_Save)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.PLAYER___BUFF__GET_DEBUFF_FROM_COUNT, i);

            Buff_buffs[i].gameObject.SetActive(true);
        }

        //
        return true;
    }

    //////////  Default Method  //////////
    void HYJ_Buff_Start()
    {
        for(int i = 0; i < Buff_parent.childCount; i++)
        {
            Image image = Buff_parent.GetChild(i).GetComponent<Image>();
            image.gameObject.SetActive(false);

            Buff_buffs.Add(image);
        }

        HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set( HYJ_ScriptBridge_EVENT_TYPE.TOPBAR___BUFF__VIEW,    HYJ_Buff_View   );
    }
}

#endregion
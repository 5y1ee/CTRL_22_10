using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHW_OptionManager : MonoBehaviour
{
    public void optionOnOff(bool arg)
    {
        GameObject optionPopup = GameObject.Find("OptionPopup").transform.GetChild(0).gameObject;
        optionPopup.SetActive(arg);


        // ����
        if(optionPopup.activeSelf==true) // �ɼ� ���� ����
            HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_NAME, JHW_SoundManager.SFX_list.OPTION_OPEN);
        else // �ɼ� Ŭ���� ����
            HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_NAME, JHW_SoundManager.SFX_list.OPTION_CLOSE);
    }
}

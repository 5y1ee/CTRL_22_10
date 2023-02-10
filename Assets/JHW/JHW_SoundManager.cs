using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JHW_SoundManager : MonoBehaviour
{
    [SerializeField] int Basic_initialize;
    [SerializeField] Dictionary<BGM_list, AudioClip> BGM_audioclips = new Dictionary<BGM_list, AudioClip>();
    [SerializeField] Dictionary<SFX_list, AudioClip> SFX_audioclips = new Dictionary<SFX_list, AudioClip>();

    [SerializeField] private float volume_BGM = 1f;
    [SerializeField] private float volume_SFX = 1f;

    [SerializeField] public List<BGM_Datas> BGM_datas = new List<BGM_Datas>();
    [SerializeField] public List<SFX_Datas> SFX_datas = new List<SFX_Datas>();

    private static JHW_SoundManager instance;

    // ���� ȿ����
    private SFX_list toStopSfx;

    private enum SoundType {
        BGM,
        SFX,
    }

    [System.Serializable]
    [SerializeField]
    public struct SFX_Datas
    {
        public SFX_list sfx_name;
        public AudioClip audio;
    }

    [System.Serializable]
    [SerializeField]
    public struct BGM_Datas
    {
        public BGM_list bgm_name;
        public AudioClip audio;
    }


    // ȿ���� ���
    public enum SFX_list
    {
        EQUIP_ON_UNIT,
        UI_CLICK1,
        UI_CLICK2,
        BOW_ATTACK1,
        BOW_ATTACK2,
        BOW_ATTACK3,
        ITEM_UNEQUIP,
        SHOP_SELL,
        HUMAN_005_ATTACK,
        HUMAN_005_CRIT,
        DWARP_001_ATTACK,
        UNLOCK1,
        UNLOCK2,
        UNLOCK3,
        UNLOCK4,
        UPGRADE,
        DARKELF_005_SKILL,
        HIGHELF_002_SKILL,
        HIGHELF_004_SKILL,
        HIGHELF_001_SKILL,
        SPIRIT_007_SKILL1,
        SPIRIT_007_SKILL3,
        SPIRIT_007_SKILL_ICE1,
        SPIRIT_007_SKILL_ICE2,
        SPIRIT_007_SKILL_ICE3,
        DWARP_003_SKILLL,
        CELESTIAL_003_SKILL,
        DEVIL_004_SKILL,
        DEVIL_005_SKILL,
        GOBLIN_006_SKILL,
        DEVIL_002_SKILL,
        UNDEAD_001_SKILL,
        UNIT_DEATH,
        EVENT_OPEN,
        EVENT_SELECT_CHOICE,
        GOLD_GET,
        PAPER_WHIP,
        BUTTON_MOUSEOVER,
        UNIT_PURCHASE,
        ITEM_PURCHASE,
        UNIT_2STAR,
        UNIT_3STAR,
        ITEM_EQUIP,
        BUTTON_CLICK,
        DUNGEON_ENTER,
        SHOP_ENTER,
        BASECAMP_OPEN_UNIT_DELETE_TITLE,
        BASECAMP_DELETE_UNIT,
        RECOVER,
        BUFF_ACTIVE,
        BUFF_DEACTIVE,
        DRINK_POTION,
        UNIT_ARRANGE,
        UNIT_HOLD,
        SYNERGY_FLATINUM,
        SYNERGY_GOLD,
        SYNERGY_SILVER,
        SYNERGY_BRONZE,
        REROLL,
        LEVELUP_PURCHASE_CLICK,
        LEVELUP,
        UNIT_PANEL_OPEN,
        UNIT_DROP,
        UNIT_PANEL_CLOSE,
        BONFIRE,
        BATTLEFIELD_ANY_SOUND,
        OPTION_OPEN,
        OPTION_CLOSE,
        BOOKSHELF_WHIP,
        GAME_VICTORY,
        GAME_DEFEAT,
        ROUND_VICTORY,
        ROUND_DEFEAT
    }

    // ����� ���
    public enum BGM_list
    {
        temp_BGM,
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
        switch (Basic_initialize)
        {
            case -1: break;
            //
            case 0:
                {
                    if (instance == null)
                    {
                        instance = this;
                        DontDestroyOnLoad(instance);
                    }
                    else
                    {
                        //Destroy(gameObject);
                    }
                    Basic_initialize = 1;
                }
                break;
            case 1:
                {
                    // ����Ʈ�� ���� SFX audioClip �� ��� dictionary�� ����
                    for(int i = 0; i < SFX_datas.Count; i++)
                    {
                        if (SFX_datas[i].audio == null) continue; // ȿ���� ������ ����X
                        SFX_audioclips.Add(SFX_datas[i].sfx_name, SFX_datas[i].audio);
                    }
                    // ����Ʈ�� ���� BGM audioClip �� ��� dictionary�� ����
                    for (int i = 0; i < BGM_datas.Count; i++)
                    {
                        if (BGM_datas[i].audio == null) continue; // ����� ������ ����X
                        BGM_audioclips.Add(BGM_datas[i].bgm_name, BGM_datas[i].audio);
                    }
                    // �޼��� ���
                    //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___GET__INSTANCE, JHW_GetSoundManager);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__BGM_NAME, PlayBGM);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_NAME, PlaySFX);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_STOP, SFX_stop);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_ISPLAYING, SFX_isPlaying);

                    Basic_initialize = -1;
                }
                break;
            }
        }


    //// ���� �Ǵ� ȿ���� �ҷ����� �Ǵ� ����
    AudioClip GetOrAddAudioClip(SFX_list name, SoundType st)
    {
        AudioClip audioClip = null;


        // ������� ã�´�
        if (st == SoundType.BGM)
        {
            // ����� Ŭ�� ������ ��ųʸ��� ���̱�
            if (SFX_audioclips.TryGetValue(name, out audioClip) == false)
            {
                // ����� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
                SFX_audioclips.Add(name, audioClip);
            }

            // ã�� ���ϸ� ����
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        // ȿ������ ã�´�
        else if (st == SoundType.SFX)
        {
            // ȿ���� Ŭ�� ������ ��ųʸ��� ���̱�
            if (SFX_audioclips.TryGetValue(name, out audioClip) == false)
            {
                // ȿ���� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/SFX/" + name);
                SFX_audioclips.Add(name, audioClip);
            }

            // ã�� ���ϸ� ����
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        return audioClip;
    }

    //// ����Ŵ��� ��������
    //public static object JHW_GetSoundManager(params object[] _arg)
    //{
    //    Debug.Log("?");
    //    if (instance == null)
    //    {
    //        instance = new JHW_SoundManager();
    //    }
    //    return instance;
    //}


    // ���� ��� - ���
    public object PlayBGM(params object[] _arg)
    {
        // ���� �̸�
        BGM_list playSoundName = (BGM_list)_arg[0];

        // ���� ��ü
        GameObject soundObject = null;

        // ���� �������� BGM �� �ִٸ� �ߴ��ϰ� �ٸ� BGM���� ����
        if (GameObject.Find("SoundManager/BGM").transform.childCount >= 1)
        {
            soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
            AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
            audioSource.clip = BGM_audioclips[playSoundName]; // ���� �ҷ�����
            audioSource.Play(); // ���� ���
        }

        // �������� BGM �� ���ٸ� ���� ������Ʈ ���� �� BGM�� ����
        else
        {
            soundObject = new GameObject("Sound");
            soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            audioSource.clip = BGM_audioclips[playSoundName]; // ���� �ҷ�����
            audioSource.loop = true; // �ݺ����
            audioSource.Play(); // ���� ���
        }

        return true;
    }

    // ���� ��� - ȿ���� (Ǯ�� �����)
    public object PlaySFX(params object[] _arg)
    {
        // ���� �̸�
        SFX_list playSoundName = (SFX_list)_arg[0];

        // ȿ���� Ǯ ������ ����
        GameObject soundPool = GameObject.Find(playSoundName + "Pool");
        if (soundPool == null)
        {
            soundPool = new GameObject(playSoundName + "Pool");
            soundPool.transform.parent = GameObject.Find("SoundManager/SFX").transform;
        }

        // ���� ������Ʈ ����
        GameObject soundObject;
        // ���� Ǯ�� ������Ʈ�� ������ ���� ����� Ǯ�� ����
        if (soundPool.transform.childCount==0)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
        // ���� ������Ʈ ����
        int idx = 0;
        while(idx<soundPool.transform.childCount)
        {
            // ��������� ���� ���� ������Ʈ �� ������ idx ����
            if (soundPool.transform.GetChild(idx).gameObject.activeSelf == true) idx++;
            else break;
        }
        // ���� idx�� pool�� �ִ� �ε������� �����ϸ� ������Ʈ ���� ����� ����
        if (idx==soundPool.transform.childCount)
        {
            // ���� ������Ʈ ����
            soundObject = new GameObject(playSoundName + "Sound");
            soundObject.transform.parent = soundPool.transform; // ����Ǯ�� ����
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            //audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            audioSource.clip = SFX_audioclips[playSoundName];
            audioSource.volume = volume_SFX;
            soundObject.SetActive(false);
        }
            
        // ���������Ʈ ������Ʈ ���� �� Ȱ��ȭ
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // ���� ��� (Play -> PlayOneshot ���� �����)
        //soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(playSoundName, SoundType.SFX),volume_SFX);
        soundObject.GetComponent<AudioSource>().PlayOneShot(SFX_audioclips[playSoundName], volume_SFX * 0.1f);

        // ���� �� ����Ǹ� ��Ȱ��ȭ
        StartCoroutine(soundSetActive(soundObject));

        return true;
    }

    IEnumerator soundSetActive(GameObject soundObject)
    {
        // �����ð� �� ���� ������Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(soundObject.GetComponent<AudioSource>().clip.length);

        soundObject.SetActive(false);
        soundObject.transform.SetAsFirstSibling(); // �ڽ� ������ �̵�
        StopCoroutine(soundSetActive(null));
    } 

    // ��� ��������
    public void BGM_volumeControl()
    {
        volume_BGM = GameObject.Find("BGM Slider").GetComponent<Slider>().value;

        // ������Ʈ �ҷ��ͼ� ���� ����
        if (GameObject.Find("SoundManager/BGM").transform.childCount == 0) return;
        GameObject soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
        AudioSource audioSource = soundObject.GetComponent<AudioSource>();
        audioSource.volume = volume_BGM;
    }

    // ȿ���� ��������
    public void SFX_volumeControl()
    {
        volume_SFX = GameObject.Find("SFX Slider").GetComponent<Slider>().value;

        // SFX Ǯ�� �ִ� ����� �ҷ��ͼ� ���� ����
        GameObject soundPools = GameObject.Find("SoundManager/SFX");

        for(int i=0;i< soundPools.transform.childCount; i++)
        {
            GameObject soundPool = soundPools.transform.GetChild(i).gameObject;
            for(int j = 0; j < soundPool.transform.childCount; j++)
            {
                GameObject soundObject = soundPool.transform.GetChild(j).gameObject;
                AudioSource audioSource = soundObject.GetComponent<AudioSource>();
                audioSource.volume = volume_SFX;
            }
        }
    }
    
    // ���� �Ͻ����� �Ǵ� �÷���
    public void BGM_pauseOrPlay()
    {
        AudioSource audioSource = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        if (audioSource == null) return;
        if (audioSource.isPlaying) audioSource.Pause();
        else audioSource.Play();
    }


    // ���� ���/�Ͻ����� ��ư
    public void BGM_playButton()
    {
        // ���� ������̸� �Ͻ�����, �Ͻ������� ���
        AudioSource audioSource = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        if (audioSource.isPlaying) audioSource.Pause();
        else audioSource.Play();
    }

    // �����ð� �� ȿ���� ����
    public object SFX_stop(params object[] _arg)
    {
        toStopSfx = (SFX_list)_arg[0];
        float playtime = (float)_arg[1];
        Invoke("sfxStop", playtime);
        return true;
    }
    private void sfxStop()
    {
        GameObject.Find(toStopSfx.ToString() + "Pool").transform.GetChild(0).gameObject.SetActive(false);
    }

    // ȿ���� ��������� üũ
    public object SFX_isPlaying(params object[] _arg)
    {
        SFX_list toCheckSfx = (SFX_list)_arg[0];
        if (GameObject.Find(toCheckSfx.ToString() + "Pool").transform.GetChild(0).gameObject.activeSelf == true) return true;
        else return false;
    }
}
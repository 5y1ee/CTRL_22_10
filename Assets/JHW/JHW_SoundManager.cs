using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JHW_SoundManager : MonoBehaviour
{
    [SerializeField] int Basic_initialize;
    [SerializeField] Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    [SerializeField] private float volume_BGM = 1f;
    [SerializeField] private float volume_SFX = 1f; 

    private static JHW_SoundManager instance;

    private enum SoundType {
        BGM,
        SFX,
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
                    // �޼��� ���
                    //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___GET__INSTANCE, JHW_GetSoundManager);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__BGM_NAME, JHW_playBGM);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SFX_NAME, JHW_playSFX);

                    Basic_initialize = -1;
                }
                break;
            }
        }


    // ���� �Ǵ� ȿ���� �ҷ����� �Ǵ� ����
    AudioClip GetOrAddAudioClip(string name, SoundType st)
    {
        AudioClip audioClip = null;
        

        // ������� ã�´�
        if(st == SoundType.BGM)
        {
            // ����� Ŭ�� ������ ��ųʸ��� ���̱�
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // ����� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
                _audioClips.Add(name, audioClip);
            }

            // ã�� ���ϸ� ����
            if (audioClip == null)
                Debug.Log($"AudioClip Missing ! {name}");
        }

        // ȿ������ ã�´�
        else if (st == SoundType.SFX)
        {
            // ȿ���� Ŭ�� ������ ��ųʸ��� ���̱�
            if (_audioClips.TryGetValue(name, out audioClip) == false)
            {
                // ȿ���� �ҷ����� ��ųʸ� ����
                audioClip = Resources.Load<AudioClip>("Sounds/SFX/" + name);
                _audioClips.Add(name, audioClip);
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
    public object JHW_playBGM(params object[] _arg)
    {
        // ���� �̸�
        string playSoundName = (string)_arg[0];

        // ���� ��ü
        GameObject soundObject = null;

        // ���� �������� BGM �� �ִٸ� �ߴ��ϰ� �ٸ� BGM���� ����
        if (GameObject.Find("SoundManager/BGM").transform.childCount >= 1)
        {
            soundObject = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject;
            AudioSource audioSource = soundObject.GetComponent<AudioSource>(); // ������Ʈ �ҷ�����
            audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.BGM); // ���� �ҷ�����
            audioSource.Play(); // ���� ���
        }

        // �������� BGM �� ���ٸ� ���� ������Ʈ ���� �� BGM�� ����
        else
        {
            soundObject = new GameObject("Sound");
            soundObject.transform.parent = GameObject.Find("SoundManager/BGM").transform;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>(); // ������Ʈ ����
            audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.BGM); // ���� �ҷ�����
            audioSource.loop = true; // �ݺ����
            audioSource.Play(); // ���� ���
        }

        return true;
    }

    // ���� ��� - ȿ���� (Ǯ�� �����)
    public object JHW_playSFX(params object[] _arg)
    {
        // ���� �̸�
        string playSoundName = (string)_arg[0];

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
            audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
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
            audioSource.clip = GetOrAddAudioClip(playSoundName, SoundType.SFX);
            soundObject.SetActive(false);
        }
            
        // ���������Ʈ ������Ʈ ���� �� Ȱ��ȭ
        soundObject = soundPool.transform.GetChild(idx).gameObject;
        soundObject.SetActive(true);

        // ���� ��� (Play -> PlayOneshot ���� �����)
        soundObject.GetComponent<AudioSource>().PlayOneShot(GetOrAddAudioClip(playSoundName, SoundType.SFX),volume_SFX);

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

    // ���� ���/�Ͻ����� ��ư
    public void BGM_playButton()
    {
        // ������� ���� ������ ���� ���
        if (GameObject.Find("SoundManager/BGM").transform.childCount == 0)
        {
            JHW_playBGM("Leafre");
            return;
        }
        
        // ���� ������̸� �Ͻ�����, �Ͻ������� ���
        AudioSource audioSource = GameObject.Find("SoundManager/BGM").transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        if (audioSource.isPlaying) audioSource.Pause();
        else audioSource.Play();
    }
}
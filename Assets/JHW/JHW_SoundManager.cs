using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JHW_SoundManager : MonoBehaviour
{
    [SerializeField] int Basic_initialize;

    [SerializeField] Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private static JHW_SoundManager instance;
    

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
                    Debug.Log(instance);
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
                    //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___GET__INSTANCE, JHW_GetSoundManager);
                    HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Set(HYJ_ScriptBridge_EVENT_TYPE.SOUNDMANAGER___PLAY__SOUND_NAME, JHW_playSound);

                    Basic_initialize = -1;
                }
                break;
            }
        }

    // ȿ���� ���
    public void SFXPlay(string sfxName)
    {
        
    }

    // ȿ���� �ҷ����� �Ǵ� ����
    AudioClip GetOrAddAudioClip(string name)
    {
        AudioClip audioClip = null;
        
        // Effect ȿ���� Ŭ�� ������ ��ųʸ��� ���̱�
        if (_audioClips.TryGetValue(name, out audioClip) == false)
        {
            // ���� �ҷ�����
            audioClip = Resources.Load<AudioClip>("Sounds/" + name);

            _audioClips.Add(name, audioClip);
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {name}");

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

    // ���� ���
    public object JHW_playSound(params object[] _arg)
    {
        string playSoundName = (string)_arg[0];

        // ���� ������Ʈ ����
        GameObject sound = new GameObject(playSoundName + "Sound");
        //sound.transform.parent = GameObject.Find("SoundManager").transform;

        // ���� ���
        AudioSource audioSource = sound.AddComponent<AudioSource>();
        audioSource.clip = GetOrAddAudioClip(playSoundName);
        audioSource.Play();

        // ���� �� ����Ǹ� �ı�
        Destroy(sound, audioSource.clip.length);

        return true;
    }
}
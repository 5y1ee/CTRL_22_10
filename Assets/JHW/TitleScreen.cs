using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    // Ÿ��Ʋȭ��

    void Start()
    {
        // Ÿ��Ʋȭ�� Ȱ��ȭ
        this.transform.GetChild(0).gameObject.SetActive(true);
        // Ÿ��Ʋȭ�� ���� ȭ�� ���̵� �� �����
        this.transform.GetChild(0).GetChild(1).GetComponent<Image>().DOFade(0f, 1f).SetDelay(1f);
        // � ȭ�� 2�ʵ� ��Ȱ��ȭ
        Invoke("blackScreenOff", 2f);
    }


    void blackScreenOff()
    {
        this.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    // ���� ���� ��ư
    public void GameStartButton()
    {
        // �κ�ȭ�� ���̵�
        this.transform.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0f, 2f);

        // ��ư disable
        this.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false); // �÷��̹�ư
        this.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false); // ���Ӽ����ư

        // ���̽�ķ�� ���� UX,
        GameObject.Find("BaseCampBackgroundCanvas").GetComponent<Basecamp_background>().StartBaseCamp_BackGround_UX();
        GameObject.Find("BasecampUI").GetComponent<Basecamp_ux>().StartBaseCamp_Button_UX();
        //2�� �� Ÿ��Ʋȭ�� OFF
        Invoke("Basecamp_disable", 2f);
    }

    void Basecamp_disable()
    {
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardBack_UX : MonoBehaviour
{
    // �޸��� ���� Ƚ��. �� ������ ���� Ŭ�� �� �Ǵ� reroll ��ư Ŭ�� �� 0���� �ʱ�ȭ�մϴ�
    public static int checkBackCardCnt;

    // ī�� �޸� Ŭ����
    public void CardBack_Click()
    {
        this.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.3f, RotateMode.Fast);
        this.transform.parent.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 0.3f, RotateMode.Fast).SetDelay(0.3f);
        // �޸� Ŭ�� 3���� �� reroll ��ư Ȱ��ȭ
        if (++checkBackCardCnt >= 3) Check_reroll_able();
    }

    // ī�� ���� �޸� ������ reroll ��ư Ȱ��ȭ
    public void Check_reroll_able()
    {
        // reroll ��ư Ȱ��ȭ
        GameObject.Find("RerollButton").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("RerollButton").transform.GetChild(0).GetComponent<Image>().DOFade(0f, 0f);
        GameObject.Find("RerollButton").transform.GetChild(0).GetComponent<Image>().DOFade(1f, 0.5f).SetDelay(0.5f);
        GameObject.Find("RerollButton").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(0f, 0f);
        GameObject.Find("RerollButton").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f).SetDelay(0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Basecamp_background : MonoBehaviour
{
    public void StartBaseCamp_BackGround_UX()
    {
        // �ʱ� ����
        this.transform.GetChild(0).DOScale(1.2f, 0f);

        // ux����
        this.transform.GetChild(0).DOScale(1f, 4f).SetEase(Ease.OutCubic);
    }

}

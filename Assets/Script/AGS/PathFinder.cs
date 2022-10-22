using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    PriorityQueue<int> PQ = new PriorityQueue<int>();

    [SerializeField]
    Transform HostObjTrans;

    // Start is called before the first frame update
    void Start()
    {
        HostObjTrans = GetComponent<Transform>();
	}

    // Update is called once per frame
    void Update()
    {

	}

    void AStar()
    {
        // Score
        // F = G + H
        // F = ���� ���� ( ���� ���� ���� ��ο� ���� �ٸ� )
        // G = ���������� �ش� ��ǥ���� �̵��ϴµ� ��� ��� ( ���� ���� ���� ��ο� ���� �ٸ� )
        // H = ���������� �󸶳� ������� ( ���� ���� ���� ���� �� )

        // (y, x) �̹� �湮�ߴ��� ���� ( �湮 == closed )
        // �迭���ϸ� �޸� ���� ���. But ����ӵ� Up
        int IdxX = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_FIELD_X);
		int IdxY = (int)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_FIELD_Y);
        bool[,] closed = new bool[IdxX, IdxY];

        //(y, x) ���� ���� �ѹ��̶� �߰��ߴ���
        // �߰� X == MaxValue
        // �߰� O == ( F = G + H )
        int[,] open = new int[IdxX, IdxY];
        for (int y = 0; y < IdxY; ++y)
        {
            for (int x = 0; x < IdxX; ++x)
            {
                open[y, x] = Int32.MaxValue;
            }
        }

        // ������ �߰� ( ���� ���� )
        //open[]
    }

}

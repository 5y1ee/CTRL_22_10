using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bear_Warrior : Character
{
	// Start is called before the first frame update
	void Start()
    {
		// HP �׽�Ʈ �� �ʱ�ȭ
		//Status_MaxHP = 100.0f;
		//Status_HP = 100.0f;
		//Status_atk = 10.0f;			
	}

    // Update is called once per frame
    void Update()
    {

		ChangeState();

	}

}

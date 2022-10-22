using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using System;
using UnityEngine.EventSystems;

public partial class Character : MonoBehaviour
{
	//-------------------------------------------------------------------
	// Field
	//-------------------------------------------------------------------
	[SerializeField] 
    protected Vector3 ori_Pos;
	[SerializeField]
	protected bool IsDead = false;

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------
	public Vector3 LSY_Unit_Position { get { return ori_Pos; } set { ori_Pos = value; } }

	//-------------------------------------------------------------------
	// Method
	//-------------------------------------------------------------------


	// Start is called before the first frame update
	void Start()
    {
		// HP �׽�Ʈ �� �ʱ�ȭ
		Status_MaxHP = 100.0f;
		Status_HP = 50.0f;
        Status_atk = 10.0f;		
	}

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
    {
		if (IsDead)
		{
			// Ǯ�� ���� ���� ������
			Destroy(gameObject);
			return;
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			state = STATE.RUN;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			state = STATE.IDLE;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			state = STATE.SKILL;
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			HitProcess(10.0f);
		}

		ChangeState();

		MoveProcess();
		DieProcess();

	}
}

// ĳ������ �ɷ�ġ
// ������ ������, ���� ���� �ܺο�ҷ� ���� ���ϴ� ��ġ���� �������ֱ� ���� ���Ǵ� ������
#region STATUS

public partial class Character
{
	//-------------------------------------------------------------------
	// Field
	//-------------------------------------------------------------------
	[Header("======================================= STATUS =======================================")]
	[Space (10f)]	

	[SerializeField] protected float Status_HP;     // ü��
    [SerializeField] protected float Status_MaxHP;  // �ִ�ü��
	[Space(10f)]

	[SerializeField] protected int Status_MP;     // ����
    [SerializeField] protected int Status_MaxMP;  // �ִ븶��
	[Space(10f)]

	[SerializeField] protected float Status_atk;    // ���ݷ�
    [SerializeField] protected int Status_magic;  // ����
	[Space(10f)]

	[SerializeField] protected int Status_atkSpeed;   // ����
	[Space(10f)]

	[SerializeField] protected int Status_critValue;  // ġ��Ÿ ��ġ
    [SerializeField] protected int Status_critPer;    // ġ��Ÿ Ȯ��

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------
	public float Stat_HP { get { return Status_HP; } set { Status_HP = value; } }
	public float Stat_MaxHP { get { return Status_MaxHP; } set { Status_MaxHP = value; } }
	public float Stat_Attack { get { return Status_atk; } set { Status_atk = value; } }

	//-------------------------------------------------------------------
	// Method
	//-------------------------------------------------------------------
	virtual public void HitProcess(float Attack)
    {
        if (Status_HP >= Attack)
            Status_HP -= Attack;
        else if (Status_HP < Attack)
        {
			Status_HP = 0.0f;         
		}            
    }

    virtual public void DieProcess()
    {
		if (Status_HP > 0.0f)
			return;
		else
			state = STATE.DIE;

		// Dissolve Shader ���� ����
		// �ϴ� ���� �� ��� Die �ִϸ��̼� ������ �� IsDead true��
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
		  animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		IsDead = true;

	}

    virtual public void MoveProcess()
    {

    }

	//////////  Default Method  //////////
}

#endregion

#region STATE

public partial class Character
{
	public enum STATE
	{
		IDLE,
		RUN,
		DIE,
		SKILL,
		STATE_END
	}

	//-------------------------------------------------------------------
	// Field
	//-------------------------------------------------------------------
	[Header("======================================= STATE =======================================")]
	[Space(10f)]

	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected STATE state = STATE.IDLE;

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------



	//-------------------------------------------------------------------
	// Method
	//-------------------------------------------------------------------
	virtual protected void ChangeState()
	{
		switch (state)
		{
			case STATE.IDLE:
					UpdateIdle();
					break;
			case STATE.RUN:
					UpdateRun();
					break;
			case STATE.DIE:
					UpdateDie();
					break;
			case STATE.SKILL:
					UpdateSkill();
					break;
			default:
					break;
		}
	}

	virtual protected void UpdateIdle()
	{
		animator.ResetTrigger("Skill");
		animator.SetBool("Run Forward", false);
	}

	virtual protected void UpdateRun()
	{
		animator.ResetTrigger("Skill");
		animator.SetBool("Run Forward", true);
	}

	virtual protected void UpdateDie()
	{
		animator.SetBool("Run Forward", false);
		animator.ResetTrigger("Skill");
		animator.SetTrigger("Die");		
	}

	virtual protected void UpdateSkill()
	{		
		animator.SetTrigger("Skill");
	}

}

#endregion

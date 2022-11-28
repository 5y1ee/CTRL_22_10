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
	protected GameObject on_Tile;
	[SerializeField]
	protected bool IsDead = false;
	[SerializeField]
	protected GameObject m_Target = null;
	//[SerializeField]
	//protected int m_CurPosIndex = 0;
	[SerializeField]
	protected PathFinder m_PathFinder = null;

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------
	public Vector3 LSY_Unit_Position { get { return ori_Pos; } set { ori_Pos = value; } }
	public GameObject Target { get { return m_Target; } set { m_Target = value; } }
	//public int CurPosIndex { get { return m_CurPosIndex; } set { m_CurPosIndex = value; } }

	//-------------------------------------------------------------------
	// Method
	//-------------------------------------------------------------------
	public void LSY_Character_Set_OnTile(GameObject tile)
	{
		on_Tile = tile;
	}
	public GameObject LSY_Character_Get_OnTile() { return on_Tile; }

    // Start is called before the first frame update
    void Start()
    {
		ori_Pos = new Vector3();
		// HP �׽�Ʈ �� �ʱ�ȭ
		Status_MaxHP = 100.0f;
		Status_HP = 50.0f;
        Status_atk = 10.0f;
		Status_moveSpeed = 5.0f;
		//CurPosIndex = 0;
	}

	private void Awake()
	{
		m_animator = GetComponentInChildren<Animator>();
		m_PathFinder = GetComponent<PathFinder>();
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
			m_PathFinder.StartPathFinding(gameObject, Target);
		//m_PathFinder.StartPathFinding(gameObject, Target);

		// ���긦 ���߰� �̵����̴� Ÿ�ϱ��� �̵����ϰ� // OnUnit������ �� �������ְ� 
		// �ٽ� StartPathFinding �ѹ� ���ְ� (�������� �����ʰ� bool�����ϳ� ���ְ�)
		// �������� Path�� �����Ӹ��� Clear�ǰ� �ٽ������ϱ� ��� ù��ġ��( �̵����̴� Ÿ�� ó���� �����༭) �����̴�. �׷��� ���ڸ��� ���� �ع���
		// �̸��� == ��ĭ �̵��ϰ� StartPathFinding ���ְ� �ٽ� �Һ��� Ǯ���ְ� �̷������� Target�����ϴ� ��ǥ�������� �ϴ� ���������� �̰� �ݺ�.
		// �̵����̰ų� �������� ���� ������ Ÿ�� üũ�ؼ� �����ϰ� ����� ������������ �Һ��� Ǯ� Start ����
		// ������������ �Һ��� false�� start�ȵ���.
		// Start�� �ȵ��Ҵ�? ���� Path���� == Moveȣ��� ���� �̵����̴� ������ Ÿ�Ϸ� �̵� �Ѵٴ°���
		m_PathFinder.MoveOnPath(gameObject, Target);

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

		BattleProcess();

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
	[SerializeField] protected float Status_moveSpeed;   // ����
	[Space(10f)]

	[SerializeField] protected int Status_critValue;  // ġ��Ÿ ��ġ
    [SerializeField] protected int Status_critPer;    // ġ��Ÿ Ȯ��
    [Space(10f)]

	[SerializeField] protected int Status_Cost;

    //-------------------------------------------------------------------
    // Property
    //-------------------------------------------------------------------
    public float Stat_HP { get { return Status_HP; } set { Status_HP = value; } }
	public float Stat_MaxHP { get { return Status_MaxHP; } set { Status_MaxHP = value; } }
	public float Stat_Attack { get { return Status_atk; } set { Status_atk = value; } }
	public float Stat_MoveSpeed { get { return Status_moveSpeed; } set { Status_moveSpeed = value; } }
	public int Stat_Cost { get { return Status_Cost; } set { Status_Cost = value; } }

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
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
		  m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		IsDead = true;

	}

	virtual public void BattleProcess()
	{

		MoveProcess();
		DieProcess();
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
	protected Animator m_animator;

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
		m_animator.ResetTrigger("Skill");
		m_animator.SetBool("Run Forward", false);
	}

	virtual protected void UpdateRun()
	{
		m_animator.ResetTrigger("Skill");
		m_animator.SetBool("Run Forward", true);
	}

	virtual protected void UpdateDie()
	{
		m_animator.SetBool("Run Forward", false);
		m_animator.ResetTrigger("Skill");
		m_animator.SetTrigger("Die");		
	}

	virtual protected void UpdateSkill()
	{		
		m_animator.SetTrigger("Skill");
	}

}

#endregion

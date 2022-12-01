using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using System;
using UnityEngine.EventSystems;
using static AnimationEvent;
using UnityEngine.UIElements;

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
	[SerializeField]
	protected GameObject m_PreTarget = null;
	//[SerializeField]
	//protected int m_CurPosIndex = 0;
	[SerializeField]
	protected PathFinder m_PathFinder = null;
	[SerializeField]
	protected AnimationEvent m_AnimEvent = null;
	[SerializeField]
	protected UI_StatusBar m_StatusBar = null;

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------
	public Vector3 LSY_Unit_Position { get { return ori_Pos; } set { ori_Pos = value; } }
	public GameObject Target { get { return m_Target; } set { m_Target = value; } }
	public GameObject PreTarget { get { return m_PreTarget; } set { m_PreTarget = value; } }
	public bool Dead { get { return IsDead; } set { IsDead = value; } }
	//public int CurPosIndex { get { return m_CurPosIndex; } set { m_CurPosIndex = value; } }
	public UI_StatusBar STATUS_BAR { get { return m_StatusBar; } set { m_StatusBar = value; } }

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
		Status_MaxMP = 60.0f;
		Status_MP = 0.0f;


		switch (m_AnimEvent.Anim_Type)
		{
			case ANIM_TYPE.BEAR:
				Status_atk = 10.0f;
				Status_MaxHP = 100.0f;
				Status_HP = Status_MaxHP;
				break;
			case ANIM_TYPE.ORC:
				Status_atk = 7.0f;
				Status_MaxHP = 30.0f;
				Status_HP = Status_MaxHP;
				break;
			case ANIM_TYPE.EVIL:
				Status_atk = 3.0f;
				Status_MaxHP = 40.0f;
				Status_HP = Status_MaxHP;
				break;
			case ANIM_TYPE.GOBLIN_T:
				Status_MaxHP = 50.0f;
				Status_HP = Status_MaxHP;
				Status_atk = 5.0f;
				break;
			case ANIM_TYPE.GOBLIN_N:
				Status_MaxHP = 60.0f;
				Status_HP = Status_MaxHP;
				Status_atk = 6.0f;
				break;
			case ANIM_TYPE.WERERAT:
				Status_MaxHP = 70.0f;
				Status_HP = Status_MaxHP;
				Status_atk = 2.0f;
				break;
		}
        
		Status_moveSpeed = 5.0f;
		// Stat_MoveSpeed = UnityEngine.Random.Range(1.0f, 8.0f);		
		//CurPosIndex = 0;
	}

	private void Awake()
	{
		m_animator = GetComponentInChildren<Animator>();
		m_PathFinder = GetComponent<PathFinder>();
		m_AnimEvent = GetComponentInChildren<AnimationEvent>();
		m_StatusBar = GetComponentInChildren<UI_StatusBar>();
	}

	// Update is called once per frame
	void Update()
    {
	

	}

	private void LateUpdate()
	{
		DieProcess();

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

		//if(m_PathFinder.Arrived == true)
		//	m_PathFinder.StartPathFinding(gameObject, Target);
		
		m_PathFinder.MoveOnPath(gameObject);


		if (IsDead)
		{
			// Ǯ�� ���� ���� ������
			//Destroy(gameObject);
			gameObject.SetActive(false);
			return;
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			State = STATE.RUN;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			State = STATE.IDLE;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			State = STATE.SKILL;
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

	[SerializeField] protected float Status_MP;     // ����
    [SerializeField] protected float Status_MaxMP;  // �ִ븶��
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
	public float Stat_MP { get { return Status_MP; } set { Status_MP = value; } }
	public float Stat_MaxHP { get { return Status_MaxHP; } set { Status_MaxHP = value; } }
	public float Stat_MaxMP { get { return Status_MaxMP; } set { Status_MaxMP = value; } }
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
			State = STATE.DIE;

		// Dissolve Shader ���� ����
		// �ϴ� ���� �� ��� Die �ִϸ��̼� ������ �� IsDead true��
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
		  m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
		IsDead = true;		

		HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___UNIT_DIE, this.gameObject);
	}

	virtual public void BattleProcess()
	{
		if (null == Target)
			State = STATE.IDLE;

		if (null != Target)
		{
			float Dist = Vector3.Magnitude(transform.position - Target.transform.position);
			Vector3 Dir = Vector3.zero;
			float Angle = 0.0f;

			if (2.5f >= Dist)
			{
				switch (State)
				{
					case STATE.IDLE:
						if (PreTarget == Target)
							transform.LookAt(Target.transform);
						State = STATE.SKILL;
						break;
					case STATE.SKILL:
						Dir = Target.transform.position - transform.position;
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Dir), 1.0f * Time.deltaTime);
						Angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(Dir));
						break;
					case STATE.SKILL_IDLE:
						transform.LookAt(Target.transform);

						Dir = Target.transform.position - transform.position;
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Dir), 1.0f * Time.deltaTime);
						Angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(Dir));
						Debug.Log(Angle);						

						if (Angle <= 10.0f)
							State = STATE.SKILL;
						break;
					default:
						break;
				}
			}
		}

		MoveProcess();		
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
		SKILL_IDLE,
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
	protected STATE m_state = STATE.IDLE;
	public STATE State { get { return m_state; } set { m_state = value; } }

	//-------------------------------------------------------------------
	// Property
	//-------------------------------------------------------------------



	//-------------------------------------------------------------------
	// Method
	//-------------------------------------------------------------------
	virtual protected void ChangeState()
	{
		switch (m_state)
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
			case STATE.SKILL_IDLE:
					UpdateIdle();
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro.Examples;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PathFinder : MonoBehaviour
{	       
    [SerializeField]
    GameObject m_HostObj;

	[SerializeField] 
    BATTLE_PHASE m_Basic_phase;

	[SerializeField]
	private HYJ_Battle_Tile CurrentTile = null;
	[SerializeField]
	private HYJ_Battle_Tile DestTile = null;

	private HYJ_Battle_Tile m_PreTile = null;

	List<NODE> m_OpenNodes = new List<NODE>();
	List<NODE> m_CloseNodes = new List<NODE>();
	
	LinkedList<int> m_Path = new LinkedList<int>();

	bool m_IsArrived = false;	 
	public bool Arrived { get { return m_IsArrived;} set { m_IsArrived = value;} }

	NODE NearNode = null;

	// ���¸���Ʈ�� �ִ� ������ �߿��� ���� ���� �ĺ��� ������ �̾ƿ������� �����̳� PriorityQueue ���� ����


	// Start is called before the first frame update
	void Start()
    {
		m_HostObj = gameObject;
		m_Basic_phase = 0;		
	}

	private void LateUpdate()
	{
		m_Basic_phase = (BATTLE_PHASE)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___BASIC__GET_PHASE);		
	}

	public void MoveOnPath(GameObject Obj)
	{
		if (Obj == null ||
		   Obj.GetComponent<Character>().Target == null ||
		   Obj.GetComponent<Character>().Stat_HP <= 0)
			return;

		if (Obj.GetComponent<Character>().State == Character.STATE.SKILL	)
			return;

		if (0 == m_Path.Count)
		{
			if(Obj.GetComponent<Character>().State == Character.STATE.RUN)
			{
				Vector3 MoveDir = Vector3.zero;
				Vector3 CurPos = Obj.transform.position;
				float CurDestDist = 0.0f;

				if (DestTile != null)
				{
					MoveDir = DestTile.Tile_Position - Obj.transform.position;
					CurDestDist = MoveDir.magnitude;
					MoveDir.Normalize();					

					CurPos += MoveDir * Obj.GetComponent<Character>().Stat_MoveSpeed * Time.deltaTime;
					Obj.transform.position = CurPos;					
				}

				if (CurDestDist <= 0.1f)
					gameObject.GetComponent<Character>().State = Character.STATE.IDLE;
			}

			return;
		}

		GameObject Target = Obj.GetComponent<Character>().Target;

		//���� �̵��� ���� ������ ���
		int Index = m_Path.First();
		HYJ_Battle_Tile Tile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, Index);
		Vector3 TilePos = Tile.Tile_Position;
		Vector3 ObjPos = Obj.transform.position;

		//���� ������ ������� �������� �¾� �ϱ����� ����
		//int nextIndex = m_Path.First.Next.Value;		
		//HYJ_Battle_Tile nextTile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, nextIndex);

		Vector3 Dir = TilePos - ObjPos;
		float Length = Dir.magnitude;
		Dir.Normalize();

		gameObject.transform.LookAt(Tile.transform);

		// ���� �̵����� Ÿ�� ��ġ - ���� ������Ʈ ��ġ == ���̰Ÿ�
		// ���� ���������� ĳ���� Ÿ����ġ�� ����.
		if (Length <= 0.1f)
		{
			float Dist = Vector3.Magnitude(Obj.transform.position - Target.transform.position);
			if (Dist <= 2.5f)
			{
				Arrived = true;
				if (Character.STATE.RUN == gameObject.GetComponent<Character>().State)
					gameObject.GetComponent<Character>().State = Character.STATE.IDLE;

				Obj.GetComponent<Character>().LSY_Character_Set_OnTile(Tile.gameObject);

				DestTile = Tile;
				m_Path.Clear();

				return;
			}

			Obj.GetComponent<Character>().LSY_Character_Set_OnTile(Tile.gameObject);
			//Tile.HYJ_Basic_onUnit = null;

			Target = Obj.GetComponent<Character>().Target;

			m_PreTile = Tile;

			m_Path.RemoveFirst();
				
			return;
		}

		//ObjPos.x = Mathf.Lerp(ObjPos.x, TilePos.x, Obj.GetComponent<Character>().Stat_MoveSpeed * Time.deltaTime);
		//ObjPos.z = Mathf.Lerp(ObjPos.z, TilePos.z, Obj.GetComponent<Character>().Stat_MoveSpeed * Time.deltaTime);
		//ObjPos.y = 0.0f;
		
		// �̵������ϴٸ� ��ü Run ���·�
		Obj.GetComponent<Character>().State = Character.STATE.RUN;

		ObjPos += Dir * Obj.GetComponent<Character>().Stat_MoveSpeed * Time.deltaTime;
		Obj.transform.position = ObjPos;
	}



	public bool StartPathFinding(GameObject StartPosUnit, GameObject EndPosUnit)
	{
		if (StartPosUnit == null ||
			EndPosUnit == null)
			return false;

		if (m_Basic_phase != BATTLE_PHASE.PHASE_COMBAT || 
			GetComponent<Character>().State == Character.STATE.SKILL)
			return false;

		m_OpenNodes.Clear();
		m_CloseNodes.Clear();
		m_Path.Clear();

		List<NODE> BattleGraph = (List<NODE>)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD_GET_GRAPH);

		var Tiles = (List<HYJ_Battle_Manager_Line>)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD_GET_TILES);
		HYJ_Battle_Tile StartTile = StartPosUnit.GetComponent<HYJ_Battle_Tile>();
		int StartIndex = StartTile.GraphIndex;
		HYJ_Battle_Tile EndTile = EndPosUnit.GetComponent<HYJ_Battle_Tile>();
		int EndIndex = EndTile.GraphIndex;

		// �߸��� �ε����� �� ��ã�� ����.
		if (0 > StartIndex || 0 > EndIndex)
			return false;

		// ������ ������ ������ ��ã�� �ʿ����
		if (StartIndex == EndIndex)
			return false;

		// �������� Neighbor�� ������ ����
		if ( 0 == BattleGraph[EndIndex].m_Neighbors.Count)
			return false;

		// ���� ��� �߿� ���� ����� ������ Select �ϴ� ���� �߰��ؾ���.
		float MinDist = float.MaxValue;
		NearNode = null;
		foreach (var Neighbor in BattleGraph[EndIndex].m_Neighbors)
		{
			if (	null != Neighbor.Tile.HYJ_Basic_onUnit)
				continue;

			float Dist = Vector3.Magnitude(Neighbor.Position - StartTile.Tile_Position);
			if (MinDist > Dist)
			{
				MinDist = Dist;
				NearNode = Neighbor;
			}
		}

		if(null != NearNode)
		{
			if (true == FindingPath(StartIndex, NearNode.MyIndex))
			{
				MakePath(StartIndex, NearNode.MyIndex);
				Arrived = false;

				if (m_Path.Count <= 0)
				{
					m_OpenNodes.Clear();
					m_CloseNodes.Clear();
					return true;
				}

				CurrentTile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, StartIndex);
				DestTile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, NearNode.MyIndex);

				return true;
			}
		}


		// �������� �������� Ž���ؼ� ���� �����̾����� �̵� ����
		//foreach (var Neighbor in BattleGraph[EndIndex].m_Neighbors)
		//{
		//	if (null == Neighbor.Tile.HYJ_Basic_onUnit)
		//	{
		//		if (true == FindingPath(StartIndex, Neighbor.MyIndex))
		//		{
		//			MakePath(StartIndex, Neighbor.MyIndex);
		//			Arrived = false;

		//			if (m_Path.Count <= 0)
		//			{
		//				m_OpenNodes.Clear();
		//				m_CloseNodes.Clear();
		//				continue;
		//			}

		//			//Neighbor.Tile.HYJ_Basic_onUnit = gameObject;

		//			CurrentTile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, StartIndex);
		//			DestTile = (HYJ_Battle_Tile)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD__GET_TILE_IN_GRAPH, Neighbor.MyIndex);					

		//			return true;
		//		}
		//	}
		//}

		return false;		
	}

	bool FindingPath(int startIdx, int endIdx)
	{
		if(m_OpenNodes.Count != 0)
		{
			m_OpenNodes.RemoveAt(0);
		}

		List<NODE> BattleGraph = (List<NODE>)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD_GET_GRAPH);

		m_CloseNodes.Add(BattleGraph[startIdx]);

		foreach(var Neighbor in BattleGraph[startIdx].m_Neighbors)
		{
			//�̿��� �̹� ��������� ��ã�� ����.
			if(endIdx == Neighbor.MyIndex)
			{
				Neighbor.ParentIndex = startIdx;
				return true;
			}

			// �̹� Ŭ��� ��������� ���¿� ���� �ʿ� ����
			if (CheckExistInClose(Neighbor))
				continue;

			// ���¿� �ִ´�.
			InsertNodeInOpen(Neighbor, startIdx, endIdx);
		}

		// ������ ��������� ���̻� �� �� ����
		if (m_OpenNodes.Count == 0)
			return false;

		// ������ ��Ż��� Fcost �������� �������� ����		
		m_OpenNodes = m_OpenNodes.OrderBy(x => x.Fcost).ToList();

		return FindingPath(m_OpenNodes[0].MyIndex, endIdx);
	}

	void MakePath(int StartIdx, int EndIdx)
	{
		int ParentIndex = EndIdx;

		List<NODE> BattleGraph = (List<NODE>)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD_GET_GRAPH);

		while (true)
		{
			if (ParentIndex == StartIdx)
				break;

			////////////////////////////////
			m_Path.AddFirst(ParentIndex);			
			ParentIndex = BattleGraph[ParentIndex].ParentIndex;
		}

		//// �׷��� ��忡 ��ŷ
		//int size = m_Path.Count;

		//int Idx = 0;

		//foreach (var num in m_Path)
		//{
		//	if (BattleGraph[num].MarkCheck(Idx))
		//	{
		//		m_Path.Clear();
		//		return;
		//	}
		//	++Idx;
		//}

	}

	bool CheckExistInClose(NODE node)
	{
		var FindNode = m_CloseNodes.Find(x => x == node);

		if (FindNode == null)
			return false;
		else
			return true;			
	}

	void InsertNodeInOpen(NODE node, int startIdx, int EndIdx)
	{		
		var Findnode = m_OpenNodes.Find(x => x == node);		
		List<NODE> BattleGraph = (List<NODE>)HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.BATTLE___FIELD_GET_GRAPH);		
		// ���¿� �������� ���� ��
		if (Findnode == null)
		{
			// ���ݱ��� ������ ��� ���� �Ÿ� ����
			node.Gcost = BattleGraph[startIdx].Gcost + Vector3.Magnitude(BattleGraph[startIdx].Position - node.Position);

			// ���������� �Ÿ� ( �޸���ƽ ���� ��)
			float Hcost = Vector3.Magnitude(BattleGraph[EndIdx].Position - node.Position);

			// ��Ż ���
			node.Fcost = node.Gcost + Hcost;

			// ���¿� �ֱ� �� �θ� �ε��� ����.
			node.ParentIndex = startIdx;

			m_OpenNodes.Add(node);
		}
		else // ���¿� �̹� �����ϴµ� ���� ���� ����� ���� ��뺸�� �� ������ ����.
		{
			float Gcost = BattleGraph[startIdx].Gcost + Vector3.Magnitude(BattleGraph[startIdx].Position - node.Position);
			float Hcost = Vector3.Magnitude(BattleGraph[EndIdx].Position - node.Position);
			float Fcost = Gcost + Hcost;

			if(Findnode.Fcost > Fcost)
			{
				Findnode.Fcost = Fcost;
				Findnode.ParentIndex = startIdx;
			}
		}
	}

}

public class NODE
{
	public NODE(int Idx, Vector3 Pos, HYJ_Battle_Tile tile)
	{
		m_MyIndex = Idx;
		m_Position = Pos;
		m_tile = tile;
		m_Neighbors = new List<NODE>();
		m_MarkingNum = new List<int>(70);
		m_MarkingNum = Enumerable.Repeat(0, 70).ToList();
	}

	private int m_MyIndex = 0;
	public int MyIndex { get { return m_MyIndex; } set { m_MyIndex = value; } }
	private int m_ParentIndex = 0;
	public int ParentIndex { get { return m_ParentIndex; } set { m_ParentIndex = value; } }
	private HYJ_Battle_Tile m_tile = null;
	public HYJ_Battle_Tile Tile { get { return m_tile; } set { m_tile = value; } }
	private float m_Fcost = 0.0f;
	public float Fcost { get { return m_Fcost; } set { m_Fcost = value; } }
	private float m_Gcost = 0.0f;
	public float Gcost { get { return m_Gcost; } set { m_Gcost = value; } }

	public Vector3 Position { get { return m_Position; } set { m_Position = value; } }
	private Vector3 m_Position = new Vector3(0.0f, 0.0f, 0.0f);

	// ���� ����
	public List<NODE> m_Neighbors;

	// ��ŷ ����Ʈ
	List<int> m_MarkingNum;
	
	public void InitializeMarking()
	{
		int size = m_MarkingNum.Count;
		for (int i = 0; i < size; ++i)
		{
			m_MarkingNum[i] = 0;
		}
	}

	// Path���� �� ��忡 ���°�� �湮�ϴ��� üũ�ؼ� ���� Ÿ�ֿ̹� �湮�ϸ� true��ȯ
	public bool MarkCheck(int index)
	{
		if (index >= m_MarkingNum.Count)
			return true;

		if (m_MarkingNum[index] == 0)
		{
			++m_MarkingNum[index];			
		}
		else if(m_MarkingNum[index] != 0)
		{
			//�̹� ��ŷ���¸� ��ŷ�ִٰ� true ��ȯ
			return true;
		}

		//��ŷ ���ٰ� false ��ȯ
		return false;
	}

}
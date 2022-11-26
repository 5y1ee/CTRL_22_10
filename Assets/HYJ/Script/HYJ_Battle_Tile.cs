using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public partial class HYJ_Battle_Tile : MonoBehaviour
{
    //(11/4) ���� ���� ���� : ���� Ÿ���� HYJ_Character ��ũ��Ʈ�� ���� �ִ� ������ �����Ͽ�����, ��� ������ ������ ��ũ��Ʈ�� ���� ���� ����.
    // ��ӹ��� ��ũ��Ʈ�� �����ϴ� ����� �� �� �����Ƿ� �±׷� �����ϴ���, �ϴ��� GameObject������ ����.

    //[SerializeField] HYJ_Character Basic_onUnit;    // Ÿ������ �ö� �ִ� ����
    [SerializeField] GameObject Basic_onUnit;   // GameObject ������ ��ü -> ������ ��� HYJ_Character ��ũ��Ʈ�� ���� ���� ����.
    [SerializeField] public List<int> Tile_Idx; // Tile�� ��/�� ����
    [SerializeField] public Vector3 Tile_Position;  // Tile�� localPosition ����
    public enum Tile_Available
    {
        Available,
        Non_Available
    }
    [SerializeField] public Tile_Available tile_Available = Tile_Available.Non_Available;

    //////////  Getter & Setter //////////

    //////////  Method          //////////
    public GameObject HYJ_Basic_onUnit { get { return Basic_onUnit; } set { Basic_onUnit = value; } }

    //////////  Default Method  //////////
    void Start()
    {
        Tile_Position = this.transform.localPosition;
    }

    void Update()
    {
        
    }

}


partial class HYJ_Battle_Tile : MonoBehaviour
{
    [Header("==================================================")]
    [Header("Unit Detect")]

    [SerializeField]
    private List<GameObject> detectedUnit = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(tile_Available == Tile_Available.Non_Available)
        {
            // �� �� �ִ� Ÿ���ΰ�, �ƴѰ�,, �ƴ϶�� ���� �ڸ��� ���ư���.
            other.gameObject.transform.position = this.transform.parent.transform.parent.transform.parent.GetComponent<LSY_DragUnit>().oriPos;
            Debug.Log("isEmpty");
            detectedUnit.Add(other.gameObject);
            Basic_onUnit = other.GetComponent<GameObject>();


        }
        else
        {
            switch (other.tag)
            {
                case "Ally":
                    if (Basic_onUnit == null && detectedUnit.Count == 0/* && other.CompareTag("Ally")*/)
                    {
                        Debug.Log("isEmpty");
                        detectedUnit.Add(other.gameObject);
                        Basic_onUnit = other.gameObject;

                        other.gameObject.transform.position = this.gameObject.transform.position; // ->  ���⼭ pos ���� ��Ű�°� �����ؾ��ϳ�..?
                        other.gameObject.GetComponent<Character>().LSY_Character_Set_OnTile(this.gameObject);

                        //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.DRAG___UNIT__SET_POSITION, Tile_Idx);
                        //Debug.Log(other.gameObject.transform.position + "<-unit // tile->" + this.gameObject.transform.position);
                    }
                    else // �̹� �ٸ� ������ ���� ��� == ��ġ�� ���
                    {
                        Debug.Log("isOverlap " + other.name + " ");
                        /*
                        //other.gameObject.transform.position = other.gameObject.GetComponent<Character>().LSY_Unit_Position;
                        // LSY : overlap ��, ���� �ڸ��� ã�ư��� �س��µ� tile�� �θ��� �θ��� �θ� Battle ������Ʈ�� �ڵ尡 �� ������. �ϴ� �۵��� �ϴϱ�.
                        // oriPos�� Character�� ������Ƽ�� ���� �ְ�, DragDrop.cs�� ���� �ִµ� Character�� Ȱ���Ϸ��� ������ ��� ������ϴµ� ������.. (������Ʈ�� �� ��ġ�� ���� ���̰�, ��ġ �ٲ� �� ������Ƽ�� set �ϸ� �Ǳ� �ҵ�?)
                        // DragDrop���� ����ó�� �ϸ� ���ϱ� �ѵ�, �̰� DragDrop�� ���� �ǵ帮�� ��ü�� �ϳ�������. ���� ������ ������ �̵���Ű�� �۵�����. ���� ���콺�� �ϳ��ϱ� ������ �����Ű����� �ϴ� ������ ���� �� ����.
                        */
                        other.gameObject.transform.position = this.transform.parent.transform.parent.transform.parent.GetComponent<LSY_DragUnit>().oriPos;
                        other.gameObject.GetComponent<Character>().LSY_Character_Set_OnTile(null);

                        //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.DRAG___UNIT__SET_ORIGINAL);
                    }

                    break;
                case "HitArea":
                    break;
            }
        }
        /*
        //if(Basic_onUnit == null && detectedUnit.Count == 0 && other.CompareTag("Ally"))
        //{
        //    Debug.Log("isEmpty");
        //    detectedUnit.Add(other.gameObject);
        //    //Basic_onUnit = other.GetComponent<HYJ_Character>();
        //    Basic_onUnit = other.gameObject;

        //    other.gameObject.transform.position = this.gameObject.transform.position;
        //    Debug.Log(other.gameObject.transform.position + " " + this.gameObject.transform.position);
        //}
        //else
        //{
        //    Debug.Log("isOverlap " + other.name);
        //    other.gameObject.transform.position = other.gameObject.GetComponent<Character>().LSY_Unit_Position;
        //}
        */
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Ally":
                detectedUnit.Remove(other.gameObject);
                other.gameObject.GetComponent<Character>().LSY_Character_Set_OnTile(null);
                //HYJ_ScriptBridge.HYJ_Static_instance.HYJ_Event_Get(HYJ_ScriptBridge_EVENT_TYPE.DRAG___UNIT__SET_ORIGINAL);
                Basic_onUnit = null;
                break;

            case "HitArea":
                break;

        }
    }

    private GameObject DetectUnitObject()
    {
        GameObject near_obj = null;

        detectedUnit.ForEach ((obj) =>
        {
            if (near_obj == null)
            {
                near_obj = obj;
            }
            else if (Vector3.Distance(near_obj.transform.position, transform.position) >
            Vector3.Distance(obj.transform.position, transform.position))
            {
                near_obj = obj;
            }
        });

        return near_obj;
    }

}
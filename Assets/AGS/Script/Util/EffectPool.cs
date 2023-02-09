using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : SingletonMonoBehaviour<EffectPool>
{	
    [SerializeField]
    private GameObject[] effects;

	Dictionary<EFFECT_TYPE, List<EffectPoolUnit>> m_dicEffectPool = new Dictionary<EFFECT_TYPE, List<EffectPoolUnit>>();
	int m_presetSize = 1; //� ������������ �⺻������ 1�� ������

	void LoadEffect()
    {
        int size = effects.Length;
		EFFECT_TYPE effect_type = EFFECT_TYPE.EFFECT_SPARK;

		for (int i = 0; i < size; i++)
        {
			var Types = Enum.GetValues(typeof(EFFECT_TYPE));

			effect_type = (EFFECT_TYPE)Types.GetValue(i);

			List<EffectPoolUnit> listObjectPool = new List<EffectPoolUnit>(); //�ν��Ͻ� ����Ʈ�ϳ��� 1���� Ǯ
			m_dicEffectPool[effect_type] = listObjectPool;
				
			GameObject obj = Instantiate(effects[i]);
			obj.layer = LayerMask.NameToLayer("TransparentFX");

			EffectPoolUnit objectPoolUnit = obj.GetComponent<EffectPoolUnit>();
			if (objectPoolUnit == null)
			{
				obj.AddComponent<EffectPoolUnit>();
			}

			if (obj.GetComponent<ParticleAutoDestroy>() == null)
			{
				obj.AddComponent<ParticleAutoDestroy>();
			}

			obj.transform.SetParent(transform);

			EFFECT_TYPE type = obj.GetComponent<EffectPoolUnit>().EffectType;

			obj.GetComponent<EffectPoolUnit>().SetObjectPool(type, this);
			if (obj.activeSelf)
			{
				//���� �� ����Ʈ�� Ǯ�����ִ� ���°��ƴ� ��Ƽ����� OnDisable �̺�Ʈ�� ���۵�
				obj.SetActive(false);
			}
			else
			{
				AddPoolUnit(type, obj.GetComponent<EffectPoolUnit>());
			}
		}

	}
	public GameObject Create(EFFECT_TYPE effectType)
	{
		return Create(effectType, Vector3.zero, Quaternion.identity);
	}
	public GameObject Create(EFFECT_TYPE effectType, Vector3 pos)
	{
		return Create(effectType, pos, Quaternion.identity);
	}

	public GameObject Create(EFFECT_TYPE effectType, Vector3 position, Quaternion rotation)
	{
		List<EffectPoolUnit> listObjectPool = m_dicEffectPool[effectType];
		if (listObjectPool == null)
		{
			return null;
		}

		if (listObjectPool.Count > 0)
		{
			if (listObjectPool[0] != null && listObjectPool[0].IsReady())//0���� �غ� �ȵǸ� �������� ������ �ȵ��ֱ⋚���� 0���˻�
			{
				EffectPoolUnit unit = listObjectPool[0];
				listObjectPool.Remove(listObjectPool[0]);
				unit.transform.position = position;
				unit.transform.rotation = rotation;
				StartCoroutine(Coroutine_SetActive(unit.gameObject));
				return unit.gameObject;
			}
		}

		GameObject obj = Instantiate(effects[(int)effectType]);
		obj.layer = LayerMask.NameToLayer("TransparentFX");

		EffectPoolUnit objectPoolUnit = obj.GetComponent<EffectPoolUnit>();
		if (objectPoolUnit == null)
		{
			obj.AddComponent<EffectPoolUnit>();
		}
		if (obj.GetComponent<ParticleAutoDestroy>() == null)
		{
			obj.AddComponent<ParticleAutoDestroy>();
		}
		obj.GetComponent<EffectPoolUnit>().SetObjectPool(effectType, this);
		StartCoroutine(Coroutine_SetActive(obj));
		return obj;
	}

	IEnumerator Coroutine_SetActive(GameObject obj)
	{
		yield return new WaitForEndOfFrame();
		obj.SetActive(true);
	}

	public void AddPoolUnit(EFFECT_TYPE effectType, EffectPoolUnit unit)
	{
		List<EffectPoolUnit> listObjectPool = m_dicEffectPool[effectType];
		if (listObjectPool != null)
		{
			listObjectPool.Add(unit);
		}
	}

	// Use this for initialization
	protected override void OnStart()
	{
		base.OnStart();
		LoadEffect();
	}
	// Update is called once per frame
	void Update()
	{

	}
}

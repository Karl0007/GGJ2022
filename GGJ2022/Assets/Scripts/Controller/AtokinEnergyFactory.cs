using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtokinEnergyFactory : MonoBehaviour
{
	public float creatTime;
	public float creatOffset;
	public GameObject Energy;

	public void Creat()
	{
		Energy = Instantiate(ResourcesManager.Instance.Get(ResourcesManager.PrefabsName.AtokinEnergy),
			transform.position + (Vector3)Random.insideUnitCircle * creatOffset, Quaternion.identity);
		Energy.GetComponent<EnergyController>().m_Factory = this;
	}

	public void OnEnergyDestroy()
	{
		StartCoroutine(TimeManager.WorkAfterRealTime(Creat, Random.Range(0.9f, 1.1f) * creatTime));
	}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
	public static ResourcesManager Instance = null;

	private void Awake()
	{
		//初始化
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Main 物体 跨场景
		DontDestroyOnLoad(gameObject);

		prefabInit();
		audioInit();
		spriteslInit();
	}

	#region 预制体加载
	private const string prefabPath = "Prefabs/";
	public enum PrefabsName { Attack,Atokin1,Atokin2,Atokin3,Hit,AtokinEnergy, AtokinEnergyFactory,FlyAttack,Player, End };
	private Dictionary<PrefabsName, GameObject> PrefabsDic = new Dictionary<PrefabsName, GameObject>();
	private void prefabInit()
	{
		for (int i = 0; i < (int)PrefabsName.End; i++)
		{
			PrefabsName name = (PrefabsName)i;
			PrefabsDic[name] = Resources.Load(prefabPath + name.ToString()) as GameObject;
		}
	}
	#endregion

	#region 音频加载
	private const string audioPath = "Audios/";
	public enum AudioNames
	{
		Menu,Combat, Atokin, AtokinHit, Attack, AttackHit, Jump, PlayerHit,Charge,Dash ,Atokin1,Atokin2,AtokinHit1, FlyAttack,End
	};
	private Dictionary<AudioNames, AudioClip> AudioDic = new Dictionary<AudioNames, AudioClip>();
	private void audioInit()
	{
		for (int i = 0; i < (int)AudioNames.End; i++)
		{
			AudioNames name = (AudioNames)i;
			AudioDic[name] = Resources.Load(audioPath + name.ToString()) as AudioClip;
			Debug.Log(name);
		}
	}
	#endregion

	#region 图片加载
	private const string spritesPath = "Sprites/";
	public enum SpriteNames { Bullet, End };
	private Dictionary<SpriteNames, Sprite> SpritesDic = new Dictionary<SpriteNames, Sprite>();
	private void spriteslInit()
	{
		for (int i = 0; i < (int)SpriteNames.End; i++)
		{
			SpriteNames name = (SpriteNames)i;
			SpritesDic[name] = Resources.Load(spritesPath + name.ToString(), typeof(Sprite)) as Sprite;
		}
	}
	#endregion

	#region 获取接口
	public GameObject Get(PrefabsName name)
	{
		return PrefabsDic[name];
	}
	public AudioClip Get(AudioNames name)
	{
		return AudioDic[name];
	}
	public Sprite Get(SpriteNames name)
	{
		return SpritesDic[name];
	}
	#endregion

}
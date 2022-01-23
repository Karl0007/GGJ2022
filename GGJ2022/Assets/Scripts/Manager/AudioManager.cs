using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	//BGM����
	private AudioSource BGMPlayer;
	//��Ч����
	private HashSet<AudioSource> SoundPlayer = new HashSet<AudioSource>();
	private Dictionary<Func<bool>, AudioSource> SoundPlayerUntil = new Dictionary<Func<bool>, AudioSource>();
	//��Ч BGM ����
	private float _BGMVolume, _SoundVolume;
	private float fixBGMVolume = 1;
	public float BGMVolume { get => _BGMVolume; set => _BGMVolume = BGMPlayer.volume = Mathf.Clamp(Mathf.Clamp(value, 0, 1) * fixBGMVolume, 0, 1); }
	public float SoundVolume { get => _SoundVolume; set => _SoundVolume = Mathf.Clamp(value, 0, 1); }
	//��ʱɾ����
	private List<Func<bool>> tmpdel = new List<Func<bool>>();

	[HideInInspector]
	public ResourcesManager.AudioNames CurBGM = ResourcesManager.AudioNames.End;


	//�½�������
	private AudioSource NewAudioSource(float volume = 1, bool loop = false)
	{
		GameObject go = new GameObject("AudioPlayer");
		DontDestroyOnLoad(go);
		AudioSource tmp = go.AddComponent<AudioSource>();
		tmp.volume = volume;
		tmp.loop = loop;
		return tmp;
	}

	//������Ч �ڶ�����������Ч�������Ļص�����
	public void PlaySound(ResourcesManager.AudioNames name)
	{
		var tmp = NewAudioSource(SoundVolume / 3);
		SoundPlayer.Add(tmp);
		tmp.clip = ResourcesManager.Instance.Get(name);
		Destroy(tmp.gameObject, ResourcesManager.Instance.Get(name).length);
		tmp.Play();
	}

	//�������� �ڶ��������ǳ������ŵ����� ͬһ���������Ų�Ҫ�ظ�����
	public void PlayUntil(ResourcesManager.AudioNames name, Func<bool> fun, bool loop = false)
	{
		if (!SoundPlayerUntil.ContainsKey(fun))
		{
			var tmp = NewAudioSource(SoundVolume / 3, loop);
			SoundPlayerUntil[fun] = tmp;
			tmp.clip = ResourcesManager.Instance.Get(name);
			tmp.Play();
		}
	}

	//���ű�������
	public void PlayBGM(ResourcesManager.AudioNames name, float fix = 1)
	{
		if (CurBGM != name)
		{
			CurBGM = name;
			fixBGMVolume = fix;
			BGMPlayer.clip = ResourcesManager.Instance.Get(name);
			BGMPlayer.volume = _BGMVolume * fixBGMVolume;
			BGMPlayer.loop = true;
			BGMPlayer.Play();
		}
	}

	public void StopBGM()
	{
		StartCoroutine(m_StopBGM());
	}

	public void ChangeBGM(ResourcesManager.AudioNames name, float time = 0, float fix = 1)
	{
		if (CurBGM != name)
		{
			StartCoroutine(m_ChangeBGM(name, time, fix));
		}
	}

	private IEnumerator m_ChangeBGM(ResourcesManager.AudioNames name, float time = 0, float fix = 1)
	{
		CurBGM = name;
		fixBGMVolume = fix;
		BGMPlayer.loop = true;
		while (BGMPlayer.volume > 0.07f)
		{
			BGMPlayer.volume *= 0.97f;
			yield return new WaitForEndOfFrame();
		}
		BGMPlayer.clip = ResourcesManager.Instance.Get(name);
		BGMPlayer.Play();
		BGMPlayer.time = time;
		Debug.Log(time);
		while (BGMPlayer.volume < _BGMVolume * fixBGMVolume)
		{
			BGMPlayer.volume *= 1.2f;
			yield return new WaitForEndOfFrame();
		}
		BGMPlayer.volume = _BGMVolume * fixBGMVolume;
	}

	private IEnumerator m_StopBGM()
	{
		for (float t = 0; t < 2; t += Time.deltaTime)
		{
			BGMPlayer.volume *= 0.98f;
			yield return new WaitForEndOfFrame();
		}
		BGMPlayer.Stop();
	}

	private void Start()
	{
		PlayBGM(ResourcesManager.AudioNames.Combat);
	}

	public float BGMTime()
	{
		return BGMPlayer.time;
	}

	//��ʼ������
	private void Awake()
	{
		//��ʼ��
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Main ���� �糡��
		DontDestroyOnLoad(gameObject);

		//����BGMPLAYER
		BGMPlayer = NewAudioSource();
		//����Ĭ������
		BGMVolume = 0.5f;
		SoundVolume = 0.5f;

	}

	private void Update()
	{
		//ά�����������б�
		foreach (var x in SoundPlayerUntil)
		{
			if (!x.Key())
			{
				tmpdel.Add(x.Key);
				Destroy(x.Value.gameObject);
			}
		}
		foreach (var x in tmpdel)
		{
			SoundPlayerUntil.Remove(x);
		}
		tmpdel.Clear();


	}
}

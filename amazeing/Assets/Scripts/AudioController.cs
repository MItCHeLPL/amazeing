using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
	public string name;

	public AudioSource source;

	public AudioClip clip;

	public bool playOnAwake = false;

	public bool loop = false;

	[Range(0f, 2f)]
	public float volume = 1;
	[HideInInspector] public float defaultVolume = 1;

	[Range(0.1f, 3f)]
	public float pitch = 1;

	[Range(0f, 1f)]
	public float spatialBlend = 0;
}


public class AudioController : MonoBehaviour
{
	[SerializeField] private List<Sound> sounds;

	[HideInInspector] public bool isMuted = false;


	private void Start()
	{
		//Set up source settings
		foreach (Sound s in sounds)
		{
			s.source.clip = s.clip;

			s.source.playOnAwake = s.playOnAwake;

			s.source.loop = s.loop;

			s.source.volume = s.volume;
			s.defaultVolume = s.volume;

			s.source.pitch = s.pitch;

			s.source.spatialBlend = s.spatialBlend;

			if (s.playOnAwake)
			{
				Play(s);
			}
		}

		GetSettingsFromPlayerPrefs();
	}


	public void Play(string name)
	{
		Sound s = sounds.Find(sound => sound.name == name); //Find source

		Play(s);
	}
	public void Play(string name, float delay)
	{
		Sound s = sounds.Find(sound => sound.name == name); //Find source

		Play(s, delay);
	}
	public void Play(Sound s, float delay = 0.0f)
	{
		if (s != null)
		{
			s.source.PlayDelayed(delay);
		}
	}


	public void Stop(string name)
	{
		Sound s = sounds.Find(sound => sound.name == name); //Find source

		Stop(s);
	}
	public void Stop(Sound s)
	{
		if (s != null && IsPlaying(s))
		{
			s.source.Stop();
		}
	}


	public bool IsPlaying(string name)
	{
		Sound s = sounds.Find(sound => sound.name == name); //Find source

		return IsPlaying(s);
	}
	public bool IsPlaying(Sound s)
	{
		if (s != null)
		{
			return s.source.isPlaying;
		}

		return false;
	}


	public void UpdateMute()
	{
		//unmute
		if(isMuted)
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = s.volume;

				if (s.playOnAwake)
				{
					Play(s);
				}
			}

			isMuted = false;
			PlayerPrefs.SetInt("Audio_Muted", 0);
		}

		//mute
		else
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = 0;
			}

			isMuted = true;
			PlayerPrefs.SetInt("Audio_Muted", 1);
		}
	}
	public void UpdateMute(bool value)
	{
		//unmute
		if (!value)
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = s.volume;

				if (s.playOnAwake)
				{
					Play(s);
				}
			}

			isMuted = false;
			PlayerPrefs.SetInt("Audio_Muted", 0);
		}

		//mute
		else
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = 0;
			}

			isMuted = true;
			PlayerPrefs.SetInt("Audio_Muted", 1);
		}
	}

	public void GetSettingsFromPlayerPrefs()
	{
		if (PlayerPrefs.HasKey("Audio_Muted"))
		{
			int keyValue = PlayerPrefs.GetInt("Audio_Muted"); //Get volume value from user settings
			bool boolValue = keyValue == 1;

			UpdateMute(boolValue);
		}
	}
	public bool GetSettingFromPlayerPrefs(string key)
	{
		bool boolValue = false;

		if (PlayerPrefs.HasKey(key))
		{
			int keyValue = PlayerPrefs.GetInt(key); //Get volume value from user settings
			boolValue = keyValue == 1;
		}

		return boolValue;
	}
}
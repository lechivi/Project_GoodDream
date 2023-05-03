using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.Storage;

public class AudioManager : BaseManager<AudioManager>
{
	private float bgmFadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH;
	//Next BGM name, SE name
	private string nextBGMName;
	private string nextSFXName;

	//Is the highlightBackground music fading out?
	private bool isFadeOut = false;

	//Separate audio sources for BGM and SE
	public AudioSource AttachBGMSource;
	public AudioSource AttachSFXSource;

    //Keep All Audio
    private Dictionary<string, AudioClip> bgmDic, sfxDic;

	protected override void Awake()
	{
		base.Awake();
		//Load all SE & BGM files from resource folder
		bgmDic = new Dictionary<string, AudioClip>();
		sfxDic = new Dictionary<string, AudioClip>();

		object[] bgmList = Resources.LoadAll("Audio/BGM");
		object[] sfxList = Resources.LoadAll("Audio/SFX");

		foreach (AudioClip bgm in bgmList)
		{
			bgmDic[bgm.name] = bgm;
		}
		foreach (AudioClip sfx in sfxList)
		{
			sfxDic[sfx.name] = sfx;
		}
	}

	private void Start()
	{
		AttachBGMSource.volume = ObscuredPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
		AttachSFXSource.volume = ObscuredPrefs.GetFloat(CONST.SFX_VOLUME_KEY, CONST.SFX_VOLUME_DEFAULT);
		AttachBGMSource.mute = ObscuredPrefs.GetBool(CONST.BGM_MUTE_KEY, CONST.BGM_MUTE_DEFAULT);
        AttachSFXSource.mute = ObscuredPrefs.GetBool(CONST.SFX_MUTE_KEY, CONST.SFX_MUTE_DEFAULT);
    }

	public void PlaySFX(AudioClip audio)
	{
        AttachSFXSource.PlayOneShot(audio);
    }

	public void PlaySFX(string sfxName, float delay = 0.0f)
	{
		if (!sfxDic.ContainsKey(sfxName))
		{
			Debug.Log(sfxName + "There is no SFX named");
			return;
		}

		nextSFXName = sfxName;
		Invoke("DelayPlaySFX", delay);
	}

	private void DelayPlaySFX()
	{
		AttachSFXSource.PlayOneShot(sfxDic[nextSFXName] as AudioClip);
	}

	public void PlayBGM(string bgmName, float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH)
	{
		if (!bgmDic.ContainsKey(bgmName))
		{
			Debug.Log(bgmName + "There is no BGM named");
			return;
		}

		//If BGM is not currently playing, play it as is
		if (!AttachBGMSource.isPlaying)
		{
			nextBGMName = "";
			AttachBGMSource.clip = bgmDic[bgmName] as AudioClip;
			AttachBGMSource.Play();
		}
		//When a different BGM is playing, fade out the BGM that is playing before playing the next one.
		//Through when the same BGM is playing
		else if (AttachBGMSource.clip.name != bgmName)
		{
			nextBGMName = bgmName;
			FadeOutBGM(fadeSpeedRate);
		}

	}

	public void FadeOutBGM(float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_LOW)
	{
		bgmFadeSpeedRate = fadeSpeedRate;
		isFadeOut = true;
	}

	private void Update()
	{
		if (!isFadeOut)
		{
			return;
		}

		//Gradually lower the volume, and when the volume reaches 0
		//return the volume and play the next song
		AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
		if (AttachBGMSource.volume <= 0)
		{
			AttachBGMSource.Stop();
			AttachBGMSource.volume = ObscuredPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
			isFadeOut = false;

			if (!string.IsNullOrEmpty(nextBGMName))
			{
				PlayBGM(nextBGMName);
			}
		}
	}

	public void ChangeBGMVolume(float BGMVolume)
	{
		AttachBGMSource.volume = BGMVolume;
		ObscuredPrefs.SetFloat(CONST.BGM_VOLUME_KEY, BGMVolume);
	}

	public void ChangeSFXVolume(float SFXVolume)
	{
		AttachSFXSource.volume = SFXVolume;
		ObscuredPrefs.SetFloat(CONST.SFX_VOLUME_KEY, SFXVolume);
	}

	public void MuteBGM(bool isMute)
	{
        AttachBGMSource.mute = isMute;
        ObscuredPrefs.SetBool(CONST.BGM_MUTE_KEY, isMute);
    }

	public void MuteSFX(bool isMute)
	{
        AttachSFXSource.mute = isMute;
        ObscuredPrefs.SetBool(CONST.SFX_MUTE_KEY, isMute);
    }
}

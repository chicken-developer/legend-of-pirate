using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	AudioSource mAudioPlayer;
	[SerializeField] AudioClip mGamePlayBGMusic, mGameOverBGMusic;
	[SerializeField] AudioClip mSFXShoot, mSFXEggHit, mSFXEggBreak, mSFXPowerEgg, mSFXRainbowEgg, mSFXFlashEgg, mSFXFrozenHazard;

    public void InitAudio()
	{
		mAudioPlayer = gameObject.GetComponent<AudioSource>();
	}

    public void StopAllAudio()
	{
		if(mAudioPlayer.isPlaying)
			mAudioPlayer.Stop();
	}

    public void PlayMusicGamePlayBackground()
    {
		mAudioPlayer.clip = mGamePlayBGMusic;
		mAudioPlayer.loop = true;
        mAudioPlayer.Play();		
    }

	public void PlayMusicGameOverBackground()
    {
		mAudioPlayer.clip = mGameOverBGMusic;
		mAudioPlayer.loop = false;
        mAudioPlayer.Play();		
    }

    public void PlaySFXShot()
    {
        mAudioPlayer.PlayOneShot(mSFXShoot);
    }

	public void PlaySFXEggHit()
    {
        mAudioPlayer.PlayOneShot(mSFXEggHit);
    }

	public void PlaySFXEggBreak()
    {
        mAudioPlayer.PlayOneShot(mSFXEggBreak);
    }

    public void PlaySFXPowerEgg()
    {
        mAudioPlayer.PlayOneShot(mSFXPowerEgg);
    }

    public void PlaySFXRainbowEgg()
    {
        mAudioPlayer.PlayOneShot(mSFXRainbowEgg);
    }

    public void PlaySFXFlashEgg()
    {
        mAudioPlayer.PlayOneShot(mSFXFlashEgg);
    }

    public void PlaySFXFrozenHazard()
    {
        mAudioPlayer.PlayOneShot(mSFXFrozenHazard);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] GameObject musicGameObject, sfxGameObject;
	AudioSource mMusicPlayer, mSFXPlayer;

    public void InitAudio()
	{
		mMusicPlayer = musicGameObject.GetComponent<AudioSource>();
		mSFXPlayer = sfxGameObject.GetComponent<AudioSource>();
	}

    public void StopAllAudio()
	{
		if(mMusicPlayer.isPlaying)
			mMusicPlayer.Stop();
		if(mSFXPlayer.isPlaying)
			mSFXPlayer.Stop();
	}

    public void PlayBackgroundMusic()
    {
        if(!mMusicPlayer.isPlaying)
            mMusicPlayer.PlayOneShot(mMusicPlayer.clip);
    }

    public void PlayShotSFX()
    {
        mSFXPlayer.PlayOneShot(mSFXPlayer.clip);
    }
}

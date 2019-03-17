﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public enum Sounds
	{
		Click,
		ClickHeavy,
		PlayerHit1,
		PlayerAttack1,
		PlayerAttack2,
		PlayerDie1,
		SkeletonAttack1,
		SkeletonHit1,
		SkeletonDie1,
		ZombieAttack1,
		ZombieHit1,
		ZombieDie1,
	};
	AudioSource m_MusicSource;
	AudioSource m_SoundSource;

	private void Awake()
	{
		m_MusicSource = gameObject.AddComponent<AudioSource>();
		m_SoundSource = gameObject.AddComponent<AudioSource>();
	}

	private void Update()
	{
		if (m_MusicSource)
		{
			if (m_MusicSource.isPlaying)
			{
				if (!GameManager.instance.Settings.PlayMusic) m_MusicSource.Stop();
			}
			else
			{
				if (GameManager.instance.Settings.PlayMusic) PlayMusic();
			}
		}
	}

	public void PlayMusic()
	{
		if (!GameManager.instance.Settings.PlayMusic) return;
		//https://downloads.khinsider.com/game-soundtracks/album/diablo-the-music-of-1996-2011-diablo-15-year-anniversary
		string currentscene = SceneManager.GetActiveScene().name;
		string songname = "Music/Diablo/";
		switch (currentscene)
		{
			case "MainMenu": songname += "01 - Diablo Intro"; break;
			case "Town": songname += "02 - Tristram"; break;
			default: songname += "03 - Dungeon"; break;
		}
		m_MusicSource.clip = (AudioClip)Resources.Load(songname);
		if (m_MusicSource.clip == null) Debug.Log("couldn't find music " + songname);
		m_MusicSource.loop = true;
		m_MusicSource.Play();
	}

	public void PlaySound(Sounds sound)
	{
		if (!GameManager.instance.Settings.PlaySound) return;
		string soundname = "";
		switch (sound)
		{
			case Sounds.Click: soundname = "File00000001"; break;
			case Sounds.ClickHeavy: soundname = "File00000002"; break;
			case Sounds.PlayerHit1: soundname = "File00000061"; break;
			case Sounds.PlayerAttack1: soundname = "File00000063"; break;
			case Sounds.PlayerAttack2: soundname = "File00000064"; break;
			case Sounds.PlayerDie1: soundname = "File00000065"; break;
			case Sounds.SkeletonAttack1: soundname = "File00001344"; break;
			case Sounds.SkeletonHit1: soundname = "File00001346"; break;
			case Sounds.SkeletonDie1: soundname = "File00001348"; break;
			case Sounds.ZombieAttack1: soundname = "File00001425"; break;
			case Sounds.ZombieHit1: soundname = "File00001427"; break;
			case Sounds.ZombieDie1: soundname = "File00001430"; break;
			default: Debug.Log("missing sound " + sound); break;
		}
		PlaySound(soundname);
	}

	void PlaySound(string soundname)
	{
		AudioClip clip = (AudioClip)Resources.Load("Sounds/" + soundname);
		if (clip != null) m_SoundSource.PlayOneShot(clip);
		else Debug.Log("missing sound effect");
	}
}

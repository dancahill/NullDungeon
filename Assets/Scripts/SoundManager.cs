using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public enum Sounds
	{
		PlayerAttack1,
		PlayerAttack2,
		SkeletonHit1,
		SkeletonDie1,
		ZombieHit1,
		ZombieDie1,
	};
	AudioSource m_Audio;

	private void Awake()
	{
		m_Audio = gameObject.AddComponent<AudioSource>();
	}

	public void PlayMusic()
	{
		//https://downloads.khinsider.com/game-soundtracks/album/diablo-the-music-of-1996-2011-diablo-15-year-anniversary
		string currentscene = SceneManager.GetActiveScene().name;
		string songname = "Music/Diablo/" + (currentscene == "Town" ? "02 - Tristram" : "03 - Dungeon");
		AudioClip clip = (AudioClip)Resources.Load(songname);
		if (clip == null) Debug.Log("couldn't find music " + songname);
		m_Audio.loop = true;
		m_Audio.PlayOneShot(clip);
	}

	public void PlaySound(Sounds sound)
	{
		string soundname = "";
		switch (sound)
		{
			case Sounds.PlayerAttack1: soundname = "File00000063"; break;
			case Sounds.PlayerAttack2: soundname = "File00000064"; break;
			case Sounds.SkeletonHit1: soundname = "File00001346"; break;
			case Sounds.SkeletonDie1: soundname = "File00001348"; break;
			case Sounds.ZombieHit1: soundname = "File00001427"; break;
			case Sounds.ZombieDie1: soundname = "File00001430"; break;
			default: Debug.Log("missing sound " + sound); break;
		}
		PlaySound(soundname);
	}

	void PlaySound(string soundname)
	{
		AudioClip clip = (AudioClip)Resources.Load("Sounds/" + soundname);
		if (clip != null) m_Audio.PlayOneShot(clip);
		else Debug.Log("missing sound effect");
	}
}

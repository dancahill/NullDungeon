using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
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

	public void PlaySound(string soundname)
	{
		AudioClip clip = (AudioClip)Resources.Load("Sounds/" + soundname);
		if (clip != null) m_Audio.PlayOneShot(clip);
		else Debug.Log("missing sound effect");
	}
}

using UnityEngine;
using System.Collections;

public class BackgroundSoundChange : MonoBehaviour {

	public AudioClip[] otherClip;
		
	public IEnumerator MusicTimer() {
		AudioSource audio = GetComponent<AudioSource>();
		
		audio.Play();
		yield return new WaitForSeconds(60);
		audio.clip = otherClip[0];
		audio.Play();
		yield return new WaitForSeconds(120);
		audio.clip = otherClip[1];
		audio.Play();
	}
}

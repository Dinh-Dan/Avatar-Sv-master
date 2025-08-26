using System;
using UnityEngine;

public class MyAudioClip
{
	public string name;

	public AudioClip clip;

	public long timeStart;

	public MyAudioClip(string filename)
	{
		clip = (AudioClip)Resources.Load(filename);
		name = filename;
	}

	public void Play()
	{
		Out.println("PLAY: " + name);
		if (isPlaying())
		{
			Debug.LogWarning("Skip " + name);
			return;
		}
		Main.main.GetComponent<AudioSource>().PlayOneShot(clip);
		timeStart = Environment.TickCount;
	}

	public bool isPlaying()
	{
		if (timeStart == 0)
		{
			return false;
		}
		return Environment.TickCount - timeStart < (long)(clip.length * 1000f);
	}
}

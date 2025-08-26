using System;
using System.Collections;
using UnityEngine;

public class SoundManager
{
	private class IActionYes : IAction
	{
		private sbyte id;

		public IActionYes(sbyte id)
		{
			this.id = id;
		}

		public void perform()
		{
			int num = setSound(string.Empty + id);
			if (num == -1)
			{
				GlobalService.gI().doRequestSoundData(id);
			}
			else
			{
				playSoundData((byte[])sound.elementAt(num));
			}
			isPlaying = 1;
		}
	}

	public static Hashtable soundPoolMap;

	public static bool isStop;

	public static bool isOpen;

	private static MyVector sound;

	private static MyVector name;

	private static int isPlaying = -1;

	public static int currentBGMusic;

	public static bool isBGplay;

	public const int snd_effect_fishing_reel = 0;

	public const int snd_effect_fish = 1;

	public const int snd_effect_buy = 2;

	public const int snd_effect_earned_money = 3;

	public const int snd_effect_dao_dat = 4;

	public const int snd_effect_tuoi_nuoc = 5;

	public const int snd_effect_thu_hoach = 6;

	public const int snd_effect_touch = 7;

	public const int snd_effect_pig = 8;

	public const int snd_effect_chicken = 9;

	public const int snd_effect_cow = 10;

	public const int snd_effect_dog = 11;

	public const int snd_ava_angry_b = 30;

	public const int snd_ava_angry_g = 31;

	public const int snd_ava_cry_b = 32;

	public const int snd_ava_cry_b_2 = 33;

	public const int snd_ava_cry_g = 34;

	public const int snd_ava_cry_g_2 = 35;

	public const int snd_ava_fight = 36;

	public const int snd_ava_fight_2 = 37;

	public const int snd_ava_fight_3 = 38;

	public const int snd_ava_fight_4 = 39;

	public const int snd_ava_jump_b = 40;

	public const int snd_ava_jump_b_2 = 41;

	public const int snd_ava_jump_g = 42;

	public const int snd_ava_jump_g_2 = 43;

	public const int snd_ava_kiss = 44;

	public const int snd_ava_kiss_2 = 45;

	public const int snd_ava_kiss_3 = 46;

	public const int snd_ava_kiss_b = 47;

	public const int snd_ava_laugh_b = 48;

	public const int snd_ava_laugh_b_2 = 49;

	public const int snd_ava_laugh_b_3 = 50;

	public const int snd_ava_laugh_g = 51;

	public const int snd_ava_laugh_g_2 = 52;

	public const int snd_ava_laugh_g_3 = 53;

	public const int snd_ava_leuleu_b = 54;

	public const int snd_ava_leuleu_b_2 = 55;

	public const int snd_ava_leuleu_g = 56;

	public const int snd_ava_leuleu_g_2 = 57;

	public const int snd_ani_cow = 70;

	public const int snd_ani_dog = 71;

	public const int snd_ani_pig = 72;

	public const int snd_ani_chicken = 73;

	public const int snd_bg_crow = 80;

	public const int snd_bg_fishing = 81;

	public const int snd_bg_wedding = 82;

	public const int snd_bg_house = 83;

	public const int snd_bg_shop = 84;

	public const int snd_bg_city = 85;

	public static void init()
	{
		if (Main.isCompactDevice)
		{
			soundPoolMap = new Hashtable();
			Out.println("SoundManager init");
			loadSoundsEffect();
			loadSoundsAva();
			loadSoundsBG();
			loadSoundsAnimal();
		}
	}

	public static void onRequestOpenSound(string des, sbyte id)
	{
		if (isPlaying != 0)
		{
			IAction action = new IActionYes(id);
			if (isPlaying == 1)
			{
				action.perform();
			}
			else
			{
				Canvas.startOKDlg(des, action);
			}
		}
	}

	public static int setSound(string na)
	{
		if (name == null)
		{
			name = new MyVector();
		}
		for (int i = 0; i < name.size(); i++)
		{
			string text = (string)name.elementAt(i);
			if (text.Equals(na))
			{
				return i;
			}
		}
		return -1;
	}

	public static void onSoundData(byte[] dataS, sbyte id)
	{
		if (sound == null)
		{
			sound = new MyVector();
			name = new MyVector();
		}
		name.addElement(string.Empty + id);
		sound.addElement(dataS);
		playSoundData(dataS);
	}

	private static void playSoundData(byte[] arr)
	{
		float[] array = ConvertByteToFloat(arr);
		AudioClip audioClip = AudioClip.Create("testSound", array.Length, 1, 44100, false, false);
		audioClip.SetData(array, 0);
		Out.println("playSoundData");
		AudioSource.PlayClipAtPoint(audioClip, new Vector3(100f, 100f, 0f), 1f);
	}

	private static float[] ConvertByteToFloat(byte[] array)
	{
		float[] array2 = new float[array.Length / 4];
		for (int i = 0; i < array2.Length; i++)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(array, i * 4, 4);
			}
			array2[i] = BitConverter.ToSingle(array, i * 4) / 2.1474836E+09f;
		}
		return array2;
	}

	public static void playSound(int index)
	{
		if (Main.isCompactDevice && (OptionScr.instance == null || OptionScr.instance.volume != 0))
		{
			isOpen = true;
			((MyAudioClip)soundPoolMap[index]).Play();
		}
	}

	public static void setVolume(float volume)
	{
		Main.main.GetComponent<AudioSource>().volume = volume;
	}

	public static void stop()
	{
		isStop = true;
	}

	public static void playSoundBG(int index)
	{
		if (Main.isCompactDevice && (OptionScr.instance == null || OptionScr.instance.volume != 0))
		{
			isOpen = true;
			Main.main.GetComponent<AudioSource>().volume = 1f;
			MyAudioClip myAudioClip = (MyAudioClip)soundPoolMap[index];
			if (!myAudioClip.isPlaying())
			{
				stopALLBGSound();
				myAudioClip.Play();
			}
		}
	}

	public static void stopALLBGSound()
	{
		if (Main.isCompactDevice)
		{
			Main.main.GetComponent<AudioSource>().Stop();
			((MyAudioClip)soundPoolMap[80]).timeStart = 0L;
			((MyAudioClip)soundPoolMap[82]).timeStart = 0L;
			((MyAudioClip)soundPoolMap[84]).timeStart = 0L;
			((MyAudioClip)soundPoolMap[83]).timeStart = 0L;
			((MyAudioClip)soundPoolMap[81]).timeStart = 0L;
			((MyAudioClip)soundPoolMap[85]).timeStart = 0L;
		}
	}

	public static void loadSoundsEffect()
	{
		soundPoolMap.Add(0, new MyAudioClip("sound/snd_effect_fishing_reel"));
		soundPoolMap.Add(1, new MyAudioClip("sound/snd_effect_fish"));
		soundPoolMap.Add(2, new MyAudioClip("sound/snd_effect_buy"));
		soundPoolMap.Add(3, new MyAudioClip("sound/snd_effect_earned_money"));
		soundPoolMap.Add(4, new MyAudioClip("sound/snd_effect_dao_dat"));
		soundPoolMap.Add(5, new MyAudioClip("sound/snd_effect_tuoi_nuoc"));
		soundPoolMap.Add(6, new MyAudioClip("sound/snd_effect_thu_hoach"));
		soundPoolMap.Add(7, new MyAudioClip("sound/snd_effect_touch"));
		soundPoolMap.Add(8, new MyAudioClip("sound/snd_effect_pig"));
		soundPoolMap.Add(9, new MyAudioClip("sound/snd_effect_chicken"));
		soundPoolMap.Add(10, new MyAudioClip("sound/snd_effect_cow"));
		soundPoolMap.Add(11, new MyAudioClip("sound/snd_effect_dog"));
	}

	public static void loadSoundsAva()
	{
		soundPoolMap.Add(30, new MyAudioClip("sound/snd_ava_angry_b"));
		soundPoolMap.Add(31, new MyAudioClip("sound/snd_ava_angry_g"));
		soundPoolMap.Add(32, new MyAudioClip("sound/snd_ava_cry_b"));
		soundPoolMap.Add(33, new MyAudioClip("sound/snd_ava_cry_b_2"));
		soundPoolMap.Add(34, new MyAudioClip("sound/snd_ava_cry_g"));
		soundPoolMap.Add(35, new MyAudioClip("sound/snd_ava_cry_g_2"));
		soundPoolMap.Add(36, new MyAudioClip("sound/snd_ava_fight"));
		soundPoolMap.Add(37, new MyAudioClip("sound/snd_ava_fight_2"));
		soundPoolMap.Add(38, new MyAudioClip("sound/snd_ava_fight_3"));
		soundPoolMap.Add(39, new MyAudioClip("sound/snd_ava_fight_4"));
		soundPoolMap.Add(40, new MyAudioClip("sound/snd_ava_jump_b"));
		soundPoolMap.Add(41, new MyAudioClip("sound/snd_ava_jump_b_2"));
		soundPoolMap.Add(42, new MyAudioClip("sound/snd_ava_jump_g"));
		soundPoolMap.Add(43, new MyAudioClip("sound/snd_ava_jump_g_2"));
		soundPoolMap.Add(44, new MyAudioClip("sound/snd_ava_kiss"));
		soundPoolMap.Add(45, new MyAudioClip("sound/snd_ava_kiss_2"));
		soundPoolMap.Add(46, new MyAudioClip("sound/snd_ava_kiss_3"));
		soundPoolMap.Add(47, new MyAudioClip("sound/snd_ava_kiss_b"));
		soundPoolMap.Add(48, new MyAudioClip("sound/snd_ava_laugh_b"));
		soundPoolMap.Add(49, new MyAudioClip("sound/snd_ava_laugh_b_2"));
		soundPoolMap.Add(50, new MyAudioClip("sound/snd_ava_laugh_b_3"));
		soundPoolMap.Add(51, new MyAudioClip("sound/snd_ava_laugh_g"));
		soundPoolMap.Add(52, new MyAudioClip("sound/snd_ava_laugh_g_2"));
		soundPoolMap.Add(53, new MyAudioClip("sound/snd_ava_laugh_g_3"));
		soundPoolMap.Add(54, new MyAudioClip("sound/snd_ava_leuleu_b"));
		soundPoolMap.Add(56, new MyAudioClip("sound/snd_ava_leuleu_g"));
	}

	public static void loadSoundsBG()
	{
		soundPoolMap.Add(80, new MyAudioClip("sound/snd_bg_crow"));
		soundPoolMap.Add(81, new MyAudioClip("sound/snd_bg_fishing"));
		soundPoolMap.Add(82, new MyAudioClip("sound/snd_bg_wedding"));
		soundPoolMap.Add(83, new MyAudioClip("sound/snd_bg_house"));
		soundPoolMap.Add(84, new MyAudioClip("sound/snd_bg_shop"));
		soundPoolMap.Add(85, new MyAudioClip("sound/snd_bg_city"));
	}

	public static void loadSoundsAnimal()
	{
		soundPoolMap.Add(70, new MyAudioClip("sound/snd_ani_cow"));
		soundPoolMap.Add(71, new MyAudioClip("sound/snd_ani_dog"));
		soundPoolMap.Add(72, new MyAudioClip("sound/snd_ani_pig"));
		soundPoolMap.Add(73, new MyAudioClip("sound/snd_ani_chicken"));
	}
}

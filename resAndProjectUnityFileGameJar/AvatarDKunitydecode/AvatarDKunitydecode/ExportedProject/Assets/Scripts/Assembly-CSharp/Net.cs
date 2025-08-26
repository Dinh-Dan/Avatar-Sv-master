using UnityEngine;

internal class Net
{
	public static WWW www;

	public static HTTPHandler h;

	public static void update()
	{
		if (www != null && www.isDone)
		{
			string text = string.Empty;
			if (www.error == null || www.error.Equals(string.Empty))
			{
				text = www.text;
			}
			www = null;
			h.onGetText(text);
			RMS.saveRMS("avServerList", mSystem.convertToSbyte(text));
		}
	}

	public static void connectHTTP(string link, HTTPHandler h)
	{
		if (www != null)
		{
			Debug.LogError("GET HTTP BUSY");
		}
		Debug.LogWarning("REQUEST " + link);
		www = new WWW(link);
		Net.h = h;
	}
}

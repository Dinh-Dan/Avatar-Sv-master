using UnityEngine;

public class GameMidlet
{
	public static string gameID;

	public static bool isEnglish;

	public static bool loadEnglish;

	public static bool saveLanguage;

	public static string IPEng;

	public static int PORTEng;

	public static string[] nameSVEng;

	public static string[][][] IP;

	public static int[][][] PORT;

	public static string[][][] nameSV;

	public static string[][] linkGetHost;

	public static int CLIENT_TYPE;

	public static sbyte PROVIDER;

	public static string AGENT;

	public const string version = "2.5.8";

	public static sbyte VERSIONCODE;

	public const int GAMEID_COTUONG = 5;

	public const int GAMEID_XITO = 6;

	public const int GAMEID_CARO = 1;

	public const int GAMEID_TIENLEN = 3;

	public const int GAMEID_PHOM = 7;

	public const int GAMEID_RACE = 12;

	public const int GAMEID_DAIMOND = 21;

	public const int GAMEID_BAUCUA = 22;

	public static GameMidlet instance;

	public static Avatar avatar;

	public static IndexPlayer myIndexP;

	public static MyVector listContainer;

	static GameMidlet()
	{
		gameID = "12";
		isEnglish = false;
		loadEnglish = false;
		saveLanguage = false;
		IPEng = "112.78.1.25";
		PORTEng = 19128;
		nameSVEng = new string[2] { "International Server", "Aries City" };
		linkGetHost = new string[2][];
		CLIENT_TYPE = 8;
		PROVIDER = 0;
		AGENT = "0";
		VERSIONCODE = 13;
		avatar = new Avatar();
		myIndexP = new IndexPlayer();
		IP = new string[2][][];
		PORT = new int[2][][];
		IP[0] = new string[2][];
		IP[1] = new string[1][];
		PORT[0] = new int[2][];
		PORT[1] = new int[1][];
		IP[0][0] = new string[3] { "avhm.teamobi.com", "avtk.teamobi.com", "avdk.teamobi.com" };
		IP[0][1] = new string[1] { "avbb.teamobi.com" };
		PORT[0][0] = new int[4] { 19128, 19128, 19128, 19128 };
		PORT[0][1] = new int[1] { 19128 };
		PORT[1][0] = new int[1] { 19128 };
		nameSV = T.getNameServer();
		linkGetHost[0] = new string[2] { "http://teamobi.com/srvips/avatarios.txt", "http://teamobi.com/srvips/avatarios.txt" };
		linkGetHost[1] = new string[2] { "http://teamobi.com/srvips/avatarios.txt", "http://teamobi.com/srvips/avatarios.txt" };
	}

	public GameMidlet()
	{
		instance = this;
		if (PROVIDER != 0)
		{
			VERSIONCODE = 0;
		}
		Debug.Log("PROVIDER: " + PROVIDER + "    " + VERSIONCODE);
		Debug.Log("AGENT: " + AGENT);
	}

	public static void sendSMS(string data, string to, IAction successAction, IAction failAction)
	{
		if (to.Contains("sms://"))
		{
			to = to.Remove(0, 6);
		}
		if (SMS.send(data, to) == -1)
		{
			if (failAction != null)
			{
				failAction.perform();
			}
			else
			{
				Canvas.startOKDlg(T.canNotSendMsg);
			}
		}
		else if (successAction != null)
		{
			successAction.perform();
		}
		else
		{
			Canvas.startOKDlg(T.sentMsg);
		}
	}

	public static void flatForm(string url)
	{
		Out.println("flatForm: " + url);
		Application.OpenURL(url);
	}
}

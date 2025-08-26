using System;
using System.Threading;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main main;

	public static Canvas canvas;

	public static MyGraphics g;

	public static GameMidlet midlet;

	public static string res = "sd";

	public static string mainThreadName;

	public static bool started;

	private long lastReleased;

	public static int hdtype;

	private int updateCount;

	private int paintCount;

	public static string text = string.Empty;

	private long timeLimit;

	public static string test = string.Empty;

	private Vector2 lastMousePos = default(Vector2);

	public string s;

	public static bool isCompactDevice = true;

	private void Start()
	{
		if (!started)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && (Screen.height == 768 || Screen.height == 1536))
			{
				Screen.SetResolution(1024, 768, true);
			}
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			Application.runInBackground = true;
			Application.targetFrameRate = 30;
			base.useGUILayout = false;
			isCompactDevice = detectCompactDevice();
			if (main == null)
			{
				main = this;
			}
			started = true;
			ScaleGUI.initScaleGUI();
			if (Thread.CurrentThread.Name != "Main")
			{
				Thread.CurrentThread.Name = "Main";
			}
			mainThreadName = Thread.CurrentThread.Name;
			Debug.Log("Start main thread name: " + mainThreadName);
			SoundManager.init();
			Out.println("aaa: " + ScaleGUI.WIDTH + "    " + ScaleGUI.HEIGHT);
			hdtype = 2;
			g = new MyGraphics();
			midlet = new GameMidlet();
			loadLanguage();
			canvas = new Canvas();
			canvas.start();
			OptionScr.gI().load();
			AvatarData.loadIP();
			canvas.sizeChanged((int)ScaleGUI.WIDTH, (int)ScaleGUI.HEIGHT);
			SplashScr.gI().switchToMe();
			AvatarData.loadMyAccount();
		}
	}

	public static void loadLanguage()
	{
		DataInputStream dataInputStream = null;
		sbyte[] array = RMS.loadRMS("avlanguage");
		if (array != null)
		{
			dataInputStream = new DataInputStream(array);
		}
		if (dataInputStream != null)
		{
			try
			{
				GameMidlet.saveLanguage = dataInputStream.readBoolean();
				GameMidlet.isEnglish = dataInputStream.readBoolean();
				dataInputStream.close();
			}
			catch (Exception e)
			{
				Out.logError(e);
			}
			if (GameMidlet.isEnglish)
			{
				GameMidlet.loadEnglish = true;
			}
		}
	}

	private void OnGUI()
	{
		checkInput();
		if (Event.current.type.Equals(EventType.Repaint))
		{
			canvas.onPaint(g);
			paintCount++;
			g.reset();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (Canvas.currentMyScreen == null)
		{
			return;
		}
		if (paused)
		{
			timeLimit = Canvas.getTick();
			if (Canvas.msgdlg.isWaiting)
			{
				MapScr.gI().exitGame();
			}
			return;
		}
		GlobalLogicHandler.isAutoLogin = false;
		if (!Session_ME.gI().isConnected())
		{
			AvCamera.gI().xCam = (AvCamera.gI().xTo = 100f);
			MapScr.gI().exitGame();
			LoginScr.gI().switchToMe();
		}
		else if ((Canvas.getTick() - timeLimit) / 1000 > 60)
		{
			Canvas.setPopupTime(T.disConnect);
			AvCamera.gI().xCam = (AvCamera.gI().xTo = 100f);
			MapScr.gI().exitGame();
			LoginScr.gI().switchToMe();
		}
	}

	private void checkInput()
	{
		if (Input.anyKeyDown)
		{
			int num = MyKeyMap.map(Event.current.keyCode);
			if (num != 0)
			{
				canvas.onKeyPressed(num);
			}
		}
		if (Event.current.type == EventType.KeyUp)
		{
			int num2 = MyKeyMap.map(Event.current.keyCode);
			if (num2 != 0)
			{
				canvas.onKeyReleased(num2);
			}
		}
	}

	public void checkKey()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = Input.mousePosition;
			canvas.pointerPressed((int)ScaleGUI.scaleX(mousePosition.x), (int)ScaleGUI.scaleY((float)Screen.height - mousePosition.y));
			lastMousePos.x = mousePosition.x;
			lastMousePos.y = mousePosition.y;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition2 = Input.mousePosition;
			canvas.pointerDragged((int)ScaleGUI.scaleX(mousePosition2.x), (int)ScaleGUI.scaleY((float)Screen.height - mousePosition2.y));
			lastMousePos.x = mousePosition2.x;
			lastMousePos.y = mousePosition2.y;
		}
		if (Input.GetMouseButtonUp(0))
		{
			Vector3 mousePosition3 = Input.mousePosition;
			lastMousePos.x = mousePosition3.x;
			lastMousePos.y = mousePosition3.y;
			long num = Environment.TickCount;
			canvas.pointerReleased((int)ScaleGUI.scaleX(mousePosition3.x), (int)ScaleGUI.scaleY((float)Screen.height - mousePosition3.y));
			lastReleased = num;
		}
	}

	private void OnApplicationQuit()
	{
		Debug.LogWarning("APP QUIT");
		Canvas.bRun = false;
		Session_ME.gI().close();
	}

	private void FixedUpdate()
	{
		canvas.update();
	}

	private void Update()
	{
		checkKey();
		ipKeyboard.update();
		RMS.update();
		Image.update();
		DataInputStream.update();
		Player.update();
		SMS.update();
		Session_ME.update();
		Net.update();
		updateCount++;
	}

	public static void exit()
	{
	}

	public static bool detectCompactDevice()
	{
		return true;
	}

	public static bool checkCanSendSMS()
	{
		return false;
	}
}

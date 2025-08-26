public class CardUtils
{
	public const sbyte TYPE_1 = 0;

	public const sbyte TYPE_SANH = 1;

	public const sbyte TYPE_2 = 2;

	public const sbyte TYPE_3 = 3;

	public const sbyte TYPE_3DOITHONG = 4;

	public const sbyte TYPE_4DOITHONG = 5;

	public const sbyte TYPE_TUQUY = 6;

	public static string[] cardValueName = new string[13]
	{
		"3", "4", "5", "6", "7", "8", "9", "10", "J", "Q",
		"K", "A", "Heo"
	};

	public static int penalty;

	public static int money;

	public static string penaltyDes;

	public static bool checkForceFinish(MyVector cards, bool newgame)
	{
		sbyte[] array = new sbyte[cards.size()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ((Card)cards.elementAt(i)).cardID;
		}
		int j;
		for (j = 9; j < 13 && array[j] / 4 == 12; j++)
		{
		}
		if (j == 13)
		{
			return true;
		}
		for (j = 0; j < 4 && array[j] / 4 == 0; j++)
		{
		}
		if (j == 4 && newgame)
		{
			return true;
		}
		int num = 0;
		for (j = 0; j < 12; j++)
		{
			if (array[j] / 4 == array[j + 1] / 4 - 1 && array[j + 1] / 4 != 12)
			{
				num++;
			}
			else if (array[j] / 4 != array[j + 1] / 4)
			{
				break;
			}
		}
		if (num >= 11)
		{
			return true;
		}
		num = 0;
		for (j = 0; j < 12; j++)
		{
			if (array[j] / 4 == array[j + 1] / 4)
			{
				num++;
				j++;
			}
		}
		if (num >= 6)
		{
			return true;
		}
		if (demDoiThong(array) >= 5)
		{
			return true;
		}
		return false;
	}

	private static int demDoiThong(sbyte[] bai)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < bai.Length - 1; i++)
		{
			if (bai.Length >= 48)
			{
				break;
			}
			if (num2 == 0 && bai[i] / 4 == bai[i + 1] / 4)
			{
				num2 = 1;
			}
			else if (num2 % 2 == 1)
			{
				if (bai[i] / 4 == bai[i + 1] / 4 - 1)
				{
					num2++;
				}
				else if (bai[i] / 4 != bai[i + 1] / 4)
				{
					if (num2 > num)
					{
						num = num2;
					}
					num2 = 0;
				}
			}
			else if (bai[i] / 4 == bai[i + 1] / 4)
			{
				num2++;
			}
			else
			{
				if (num2 > num)
				{
					num = num2;
				}
				num2 = 0;
			}
		}
		if (num2 > num)
		{
			num = num2;
		}
		return (num + 1) / 2;
	}

	public static void sort(sbyte[] bai)
	{
		for (int i = 0; i < bai.Length - 1; i++)
		{
			for (int j = i + 1; j < bai.Length; j++)
			{
				if (bai[i] > bai[j])
				{
					sbyte b = bai[i];
					bai[i] = bai[j];
					bai[j] = b;
				}
			}
		}
	}

	public static sbyte getType(sbyte[] bai)
	{
		if (is1(bai))
		{
			return 0;
		}
		if (is2(bai))
		{
			return 2;
		}
		if (is3(bai))
		{
			return 3;
		}
		if (is3DoiThong(bai))
		{
			return 4;
		}
		if (is4DoiThong(bai))
		{
			return 5;
		}
		if (isTuQuy(bai))
		{
			return 6;
		}
		if (isSanh(bai))
		{
			return 1;
		}
		return -1;
	}

	public static bool is1(sbyte[] bai)
	{
		return bai.Length == 1;
	}

	public static bool isSanh(sbyte[] bai)
	{
		if (bai.Length < 3)
		{
			return false;
		}
		for (int i = 1; i < bai.Length; i++)
		{
			if (bai[i - 1] / 4 != bai[i] / 4 - 1)
			{
				return false;
			}
		}
		if (bai[bai.Length - 1] / 4 == 12)
		{
			return false;
		}
		return true;
	}

	public static bool is2(sbyte[] bai)
	{
		if (bai.Length == 2 && bai[0] / 4 == bai[1] / 4)
		{
			return true;
		}
		return false;
	}

	public static bool is3(sbyte[] bai)
	{
		if (bai.Length == 3 && bai[0] / 4 == bai[1] / 4 && bai[1] / 4 == bai[2] / 4)
		{
			return true;
		}
		return false;
	}

	public static bool isTuQuy(sbyte[] bai)
	{
		if (bai.Length == 4 && bai[0] / 4 == bai[1] / 4 && bai[1] / 4 == bai[2] / 4 && bai[2] / 4 == bai[3] / 4)
		{
			return true;
		}
		return false;
	}

	public static bool is3DoiThong(sbyte[] bai)
	{
		if (bai.Length != 6)
		{
			return false;
		}
		for (int i = 1; i < bai.Length; i++)
		{
			if (i % 2 == 1 && bai[i - 1] / 4 != bai[i] / 4)
			{
				return false;
			}
			if (i % 2 == 0 && bai[i - 1] / 4 != bai[i] / 4 - 1)
			{
				return false;
			}
		}
		return true;
	}

	public static bool is4DoiThong(sbyte[] bai)
	{
		if (bai.Length != 8)
		{
			return false;
		}
		for (int i = 1; i < bai.Length; i++)
		{
			if (i % 2 == 1 && bai[i - 1] / 4 != bai[i] / 4)
			{
				return false;
			}
			if (i % 2 == 0 && bai[i - 1] / 4 != bai[i] / 4 - 1)
			{
				return false;
			}
		}
		return true;
	}

	public static bool available(sbyte[] bai_sapdanh, sbyte type_bai_sapdanh, sbyte[] baidanh, sbyte type_baidanh)
	{
		penalty = 0;
		switch (type_baidanh)
		{
		case -1:
			if (type_bai_sapdanh != -1)
			{
				return true;
			}
			break;
		case 0:
			if (type_bai_sapdanh == 0 && bai_sapdanh[0] > baidanh[0])
			{
				return true;
			}
			if (baidanh[0] / 4 == 12 && (type_bai_sapdanh == 4 || type_bai_sapdanh == 5 || type_bai_sapdanh == 6))
			{
				if (baidanh[0] % 4 < 2)
				{
					penalty = money / 2;
					penaltyDes = T.chatHeo[0];
				}
				else
				{
					penalty = money;
					penaltyDes = T.chatHeo[1];
				}
				return true;
			}
			break;
		case 2:
			if (type_bai_sapdanh == 2 && bai_sapdanh[1] > baidanh[1])
			{
				return true;
			}
			if (baidanh[0] / 4 == 12 && (type_bai_sapdanh == 6 || type_bai_sapdanh == 5))
			{
				if (baidanh[1] % 4 < 2)
				{
					penalty = money;
					penaltyDes = T.chatHeo[2];
				}
				else if (baidanh[0] % 4 >= 2)
				{
					penalty = 2 * money;
					penaltyDes = T.chatHeo[3];
				}
				else
				{
					penalty = money + money / 2;
					penaltyDes = T.chatHeo[4];
				}
				return true;
			}
			break;
		case 3:
			if (type_bai_sapdanh == 3 && bai_sapdanh[2] > baidanh[2])
			{
				return true;
			}
			break;
		case 1:
			if (type_bai_sapdanh == 1 && bai_sapdanh.Length == baidanh.Length && bai_sapdanh[bai_sapdanh.Length - 1] > baidanh[baidanh.Length - 1])
			{
				return true;
			}
			break;
		case 4:
			if ((type_bai_sapdanh == 4 && bai_sapdanh[5] > baidanh[5]) || type_bai_sapdanh == 6 || type_bai_sapdanh == 5)
			{
				penalty = money;
				penaltyDes = T.chatHeo[5];
				return true;
			}
			break;
		case 6:
			if ((type_bai_sapdanh == 6 && bai_sapdanh[3] > baidanh[3]) || type_bai_sapdanh == 5)
			{
				penalty = money + money / 2;
				penaltyDes = T.chatHeo[6];
				return true;
			}
			break;
		case 5:
			if (type_bai_sapdanh == 5 && bai_sapdanh[7] > baidanh[7])
			{
				penalty = 2 * money;
				penaltyDes = T.chatHeo[7];
				return true;
			}
			break;
		}
		return false;
	}
}

public class RoomInfo
{
	public sbyte id;

	public sbyte roomFree;

	public sbyte roomWait;

	public sbyte lv;

	public RoomInfo(sbyte id, sbyte roomFree, sbyte roomWait, sbyte lv)
	{
		this.id = id;
		this.roomFree = roomFree;
		this.roomWait = roomWait;
		this.lv = lv;
	}

	public RoomInfo()
	{
	}
}

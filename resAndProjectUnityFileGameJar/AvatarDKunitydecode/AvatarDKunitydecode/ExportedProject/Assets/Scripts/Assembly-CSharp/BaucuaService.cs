public class BaucuaService
{
	private static BaucuaService me;

	public ISession session = Session_ME.gI();

	public static BaucuaService gI()
	{
		return (me != null) ? me : (me = new BaucuaService());
	}
}

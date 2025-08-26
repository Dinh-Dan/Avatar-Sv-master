using System;
using System.Collections;
using System.IO;

public class FilePack
{
	public static Hashtable cachedFilePack = new Hashtable();

	public static FilePack instance;

	public static FilePack instanceHome;

	private string[] fname;

	private int[] fpos;

	private int[] flen;

	private sbyte[] fullData;

	private int nFile;

	private int hSize;

	private string name;

	public sbyte[] code = new sbyte[13]
	{
		78, 103, 117, 121, 101, 110, 86, 97, 110, 77,
		105, 110, 104
	};

	private int codeLen;

	private DataInputStream file;

	public FilePack(string name)
	{
		codeLen = code.Length;
		int num = 0;
		int num2 = 0;
		this.name = name;
		hSize = 0;
		open();
		try
		{
			nFile = encode(file.readUnsignedByte());
			hSize++;
			fname = new string[nFile];
			fpos = new int[nFile];
			flen = new int[nFile];
			for (int i = 0; i < nFile; i++)
			{
				int num3 = encode(file.readByte());
				sbyte[] data = new sbyte[num3];
				file.read(ref data);
				encode(data);
				fname[i] = new string(ArrayCast.ToCharArray(data));
				fpos[i] = num;
				flen[i] = encode(file.readUnsignedShort());
				num += flen[i];
				num2 += flen[i];
				hSize += num3 + 3;
			}
			fullData = new sbyte[num2];
			file.readFully(ref fullData);
			encode(fullData);
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
		close();
	}

	public static void reset()
	{
		instance.close();
		instance = null;
	}

	public static Image getImg(string path)
	{
		return instance.loadImage(path + string.Empty);
	}

	public static Image getImgHome(string path)
	{
		return instanceHome.loadImage(path + string.Empty);
	}

	public static void init(string path)
	{
		FilePack filePack = (FilePack)cachedFilePack[path];
		if (filePack != null)
		{
			instance = filePack;
			return;
		}
		instance = new FilePack(T.getPath() + path);
		cachedFilePack.Add(path, instance);
	}

	public static void initHome(string path)
	{
		instanceHome = new FilePack(T.getPath() + path);
	}

	private int encode(int i)
	{
		return i;
	}

	private void encode(sbyte[] bytes)
	{
		int num = bytes.Length;
		for (int i = 0; i < num; i++)
		{
			bytes[i] ^= code[i % codeLen];
		}
	}

	private void open()
	{
		file = DataInputStream.getResourceAsStream(name);
	}

	public void close()
	{
		try
		{
			if (file != null)
			{
				file.close();
			}
		}
		catch (IOException e)
		{
			Out.logError(e);
		}
	}

	public sbyte[] loadFile(string fileName)
	{
		for (int i = 0; i < nFile; i++)
		{
			if (fname[i].CompareTo(fileName) == 0)
			{
				sbyte[] array = new sbyte[flen[i]];
				Array.Copy(fullData, fpos[i], array, 0, flen[i]);
				return array;
			}
		}
		throw new Exception("File '" + fileName + "' not found!");
	}

	public Image loadImage(string fileName)
	{
		fileName += ".png";
		for (int i = 0; i < nFile; i++)
		{
			if (fname[i].CompareTo(fileName) == 0)
			{
				sbyte[] array = new sbyte[flen[i]];
				Array.Copy(fullData, fpos[i], array, 0, flen[i]);
				return Image.createImage(ArrayCast.cast(array));
			}
		}
		return null;
	}

	public sbyte[] loadData(string name)
	{
		for (int i = 0; i < nFile; i++)
		{
			if (fname[i].CompareTo(name) == 0)
			{
				sbyte[] array = new sbyte[flen[i]];
				Array.Copy(fullData, fpos[i], array, 0, flen[i]);
				return array;
			}
		}
		return null;
	}
}

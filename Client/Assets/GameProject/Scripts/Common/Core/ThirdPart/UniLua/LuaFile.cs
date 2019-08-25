using System;
using System.IO;
using System.Collections.Generic;

namespace UniLua
{
    public delegate byte[] CustomFileLoader(ref string filepath);

	public class LuaFile
	{
        private static List<CustomFileLoader> customLoaders = new List<CustomFileLoader>();

        public static void AddLoader(CustomFileLoader loader)
        {
            customLoaders.Add(loader);
        }

        public static FileLoadInfo OpenFile(string filename)
		{
            foreach (var loader in customLoaders)
            {
                byte[] bytes = loader(ref filename);
                if (bytes != null)
                {
                    return new FileLoadInfo(bytes);
                }
            }
            return null;
			
		}

		public static bool Readable( string filename )
		{
            foreach (var loader in customLoaders)
            {
                byte[] bytes = loader(ref filename);
                if (bytes != null)
                {
                    return true;
                }
            }
            return false;
		}
	}

    public class FileLoadInfo : ILoadInfo, IDisposable
    {
        private Queue<byte> Buf;

        public FileLoadInfo(byte[] bytes) {
            Buf = new Queue<byte>(bytes);
        }

        public int ReadByte()
        {
            if (Buf.Count > 0)
                return (int)Buf.Dequeue();
            else
                return -1;
        }

        public int PeekByte()
        {
            if (Buf.Count > 0)
                return (int)Buf.Peek();
            else
                return -1;
        }

        public void Dispose()
        {
           
        }
    }
    /*
	public class FileLoadInfo : ILoadInfo, IDisposable
	{
        private const string UTF8_BOM = "\u00EF\u00BB\u00BF";
        private FileStream Stream;
        private StreamReader Reader;
        private Queue<char> Buf;

		public FileLoadInfo( FileStream stream )
		{
			Stream = stream;
            Reader = new StreamReader(Stream, System.Text.Encoding.UTF8);
			Buf = new Queue<char>();
            SkipComment();
		}

        private void Save(char b)
        {
            Buf.Enqueue(b);
        }

        private void Clear()
        {
            Buf.Clear();
        }

        private void SkipComment()
        {
            var c = Reader.Read();//SkipBOM();
            // first line is a comment (Unix exec. file)?
            if (c == '#')
            {
                do
                {
                    c = Reader.Read();
                } while (c != -1 && c != '\n');
                Save((char)'\n'); // fix line number
            }
            else if (c != -1)
            {
                Save((char)c);
            }
        }

		public int ReadByte()
		{
			if( Buf.Count > 0 )
				return (int)Buf.Dequeue();
			else
				return Reader.Read();
		}

		public int PeekByte()
		{
			if( Buf.Count > 0 )
				return (int)Buf.Peek();
			else
			{
				var c = Reader.Read();
				if( c == -1 )
					return c;
				Save( (char)c );
				return c;
			}
		}

		public void Dispose()
		{
            Reader.Dispose();
			Stream.Dispose();
		}

		

#if false
		private int SkipBOM()
		{
			for( var i=0; i<UTF8_BOM.Length; ++i )
			{
				var c = Stream.ReadByte();
				if(c == -1 || c != (byte)UTF8_BOM[i])
					return c;
				Save( (char)c );
			}
			// perfix matched; discard it
			Clear();
			return Stream.ReadByte();
		}
#endif

		
	}
    */
}


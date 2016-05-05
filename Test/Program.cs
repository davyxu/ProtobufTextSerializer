using System;
using System.Collections.Generic;
using System.Linq;
using ProtobufText;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Test
{    
    class Program
    {
        [Serializable()]  
        class CharDefine
        {
            public CharDefine() { }
            public float Tick { get; set; }
            public int LastLoginUTC { get; set; }
            public string CharName { get; set; }
        }

        [Serializable()]  
        class CharList
        {
            public CharList() { }

            List<CharDefine> _info = new List<CharDefine>();
            public List<CharDefine> CharInfo
            {
                get { return _info; }

            }
        }

        static void Main(string[] args)
        {            
            var charlist = new CharList();

            {
                var chardef = new CharDefine();
                chardef.CharName = "hello";
                chardef.LastLoginUTC = 123;
                chardef.Tick = 3.14f;

                charlist.CharInfo.Add( chardef );
            }

            {
                var chardef = new CharDefine();
                chardef.CharName = "world";
                chardef.LastLoginUTC = 345;

                charlist.CharInfo.Add(chardef);
            }

            TestInOut(charlist);

            // 字符串转义
            var originalText = "Tick: 0 LastLoginUTC: 0 CharName: \"a\\rb\\nc\"";
            var structOut = Serializer.Deserialize<CharDefine>(originalText);
            var textOut = Serializer.Serialize(structOut).Trim();
            Debug.Assert(originalText == textOut);

            
        }

        static void TestInOut<T>( T structIn )
        {
            var formatter = new BinaryFormatter();



            var memIn = new MemoryStream();
            formatter.Serialize(memIn, structIn);

            var text = Serializer.Serialize(structIn);

            var structOut = Serializer.Deserialize<T>(text);

            var memOut = new MemoryStream();
            formatter.Serialize(memOut, structOut);

            if (!CompareMemoryStreams(memIn, memOut))
            {
                Debug.Assert(false);
            }
        }

        private static bool CompareMemoryStreams(MemoryStream ms1, MemoryStream ms2)
        {
            if (ms1.Length != ms2.Length)
                return false;
            ms1.Position = 0;
            ms2.Position = 0;

            var msArray1 = ms1.ToArray();
            var msArray2 = ms2.ToArray();

            return msArray1.SequenceEqual(msArray2);
        }
    }
}

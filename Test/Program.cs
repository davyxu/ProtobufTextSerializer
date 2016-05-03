using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtobufTextSerializer;

namespace Test
{    
    class Program
    {
        class CharDefine
        {
            public CharDefine() { }
            public int LastLoginUTC { get; set; }
            public string CharName { get; set; }
        }

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
            var p = new Parser();

            var t = new CharList();

            p.Merge("CharInfo {CharName: 'cat' LastLoginUTC: 123}  CharInfo {CharName: \"dog\" LastLoginUTC: 456}", new Message(t));
        }
    }
}

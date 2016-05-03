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
        static void Main(string[] args)
        {
            var p = new Parser();
            p.Merge("Info {Rank: -1 RewardID: 40020001 BoxIcon: \"hello\"}", new ClassSetter() );
        }
    }
}

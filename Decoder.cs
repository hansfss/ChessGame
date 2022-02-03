using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessDemo
{
     class Decoder
    {
        public int ASCIIConverter(string s)
        {//converts {a, b, c, d, e, f, g, h} to {0, 1, 2, 3, 4, 5, 6, 7}
            int value = char.Parse(s.Substring(0, 1)) - 97;
            return value;
        }

        public int rowConverter(int row)//converts value of row in tileName into point(Y) value on board
        {
            int _row = 0;
            switch (row)
            {
                case 0:
                    _row = 7;
                    break;
                case 1:
                    _row = 6;
                    break;
                case 2:
                    _row = 5;
                    break;
                case 3:
                    _row = 4;
                    break;
                case 4:
                    _row = 3;
                    break;
                case 5:
                    _row = 2;
                    break;
                case 6:
                    _row = 1;
                    break;
                case 7:
                    _row = 0;
                    break;
            }
            return _row;
        }
    }
}

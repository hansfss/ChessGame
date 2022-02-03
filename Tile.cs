using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessDemo
{
    public class Tile
    {
        public string name;//name that will be compared with name of button for each tile, names (00,01,02) which will be split to determine point
        public Point position;
        public bool hasPiece;
        public TileColour colour;

        public Tile()
        {
            this.name = "";
            this.position = new Point(0, 0);
            this.hasPiece = false;
            this.colour = TileColour.Black;
        }

        public Tile(string Name, int x, int y, bool haspiece, TileColour Colour)
        {
            this.name = Name;
            this.position = new Point(x, y);
            this.hasPiece = haspiece;
            this.colour = Colour;
        }

        public string convertTileName()
        {
            string nombre = name;
            string number = name[0].ToString();
            int num = int.Parse(number);
            char letter = '-';
            switch (num)
            {
                case 0:
                    letter = 'a';
                    break;
                case 1:
                    letter = 'b';
                    break;
                case 2:
                    letter = 'c';
                    break;
                case 3:
                    letter = 'd';
                    break;
                case 4:
                    letter = 'e';
                    break;
                case 5:
                    letter = 'f';
                    break;
                case 6:
                    letter = 'g';
                    break;
                case 7:
                    letter = 'h';
                    break;
            }
            name = letter + "" + nombre[1];
            return name;
        }
    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessDemo
{
	public class ChessPiece/* : ViewModelBase*/
	{
        public Point Pos { get; set; }
        public PieceType Type { get; set; }
        public Player Player { get; set; }

        public ChessPiece()
        {
            this.Pos = new Point(20, 20);
        }

        public ChessPiece(int x, int y, PieceType type, Player player)
        {
            this.Pos = new Point(x,y);
            this.Type = type;
            this.Player = player;
        }

        public ChessPiece movePiece(ObservableCollection<ChessPiece> pieces, int x, int y)
        {
            ChessPiece piece;
            PieceType type = Type;
            Player colour = Player;
            pieces.Add(piece = new ChessPiece(x, y, type, colour));
            pieces.Remove(this);
            return piece;
        }

        public void eliminateOpposingPiece(ObservableCollection<ChessPiece> pieces, ChessPiece movedPiece, ChessPiece piece)
        {
            if (movedPiece.Pos == piece.Pos && movedPiece.Player != piece.Player)
            {
                pieces.Remove(piece);
            }
        }
        /*
         * int parameter in pieceMovement is always = 1 or 2, depends on whether or not I want the method to return vertical pawn movements
         */
        //try passing chesspiece being moved as a parameter as well to avoid stupid fucking issue where method goes inside if of black pawn
        //index out of bounds issue because value of c or r are greater than 7 or less than 0 on some occasions
        public List<Point> pieceMovement(Tile[,] tiles, ObservableCollection<ChessPiece> pieces, ChessPiece piece, int dual)
        {//method returns list of points that a piece can move to, depending on type of piece
            //GET PIECE POSITION AND USE IT TO AVOID INDEX ISSUES WHE NSELECTING TILES THAT ARE OUTSIDE THE PIECES RANGE

            List<Point> movement = new List<Point>();
            //x = column, y = row
            int x = int.Parse(Pos.X.ToString());//coordinates of point are assigned to x and y
            int y = int.Parse(Pos.Y.ToString());
            int _x = x, x_ = x;
            int _y = y, y_ = y;
            bool breakWhile = false, breakLDiagonal = false, breakUDiagonal = false;//used to break loops if iteration is no longer necessary
              ////////////////////////////////////////////
             //////////P A W N  M O V E M E N T//////////
            ////////////////////////////////////////////
            //MessageBox.Show(Pos.ToString() + ", " + Player);
            if (Type == PieceType.Pawn && Player == Player.White)
            {//try == new Point() again
                if (tiles[x, y - 1].hasPiece == false && dual != 2)
                    movement.Add(new Point(x, y - 1));
                if (tiles[x, y - 2].hasPiece == false && y == 6 && dual != 2)
                    movement.Add(new Point(x, y - 2));
                if (tiles[x - 1, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y - 1));
                if (tiles[x + 1, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y - 1));
            }
            else if (Type == PieceType.Pawn && Player == Player.Black)
            {//WHY THE FUCK IS THE PROGRAM GOING INSIDE THIS IF WHEN I SELECT A WHITE PAWN!!!!!!!!!!!!!!??????!?!?!?!?!?!?!?!?!?!?!
                if (tiles[x, y + 1].hasPiece == false && dual != 2)
                    movement.Add(new Point(x, y + 1));
                if (tiles[x, y + 2].hasPiece == false && y == 1 && dual != 2)
                    movement.Add(new Point(x, y + 2));
                if (tiles[x - 1, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y + 1));
                if (tiles[x + 1, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y + 1));
            }
            ////////////////////////////////////////////
            //////////R O O K  M O V E M E N T//////////
            ////////////////////////////////////////////
            if (Type == PieceType.Rook)
            {
                //bool breakWhile = false;//breaks while loop if adjacent tile is occupied
                while (x >= 0 && breakWhile == false)
                {//change in value of x represents lateral movement
                    if (tiles[x - 1, y].hasPiece == false)
                    {
                        movement.Add(new Point(x - 1, y));
                    }
                    else if (tiles[x - 1, y].hasPiece == true)
                    {
                        if (tiles[x - 1, y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x - 1, y));
                        }
                        breakWhile = true;
                    }
                    x--;
                }
                breakWhile = false;
                while (_x < 7 && breakWhile == false)
                {
                    _x++;//value is incremented first cuz initial value of x was used in previous while loop
                    if (tiles[_x, y].hasPiece == false)
                    {
                        movement.Add(new Point(_x, y));
                    }
                    else if (tiles[_x, y].hasPiece == true)
                    {
                        if (tiles[_x, y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(_x, y));
                        }
                        breakWhile = true;
                    }
                }
                breakWhile = false;
                while (y >= 0 && breakWhile == false)
                {//change in value of y represents vertical movement
                    if (tiles[x_, y - 1].hasPiece == false)
                    {
                        movement.Add(new Point(x_, y - 1));
                    }
                    else if (tiles[x_, y - 1].hasPiece == true)
                    {
                        if (tiles[x_, y - 1].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_, y - 1));
                        }
                        breakWhile = true;
                    }
                    y--;//change in y represents a different row
                }
                breakWhile = false;
                while (_y < 7 && breakWhile == false)
                {
                    _y++;
                    if (tiles[x_, _y].hasPiece == false)
                    {
                        movement.Add(new Point(x_, _y));
                    }
                    else if (tiles[x_, _y].hasPiece == true)
                    {
                        if (tiles[x_, _y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_, _y));
                        }
                        breakWhile = true;
                    }
                }
            }
              ////////////////////////////////////////////
             ////////K N I G H T  M O V E M E N T////////
            ////////////////////////////////////////////
            if (Type == PieceType.Knight)
            {
                if (tiles[x + 1, y + 2].hasPiece == false || tiles[x + 1, y + 2].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y + 2));
                else if (tiles[x + 1, y - 2].hasPiece == false || tiles[x + 1, y - 2].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y - 2));
                else if (tiles[x - 1, y + 2].hasPiece == false || tiles[x - 1, y + 2].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y + 2));
                else if (tiles[x - 1, y - 2].hasPiece == false || tiles[x - 1, y - 2].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y - 2));
                else if (tiles[x + 2, y + 1].hasPiece == false || tiles[x + 2, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 2, y + 1));
                else if (tiles[x + 2, y - 1].hasPiece == false || tiles[x + 2, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 2, y - 1));
                else if (tiles[x - 2, y + 1].hasPiece == false || tiles[x - 2, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 2, y + 1));
                else if (tiles[x - 2, y - 1].hasPiece == false || tiles[x - 2, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 2, y - 1));
            }
              ////////////////////////////////////////////
             ////////B I S H O P  M O V E M E N T////////
            ////////////////////////////////////////////
            if (Type == PieceType.Bishop)
            {//x_ remains unchanged inside this if
                while (x < 7 && breakLDiagonal == false)
                {
                    x++;
                    if (tiles[x, y + (x - x_)].hasPiece == true)
                    {
                        if (tiles[x, y + (x - x_)].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x, y + (x - x_)));
                        }
                        breakLDiagonal = true;
                    }
                    else if (tiles[x, y + (x - x_)].hasPiece == false)
                    {
                        movement.Add(new Point(x, y + (x - x_)));
                    }
                }
                while (_x >= 0 && breakUDiagonal == false)
                {
                    _x--;
                    if (tiles[_x, y - (x_ - _x)].hasPiece == true)
                    {
                        if (tiles[_x, y - (x_ - _x)].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(_x, y - (x_ - _x)));
                        }
                        breakUDiagonal = true;
                    }
                    else if (tiles[_x, y - (x_ - _x)].hasPiece == false)
                    {
                        movement.Add(new Point(_x, y - (x_ - _x)));
                    }
                }
                breakLDiagonal = false;
                breakUDiagonal = false;
                while (y < 7 && breakLDiagonal == false)
                {
                    y++;
                    if (tiles[x_ - (y - _y), y].hasPiece == false && breakLDiagonal == false)
                    {
                        movement.Add(new Point(x_ - (y - _y), y));
                    }
                    else if (tiles[x_ - (y - _y), y].hasPiece == true)
                    {
                        if (tiles[x_ - (y - _y), y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_ - (y - _y), y));
                        }
                        breakLDiagonal = true;
                    }
                }
                while (_y >= 0 && breakUDiagonal == false)
                {
                    _y--;
                    if (tiles[x_ + (y_ - _y), _y].hasPiece == false && breakUDiagonal == false)
                    {
                        movement.Add(new Point(x_ + (y_ - _y), _y));
                    }
                    else if (tiles[x_ + (y_ - _y), _y].hasPiece == true)
                    {
                        if (tiles[x_ + (y_ - _y), _y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_ + (y_ - _y), _y));
                        }
                        breakUDiagonal = true;
                    }
                }
            }
              ////////////////////////////////////////////
             /////////Q U E E N  M O V E M E N T/////////
            ////////////////////////////////////////////
            if (Type == PieceType.Queen)
            {//contains elements of code in rook and bishop movement  
                while (x < 7 && breakLDiagonal == false || x < 7 && breakWhile == false)
                {//lateral movement to the right of the board
                    x++;
                    if (tiles[x, y + (x - x_)].hasPiece == true && breakLDiagonal == false)
                    {
                        if (tiles[x, y + (x - x_)].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x, y + (x - x_)));
                        }
                        breakLDiagonal = true;
                    }
                    else if (tiles[x, y + (x - x_)].hasPiece == false && breakLDiagonal == false)
                    {
                        movement.Add(new Point(x, y + (x - x_)));
                    }
                    if (tiles[x, y].hasPiece == false && breakWhile == false)
                    {
                        movement.Add(new Point(x, y));
                    }
                    else if (tiles[x, y].hasPiece == true && breakWhile == false)
                    {
                        if (tiles[x, y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x, y));
                        }
                        breakWhile = true;
                    }
                }
                breakWhile = false;
                while (_x >= 0 && breakUDiagonal == false || _x >= 0 && breakWhile == false)
                {
                    _x--;
                    if (tiles[_x, y - (x_ - _x)].hasPiece == true && breakUDiagonal == false)
                    {
                        if (tiles[_x, y - (x_ - _x)].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(_x, y - (x_ - _x)));
                        }
                        breakUDiagonal = true;
                    }
                    else if (tiles[_x, y - (x_ - _x)].hasPiece == false && breakUDiagonal == false)
                    {
                        movement.Add(new Point(_x, y - (x_ - _x)));
                    }
                    if (tiles[_x, y].hasPiece == false && breakWhile == false)
                    {
                        movement.Add(new Point(_x, y));
                    }
                    else if (tiles[_x, y].hasPiece == true && breakWhile == false)
                    {
                        if (tiles[_x, y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(_x, y));
                        }
                        breakWhile = true;
                    }
                }
                breakUDiagonal = false;
                breakLDiagonal = false;
                breakWhile = false;
                while (y < 7 && breakLDiagonal == false || y < 7 && breakWhile == false)
                {
                    y++;
                    if (tiles[x_ - (y - _y), y].hasPiece == false && breakLDiagonal == false)
                    {
                        movement.Add(new Point(x_ - (y - _y), y));
                    }
                    else if (tiles[x_ - (y - _y), y].hasPiece == true && breakLDiagonal == false)
                    {
                        if (tiles[x_ - (y - _y), y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_ - (y - _y), y));
                        }
                        breakLDiagonal = true;
                    }
                    if (tiles[x_, y].hasPiece == false && breakWhile == false)
                    {
                        movement.Add(new Point(x_, y));
                    }
                    else if (tiles[x_, y].hasPiece == true && breakWhile == false)
                    {
                        if (tiles[x_, y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_, y));
                        }
                        breakWhile = true;
                    }
                }
                breakWhile = false;
                while (_y >= 0 && breakUDiagonal == false || _y >= 0 && breakWhile == false)
                {
                    _y--;
                    if (tiles[x_ + (y_ - _y), _y].hasPiece == false && breakUDiagonal == false)
                    {
                        movement.Add(new Point(x_ + (y_ - _y), _y));
                    }
                    else if (tiles[x_ + (y_ - _y), _y].hasPiece == true && breakUDiagonal == false)
                    {
                        if (tiles[x_ + (y_ - _y), _y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_ + (y_ - _y), _y));
                        }
                        breakUDiagonal = true;
                    }
                    if (tiles[x_, _y].hasPiece == false && breakWhile == false)
                    {
                        movement.Add(new Point(x_, _y));
                    }
                    else if (tiles[x_, _y].hasPiece == true && breakWhile == false)
                    {
                        if (tiles[x_, _y].hasPiece == true && Player != piece.Player)
                        {
                            movement.Add(new Point(x_, _y));
                        }
                        breakWhile = true;
                    }
                }
            }
              ////////////////////////////////////////////
             //////////K I N G  M O V E M E N T//////////
            ////////////////////////////////////////////
            if (Type == PieceType.King)
            {
                if (tiles[x + 1, y + 1].hasPiece == false || tiles[x + 1, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y + 1));
                else if (tiles[x + 1, y - 1].hasPiece == false || tiles[x + 1, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y - 1));
                else if (tiles[x - 1, y + 1].hasPiece == false || tiles[x - 1, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y + 1));
                else if (tiles[x - 1, y - 1].hasPiece == false || tiles[x - 1, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y - 1));
                else if (tiles[x, y + 1].hasPiece == false || tiles[x, y + 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x, y + 1));
                else if (tiles[x, y - 1].hasPiece == false || tiles[x, y - 1].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x, y - 1));
                else if (tiles[x + 1, y].hasPiece == false || tiles[x + 1, y].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x + 1, y));
                else if (tiles[x - 1, y].hasPiece == false || tiles[x - 1, y].hasPiece == true && Player != piece.Player)
                    movement.Add(new Point(x - 1, y));
            }
            return movement;
        }
        /*public List<Point> pieceMovement(List<Tile> tiles, ObservableCollection<ChessPiece> pieces, ChessPiece piece, int dual)
        {//method returns list of points that a piece can move to, depending on type of piece
            List<Point> movement = new List<Point>();
            //x = column, y = row
            int x = int.Parse(Pos.X.ToString());//coordinates of point are assigned to x and y
            int y = int.Parse(Pos.Y.ToString());
            int _x = x, x_ = x;
            int _y = y, y_ = y;
            bool breakWhile = false, breakLDiagonal = false, breakUDiagonal = false;//used to break loops if iteration is no longer necessary
             ////////////////////////////////////////////
            //////////P A W N  M O V E M E N T//////////
           ////////////////////////////////////////////
            if (Type == PieceType.Pawn && Player == Player.White)
            {
                foreach (Tile tile in tiles)
                {//foreach loop is used to find tiles and see if they already have chess pieces
                    //if (tiles[r,c - 1].hasPiece == false && dual != 2) i think this works...
                    if (tile.position == new Point(x, y - 1) && tile.hasPiece == false && dual != 2)
                        movement.Add(new Point(x, y - 1));
                    if (tile.position == new Point(x, y - 2) && tile.hasPiece == false && y == 6 && dual != 2)
                        movement.Add(new Point(x, y - 2));
                    if (tile.position == new Point(x - 1, y - 1) && tile.hasPiece == true && Player != piece.Player)
                        movement.Add(new Point(x - 1, y - 1));
                    if (tile.position == new Point(x + 1, y - 1) && tile.hasPiece == true && Player != piece.Player)
                        movement.Add(new Point(x + 1, y - 1));
                    if (movement.Count == 4)//if count = 4, there are no more places for the pawn to move so we break the loop and save time
                        break;
                }
            }
            else if (Type == PieceType.Pawn && Player == Player.Black)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile.position == new Point(x, y + 1) && tile.hasPiece == false && dual != 2)
                        movement.Add(new Point(x, y + 1));
                    if (tile.position == new Point(x, y + 2) && tile.hasPiece == false && y == 1 && dual != 2)
                        movement.Add(new Point(x, y + 2));
                    if (tile.position == new Point(x - 1, y + 1) && tile.hasPiece == true && Player != piece.Player)
                        movement.Add(new Point(x - 1, y + 1));
                    if (tile.position == new Point(x + 1, y + 1) && tile.hasPiece == true && Player != piece.Player)
                        movement.Add(new Point(x + 1, y + 1));
                    if (movement.Count == 4)
                        break;
                }
            }
             ////////////////////////////////////////////
            //////////R O O K  M O V E M E N T//////////
           ////////////////////////////////////////////
            if (Type == PieceType.Rook)
            {
                //bool breakWhile = false;//breaks while loop if adjacent tile is occupied
                while (x >= 0 && breakWhile == false)
                {//columns
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(x - 1, y) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(x - 1, y));
                            break;
                        }
                        else if (tile.position == new Point(x - 1, y) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x - 1, y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x - 1, y));
                            }
                            breakWhile = true;
                            break;
                        }
                    }
                    x--;//change in value of x represents a different column
                }
                breakWhile = false;
                while (_x < 7 && breakWhile == false)
                {
                    _x++;//value is incremented first cuz initial value of x was used in previous while loop
                    foreach(Tile tile in tiles)
                    {
                        if (tile.position == new Point(_x, y) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(_x, y));
                            break;
                        }
                        else if (tile.position == new Point(_x, y) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(_x, y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(_x, y));
                            }
                            breakWhile = true;
                            break;
                        }
                    }
                }
                breakWhile = false;
                while (y >= 0 && breakWhile == false)
                {
                    foreach(Tile tile in tiles)
                    {
                        if (tile.position == new Point(x_, y - 1) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(x_, y - 1));
                            break;
                        }
                        else if (tile.position == new Point(x_, y - 1) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x_, y - 1) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_, y - 1));
                            }
                            breakWhile = true;
                            break;
                        }
                    }
                    y--;//change in y represents a different row
                }
                breakWhile = false;
                while (_y < 7 && breakWhile == false)
                {
                    _y++;
                    foreach(Tile tile in tiles)
                    {
                        if (tile.position == new Point(x_, _y) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(x_, _y));
                            break;
                        }
                        else if (tile.position == new Point(x_, _y) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x_, _y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_, _y));
                            }
                            breakWhile = true;
                            break;
                        }
                    }
                }
            }
             ////////////////////////////////////////////
            ////////K N I G H T  M O V E M E N T////////
           ////////////////////////////////////////////
            if (Type == PieceType.Knight)
            {
                foreach(Tile tile in tiles)
                {                                                                 
                    if (tile.position==new Point(x+1,y+2) && tile.hasPiece==false || tile.position==new Point(x+1,y+2) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 1, y + 2));
                    else if (tile.position==new Point(x+1,y-2) && tile.hasPiece==false || tile.position==new Point(x+1,y-2) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 1, y - 2));
                    else if (tile.position==new Point(x-1,y+2) && tile.hasPiece==false || tile.position==new Point(x-1,y+2) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 1, y + 2));
                    else if (tile.position==new Point(x-1,y-2) && tile.hasPiece==false || tile.position==new Point(x-1,y-2) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 1, y - 2));
                    else if (tile.position==new Point(x+2,y+1) && tile.hasPiece==false || tile.position==new Point(x+2,y+1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 2, y + 1));
                    else if (tile.position==new Point(x+2,y-1) && tile.hasPiece==false || tile.position==new Point(x+2,y-1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 2, y - 1));
                    else if (tile.position==new Point(x-2,y+1) && tile.hasPiece==false || tile.position==new Point(x-2,y+1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 2, y + 1));
                    else if (tile.position==new Point(x-2,y-1) && tile.hasPiece==false || tile.position==new Point(x-2,y-1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 2, y - 1));
                    else if (movement.Count == 8)
                        break;
                }
            }
             ////////////////////////////////////////////
            ////////B I S H O P  M O V E M E N T////////
           ////////////////////////////////////////////
            if (Type == PieceType.Bishop)
            {//x_ remains unchanged inside this if
                while (x < 7 && breakLDiagonal == false)
                {
                    x++;
                    foreach(Tile tile in tiles)
                    {
                        if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x, y + (x - x_)));
                            }
                            breakLDiagonal = true;
                            break;
                        }
                        else if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(x, y + (x - x_)));
                            break;
                        }
                    }
                }
                while (_x >= 0 && breakUDiagonal == false)
                {
                    _x--;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(_x, y - (x_ - _x)));
                            }
                            breakUDiagonal = true;
                            break;
                        }
                        else if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == false)
                        {
                            movement.Add(new Point(_x, y - (x_ - _x)));
                            break;
                        }
                    }
                }
                breakLDiagonal = false;
                breakUDiagonal = false;
                while (y < 7 && breakLDiagonal == false)
                {
                    y++;
                    foreach(Tile tile in tiles)
                    {
                        if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == false && breakLDiagonal == false)
                        {
                            movement.Add(new Point(x_ - (y - _y), y));
                            break;
                        }
                        else if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_ - (y - _y), y));
                            }
                            breakLDiagonal = true;
                            break;
                        }
                    }
                }
                while (_y >= 0 && breakUDiagonal == false)
                {
                    _y--;
                    foreach(Tile tile in tiles)    
                    {                               
                        if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == false && breakUDiagonal == false)
                        {
                            movement.Add(new Point(x_ + (y_ - _y), _y));
                            break;
                        }
                        else if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == true)
                        {
                            if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_ + (y_ - _y), _y));
                            }
                            breakUDiagonal = true;
                            break;
                        }
                    }
                }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
            }
             ////////////////////////////////////////////
            /////////Q U E E N  M O V E M E N T/////////
           ////////////////////////////////////////////
            if (Type == PieceType.Queen)     
            {//contains elements of code in rook and bishop movement  
                while (x < 7 && breakLDiagonal == false || x < 7 && breakWhile == false)
                {
                    x++;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == true && breakLDiagonal == false)
                        {
                            if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x, y + (x - x_)));
                            }
                                breakLDiagonal = true;
                        }
                        else if (tile.position == new Point(x, y + (x - x_)) && tile.hasPiece == false && breakLDiagonal == false)
                        {
                            movement.Add(new Point(x, y + (x - x_)));
                        }
                        if (tile.position == new Point(x, y) && tile.hasPiece == false && breakWhile == false)
                        {
                            movement.Add(new Point(x, y));
                        }
                        else if (tile.position == new Point(x, y) && tile.hasPiece == true && breakWhile == false)
                        {
                            if (tile.position == new Point(x, y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x, y));
                            }
                                breakWhile = true;
                        }
                        if (breakLDiagonal == true && breakWhile == true)
                            break;
                    }
                }
                breakWhile = false;
                while (_x >= 0 && breakUDiagonal == false || _x >= 0 && breakWhile == false)
                {
                    _x--;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == true && breakUDiagonal == false)
                        {
                            if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(_x, y - (x_ - _x)));
                            }
                                breakUDiagonal = true;
                        }
                        else if (tile.position == new Point(_x, y - (x_ - _x)) && tile.hasPiece == false && breakUDiagonal == false)
                        {
                            movement.Add(new Point(_x, y - (x_ - _x)));
                        }
                        if (tile.position == new Point(_x, y) && tile.hasPiece == false && breakWhile == false)
                        {
                            movement.Add(new Point(_x, y));
                        }
                        else if (tile.position == new Point(_x, y) && tile.hasPiece == true && breakWhile == false)
                        {
                            if (tile.position == new Point(_x, y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(_x, y));
                            }
                                breakWhile = true;
                        }
                        if (breakUDiagonal == true && breakWhile == true)
                            break;
                    }
                }
                breakUDiagonal = false;
                breakLDiagonal = false;
                breakWhile = false;
                while (y < 7 && breakLDiagonal == false || y < 7 && breakWhile == false)
                {
                    y++;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == false && breakLDiagonal == false)
                        {
                            movement.Add(new Point(x_ - (y - _y), y));
                        }
                        else if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == true && breakLDiagonal == false)
                        {
                            if (tile.position == new Point(x_ - (y - _y), y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_ - (y - _y), y));
                            }
                                breakLDiagonal = true;
                        }
                        if (tile.position == new Point(x_, y) && tile.hasPiece == false && breakWhile == false)
                        {
                            movement.Add(new Point(x_, y));
                        }
                        else if (tile.position == new Point(x_, y) && tile.hasPiece == true && breakWhile == false)
                        {
                            if (tile.position == new Point(x_, y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_, y));
                            }
                                breakWhile = true;
                        }
                        if (breakLDiagonal == true && breakWhile == true)
                            break;
                    }
                }
                breakWhile = false;
                while (_y >= 0 && breakUDiagonal == false || _y >= 0 && breakWhile == false)
                {
                    _y--;
                    foreach (Tile tile in tiles)
                    {
                        if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == false && breakUDiagonal == false)
                        {
                            movement.Add(new Point(x_ + (y_ - _y), _y));
                        }
                        else if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == true && breakUDiagonal == false)
                        {
                            if (tile.position == new Point(x_ + (y_ - _y), _y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_ + (y_ - _y), _y));
                            }
                                breakUDiagonal = true;
                        }
                        if (tile.position == new Point(x_, _y) && tile.hasPiece == false && breakWhile == false)
                        {
                            movement.Add(new Point(x_, _y));
                        }
                        else if (tile.position == new Point(x_, _y) && tile.hasPiece == true && breakWhile == false)
                        {
                            if (tile.position == new Point(x_, _y) && tile.hasPiece == true && Player != piece.Player)
                            {
                                movement.Add(new Point(x_, _y));
                            }
                                breakWhile = true;
                        }
                        if (breakUDiagonal == true && breakWhile == true)
                            break;
                    }
                }
            }
             ////////////////////////////////////////////
            //////////K I N G  M O V E M E N T//////////
           ////////////////////////////////////////////
            if (Type == PieceType.King)
            {
                foreach(Tile tile in tiles)
                {//simplify??????????????
                    if (tile.position==new Point(x+1,y+1) && tile.hasPiece==false || tile.position==new Point(x+1,y+1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 1, y + 1));
                    else if (tile.position==new Point(x+1,y-1) && tile.hasPiece==false || tile.position==new Point(x+1,y-1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 1, y - 1));
                    else if (tile.position==new Point(x-1,y+1) && tile.hasPiece==false || tile.position==new Point(x-1,y+1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 1, y + 1));
                    else if (tile.position==new Point(x-1,y-1) && tile.hasPiece==false || tile.position==new Point(x-1,y-1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 1, y - 1));
                    else if (tile.position==new Point(x,y+1) && tile.hasPiece==false || tile.position==new Point(x,y+1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x, y + 1));
                    else if (tile.position==new Point(x,y-1) && tile.hasPiece==false || tile.position==new Point(x,y-1) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x, y - 1));
                    else if (tile.position==new Point(x+1,y) && tile.hasPiece==false || tile.position==new Point(x+1,y) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x + 1, y));
                    else if (tile.position==new Point(x-1,y) && tile.hasPiece==false || tile.position==new Point(x-1,y) && tile.hasPiece==true && Player!=piece.Player)
                        movement.Add(new Point(x - 1, y));
                    else if (movement.Count == 8)//sum of movement.count and separate int that counts unsuccessfull moves,,,more efficient
                        break;
                }
            }
            return movement;
        }*/
    }
}

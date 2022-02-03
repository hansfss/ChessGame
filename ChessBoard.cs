using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessDemo
{
    public class ChessBoard
    {
        public Tile[,] tiles { get; set; }
        public ObservableCollection<ChessPiece> pieces { get; set; }
        public int move { get; set; }
        public int turn { get; set; }
        public bool check { get; set; }
        public bool checkMate { get; set; }

        public ChessBoard()
        {
            this.tiles = new Tile[8, 8];
            this.pieces = new ObservableCollection<ChessPiece>();
            this.move = 1;
            this.turn = 1;
            this.check = false;
            this.checkMate = false;
        }

        public ChessBoard(int move, int turn, bool check, bool checkmate)
        {
            this.move = move;
            this.turn = turn;
            this.check = check;
            this.checkMate = checkmate;
        }

        public int turnsMoves()//method updates values of attributes 'move' and 'turn' accordingly during play
        {//ADJUST METHOD INCASE PLAYER TRIES TO MOVE KING TO CHECKED TILE
            if (move == 2)
            {
                move--;
                turn++;
            }
            else if (move == 1)
            {
                move++;
            }
            return turn;
        }

        public bool correctTurn(string position)
        {//method determines if a piece can be moved (depends on value of attribute move [if move=1 and player tries to move black piece=>error])
            bool canMove = false;

            if (move == 1)
            {
                foreach(ChessPiece piece in pieces)
                {
                    if (piece.Pos.ToString().Equals(position) && piece.Player == Player.White)
                    {
                        canMove = true;
                        break;
                    }
                }
            }
            else if (move == 2)
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Pos.ToString().Equals(position) && piece.Player == Player.Black)
                    {
                        canMove = true;
                        break;
                    }
                }
            }
            return canMove;
        }

        public void castling()
        {

        }

        public void enPassant()
        {

        }

        public void promotion()
        {

        }

        public Point convertPosition(string position)//converts a string position into a point
        {
            Point pos = new Point(0, 0)
            {
                X = int.Parse(position[0].ToString()),
                Y = int.Parse(position[1].ToString())
            };
            return pos;
        }

        public bool validKMove(ObservableCollection<ChessPiece> pieces, Tile[,] tiles/*List<Tile> tiles*/, string origin, int x, int y)
        {//SOMETHING WRONG WITH METHOD, DONT KNOW WHAT :(
            bool valid = true;

            double _x, _y;
            int dual = 2;

            List<Point> potentialMoves = new List<Point>();
            List<Point> additionalMoves = new List<Point>();

            Point KPosition = new Point(0, 0);
            ChessPiece king = new ChessPiece();

            Point position = convertPosition(origin);
            _x = position.X;
            _y = position.Y;

            if (move == 1)
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.White && piece.Type == PieceType.King)
                    {
                        KPosition = piece.Pos;
                        king = piece;
                        break;
                    }
                }
                if (KPosition == position)
                {
                    king.Pos = new Point(x, y);

                    foreach (ChessPiece piece in pieces)
                    {
                        if (piece.Player == Player.Black)
                        {
                            if (potentialMoves.Count == 0)
                            {
                                potentialMoves = piece.pieceMovement(tiles, pieces, king, dual);
                            }
                            else
                            {
                                additionalMoves = piece.pieceMovement(tiles, pieces, king, dual);
                                foreach (Point tile in additionalMoves)
                                {
                                    potentialMoves.Add(tile);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < potentialMoves.Count; i++)
                    {
                        if (potentialMoves.Contains(king.Pos))
                        {
                            valid = false;
                            break;
                        }
                    }
                    king.Pos = new Point(_x, _y);//restores original position of piece
                }
            }
            else
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.Black && piece.Type == PieceType.King)
                    {
                        KPosition = piece.Pos;
                        king = piece;
                        break;
                    }
                }
                if (KPosition == position)
                {
                    king.Pos = new Point(x, y);

                    foreach (ChessPiece piece in pieces)
                    {
                        if (piece.Player == Player.White)
                        {
                            if (potentialMoves.Count == 0)
                            {
                                potentialMoves = piece.pieceMovement(tiles, pieces, king, dual);
                            }
                            else
                            {
                                additionalMoves = piece.pieceMovement(tiles, pieces, king, dual);
                                foreach (Point tile in additionalMoves)
                                {
                                    potentialMoves.Add(tile);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < potentialMoves.Count; i++)
                    {
                        if (potentialMoves.Contains(king.Pos))
                        {
                            valid = false;
                            break;
                        }
                    }
                    king.Pos = new Point(_x, _y);
                }
            }
            return valid;
        }

        public bool Check(ObservableCollection<ChessPiece> pieces, Tile[,] tiles)//puts players king under check at end of other players' turn
        {
            List<Point> potentialMoves = new List<Point>();
            List<Point> additionalMoves = new List<Point>();

            Point kPosition = new Point(0, 0);
            ChessPiece king = new ChessPiece();

            int dual = 2;

            if (move == 1)
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.Black && piece.Type == PieceType.King)
                    {
                        kPosition = piece.Pos;
                        king = piece;
                        break;
                    }
                }
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.White)
                    {                                                //parameter implemented incorrectly? Not anymore...
                        if (potentialMoves.Count == 0)                          //|
                        {                                                       //v
                            potentialMoves = piece.pieceMovement(tiles, pieces, king, dual);
                        }
                        else
                        {
                            additionalMoves = piece.pieceMovement(tiles, pieces, king, dual);
                            foreach (Point tile in additionalMoves)
                            {
                                potentialMoves.Add(tile);
                            }
                        }
                    }
                }
                for (int i = 0; i < potentialMoves.Count; i++)
                {
                    if (potentialMoves.Contains(kPosition))
                    {
                        check = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.White && piece.Type == PieceType.King)
                    {
                        kPosition = piece.Pos;
                        king = piece;
                        break;
                    }
                }
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.Black)
                    {
                        if (potentialMoves.Count == 0)
                        {
                            potentialMoves = piece.pieceMovement(tiles, pieces, king, dual);
                        }
                        else
                        {
                            additionalMoves = piece.pieceMovement(tiles, pieces, king, dual);
                            foreach (Point tile in additionalMoves)
                            {
                                potentialMoves.Add(tile);
                            }
                        }
                    }
                }
                for (int i = 0; i < potentialMoves.Count; i++)
                {
                    if (potentialMoves.Contains(kPosition))
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
        }

        public bool Checkmate(ObservableCollection<ChessPiece> pieces, Tile[,] tiles)//method works now, fix it dumbass???
        {                                                                               
            List<Point> potentialMoves = new List<Point>();
            List<Point> potentialKMoves = new List<Point>();
            List<Point> additionalMoves = new List<Point>();

            int _checkmate = 0;
            int dual = 2;

            if (move == 1)
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.White)
                    {
                        if (potentialMoves.Count == 0)
                        {
                            potentialMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                        }
                        else
                        {
                            additionalMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                            foreach(Point tile in additionalMoves)
                            {//additionalMoves is used to add tiles onto end of primary list, potentialMoves
                                potentialMoves.Add(tile);
                            }
                        }
                    }
                }
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.Black && piece.Type == PieceType.King)
                    {                        
                        potentialKMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                    }
                }
                if (potentialKMoves.Count != 0)
                {
                    for (int i = 0; i < potentialKMoves.Count; i++)
                    {
                        if (potentialMoves.Contains(potentialKMoves[i]))
                        {
                            _checkmate++;
                        }
                    }
                    if (_checkmate == potentialKMoves.Count)
                    {
                        checkMate = true;
                    }
                }
            }
            else
            {
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.Black)
                    {
                        if (potentialMoves.Count == 0)
                        {
                            potentialMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                        }
                        else
                        {
                            additionalMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                            foreach (Point tile in additionalMoves)
                            {
                                potentialMoves.Add(tile);
                            }
                        }
                    }
                }
                foreach (ChessPiece piece in pieces)
                {
                    if (piece.Player == Player.White && piece.Type == PieceType.King)
                    {
                        potentialKMoves = piece.pieceMovement(tiles, pieces, piece, dual);
                    }
                }
                if (potentialKMoves.Count != 0)
                {
                    for (int i = 0; i < potentialKMoves.Count; i++)
                    {
                        if (potentialMoves.Contains(potentialKMoves[i]))
                        {
                            _checkmate++;
                        }
                    }
                    if (_checkmate == potentialKMoves.Count)
                    {
                        checkMate = true;
                    }
                }
            }
            return checkMate;
        }

        public int getmove()
        {
            return move;
        }
    }
}

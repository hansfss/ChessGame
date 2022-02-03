using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessDemo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<ChessPiece> pieces;
        ChessPiece movedPiece;//new piece that is created when a piece is moved is assigned to this
        Tile[,] tiles = new Tile[8, 8];
        Tile _tile;//tile selected when selectOrMove = 1 is assigned to _tile
        ChessBoard board = new ChessBoard(1, 1, false, false);
        Decoder d = new Decoder();
        int selectOrMove = 1;// this int determines what action the method btnTile_Click will perform
        string originPoint = "";//game records name of button selected when selectOrMove = 1, uses value when selectOrMove = 2 to move correct piece
        public MainWindow()
		{
			InitializeComponent();
            createChessPieces();
            //createTiles();
            createTiless();
            fillChessboard();
			this.ChessBoard.ItemsSource = this.pieces;
            ResizeMode = ResizeMode.NoResize;
            //trash();
            //ChessBoard is created in xaml of MainWindow
        }

        public void fillChessboard()
        {
            board.tiles = tiles;
            board.pieces = pieces;
        }

        public int ASCIIConverter(string s)
        {//converts {a, b, c, d, e, f, g, h} to {0, 1, 2, 3, 4, 5, 6, 7}
            int value = char.Parse(s.Substring(0, 1)) - 97;
            return value;
        }

        

        public void createChessPieces()//method creates chesspieces seen on the chessboard
        {
            this.pieces = new ObservableCollection<ChessPiece>();
            for(int r = 0; r < 2; r++)//rows
            {//creates pawns
                for (int c = 0; c < 8; c++)//columns
                {
                    if (r == 0) 
                        pieces.Add(new ChessPiece(c, r + 6, PieceType.Pawn, Player.White));
                    else
                        pieces.Add(new ChessPiece(c, r, PieceType.Pawn, Player.Black));
                }
            }
            for (int c = 0; c < 8; c++)
            {
                if (c == 0 || c == 7)
                {
                    pieces.Add(new ChessPiece(c, 7, PieceType.Rook, Player.White));
                    pieces.Add(new ChessPiece(c, 0, PieceType.Rook, Player.Black));
                }
                if (c == 1 || c == 6)
                {
                    pieces.Add(new ChessPiece(c, 7, PieceType.Knight, Player.White));
                    pieces.Add(new ChessPiece(c, 0, PieceType.Knight, Player.Black));
                }
                if (c == 2 || c == 5)
                {
                    pieces.Add(new ChessPiece(c, 7, PieceType.Bishop, Player.White));
                    pieces.Add(new ChessPiece(c, 0, PieceType.Bishop, Player.Black));
                }
                if (c == 3)
                {
                    pieces.Add(new ChessPiece(c, 7, PieceType.Queen, Player.White));
                    pieces.Add(new ChessPiece(c, 0, PieceType.Queen, Player.Black));
                }                            
                if (c == 4)
                {
                    pieces.Add(new ChessPiece(c, 7, PieceType.King, Player.White));
                    pieces.Add(new ChessPiece(c, 0, PieceType.King, Player.Black));
                }                           
            }    
        }

        public void createTiless()
        {
            for (int r = 0; r < 8; r++)//numbers
            {
                for (int c = 0; c < 8; c++)//letters
                {
                    string name = c.ToString() + "" + (r + 1).ToString();//name consists of column and row 
                    if ((r + 10) % 2 == 0)//tiles in rows 0, 2, 4, 6 return 0
                    {
                        if ((c + 10) % 2 == 0)//tiles in columns 0, 2, 4, 6 return 0
                        {
                            if (r == 0 || r == 6)
                                //at the start of a new game, these rows contain pieces
                                tiles[c, r] = new Tile(name, c, r, true, TileColour.White);
                            else
                                tiles[c, r] = new Tile(name, c, r, false, TileColour.White);
                        }
                        else if ((c + 10) % 2 != 0)//tiles in columns 1, 3, 5, 7 dont return 0
                        {
                            if (r == 0 || r == 6)
                                tiles[c, r] = new Tile(name, c, r, true, TileColour.Black);
                            else
                                tiles[c, r] = new Tile(name, c, r, false, TileColour.Black);
                        }
                    }
                    else
                    {
                        if ((c + 10) % 2 == 0)
                        {
                            if (r == 1 || r == 7)
                                tiles[c, r] = new Tile(name, c, r, true, TileColour.Black);
                            else
                                tiles[c, r] = new Tile(name, c, r, false, TileColour.Black);
                        }
                        else if ((c + 10) % 2 != 0) 
                        {
                            if (r == 1 || r == 7)
                                tiles[c, r] = new Tile(name, c, r, true, TileColour.White);
                            else
                                tiles[c, r] = new Tile(name, c, r, false, TileColour.White);
                        }
                    }
                    tiles[c, r].convertTileName();
                }
            }
        }

        public void trash()
        {
            foreach(ChessPiece p in pieces)
            {
                MessageBox.Show(p.Type + ", " + p.Player + ", " + p.Pos);
            }
        }

        /*public void createTiles()//this method does not create the tiles seen on the chessboard
        {//it creates a list of tiles that represent the tiles seen on the board, used to make the game operational
            for (int i = 0; i < 8; i++)//rows
            {
                for (int j = 0; j < 8; j++)//columns
                {
                    string name = j.ToString() + "" + i.ToString();//name consists of column and row
                    if ((j + 10) % 2 == 0)//tiles in columns 0, 2, 4, 6 return 0
                    {
                        if ((i + 10) % 2 == 0)//tiles in rows 0, 2, 4, 6 return 0
                        {
                            if (i == 0 || i ==1 || i ==6 || i ==7)//at the start of a new game, these rows contain pieces
                                tiles.Add(new Tile(name, j, i, true, TileColour.White));//true
                            else
                                tiles.Add(new Tile(name, j, i, false, TileColour.White));//false
                        }
                        else if ((i + 10) % 2 != 0)//tiles in rows 1, 3, 5, 7 dont return 0
                        {
                            if (i == 0 || i == 1 || i == 6 || i == 7)
                                tiles.Add(new Tile(name, j, i, true, TileColour.Black));//true
                            else
                                tiles.Add(new Tile(name, j, i, false, TileColour.Black));//false
                        }
                    }
                    else if ((j + 10) % 2 != 0)//tiles in columns 1, 3, 5, 7 dont return 0
                    {
                        if ((i + 10) % 2 == 0)
                        {
                            if (i == 0 || i == 1 || i == 6 || i == 7)
                                tiles.Add(new Tile(name, j, i, true, TileColour.Black));//true
                            else
                                tiles.Add(new Tile(name, j, i, false, TileColour.Black));//false
                        }
                        else if ((i + 10) % 2 != 0)
                        {
                            if (i == 0 || i == 1 || i == 6 || i == 7)
                                tiles.Add(new Tile(name, j, i, true, TileColour.White));//true
                            else
                                tiles.Add(new Tile(name, j, i, false, TileColour.White));//false
                        }
                    }
                }
            }
        }*/

        private void btnTile_Click(object sender, RoutedEventArgs e)//position format: 0,1; 2,2; 4,1 etc...
        {
            if (selectOrMove == 1)
            {
                var b = (Button)sender;//buttons are named after the tile they represent
                string namePoint = b.Name;//create method that recieves this string, dissect string inside said method

                int c = ASCIIConverter(namePoint[0].ToString());//returns column value represented on board
                int r = (int.Parse(namePoint[1].ToString()) - 1);//returns row value represented on board
                int _r = (int.Parse(namePoint[1].ToString()));//returns y value of point

                string _c = namePoint[0].ToString();
                string _point = c.ToString() + "" + d.rowConverter(r).ToString();
                ///string position = _c + "" + _r.ToString();//returns tile name
                string point = c.ToString() + "," + d.rowConverter(r).ToString();//returns tile point

                originPoint = _point;//row and column is recorded in originPoint for use when selectOrMove = 2

                Tile tile = tiles[c, d.rowConverter(r)];
                if (/*tile.name.Equals(position) && */tile.hasPiece == true && board.correctTurn(point))
                {//correctTurn method checks to see if selected piece corresponds to correct value of move (move = 1, white: move = 2, black)
                    selectOrMove++;
                    tile.hasPiece = false;
                    _tile = tile;
                }
            }
            else if (selectOrMove == 2)
            {
                var b = (Button)sender;
                string namePoint = b.Name;
                
                int c = ASCIIConverter(namePoint[0].ToString());
                int r = (int.Parse(namePoint[1].ToString()) - 1);

                ChessPiece eliminatedPiece = new ChessPiece();//chess piece that may or may not be eliminated
                
                foreach(ChessPiece piece in pieces)
                {
                    if (piece.Pos == new Point(c, d.rowConverter(r)))
                        eliminatedPiece = piece;
                }

                foreach (ChessPiece piece in pieces)
                {
                    string position = piece.Pos.ToString();
                    string _position = position[0] + "" + position[2];

                    if (_position.Equals(originPoint))//finds chess piece that was chosen when selectOrMove was equal to 1
                    {
                        int dual = 1;
                        List<Point> options = piece.pieceMovement(tiles, pieces, eliminatedPiece, dual);//method pieceMovement returns a list of points where selected chessPiece can move
                        Point validTile = new Point(c, d.rowConverter(r));//validTile represents position of tile selected when selectOrMove = 2

                        if (options.Contains(validTile))
                        {
                            if (piece.Type == PieceType.King)
                            {//validKMove finds tiles a king can/cannot move to (can't move into a check position)
                                if (board.validKMove(pieces, tiles, originPoint, c, d.rowConverter(r)))
                                {
                                    movedPiece = piece.movePiece(pieces, c, d.rowConverter(r));
                                    piece.eliminateOpposingPiece(pieces, movedPiece, eliminatedPiece);//method only elims a piece if opponent attacks
                                    selectOrMove = 1;
                                    _tile = null;//null in case player right clicks on board after moving piece, else tile assigned to _tile becomes unavailable (hasPiece becomes true)
                                    movedPiece = null;
                                    board.turnsMoves();//updates moves and turns
                                }
                            }
                            else
                            {
                                movedPiece = piece.movePiece(pieces, c, d.rowConverter(r));
                                piece.eliminateOpposingPiece(pieces, movedPiece, eliminatedPiece);
                                selectOrMove = 1;
                                _tile = null;
                                movedPiece = null;
                                board.turnsMoves();
                            }
                            tiles[c, d.rowConverter(r)].hasPiece = true;
                            //if (tiles[c, r].position == validTile)
                            /*foreach (Tile tile in tiles)//finds tile that piece moved to, and changes hasPiece status
                                if (tile.position == validTile)
                                {//loop identifies selected tile, changes hasPiece to true                    
                                    tile.hasPiece = true;      
                                    break;                     
                                } */
                            break;
                        }
                    }
                }
                if (board.Checkmate(pieces, tiles))
                {
                    //create method that checks if checkmating piece can be taken out next turn, if so, checkmate reverts to FALSE
                    //insert (and create) method that declares winner
                    //MessageBox.Show("Checkmate BITCH");
                }
                if (board.Check(pieces, tiles))
                {
                    MessageBox.Show("CHECK");
                }
            }
        }

        private void deSelectPiece(object sender, MouseButtonEventArgs e)//method is only called if you right-click a tile on the chessboard
        {//method allows player to deselect a chosen piece and select another
            selectOrMove = 1;
            if (_tile != null)
                _tile.hasPiece = true;//hasPiece status changes back to true if player deselects piece
        }
    }
}
//GAME LETS YOU MOVE PIECE FROM SAME COLOUR REPEATEDLY IF YOU TRY TO MOVE A PIECE ONTO A TILE IT CANT GO TO
//CANT MOVE KING ON A TILE IN FRONT OF A PAWN, FIX
//FIX PIECE POSITIONS


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Snakes_and_ladders
{
    class Program
    {
        static void Main(string[] args)
        {
            Logic startGame = new Logic();
            bool ans=true;
            do
            {
                ans = startGame.gameOn();
                startGame.init();
            } while (ans);
                Console.WriteLine("Goodbye :)");
        }
    }
    class Logic
    {
        private int player1, player2, numOfSnakes, numOfLadders, boardRange, endOfBoard, numOfGold, numOfSpecial;
        Random rand = new Random();
        Inlay [] boardGame;
        public Logic()
        {
            boardRange = 10;
            endOfBoard = boardRange * boardRange;
            player1 = player2 = 0;
            numOfSnakes = 7;
            numOfLadders = 8;
            numOfGold = 2;
            boardGame = new Inlay[endOfBoard];
            init();
        }
        public int Player1
        {
            get { return this.player1; }
            set { this.player1 = value; }
        }
        public int Player2
        {
            get { return this.player2; }
            set { this.player2 = value; }
        }
        /// <summary>
        /// The function returns the sum of rolling 2 dice
        /// </summary>
        public int ResultOfRollingTheDice()
        {
            return rand.Next(1, 7) + rand.Next(1, 7);
        }
        /// <summary>
        /// The function runs the snakes and ladders game according to the game conditions.
        /// </summary>
        /// <returns>The function return true to play a new game or false to exit</returns>
        public bool gameOn()
        {
            while (player1< endOfBoard && player2< endOfBoard)
            {
                this.Player1 = realGame(this.Player1,this.Player2);
                this.player2 = realGame(this.Player2, this.Player1);
                CurrentStatus();
            }
            Console.WriteLine("Game over! do you want to start a new game? To continue press any button. To exit press the Enter button");
            string ans = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(ans))
                return false;
            return true;
        }
        /// <summary>
        /// As long as none of the players have reached the end of the board
        /// The function check where they are at the moment and whether she should call one of the snakes/slams/golden inlays functions
        /// </summary>
        /// <returns>The function returns the position of the current player</returns>
        private int realGame(int currPlayer,int otherPlayer)
        {
            currPlayer += ResultOfRollingTheDice();
            if(currPlayer < endOfBoard)
            {
                string type = boardGame[currPlayer].InlayCharacter;
                switch (type)
                {
                    case "regular":
                        return currPlayer;
                    // break;
                    case "ladderHead":
                        currPlayer = boardGame[currPlayer].EndLocat;
                        break;
                    case "SnakeHead":
                        currPlayer = boardGame[currPlayer].EndLocat;
                        break;
                    case "gold":
                        if (currPlayer == this.Player1)
                        {
                            if (this.Player1 < this.Player2)
                            {
                                this.Player1 = otherPlayer;
                                this.Player2 = currPlayer;
                                currPlayer = this.Player1;
                            }
                        }
                        else
                        {
                            if (this.Player2 < this.Player1)
                            {
                                this.Player2 = otherPlayer;
                                this.Player1 = currPlayer;
                                currPlayer = this.Player2;
                            }
                        }
                        break;
                }
            }
            return currPlayer;
        }
        /// <summary>
        /// The function initializes the inlays and player values ​​for a new game
        /// </summary>
        public void init()
        {
            this.player1 = this.player2=0;
            for (int i = 0; i < endOfBoard; i++)
                boardGame[i] = new Inlay(i / boardRange, i+1, boardRange);
            choosingLocationOfSpecials(numOfGold, numOfLadders, numOfSnakes, endOfBoard, boardRange, boardGame);
        }
        /// <summary>
        /// The function prints after each round where each of the players is located on the board
        /// </summary>
        public void CurrentStatus()
        {
            if(player1<endOfBoard && player2 < endOfBoard)
                Console.WriteLine(" Players's positions are: Player1 - {0} , player2 - {1}.", player1, player2);
            else if (player1 >= endOfBoard && player1>player2)
                Console.WriteLine("The winner is: player1, he got to {0} position. player2 got to {1} position.", player1, player2);
            else if (player2 >= endOfBoard && player2 > player1)
                Console.WriteLine("The winner is: player2, he got to {0} position. player1 got to {1} position.", player2, player1);
            else
                Console.WriteLine("We have a draw , player1 got to {0} place and player2 got to {1} place too.", player1, player2);
        }
        /// <summary>
        /// The function randomly places on the game board snakes, ladders and gold inlays according to the defined parameters.
        /// The function verifies that each inlay can be of only one "special" type.
        /// The function verifies that the head of a ladder/snake and the end of a ladder/snake connects at least 2 different lines
        /// </summary>
        public void choosingLocationOfSpecials(int anumOfGold, int anumOfLadders, int anumOfSnakes , int aboardRange,int rowSize, Inlay[] arrInlay)
        {
            int iHeadLocate, iEndlLocate;
            while (anumOfLadders > 0 )
            {
                do
                {
                    iHeadLocate = rand.Next(0, aboardRange- rowSize);
                }
                while (!(arrInlay[iHeadLocate].isRegularType(arrInlay[iHeadLocate])));
                { }
                arrInlay[iHeadLocate] = arrInlay[iHeadLocate].initLadderHead(arrInlay[iHeadLocate]);
                arrInlay[iHeadLocate].InlayRow = iHeadLocate/rowSize;
                do
                {
                        iEndlLocate = rand.Next(iHeadLocate + 1, aboardRange);
                }
                while (!(arrInlay[iEndlLocate].isRegularType(arrInlay[iEndlLocate])) || arrInlay[iHeadLocate].InlayRow == iEndlLocate/rowSize);
                { }
                arrInlay[iEndlLocate] = arrInlay[iEndlLocate].initLadderEnd(arrInlay[iEndlLocate]);
                arrInlay[iHeadLocate].EndLocat = iEndlLocate;
                anumOfLadders--;
            }
            while (anumOfSnakes > 0 )
            {
                do { iHeadLocate = rand.Next(rowSize, aboardRange); }
                    while (!(arrInlay[iHeadLocate].isRegularType(arrInlay[iHeadLocate])));{ }
                arrInlay[iHeadLocate] = arrInlay[iHeadLocate].initSnakeHead(arrInlay[iHeadLocate]);
                arrInlay[iHeadLocate].InlayRow = iHeadLocate/rowSize;
                do{iEndlLocate = rand.Next(0 , iHeadLocate);}
                    while (!(arrInlay[iEndlLocate].isRegularType(arrInlay[iEndlLocate])) || arrInlay[iHeadLocate].InlayRow == iEndlLocate/rowSize);{ }
                arrInlay[iEndlLocate] = arrInlay[iEndlLocate].initSnakeTail(arrInlay[iEndlLocate]);
                arrInlay[iHeadLocate].EndLocat = iEndlLocate;
                anumOfSnakes--;
            }
            while (anumOfGold > 0)
            {
                do { iHeadLocate = rand.Next(0, aboardRange); }
                    while (!(arrInlay[iHeadLocate].isRegularType(arrInlay[iHeadLocate])));{ }
                arrInlay[iHeadLocate] = arrInlay[iHeadLocate].initGold(arrInlay[iHeadLocate]);
                arrInlay[iHeadLocate].EndLocat = iHeadLocate;
                anumOfGold--;
            }
        }
    }
    /// <summary>
    /// The class defines an object of type inlay.
    /// the properties of the object will be: the number of the square, the row of the square within the game board, the borders of the board,
    /// The location of the end of the "special" shape on the board and the type of the inlay (regular/gold/ head or end of ladder/head or tail of snake).
    /// </summary>
    class Inlay
    {
        private string inlayCharacter = "";
        private int inlayId, inlayRow, boardRange,endLocat;
        public Inlay(int i, int j, int boardRange)
        {
            this.inlayCharacter = "regular";
            this.inlayRow = i;
            this.inlayId = j;
            this.boardRange = boardRange;
            this.endLocat = -1;
        }
        public int EndLocat
        {
            get { return this.endLocat; }
            set { this.endLocat = value; }
        }
        public int InlayRow
        {
            get { return this.inlayRow; }
            set
            {
                if (inlayRow<0 || inlayRow > boardRange)
                    throw new Exception("The inlay row out of the range!");
                this.inlayRow = value;
            }
        }
        public int InlayId
        {
            get { return this.inlayId; }
            set { this.inlayId = value; }
        }
        public string InlayCharacter
        {
            get { return inlayCharacter; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("The character of inlay cannot be empty");
                this.inlayCharacter = value;
            }
        }
        public Inlay initLadderHead(Inlay currInaly)
        {
            currInaly.InlayCharacter = "ladderHead";
            return currInaly;
        }
        public Inlay initLadderEnd(Inlay currInaly)
        {
            currInaly.InlayCharacter = "ladderEnd";
            return currInaly;
        }
        public Inlay initSnakeHead(Inlay currInaly)
        {
            currInaly.InlayCharacter = "snakeHead";
            return currInaly;
        }
        public Inlay initSnakeTail(Inlay currInaly)
        {
            currInaly.InlayCharacter = "snakeTail";
            return currInaly;
        }
        public Inlay initGold(Inlay currInaly)
        {
            currInaly.InlayCharacter = "gold";
            return currInaly;
        }
        public bool isRegularType(Inlay currInaly)
        {
            return (currInaly.InlayCharacter == "regular");
        }
    }
}
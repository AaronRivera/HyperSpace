using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            //sets the console window parameters
            Console.WindowWidth = 50;
            Console.BufferWidth = 50;
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;
            Console.WriteLine("*************************************************");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press any key");
            Console.WriteLine("when you are ready to play the game");
            Console.ReadKey();

            Hyperspace newGame = new Hyperspace();
            newGame.PlayGame();
            
        }
    }


    /// <summary>
    /// class for obstacles
    /// </summary>
    class Unit
    {
        //set x and y prop
        public int X { get; set; }
        public int Y { get; set; }
        //color to print
        public ConsoleColor Color { get; set; }
        //design of obstacle
        public string Symbol { get; set; }
        //determines if it is a space rift
        public bool IsSpaceRift { get; set; }

        //list of obstacles 
        List<string> ObstacleList = new List<string> { "!", "*", ".", ":", ";", "'","\"" };
        //new random number
        Random rnd = new Random();

        /// <summary>
        /// contructor 
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public Unit(int x, int y)
        {
            //sets values based on params and defaults
            this.X = x;
            this.Y = y;
            this.Symbol = ObstacleList[rnd.Next(ObstacleList.Count())];
            this.IsSpaceRift = false;
            this.Color = ConsoleColor.Cyan;

        }
        
        /// <summary>
        /// constructor with color, string, and bool overloads
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colorSet"></param>
        /// <param name="symbolSet"></param>
        /// <param name="isSpaceRift"></param>
        public Unit(int x, int y, ConsoleColor colorSet, string symbolSet, bool isSpaceRift)
        {
            //equates params to property values
            this.X = x;
            this.Y = y;
            this.Symbol = symbolSet;
            this.Color = colorSet;
            this.IsSpaceRift = isSpaceRift;
        }

       
        /// <summary>
        /// Draw function
        /// </summary>
        public void Draw()
        {
            //draws the unit on x and y sets the color to color default and writes the symbol
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);

        }

    }
    /// <summary>
    /// hypersapce
    /// </summary>
    class Hyperspace
    {
        // speed and score
        public int PlayerScore { get; set; }
        public int Speed { get; set; }
        //list to hold our obstacle s
        public List<Unit> ObstacleList { get; set; }
        //spaceship object
        public Unit SpaceShip { get; set; }
        //bool if it is smashed
        public bool Smashed { get; set; }

        //new random
        Random rnd = new Random();
        //constructor
        
        /// <summary>
        /// contructor for a spaceship
        /// </summary>
        public Hyperspace()
        {

            //sets intial score and speed
            this.PlayerScore = 0;
            this.Speed = 0;
            //is not smashed as deafult
            this.Smashed = false;
            //creates new list of obstaces
            this.ObstacleList = new List<Unit>();
            //creates a new spaceship object using the unit constructor
            this.SpaceShip = new Unit((Console.WindowWidth / 2) - 1, Console.WindowHeight - 1, ConsoleColor.Red, "@", false);


        }
        //play game method
        public void PlayGame()
        {
            //while the ship isnt smashed
            while (!Smashed)
            {
                //new var for chance of rift
                int riftSpawn = rnd.Next(10);
                //rift chance of 10%
                if (riftSpawn > 8)
                {
                    //create new rift using unit contructor
                    Unit rift = new Unit(rnd.Next(Console.WindowWidth - 2), 5, ConsoleColor.Green, "@", true);
                    //add to list
                    this.ObstacleList.Add(rift);
                }
                else
                {
                    //otherwise create new unit using unit constructor
                    Unit obstacle = new Unit(rnd.Next(Console.WindowWidth - 2), 5);
                    //adds to list
                    this.ObstacleList.Add(obstacle);
                }
                //moves our ship
                MoveShip();
                //moves our obstacles
                MoveObstacles();
                //draws game
                DrawGame();
                //if speed is less than 170
                if (Speed < 170)
                {
                    //increment speed
                    Speed++;
                }
                //slows the write speed according to speed
                System.Threading.Thread.Sleep(170 - Speed);
            }

            Console.WriteLine("");
            Console.WriteLine("Game Over!!!!!");
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// moves the ship
        /// </summary>
        public void MoveShip()
        {
            //if theres been  a new key pressed
            if (Console.KeyAvailable)
            {
                //gets the key
                ConsoleKey keyPressed = Console.ReadKey().Key;

                //while a key is been pressed
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);


                }

                //if the key pressed was right arrow
                if (keyPressed == ConsoleKey.RightArrow
                        && SpaceShip.X < (Console.WindowWidth - 2))
                {
                    //increments x
                    SpaceShip.X++;
                }
                    //if the key pressed was the left arrow
                else if (keyPressed == ConsoleKey.LeftArrow
                    && SpaceShip.X > 0)
                {
                    //decrease x
                    SpaceShip.X--;
                }
                else
                {
                    //send to console message
                    Console.WriteLine("Invalid Move");
                }
            }
        }


        /// <summary>
        /// move obstacles
        /// </summary>
        public void MoveObstacles()
        {
            //cretes a list of units
            List<Unit> newObstacleList = new List<Unit>();

            //for each obstacle in the list increases Y
            foreach (Unit obstacle in ObstacleList)
            {
                obstacle.Y++;

                //if obstacle is a rift 
                if (obstacle.IsSpaceRift
                    && obstacle.X == SpaceShip.X
                    && obstacle.Y == SpaceShip.Y)
                {
                    //decrease speed
                    Speed -= 50;
                    Smashed = false;
                }
                    //if not
                else if (obstacle.X == SpaceShip.X
                    && obstacle.Y == SpaceShip.Y)
                {
                    //its been smashed
                    Smashed = true;
                }
                if (obstacle.Y < Console.WindowHeight)
                {
                    //adds the obstacle to the list
                    newObstacleList.Add(obstacle);
                }
                else
                {
                    //increases score
                    PlayerScore++;

                }


            }
            ObstacleList = newObstacleList;
        }

       /// <summary>
       /// draws the game
       /// </summary>
        public void DrawGame()
        {
            Console.Clear();
            SpaceShip.Draw();

            foreach (Unit obstacle in ObstacleList)
            {
                obstacle.Draw();
            }
            PrintAtPosition(20, 2, "Score: " + this.PlayerScore, ConsoleColor.Green);
            PrintAtPosition(20, 3, "Speed:" + this.Speed, ConsoleColor.Green);

        }


        /// <summary>
        /// prints the position 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            //prints a position
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);

        }
    }


  }

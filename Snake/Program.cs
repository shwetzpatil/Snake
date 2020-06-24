using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Speech.Synthesis;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Vars
            int[] xPosition = new int[50];
                xPosition[0] = 35;
            int[] yPosition = new int[50];
                yPosition[0] = 20;
            int foodXDim = 10;
            int foodYDim = 10;
            int foodEaten = 0;

            string userAction = ""; 

            decimal gameSpeed = 150m;

            bool isGameOn = true;
            bool isWallHit = false;
            bool isFoodEaten = false;
            bool isStayInMenu = true;

            Random random = new Random();

            Console.CursorVisible = false;

            // welcome screen
            showMenu(out userAction);
            #endregion

            do
            {
                switch (userAction)
                {
                    // give player option to read direction
                    #region caseDirections

                    case "1":
                    case "d":
                    case "directions":
                        Console.Clear();
                        BuildWall();
                        Console.SetCursorPosition(5, 5);
                        Console.WriteLine("Press enter to return to the main menu");
                        Console.ReadLine();
                        Console.Clear();
                        showMenu(out userAction);
                        break;
                    #endregion

                    #region casePlay

                    case "2":
                    case "p":
                    case "play":
                        Console.Clear();

                        #region GameSetup
                        // set a snake on screen
                        paintSnake(foodEaten, xPosition, yPosition, out xPosition, out yPosition);

                        // set food on screen
                        setFoodPositionOnScreen(random, out foodXDim, out foodYDim);
                        paintFood(foodXDim, foodYDim);

                        // build boundary
                        BuildWall();

                        ConsoleKey command = Console.ReadKey().Key;

                        #endregion

                        // get snake to move
                        do
                        {
                            #region ChangeDirection

                            switch (command)
                            {
                                case ConsoleKey.LeftArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.WriteLine(" ");
                                    xPosition[0]--;
                                    break;
                                case ConsoleKey.UpArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.WriteLine(" ");
                                    yPosition[0]--;
                                    break;
                                case ConsoleKey.RightArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.WriteLine(" ");
                                    xPosition[0]++;
                                    break;
                                case ConsoleKey.DownArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.WriteLine(" ");
                                    yPosition[0]++;
                                    break;
                            }

                            #endregion

                            #region PlayingGame

                            //paint the snake, make snake longer
                            paintSnake(foodEaten, xPosition, yPosition, out xPosition, out yPosition);

                            // detect when snake hits boundary
                            isWallHit = DidSnakeHitTheWall(xPosition[0], yPosition[0]);

                            if (isWallHit)
                            {
                                isGameOn = false;
                                Console.SetCursorPosition(16, 16);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("The snake hits the wall and died.");
                                // Score
                                Console.SetCursorPosition(18, 18);
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("YOUR SCORE = " + foodEaten * 100);
                                // Replay
                                Console.SetCursorPosition(17, 20);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Press Enter To Continue...");
                                foodEaten = 0;
                                Console.ReadLine();
                                Console.Clear();
                                // give player option to replay
                                showMenu(out userAction);

                            }

                            // detect when food was eaten 
                            isFoodEaten = determineIfFoodWasEaten(xPosition[0], yPosition[0], foodXDim, foodYDim);

                            // place food on board
                            if (isFoodEaten)
                            {
                                setFoodPositionOnScreen(random, out foodXDim, out foodYDim);
                                paintFood(foodXDim, foodYDim);
                                // keep track of how much food consumed
                                foodEaten++;
                                // make snake faster
                                gameSpeed *= .900m;
                            }

                            //slow game down
                            if (Console.KeyAvailable) command = Console.ReadKey().Key;
                            System.Threading.Thread.Sleep(Convert.ToInt32(gameSpeed));

                            #endregion

                        } while (isGameOn);

                        break;
                    #endregion

                    #region caseExit
                    case "3":
                    case "e":
                    case "exit":
                        isStayInMenu = false;
                        Console.Clear();
                        break;
                    #endregion

                    #region caseDefault
                    default:
                        Console.WriteLine("Your input is invalid, please press Enter and try again.");
                        Console.ReadLine();
                        Console.Clear();
                        showMenu(out userAction);
                        break;
                    #endregion
                }
                
            } while (isStayInMenu);
            //wait to see output
            Console.ReadLine();
        }

        #region HelperMethods
        //menu
        #region Menu

        private static void showMenu(out string userAction)
        {
            string menu1 = "1) Direction\n2) Play\n3) Exit \n\n\n" + @"




                      _                   .-=-.          .-==-.
                     { }      __        .' O o '.       /  -<' )
                     { }    .' O'.     / o .-. O \     /  .--v`
                     { }   / .-. o\   /O  /   \  o\   /O /
                      \ `-` /   \ O`-'o  /     \  O`-`o /
                       `-.-`     '.____.'       `.____.'
            ";
            string menu2 = "1) Direction\n 2) Play\n3) Exit \n\n\n" + @"




                          _                   .-=-.          .-==-.
                         { }      __        .' O o '.       /  -<' )
                         { }    .' O'.     / o .-. O \     /  .--v`
                         { }   / .-. o\   /O  /   \  o\   /O /
                          \ `-` /   \ O`-'o  /     \  O`-`o /
                           `-.-`     '.____.'       `.____.'

            ";

            string menu3 = "1) Direction\n2) Play\n3) Exit \n\n\n" + @"




              _                   .-=-.          .-==-.
             { }      __        .' O o '.       /  -<' )
             { }    .' O'.     / o .-. O \     /  .--v`
             { }   / .-. o\   /O  /   \  o\   /O /
              \ `-` /   \ O`-'o  /     \  O`-`o /
               `-.-`     '.____.'       `.____.'

            ";
            string menu4 = "1) Direction\n2) Play\n3) Exit \n\n\n" + @"




                      _                   .-=-.          .-==-.
                     { }      __        .' O o '.       /  -<' )
                     { }    .' O'.     / o .-. O \     /  .--v`
                     { }   / .-. o\   /O  /   \  o\   /O /
                      \ `-` /   \ O`-'o  /     \  O`-`o /
                       `-.-`     '.____.'       `.____.'
            ";

            string menu5 = "1) Direction\n2) Play\n3) Exit \n\n\n" + @"




                              _                   .-=-.          .-==-.
                             { }      __        .' O o '.       /  -<' )
                             { }    .' O'.     / o .-. O \     /  .--v`
                             { }   / .-. o\   /O  /   \  o\   /O /
                              \ `-` /   \ O`-'o  /     \  O`-`o /
                               `-.-`     '.____.'       `.____.'
            ";

            Console.WriteLine(menu1);
            System.Threading.Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine(menu2);
            System.Threading.Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine(menu3);
            System.Threading.Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine(menu4);
            System.Threading.Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine(menu5);
            System.Threading.Thread.Sleep(100);

//            SpeechSynthesizer toSpeak = new SpeechSynthesizer();
//            toSpeak.SetOutputToDefaultAudioDevice();
//            toSpeak.Speak("The snake game!");

            userAction = Console.ReadLine().ToLower();

        }

        #endregion
        private static void paintSnake(int foodEaten, int[] xPositionIn, int[] yPositionIn, out int[] xPositionOut, out int[] yPositionOut)
        {
            //paint the head
            Console.SetCursorPosition(xPositionIn[0], yPositionIn[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((char)214);

            //paint the body
            for (int i = 1; i < foodEaten + 1 ; i++)
            {
                Console.SetCursorPosition(xPositionIn[i], yPositionIn[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("O");

            }

            //erase last part of body
            Console.SetCursorPosition(xPositionIn[foodEaten + 1], yPositionIn[foodEaten + 1]);
            Console.WriteLine(" ");

            //record location of each body part
            for (int i = foodEaten + 1; i > 0; i--)
            {
                xPositionIn[i] = xPositionIn[i - 1];
                yPositionIn[i] = yPositionIn[i - 1];
            }

            //return the new array
            xPositionOut = xPositionIn;
            yPositionOut = yPositionIn;

        } //end paintSnake

        private static bool DidSnakeHitTheWall(int xPosition, int yPosition)
        {
            if (xPosition == 1 || xPosition == 70 || yPosition == 1 || yPosition == 40) return true;
            return false;
        }

        private static void BuildWall()
        {
            for (var i = 1; i < 41; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(1, i);
                Console.WriteLine("#");
                Console.SetCursorPosition(70,i);
                Console.WriteLine("#");
            }

            for (var i = 1; i < 71 ; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i, 1);
                Console.WriteLine("#");
                Console.SetCursorPosition(i, 40);
                Console.WriteLine("#");

            }
        } //end buildWall

        private static void paintFood(int foodXDim, int foodYDim)
        {
            Console.SetCursorPosition(foodXDim, foodYDim);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine((char)64);
        }
        private static bool determineIfFoodWasEaten(int xPosition, int yPosition, int foodXDim, int foodYDim)
        {
            if (xPosition == foodXDim && yPosition == foodYDim) return true;
            return false;
        }
        private static void setFoodPositionOnScreen(Random random, out int foodXDim, out int foodYDim)
        {
            foodXDim = random.Next(0 + 2, 70 - 2);
            foodYDim = random.Next(0 + 2, 40 - 2);
        }
        #endregion

    }
}

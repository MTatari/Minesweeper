using System.ComponentModel;
using System.Linq.Expressions;

namespace Minesweeper;

class Minesweeper
{
    static void Main()
    {
        var gridInstance = new MineGrid(5, 5);


        //set the bombs...
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);

        //Print the state
        Console.WriteLine(gridInstance.GetState(false));


        // Game code...
        Boolean gameWon = false;
        Boolean gameLost = false;
        while (!gameWon && !gameLost)
        {
            Console.WriteLine("Write the coordiantes in the form x y: ");
            try
            {
                var userInput = Console.ReadLine(); // The input should be in form of coordinates "x y"
                var coordinates = userInput?.Split(' ')?.Select(Int32.Parse)?.ToList();
                if (coordinates.Count > 2)
                {
                    throw new Exception("Invalid input");
                }

                //Uncover the cell according to the user input and the game rules
                CellStatus cellStatus = gridInstance.UncoverCell(coordinates[0], coordinates[1]);

                switch (cellStatus)
                {
                    case CellStatus.Mine:
                        gameLost = true;
                        Console.WriteLine("Game lost! You encountered a mine");
                        break;
                    case CellStatus.Visited:
                        Console.WriteLine("This cell is Visited before!");
                        break;
                    default:
                        Console.WriteLine("The current state is: \n" + gridInstance.GetState(false));
                        //Check if the user has uncovered all cells that don't have mines => Won
                        gameWon = gridInstance.IsGameWon();
                        if (gameWon)
                        {
                            Console.WriteLine("Game won! Well played");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

        }

    }
}

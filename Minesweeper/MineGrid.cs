namespace Minesweeper;
//This class represents the game with mines and other coordinates in a 2d array of type int.
//The representation follows the rules of the Minesweeper game and the task description.
public class MineGrid
{
    private readonly int rows;
    private readonly int cols;
    private int minesCount;
    private readonly int[,] mineGrid;
    private readonly HashSet<int> unCoveredCells;
    private readonly HashSet<int> visited;

    public MineGrid(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        minesCount = 0;
        mineGrid = new int[rows, cols];
        unCoveredCells = new HashSet<int>();
        visited = new HashSet<int>();
    }

    public void SetBomb(int x, int y)
    {
        if (mineGrid[y, x] != -1)
        {
            try 
            {
                mineGrid[y, x] = -1; // a mine is represented with -1
                minesCount++;
                FixNeighborsOfBomb(y, x);
            }
            catch (IndexOutOfRangeException)
            {
                //Do nothing
            }

        }

    }

    //Fix the relevant cells adjacent to the the cell at rowIndex and colIndex and increment their value by 1.
    private void FixNeighborsOfBomb(int rowIndex, int colIndex)
    {
        for (int i = rowIndex - 1; i <= rowIndex + 1; i++)
        {
            for (int j = colIndex - 1; j <= colIndex + 1; j++)
            {
                try
                {
                    if (mineGrid[i, j] != -1) //If not a bomb
                    {
                        mineGrid[i, j] += 1;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                   //Do nothing
                }
            }
        }
    }

    private Boolean IsMine(int x, int y)
    {
        return mineGrid[y, x] == -1;
    }

    public Boolean IsGameWon()
    {
        return rows * cols == unCoveredCells.Count + minesCount;
    }

    public CellStatus UncoverCell(int x, int y)
    {
        if (IsMine(x, y))
        {
            return CellStatus.Mine;
        }
        if (unCoveredCells.Contains(y * 10 + x))//y * 10 + x is Cell represenation as a number
        {
            return CellStatus.Visited;
        }
        else if (mineGrid[y, x] > 0)
        {
            unCoveredCells.Add(y * 10 + x);
        }
        else
        {
            RecursiveUncoverCell(x, y);
        }
        return CellStatus.Unvisitedbefore;
    }

    private void RecursiveUncoverCell(int x, int y)
    {
        //Visit the cell
        int cellRepresentation = y * 10 + x;
        visited.Add(cellRepresentation);
        //Uncover the cell and save the (unvisited and should be visisted cells) in "unvisitedEmptyCells"
        var unvisitedEmptyCells = UncoverAdjacent(y, x);

        //Repeat above for all should be visited cells "unvisitedEmptyCells"
        for (int i = 0; i < unvisitedEmptyCells.Count(); i++)
        {
            int cellrepresentation = unvisitedEmptyCells[i];
            int yCellCoordinate = cellrepresentation / 10;
            int xCellCoordinate = cellrepresentation % 10;
            RecursiveUncoverCell(xCellCoordinate, yCellCoordinate);
        }
    }


    //Fix the cell at rowIndex and colIndex and the cells that are adjacent to it according to the game rules.
    private List<int> UncoverAdjacent(int rowIndex, int colIndex)
    {
        var unvisitedEmptyCells = new List<int>();

        for (int i = rowIndex - 1; i <= rowIndex + 1; i++)
        {
            for (int j = colIndex - 1; j <= colIndex + 1; j++)
            {
                try
                {
                    if (mineGrid[i, j] >= 0)
                    {
                        int cellRepresentation = i * 10 + j; //Cell represenation as a number
                        unCoveredCells.Add(cellRepresentation);
                        if (mineGrid[i, j] == 0 && !visited.Contains(cellRepresentation))
                        {
                            unvisitedEmptyCells.Add(cellRepresentation);
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    //Do nothing
                }
            }
        }
        return unvisitedEmptyCells;
    }

    public string GetState(Boolean exposeUncoveredCells)//exposeUncoveredCells is usefull when testing
    {
        String temp = "  ";
        for (int i = 0; i < cols; i++)
        {
            temp += i;
        }
        temp += "\n";
        for (int i = rows - 1; i >= 0; i--)
        {
            temp += i + "|";
            for (int j = 0; j < this.cols; j++)
            {
                if (exposeUncoveredCells || unCoveredCells.Contains(i * 10 + j))
                {
                    if (mineGrid[i, j] == -1)
                    {
                        temp += 'x';
                    }
                    else if (mineGrid[i, j] == 0)
                    {
                        temp += ' ';
                    }
                    else
                    {
                        temp += mineGrid[i, j];
                    }

                }
                else
                {
                    temp += ('?');
                }

            }
            temp += "\n";
        }
        return temp;
    }

    public override string ToString()
    {
        return GetState(false);
    }


}

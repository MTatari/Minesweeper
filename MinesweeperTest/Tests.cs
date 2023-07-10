using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper;
using System;

namespace MinesweeperTest;

[TestClass]
public class Tests
{
    [TestMethod]
    public void TestInitializeGame()
    {
        var gridInstance = new MineGrid(6, 4);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        var expectedstate =
            "  0123\n" +
            "5|????\n" +
            "4|????\n" +
            "3|????\n" +
            "2|????\n" +
            "1|????\n" +
            "0|????\n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));
    }

    [TestMethod]
    public void TestInitializeGrid()
    {
        var gridInstance = new MineGrid(10, 10);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        var expectedstate =
            "  0123456789\n" +
            "9|          \n" +
            "8|          \n" +
            "7|          \n" +
            "6|          \n" +
            "5|          \n" +
            "4|          \n" +
            "3|          \n" +
            "2|          \n" +
            "1|          \n" +
            "0|          \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(true));
    }


    [TestMethod]
    public void TestSetBombs()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);

        var expectedstate =
            "  01234\n" +
            "4|1x1  \n" +
            "3|11111\n" +
            "2|2211x\n" +
            "1|xx111\n" +
            "0|x31  \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(true));
    }

    [TestMethod]
    public void TestSetoutOfRangeBombs()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());

        try
        {
            gridInstance.SetBomb(-1, 0);
            gridInstance.SetBomb(0, 10);
            Assert.Fail("An out of range exception was expected");
        }
        catch (IndexOutOfRangeException)
        {
            var expectedstate =
                "  01234\n" +
                "4|     \n" +
                "3|     \n" +
                "2|     \n" +
                "1|     \n" +
                "0|     \n";
            Assert.AreEqual(expectedstate, gridInstance.GetState(true));
        }
        catch (Exception e)
        {
            Assert.Fail("Somthing went wrong: " + e.Message);
        }
    }

    [TestMethod]
    public void TestGameWon()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);



        var expectedstate =
            "  01234\n" +
            "4|1?1  \n" +
            "3|11111\n" +
            "2|2211?\n" +
            "1|??111\n" +
            "0|?31  \n";

        gridInstance.UncoverCell(4, 4);
        gridInstance.UncoverCell(4, 0);
        gridInstance.UncoverCell(0, 4);
        gridInstance.UncoverCell(1, 0);
        gridInstance.UncoverCell(1, 3);
        gridInstance.UncoverCell(0, 3);
        gridInstance.UncoverCell(0, 2);
        gridInstance.UncoverCell(1, 2);
        gridInstance.UncoverCell(2, 2);
        gridInstance.UncoverCell(3, 2);

        Assert.IsTrue(gridInstance.IsGameWon());
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));
        Assert.AreEqual(expectedstate, gridInstance.ToString());
        Assert.AreNotEqual(expectedstate, gridInstance.GetState(true));
    }

    [TestMethod]
    public void TestSetBombsAtSameLocationTwice()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 1);//Should do nothing 
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);

        var expectedstate =
            "  01234\n" +
            "4|1?1  \n" +
            "3|11111\n" +
            "2|2211?\n" +
            "1|??111\n" +
            "0|?31  \n";

        gridInstance.UncoverCell(4, 4);
        gridInstance.UncoverCell(4, 0);
        gridInstance.UncoverCell(0, 4);
        gridInstance.UncoverCell(1, 0);
        gridInstance.UncoverCell(1, 3);
        gridInstance.UncoverCell(0, 3);
        gridInstance.UncoverCell(0, 2);
        gridInstance.UncoverCell(1, 2);
        gridInstance.UncoverCell(2, 2);
        gridInstance.UncoverCell(3, 2);

        Assert.IsTrue(gridInstance.IsGameWon());
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));
        Assert.AreEqual(expectedstate, gridInstance.ToString());
        Assert.AreNotEqual(expectedstate, gridInstance.GetState(true));
    }

    [TestMethod]
    public void TestGameLost()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);


        gridInstance.UncoverCell(4, 4);
        CellStatus c2 = gridInstance.UncoverCell(1, 1);

        Assert.IsFalse(gridInstance.IsGameWon());
        Assert.AreEqual(c2, CellStatus.Mine);
    }


    [TestMethod]
    public void TestReturntypeForUncoverCell()
    {
        var gridInstance = new MineGrid(5, 5);
        Assert.IsNotNull(gridInstance);
        Assert.IsFalse(gridInstance.IsGameWon());
        gridInstance.SetBomb(0, 0);
        gridInstance.SetBomb(0, 1);
        gridInstance.SetBomb(1, 1);
        gridInstance.SetBomb(1, 4);
        gridInstance.SetBomb(4, 2);


        CellStatus c1 = gridInstance.UncoverCell(4, 4);
        CellStatus c2 = gridInstance.UncoverCell(4, 2);
        CellStatus c3 = gridInstance.UncoverCell(4, 4);

        Assert.AreEqual(c1, CellStatus.Unvisitedbefore);
        Assert.AreEqual(c2, CellStatus.Mine);
        Assert.AreEqual(c3, CellStatus.Visited);


        Assert.IsFalse(gridInstance.IsGameWon());
    }

    [TestMethod]
    public void TestGameScenario()
    {
        var gridInstance = new MineGrid(9, 9);

        gridInstance.SetBomb(7, 8);
        gridInstance.SetBomb(6, 8);
        gridInstance.SetBomb(4, 5);
        gridInstance.SetBomb(7, 4);
        gridInstance.SetBomb(3, 3);
        gridInstance.SetBomb(1, 2);
        gridInstance.SetBomb(2, 2);

        var expectedstate =
        "  012345678\n" +
        "8|?????????\n" +
        "7|?????????\n" +
        "6|?????????\n" +
        "5|?????????\n" +
        "4|?????????\n" +
        "3|?????????\n" +
        "2|?????????\n" +
        "1|?????????\n" +
        "0|?????????\n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //move 1
        gridInstance.UncoverCell(5, 5);
        expectedstate =
       "  012345678\n" +
       "8|?????????\n" +
       "7|?????????\n" +
       "6|?????????\n" +
       "5|?????1???\n" +
       "4|?????????\n" +
       "3|?????????\n" +
       "2|?????????\n" +
       "1|?????????\n" +
       "0|?????????\n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //move 2
        gridInstance.UncoverCell(1, 6);
        expectedstate =
        "  012345678\n" +
        "8|     1???\n" +
        "7|     1???\n" +
        "6|   111???\n" +
        "5|   1?1???\n" +
        "4|  12?????\n" +
        "3|123??????\n" +
        "2|?????????\n" +
        "1|?????????\n" +
        "0|?????????\n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //Move 3
        gridInstance.UncoverCell(6, 1);
        expectedstate =
        "  012345678\n" +
        "8|     1???\n" +
        "7|     1???\n" +
        "6|   111???\n" +
        "5|   1?1???\n" +
        "4|  12211??\n" +
        "3|123?1 111\n" +
        "2|???21    \n" +
        "1|1221     \n" +
        "0|         \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //Move 4
        gridInstance.UncoverCell(0, 2);
        expectedstate =
        "  012345678\n" +
        "8|     1???\n" +
        "7|     1???\n" +
        "6|   111???\n" +
        "5|   1?1???\n" +
        "4|  12211??\n" +
        "3|123?1 111\n" +
        "2|1??21    \n" +
        "1|1221     \n" +
        "0|         \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //Move 5
        gridInstance.UncoverCell(8, 8);
        expectedstate =
        "  012345678\n" +
        "8|     1??1\n" +
        "7|     1???\n" +
        "6|   111???\n" +
        "5|   1?1???\n" +
        "4|  12211??\n" +
        "3|123?1 111\n" +
        "2|1??21    \n" +
        "1|1221     \n" +
        "0|         \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));


        //Move 6
        gridInstance.UncoverCell(7, 6);
        expectedstate =
        "  012345678\n" +
        "8|     1??1\n" +
        "7|     1221\n" +
        "6|   111   \n" +
        "5|   1?1111\n" +
        "4|  12211??\n" +
        "3|123?1 111\n" +
        "2|1??21    \n" +
        "1|1221     \n" +
        "0|         \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));

        //Move 7
        gridInstance.UncoverCell(8, 4);
        expectedstate =
        "  012345678\n" +
        "8|     1??1\n" +
        "7|     1221\n" +
        "6|   111   \n" +
        "5|   1?1111\n" +
        "4|  12211?1\n" +
        "3|123?1 111\n" +
        "2|1??21    \n" +
        "1|1221     \n" +
        "0|         \n";
        Assert.AreEqual(expectedstate, gridInstance.GetState(false));



        Assert.IsTrue(gridInstance.IsGameWon());
    }


}

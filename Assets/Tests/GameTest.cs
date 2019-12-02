using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameTest
    {
        [Test]
        public void GameTest_FormManager_OpenForm()
        {
            GameObject mainCanvas = new GameObject();
            FormManager.Instance.Initialize(mainCanvas);

            MainForm form = FormManager.Instance.Get<MainForm>();
            form.SetData(new Board(8, 8), new DiagonalPieceStrategy());

            FormManager.Instance.OpenForm<MainForm>();

            Assert.IsTrue(form.IsOpen);
        }

        [Test]
        public void GameTest_Board_SetPiece()
        {
            Board board = new Board(8, 8);
            Piece piece = new Piece(true);

            board.SetPiece(piece, 5, 6);

            Assert.IsTrue(piece.cellRef == board[5, 6]);
        }

        [Test]
        public void GameTest_DiagonalPieceStrategy_GetAvailableCells()
        {
            Board board = new Board(8, 8);
            DiagonalPieceStrategy strategy = new DiagonalPieceStrategy();
            strategy.SetBoard(board);

            List<Cell> cells = strategy.GetAvailableCells(board[3, 3]);
            List<Cell> expected = new List<Cell>()
            {
                board[2,2], board[1,1], board[0,0],
                board[4,2],board[5,1],board[6,0],
                board[2,4],board[1,5],board[0,6],
                board[4,4],board[5,5],board[6,6],board[7,7]
            };
            CollectionAssert.AreEquivalent(expected, cells);
        }

        [Test]
        public void GameTest_StriaghtPieceStrategy_GetAvailableCells()
        {
            Board board = new Board(8, 8);
            StraightPieceStrategy strategy = new StraightPieceStrategy();
            strategy.SetBoard(board);

            List<Cell> cells = strategy.GetAvailableCells(board[3, 3]);
            List<Cell> expected = new List<Cell>()
            {
                board[2,3], board[1,3], board[0,3],
                board[3,2],board[3,1],board[3,0],
                board[4,3],board[5,3],board[6,3],board[7,3],
                board[3,4],board[3,5],board[3,6],board[3,7]
            };
            CollectionAssert.AreEquivalent(expected, cells);
        }

        [Test]
        public void GameTest_OneStepPieceStrategy_GetAvailableCells()
        {
            Board board = new Board(8, 8);
            OneStepPieceStrategy strategy = new OneStepPieceStrategy();
            strategy.SetBoard(board);

            List<Cell> cells = strategy.GetAvailableCells(board[3, 3]);
            List<Cell> expected = new List<Cell>()
            {
                board[2,2], board[2,3], board[2,4],
                board[3,4],board[4,4],board[4,3],
                board[4,2],board[3,2]
            };
            CollectionAssert.AreEquivalent(expected, cells);
        }

        [UnityTest]
        public IEnumerator GameTestWithEnumeratorPasses()
        {

            yield return null;
        }
    }
}

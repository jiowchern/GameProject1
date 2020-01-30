using System;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class Cell
    {
        /// <summary>
        /// The k cell size.
        /// </summary>
        public static int kCellSize = 10;

        /// <summary>
        /// The k padding.
        /// </summary>
        public static int kPadding = 5;

        /// <summary>
        /// The seed.
        /// </summary>
        private static readonly long Seed = DateTime.Now.Ticks;

        /// <summary>
        /// The the random.
        /// </summary>
        public static Random TheRandom = new Random((int)Cell.Seed);

        /// <summary>
        /// The column.
        /// </summary>
        public int Column;

        /// <summary>
        /// The row.
        /// </summary>
        public int Row;

        /// <summary>
        /// The walls.
        /// </summary>
        public int[] Walls = new[] { 1, 1, 1, 1 };

        /// <summary>
        /// The has all walls.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasAllWalls()
        {
            for (var i = 0; i < 4; i++)
            {
                if (this.Walls[i] == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// The knock down wall.
        /// </summary>
        /// <param name="theWall">
        /// The the wall.
        /// </param>
        public void KnockDownWall(int theWall)
        {
            this.Walls[theWall] = 0;
        }

        /// <summary>
        /// The knock down wall.
        /// </summary>
        /// <param name="theCell">
        /// The the cell.
        /// </param>
        public void KnockDownWall(Cell theCell)
        {
            // find adjacent wall
            var theWallToKnockDown = this.FindAdjacentWall(theCell);
            this.Walls[theWallToKnockDown] = 0;
            var oppositeWall = (theWallToKnockDown + 2) % 4;
            theCell.Walls[oppositeWall] = 0;
        }

        /// <summary>
        /// The find adjacent wall.
        /// </summary>
        /// <param name="theCell">
        /// The the cell.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int FindAdjacentWall(Cell theCell)
        {
            if (theCell.Row == this.Row)
            {
                if (theCell.Column < this.Column)
                    return 0;
                return 2;
            }

            if (theCell.Row < this.Row)
                return 1;
            return 3;
        }


    }
}
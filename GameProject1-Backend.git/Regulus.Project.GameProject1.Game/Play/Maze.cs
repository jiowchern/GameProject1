using System.Collections;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class Maze
    {
        /// <summary>
        /// The k dimension.
        /// </summary>
        private int _Dimension;

        /// <summary>
        /// The cell stack.
        /// </summary>
        private readonly Stack CellStack = new Stack();

        /// <summary>
        /// The cells.
        /// </summary>
        public Cell[,] Cells;

        /// <summary>
        /// The current cell.
        /// </summary>
        private Cell CurrentCell;

        /// <summary>
        /// The total cells.
        /// </summary>
        private int TotalCells ;

        /// <summary>
        /// The visited cells.
        /// </summary>
        private int VisitedCells = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maze"/> class.
        /// </summary>
        public Maze(int dimension)
        {
            _Dimension = dimension;
            TotalCells = _Dimension * _Dimension;
            this.Initialize();
        }

        /// <summary>
        /// The get neighbors with walls.
        /// </summary>
        /// <param name="aCell">
        /// The a cell.
        /// </param>
        /// <returns>
        /// The <see cref="ArrayList"/>.
        /// </returns>
        private ArrayList GetNeighborsWithWalls(Cell aCell)
        {
            var Neighbors = new ArrayList();
            
            for (var countRow = -1; countRow <= 1; countRow++)
                for (var countCol = -1; countCol <= 1; countCol++)
                {
                    if ((aCell.Row + countRow < _Dimension) &&
                        (aCell.Column + countCol < _Dimension) &&
                        (aCell.Row + countRow >= 0) &&
                        (aCell.Column + countCol >= 0) &&
                        ((countCol == 0) || (countRow == 0)) &&
                        (countRow != countCol)
                        )
                    {
                        if (this.Cells[aCell.Row + countRow, aCell.Column + countCol].HasAllWalls())
                        {
                            Neighbors.Add(this.Cells[aCell.Row + countRow, aCell.Column + countCol]);
                        }
                    }
                }

            return Neighbors;
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public void Initialize()
        {
            this.Cells = new Cell[_Dimension, _Dimension];
            this.TotalCells = _Dimension * _Dimension;
            for (var i = 0; i < _Dimension; i++)
                for (var j = 0; j < _Dimension; j++)
                {
                    this.Cells[i, j] = new Cell();
                    this.Cells[i, j].Row = i;
                    this.Cells[i, j].Column = j;
                }

            this.CurrentCell = this.Cells[0, 0];
            this.VisitedCells = 1;
            this.CellStack.Clear();
        }

        /// <summary>
        /// The generate.
        /// </summary>
        public void Generate()
        {
            while (this.VisitedCells < this.TotalCells)
            {
                // get a list of the neighboring cells with all walls intact
                var AdjacentCells = this.GetNeighborsWithWalls(this.CurrentCell);

                // test if a cell like this exists
                if (AdjacentCells.Count > 0)
                {
                    // yes, choose one of them, and knock down the wall between it and the current cell
                    var randomCell = Cell.TheRandom.Next(0, AdjacentCells.Count);
                    var theCell = (Cell)AdjacentCells[randomCell];
                    this.CurrentCell.KnockDownWall(theCell);
                    this.CellStack.Push(this.CurrentCell); // push the current cell onto the stack
                    this.CurrentCell = theCell; // make the random neighbor the new current cell
                    this.VisitedCells++;
                }
                else
                {
                    // No cells with walls intact, pop current cell from stack
                    this.CurrentCell = (Cell)this.CellStack.Pop();
                }
            }
        }
    }
}
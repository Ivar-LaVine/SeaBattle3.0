using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public class BattleField : Control
    {
        /// <summary>
        /// Battlefield Constructor
        /// </summary>
        private int mapSize = 10;
        private int cellSize = 30;
        private const int scoreNeed = 20;
        private int circleDiameter = 16;
        private int playerScore = 0;
        private bool isPlacement;
        private int[,] myMap;
        private int[,] enemyMap;
        private bool isGameStart;
        private bool turn = true;
        private bool orientation = true;
        private List<BattleShip> battleShips = new List<BattleShip>();
        private List<BattleShip> enemyBattleShips = new List<BattleShip>();
        //Foreach
        private int index = 0;
        private int[] shipsLengthArray = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        public int[,] MyMap
        {
            get
            {
                return myMap;
            }
        }
        public int[,] EnemyMap
        {
            get
            {
                return enemyMap;
            }
        }
        private string alphabet = "АБВГДЕЖЗИК";
        private Font font = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
        private int enemyScore = 0;

        public BattleField()
        {
            myMap = new int[mapSize, mapSize];
            enemyMap = new int[mapSize, mapSize];
            FillMap();
            isPlacement = true;
            CreateBattleShips();
            StartAIPlacement();
            isGameStart = false;
        }
        private void CreateBattleShips()
        {
            for (int i = 0; i < shipsLengthArray.Count(); i++)
            {
                battleShips.Add(new BattleShip(shipsLengthArray[i], this));
                enemyBattleShips.Add(new BattleShip(shipsLengthArray[i], this));
            }
        }

        private void FillMap()
        {
            var rand = new Random();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;
                    // AIMap Random Generate
                    enemyMap[i, j] = 0;
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i <= mapSize; i++)
            {
                for (int j = 0; j <= mapSize * 2 + 3; j++)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Location = new Point(cellSize * j, cellSize * i),
                        Size = new Size(cellSize, cellSize)
                    };
                    if (!(j == mapSize + 1 || j == mapSize + 2))
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), rectangle);
                        e.Graphics.DrawRectangle(new Pen(Color.Black), rectangle);
                    }
                    if (i == 0 && j > 0 && j <= mapSize)
                    {
                        e.Graphics.DrawString(alphabet[j - 1].ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                    }
                    else if (i == 0 && j > mapSize + 3 && j <= mapSize * 2 + 3)
                    {
                        e.Graphics.DrawString(alphabet[j - 1 - mapSize - 3].ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                    }
                    else if (i > 0 && j == 0 || i > 0 && j == mapSize + 3)
                    {
                        if (i == 10)
                        {
                            e.Graphics.DrawString(i.ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 5, cellSize * i + 9));
                        }
                        else
                        {
                            e.Graphics.DrawString(i.ToString(), font, new SolidBrush(Color.Black), new Point(cellSize * j + 9, cellSize * i + 9));
                        }
                    }
                }
            }
            foreach (BattleShip ship in battleShips)
            {
                if (ship.Orientation == true)
                {
                    if ((ship.Column >= 0) && (ship.Row >= 0))
                        //for (int i = ship.Column; i < ship.Column + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize + (ship.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((ship.Column >= 0) && (ship.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize, cellSize + (ship.Length - 1) * cellSize);
                }
            }
            foreach (BattleShip enemyShip in enemyBattleShips)
            {
                if (enemyShip.Orientation == true)
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Column; i < ship.Column + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), cellSize + enemyShip.Column * cellSize + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize + (enemyShip.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), (cellSize + enemyShip.Column * cellSize) + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize, cellSize + (enemyShip.Length - 1) * cellSize);
                }
            }
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Location = new Point(cellSize + cellSize * j, cellSize + cellSize * i),
                        Size = new Size(cellSize, cellSize)
                    };
                    if (myMap[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.YellowGreen), rectangle);
                    }
                    if (myMap[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.YellowGreen), rectangle);
                    }
                    if (enemyMap[i, j] == 3)
                    {
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), (mapSize + j + 1 + 3) * cellSize, (i + 1) * cellSize, (mapSize + j + 2 + 3) * cellSize, (i + 2) * cellSize);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), (mapSize + j + 2 + 3) * cellSize, (i + 1) * cellSize, (mapSize + j + 1 + 3) * cellSize, (i + 2) * cellSize);
                    }
                    else if (enemyMap[i, j] == 4)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.BlueViolet), (cellSize / 2 - circleDiameter / 2) + ((j + 3 + mapSize + 1) * cellSize), (cellSize / 2 - circleDiameter / 2) + ((i + 1) * cellSize), circleDiameter, circleDiameter);
                    }
                    if (myMap[i, j] == 3)
                    {
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), (j + 1) * cellSize, (i + 1) * cellSize, (j + 2) * cellSize, (i + 2) * cellSize);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), (j + 2) * cellSize, (i + 1) * cellSize, (j + 1) * cellSize, (i + 2) * cellSize);
                    }
                    else if (myMap[i, j] == 4)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.BlueViolet), (cellSize / 2 - circleDiameter / 2) + ((j + 1) * cellSize), (cellSize / 2 - circleDiameter / 2) + ((i + 1) * cellSize), circleDiameter, circleDiameter);
                    }
                    //else if (myMap[i, j] == 2)
                    //{
                    //    e.Graphics.FillRectangle(new SolidBrush(Color.Red), rectangle);
                    //}
                }
            }

            UpdateMap();
        }
        private void UpdateMap()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        myMap[i, j] = 0;
                    }
                }
            }
        }
        private void StartAIPlacement()
        {
            int k = 0;
            while (k < 10)
            {
                BattleShip enemyShip = enemyBattleShips[k];
                Random random = new Random();
                int buf = random.Next(0, 2);
                if (buf == 1)
                {
                    enemyShip.Orientation = true;
                }
                else
                {
                    enemyShip.Orientation = false;
                }
                int x = random.Next(1, mapSize);
                int y = random.Next(1, mapSize);
                if (enemyShip.Orientation)
                {
                    enemyShip.SetCoordinatesAI(y, x);
                    if (enemyShip.Column == x && enemyShip.Row == y)
                    {
                        k++;
                        for (int i = 0; i < enemyShip.Length; i++)
                        {
                            enemyMap[enemyShip.Row, enemyShip.Column + i] = 2;
                            enemyShip.Status[i] = 2;
                        }
                    }


                }
                else
                {
                    enemyShip.SetCoordinatesAI(y, x);
                    if (enemyShip.Column == x && enemyShip.Row == y)
                    {
                        k++;
                        for (int i = 0; i < enemyShip.Length; i++)
                        {
                            enemyMap[enemyShip.Row + i, enemyShip.Column] = 2;
                            enemyShip.Status[i] = 2;
                        }
                    }
                }
            }
        }
        private void ShipDestroyed(BattleShip ship, int flag) // flag = AI's / player's ship
        {
            int cnt = 0;
            if (flag == 1)
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    if (ship.Orientation)
                    {
                        // Horizotnal
                        if (enemyMap[ship.Row, ship.Column + i] == 3)
                        {
                            ship.Status[i] = 3;
                        }
                    }
                    else if (!ship.Orientation)
                    {
                        // Vertical
                        if (enemyMap[ship.Row + i, ship.Column] == 3)
                        {
                            ship.Status[i] = 3;
                        }
                    }
                    if (ship.Status[i] == 3)
                    {
                        cnt++;
                    }
                }
                if (cnt == ship.Length && ship.Length > 1)
                {
                    if (ship.Orientation && ship.Column > 0)
                    {
                        enemyMap[ship.Row, ship.Column - 1] = 4; // Up
                    }
                    if (ship.Orientation && ship.Column + ship.Length < mapSize)
                    {
                        enemyMap[ship.Row, ship.Column + ship.Length] = 4; // Down
                    }
                    if (!ship.Orientation && ship.Row > 0)
                    {
                        enemyMap[ship.Row - 1, ship.Column] = 4; // Left
                    }
                    if (!ship.Orientation && ship.Row + ship.Length < mapSize)
                    {
                        enemyMap[ship.Row + ship.Length, ship.Column] = 4; // Right
                    }
                }
                else if (cnt == ship.Length && ship.Length == 1)
                {
                    if (ship.Column > 0)
                    {
                        enemyMap[ship.Row, ship.Column - 1] = 4; // Up
                    }
                    if (ship.Column + ship.Length < mapSize)
                    {
                        enemyMap[ship.Row, ship.Column + ship.Length] = 4; // Down
                    }
                    if (ship.Row > 0)
                    {
                        enemyMap[ship.Row - 1, ship.Column] = 4; // Left
                    }
                    if (ship.Row + ship.Length < mapSize)
                    {
                        enemyMap[ship.Row + ship.Length, ship.Column] = 4; // Right
                    }
                }
            }
            else
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    if (ship.Orientation)
                    {
                        // Horizotnal
                        if (myMap[ship.Row, ship.Column + i] == 3)
                        {
                            ship.Status[i] = 3;
                        }
                    }
                    else if (!ship.Orientation)
                    {
                        // Vertical
                        if (myMap[ship.Row + i, ship.Column] == 3)
                        {
                            ship.Status[i] = 3;
                        }
                    }
                    if (ship.Status[i] == 3)
                    {
                        cnt++;
                    }
                }
                if (cnt == ship.Length && ship.Length > 1)
                {
                    if (ship.Orientation && ship.Column > 0)
                    {
                        myMap[ship.Row, ship.Column - 1] = 4; // Up
                    }
                    if (ship.Orientation && ship.Column + ship.Length < mapSize)
                    {
                        myMap[ship.Row, ship.Column + ship.Length] = 4; // Down
                    }
                    if (!ship.Orientation && ship.Row > 0)
                    {
                        myMap[ship.Row - 1, ship.Column] = 4; // Left
                    }
                    if (!ship.Orientation && ship.Row + ship.Length < mapSize)
                    {
                        myMap[ship.Row + ship.Length, ship.Column] = 4; // Right
                    }
                }
                else if (cnt == ship.Length && ship.Length == 1)
                {
                    if (ship.Column > 0)
                    {
                        myMap[ship.Row, ship.Column - 1] = 4; // Up
                    }
                    if (ship.Column + ship.Length < mapSize)
                    {
                        myMap[ship.Row, ship.Column + ship.Length] = 4; // Down
                    }
                    if (ship.Row > 0)
                    {
                        myMap[ship.Row - 1, ship.Column] = 4; // Left
                    }
                    if (ship.Row + ship.Length < mapSize)
                    {
                        myMap[ship.Row + ship.Length, ship.Column] = 4; // Right
                    }
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isPlacement)
            {
                BattleShip ship = battleShips[index];
                int x = e.X / cellSize - 1;
                int y = e.Y / cellSize - 1;
                if (e.X > cellSize && e.X < cellSize * mapSize + cellSize && e.Y > cellSize && e.Y < cellSize * mapSize + cellSize)
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        try
                        {
                            if (orientation && (myMap[y, x + i] != 2) && x + ship.Length - 1 <= 9) myMap[y, x + i] = 1;
                            else if (!orientation && (myMap[y + i, x] != 2) && y + ship.Length - 1 <= 9) myMap[y + i, x] = 1;
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
                Invalidate();
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isPlacement)
            {
                BattleShip ship = battleShips[index];
                int x = e.X / cellSize - 1;
                int y = e.Y / cellSize - 1;
                if (e.X > cellSize && e.X < cellSize * mapSize + cellSize && e.Y > cellSize && e.Y < cellSize * mapSize + cellSize)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        try
                        {
                            if (orientation)
                            {
                                ship.Orientation = orientation;
                                ship.SetCoordinates(y, x);
                                if ((ship.Column == x && ship.Row == y))
                                {

                                    index++;
                                    for (int i = 0; i < ship.Length; i++)
                                    {
                                        myMap[ship.Row, ship.Column + i] = 2;
                                        ship.Status[i] = 2;
                                    }
                                }
                            }
                            else
                            {
                                ship.Orientation = orientation;
                                ship.SetCoordinates(y, x);
                                if (ship.Column == x && ship.Row == y)
                                {
                                    index++;
                                    for (int i = 0; i < ship.Length; i++)
                                    {
                                        myMap[ship.Row + i, ship.Column] = 2;
                                        ship.Status[i] = 2;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        if (index == 10)
                        {
                            isPlacement = false;
                            isGameStart = true;
                        }
                    }
                    else if (e.Button == MouseButtons.Right) orientation = !orientation;
                }
            }
            else if (isGameStart)
            {
                int i;
                int j;
                bool isTurnDone;
                try
                {
                    isTurnDone = true;
                    while (isTurnDone)
                    {
                        i = e.Y / cellSize - 1; // Row
                        j = (e.X / cellSize - 1) - mapSize - 3; // Col
                        // 0 - empty, 1 - hover, 2 - placed, 3 - cross, 4 - empty and fired
                        if (enemyMap[i, j] < 3)
                        {
                            switch (enemyMap[i, j])
                            {
                                case 0:
                                    enemyMap[i, j] = 4; // player missed
                                    turn = false; // make next turn for AI
                                    break;
                                case 2:
                                    playerScore++; // adds to player's score 1
                                    enemyMap[i, j] = 3; // marking that the ship is hit
                                    turn = true; // make next turn for player
                                    // reserving diagonally
                                    if (i > 0 && j > 0)
                                    {
                                        enemyMap[i - 1, j - 1] = 4;
                                    }
                                    if (i > 0 && j < mapSize - 1)
                                    {
                                        enemyMap[i - 1, j + 1] = 4;
                                    }
                                    if (i < mapSize - 1 && j < mapSize - 1)
                                    {
                                        enemyMap[i + 1, j + 1] = 4;
                                    }
                                    if (j > 0 && i < mapSize - 1)
                                    {
                                        enemyMap[i + 1, j - 1] = 4;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            isTurnDone = false;
                        }
                    }
                }

                catch (IndexOutOfRangeException) { }
                // Check if ship is destroyed
                foreach (BattleShip battleShip in enemyBattleShips)
                {
                    ShipDestroyed(battleShip, 1);
                }
                // !turn = AI turn
                int botStop = 0;
                while (!turn)
                {
                    isTurnDone = true;
                    while (isTurnDone && botStop < 50)
                    {
                        Random random = new Random();
                        i = random.Next(0, mapSize);
                        j = random.Next(0, mapSize);
                        int bufi = -1;
                        int bufj = -1;
                        if (myMap[i, j] < 3)
                        {
                            switch (myMap[i, j])
                            {
                                case 0:
                                    myMap[i, j] = 4; // player missed
                                    turn = true; // make next turn for player
                                    break;
                                case 2:
                                    bufi = i;
                                    bufj = j;
                                    enemyScore++; // adds to AI's score 1
                                    myMap[i, j] = 3; // marking that the ship is hit
                                    turn = false; // make next turn for AI
                                    #region diagonally
                                    if (i > 0 && j > 0)
                                    {
                                        myMap[i - 1, j - 1] = 4;
                                    }
                                    if (i > 0 && j < mapSize - 1)
                                    {
                                        myMap[i - 1, j + 1] = 4;
                                    }
                                    if (i < mapSize - 1 && j < mapSize - 1)
                                    {
                                        myMap[i + 1, j + 1] = 4;
                                    }
                                    if (j > 0 && i < mapSize - 1)
                                    {
                                        myMap[i + 1, j - 1] = 4;
                                    }
                                    #endregion
                                    foreach (BattleShip battleShip in battleShips)
                                    {
                                        ShipDestroyed(battleShip, 2);
                                    }
                                    switch (random.Next(1, 5))
                                    {
                                        // 1 - up; 2 - down; 3 - left; 4 - right;
                                        case 1:
                                            if (bufi > 0 && myMap[bufi - 1, bufj] == 2)
                                            {
                                                myMap[bufi - 1, bufj] = 3;
                                                bufi = i;
                                                bufj = j;
                                            }
                                            break;
                                        case 2:
                                            if (bufi < mapSize - 1 && myMap[bufi + 1, bufj] == 2)
                                            {
                                                myMap[bufi + 1, bufj] = 3;
                                                bufi = i;
                                                bufj = j;
                                            }
                                            break;
                                        case 3:
                                            if (bufj > 0 && myMap[bufi, bufj - 1] == 2)
                                            {
                                                myMap[bufi, bufj - 1] = 3;
                                                bufi = i;
                                                bufj = j;
                                            }
                                            break;
                                        case 4:
                                            if (bufj < mapSize - 1 && myMap[bufi, bufj + 1] == 2)
                                            {
                                                myMap[bufi, bufj + 1] = 3;
                                                bufi = i;
                                                bufj = j;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    break;
                            }
                            
                        }
                        else
                        {
                            isTurnDone = false;
                        }
                        botStop++;
                    }
                }

            }
            Invalidate();
            if (isGameStart)
            {
                if (playerScore == scoreNeed)
                {
                    MessageBox.Show("You Win!");
                    isGameStart = false;
                }
                else if (enemyScore == scoreNeed)
                {
                    MessageBox.Show("You Lose!");
                    isGameStart = false;
                }
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}

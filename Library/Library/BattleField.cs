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
        private SolidBrush shipColor = new SolidBrush(Color.Blue);
        private bool orientation = true;
        private List<BattleShip> battleShips = new List<BattleShip>();
        private List<BattleShip> enemyBattleShips = new List<BattleShip>();
        private List<Point> coordinates = new List<Point>();
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
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;
                    // AIMap Random Generate
                    enemyMap[i, j] = 0;
                    coordinates.Add(new Point(i, j));
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
                        e.Graphics.FillEllipse(shipColor, cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize + (ship.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((ship.Column >= 0) && (ship.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(shipColor, cellSize + ship.Column * cellSize, cellSize + ship.Row * cellSize, cellSize, cellSize + (ship.Length - 1) * cellSize);
                }
            }
            foreach (BattleShip enemyShip in enemyBattleShips)
            {
                if (enemyShip.Orientation == true)
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Column; i < ship.Column + ship.Length; i++)
                        e.Graphics.FillEllipse(shipColor, cellSize + enemyShip.Column * cellSize + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize + (enemyShip.Length - 1) * cellSize, cellSize);
                }
                else
                {
                    if ((enemyShip.Column >= 0) && (enemyShip.Row >= 0))
                        //for (int i = ship.Row; i < ship.Row + ship.Length; i++)
                        e.Graphics.FillEllipse(shipColor, (cellSize + enemyShip.Column * cellSize) + cellSize * (mapSize + 3), cellSize + enemyShip.Row * cellSize, cellSize, cellSize + (enemyShip.Length - 1) * cellSize);
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
                    enemyShip.SetCoordinates(y, x, false);
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
                    enemyShip.SetCoordinates(y, x, false);
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
            int x = e.X / cellSize - 1;
            int y = e.Y / cellSize - 1;
            if (x >= 0 && x < mapSize && y >= 0 && y < mapSize)
            {
                if (isPlacement)
                {
                    BattleShip ship = battleShips[index];

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
            }
            Invalidate();
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isPlacement)
            {
                // Инициализация очередного экземпляра корабля из листа кораблей
                BattleShip ship = battleShips[index];
                // Рассчет координат в зависимости от положения курсора
                int x = e.X / cellSize - 1; // x = Column
                int y = e.Y / cellSize - 1; // y = Row
                if (x >= 0 && x < mapSize && y >= 0 && y < mapSize) // Проверка на принадлежность координат к карте пользователя
                {
                    if (e.Button == MouseButtons.Left) // Если нажатая кнопка - левая
                    {
                        try
                        {
                            if (orientation) // Если ориентация = true - корабль будет расположен горизонтально
                            {
                                ship.Orientation = orientation; // Привязка ориентации экземпляра корабля к выбранной ориентации 
                                ship.SetCoordinates(y, x, true); // Внутренний метод корабля, осуществляющий проверку на правильное положение корабля на поле
                                // Проверка на то, что вышедшие из метода SetCoordinates параметры не изменились
                                // (В случае неправильной расстановки метод возвращает -1 в обе координаты)
                                if (ship.Column == x && ship.Row == y) 
                                {
                                    for (int i = 0; i < ship.Length; i++) // Проход по всей длине корабля
                                    {
                                        // Прибавление к столбцу корабля значения i, для расположения корабля по горизонтали (по столбцам)
                                        myMap[ship.Row, ship.Column + i] = 2; // Запись состояния корабля в двумерный массив карты пользователя
                                        // 2 - обозначает, что в этой ячейке расположен корабль
                                        ship.Status[i] = 2; // Запись статуса корабля, если все значение = 2, то корабль не сбит и установлен
                                    }
                                    // Инкремент индекса, чтобы из листа достать следующий экземпляр корабля
                                    index++;
                                }
                            }
                            else // Иначе - вертикально
                            {
                                ship.Orientation = orientation;
                                ship.SetCoordinates(y, x, true);
                                if (ship.Column == x && ship.Row == y)
                                {
                                    index++;
                                    for (int i = 0; i < ship.Length; i++)
                                    {
                                        // Прибавление к строке корабля значения i, для расположения корабля по вертикали (по строкам)
                                        myMap[ship.Row + i, ship.Column] = 2;
                                        ship.Status[i] = 2;
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        if (index == 10) // В том случае если индекс получит значение 10, значит корабли в листе закончились и продолжение расстановки прекращается
                        {
                            isPlacement = false; // Окончание расстановки
                            isGameStart = true; // Начало игры
                        }
                    }
                    else if (e.Button == MouseButtons.Right) orientation = !orientation; // Смена ориентации корабля по клике правой кнопкой мыши
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
                                    if (playerScore == 20)
                                    {
                                        isTurnDone = false;
                                        isGameStart = false;
                                        Invalidate();
                                        MessageBox.Show("You win!");
                                    }
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
                while (!turn)
                {
                    isTurnDone = true;
                    turn = true;
                    while (isTurnDone)
                    {
                        Random random = new Random();
                        if (coordinates.Count > 0 && enemyScore < 20)
                        {
                            Point point = coordinates[random.Next(0, coordinates.Count)];
                            i = point.X;
                            j = point.Y;
                            coordinates.Remove(point);
                            if (myMap[i, j] < 3)
                            {
                                switch (myMap[i, j])
                                {
                                    case 0:
                                        myMap[i, j] = 4; // player missed
                                        turn = true; // make next turn for player
                                        isTurnDone = false;
                                        break;
                                    case 2:
                                        enemyScore++; // adds to AI's score 1
                                        myMap[i, j] = 3; // marking that the ship is hit
                                        if (enemyScore == 20)
                                        {
                                            isTurnDone = false;
                                            isGameStart = false;
                                            Invalidate();
                                            MessageBox.Show("You Lose!");
                                        }
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
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            Invalidate();
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

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        const int n = 3;
        const int sizeButton = 50;
        public int[,] map = new int[n * n, n * n];
        public Button[,] cells = new Button[n * n, n * n];

        public Form1()
        {
            InitializeComponent();
        }

        // Функция генерация игрового поля. Включает в себя функции для перемешивания, создания ячеек, сокрытия ячеек
        public void GenerateMap()
        {

            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    map[i, j] = (i * n + i / n + j) % (n * n) + 1; // Задание значений элементам map
                }
            }
            Random r = new Random();
            for (int i = 0; i < r.Next(100, 1000); i++)
            {
                ShuffleMap(i % 5);
            }

            Go();
        }

        // Функция обработки ввода в поле кол-ва ячеек
        private void NumberOfCells(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number)
                && e.KeyChar != (char)8) //Настройка доступных символов для бокса с кол-вом ячеек(числа + Backspace)
            {
                e.Handled = true;
            }

        }

        // Функция обработки нажатия на кнопку Инструкций
        private void InstructionsButton(object sender, EventArgs e)
        {
            MessageBox.Show(
                "1. Выберите уровень сложности ИЛИ укажите кол-во закрытых ячеек.\n" +
                "2. Нажмите на кнопку 'Начать генерацию'.\n" +
                "3. На экране визуализировано поле соответсвующего уровня сложности ИЛИ с определенным кол-вом закрытых ячеек.\n" +
                "Уровни сложности: \nЛегкий - 30 ячеек, \nСредний - 45 ячеек, \nТяжелый - 60 ячеек\n" +
                "4. Нажимая на кнопки, с закрытым числом, попытайтесь заполнить судоку.\n" +
                "5. Нажмите Check для проверки чисел.\n" +
                "Если неверно, то измените значения. Если верно, то вы вернетесь в главное меню.");
        }

        // Функция обработки нажатия на кнопку Информация
        private void InformationButton(object sender, EventArgs e)
        {
            MessageBox.Show("Генератор Судоку. БИВТ-20-6 Голубев Артем");
        }

        // Функция обмена столбцов в одном районе
        public void SwapColumnsInDistrict()
        {
            Random r = new Random();

            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);

            while (block1 == block2)
            {
                block2 = r.Next(0, n);
            }

            block1 *= n;
            block2 *= n;

            for (int i = 0; i < n * n; i++)
            {
                var k = block2;

                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[i, j];
                    map[i, j] = map[i, k];
                    map[i, k] = temp;
                    k++;
                }
            }
        }

        // Функция обмена строк в одном районе
        public void SwapRowsInDistrict()
        {
            Random r = new Random();

            var block1 = r.Next(0, n);
            var block2 = r.Next(0, n);

            while (block1 == block2)
            {
                block2 = r.Next(0, n);
            }

            block1 *= n;
            block2 *= n;

            for (int i = 0; i < n * n; i++)
            {
                var k = block2;

                for (int j = block1; j < block1 + n; j++)
                {
                    var temp = map[j, i];
                    map[j, i] = map[k, i];
                    map[k, i] = temp;
                    k++;
                }
            }
        }

        // Функция обмена районеов по рядам
        public void SwapDistrictInRows()
        {
            Random r = new Random();

            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var row2 = r.Next(0, n);
            var line1 = block * n + row1;

            while (row1 == row2)
            {
                row2 = r.Next(0, n);
            }

            var line2 = block * n + row2;

            for (int i = 0; i < n * n; i++)
            {
                var temp = map[line1, i];
                map[line1, i] = map[line2, i];
                map[line2, i] = temp;
            }
        }

        // Функция обмена районов по столбцам
        public void SwapDistrictInColumns()
        {
            Random r = new Random();

            var block = r.Next(0, n);
            var row1 = r.Next(0, n);
            var row2 = r.Next(0, n);
            var line1 = block * n + row1;

            while (row1 == row2)
            {
                row2 = r.Next(0, n);
            }

            var line2 = block * n + row2;

            for (int i = 0; i < n * n; i++)
            {
                var temp = map[i, line1];
                map[i, line1] = map[i, line2];
                map[i, line2] = temp;
            }
        }

        // Транспонирование матрицы
        public void MatrixTransposition()
        {
            int[,] tMap = new int[n * n, n * n];

            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    tMap[i, j] = map[j, i];
                }
            }
            map = tMap;
        }

        // Функция для перемешивания ячеек, включает в себя функции для перешивания
        public void ShuffleMap(int i)
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowsInDistrict();
                    break;
                case 2:
                    SwapColumnsInDistrict();
                    break;
                case 3:
                    SwapDistrictInRows();
                    break;
                case 4:
                    SwapDistrictInColumns();
                    break;
                default:
                    MatrixTransposition();
                    break;
            }
        }

        // Функция обработки нажатия на ячейку
        private void CellClick(object sender, EventArgs e)
        {
            Button cell = sender as Button;

            if (cell.Text == "")
            {
                cell.Text = "1"; // Начальное значение при нажатии на ячейку
            }
            else
            {
                int current = (int.Parse(cell.Text.ToString()) + 1); // Сколько прибавляется при нажатии

                if (current > 9)
                {
                    current = 1;
                }
                cell.Text = current.ToString();
            }
        }

        // Проверка кнопкой Check
        private void CheckButtonClick(object sender, EventArgs e)
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    var btnText = cells[i, j].Text;

                    if (btnText != map[i, j].ToString())
                    {
                        MessageBox.Show("Неверно!");
                        return;
                    }
                }
            }
            MessageBox.Show("Верно!");
            Controls.Clear();
            InitializeComponent();
        }

        // Возврат в главное меню
        private void MenuButtonClick(object sender, EventArgs e)
        {
            Controls.Clear();
            InitializeComponent();
        }

        // Создание ячеек игрового поля
        public void CreateGameCells()
        {
            Button cell;

            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    // Создание ячеек
                    cell = new Button();
                    cells[i, j] = cell;
                    // Настройка ячеек
                    cell.Size = new Size(sizeButton, sizeButton);
                    cell.Font = new Font("Cambria Code", 24, FontStyle.Bold);
                    cell.Text = map[i, j].ToString();
                    cell.Location = new Point(j * sizeButton + (15 * (int)(j / n)) + 20,
                                                i * sizeButton + (15 * (int)(i / n)) + 20);
                    cell.Click += CellClick;
                    cell.BackColor = Color.White;
                    cell.Enabled = false;
                    // Добавляет ячейку на экран
                    this.Controls.Add(cell);
                }
            }
        }

        // Создание кнопки Check
        public void CreateCheckButton()
        {
            // Создание кнопки Check
            Button checkButton = new Button();
            // Настройка кнопки Check
            checkButton.Size = new Size(150, 50);
            checkButton.Font = new Font("Cambria Code", 24, FontStyle.Bold);
            checkButton.Text = "Check";
            checkButton.Location = new Point(sizeButton * n * n + 60, 20);
            checkButton.Click += CheckButtonClick;
            // Добавляет кнопку Check на экран
            this.Controls.Add(checkButton);
        }

        // Создание кнопки Menu
        public void CreateMenuButton()
        {
            // Создание кнопки Menu
            Button menuButton = new Button();
            // Настройка кнопки Menu
            menuButton.Size = new Size(150, 50);
            menuButton.Font = new Font("Cambria Code", 24, FontStyle.Bold);
            menuButton.Text = "Menu";
            menuButton.Location = new Point(sizeButton * n * n + 60, 70);
            menuButton.Click += MenuButtonClick;
            // Добавляет кнопку Menu на экран
            this.Controls.Add(menuButton);
        }

        // Функция для создания ячеек и кнопки Check
        public void CreateInterface()
        {
            CreateGameCells();
            CreateCheckButton();
            CreateMenuButton();
        }

        // Кол-во ячеек на закрытие
        public int GetCellsToHide()
        {
            int N = -1;
            string difficulty = comboBox1.Text.ToString();
            string[] difficulties = new string[] {"Лёгкий", "Средний", "Тяжелый"};

            if ((textBox1.Text.ToString() != "" &&                                // Пользователь в параметрах указал 
                comboBox1.Text.ToString() != "Выберите уровень сложности") ||
                (textBox1.Text.ToString() == "" &&
                comboBox1.Text.ToString() == "Выберите уровень сложности"))       // сложность и кол-во ячеек
            {
                N = -1;
            }
            else if (!difficulties.Contains(difficulty) && comboBox1.Text.ToString() != "Выберите уровень сложности")
            {
                N = -2;
            }
            else if (textBox1.Text.ToString() != "") // Пользователь выбрал настройку с кол-вом ячеек
            {
                N = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                switch (difficulty)
                {
                    case "Лёгкий":
                        N = 30;
                        break;
                    case "Средний":
                        N = 45;
                        break;
                    case "Тяжелый":
                        N = 60;
                        break;
                    default:
                        break;
                }
            }
            return N;
        }

        // Проверка на ошибки
        public bool CheckAlerts()
        {
            if (GetCellsToHide() == -1)
            {
                MessageBox.Show("Выберите один из вариантов.");
                Controls.Clear();
                InitializeComponent();
                return true;
            }
            else if (GetCellsToHide() == -2)
            {
                MessageBox.Show("Выберите один из предложенных уровней сложности.");
                Controls.Clear();
                InitializeComponent();
                return true;
            }
            else if (GetCellsToHide() == 0 || GetCellsToHide() > 81)
            {
                MessageBox.Show("Диапазон от 1 до 81.");
                Controls.Clear();
                InitializeComponent();
                return true;
            }
            
            return false;
        }

        // Функция для сокрытия ячеек
        public void HideCells()
        {
            int N = GetCellsToHide();
            Random r = new Random();

            if (N == 0)
            {
                return;
            }

            while (N > 0) // Пока кол-во ячеек для сокрытия > 0, отключение функции изменения для ячеек и удаление текста
            {
                int x = r.Next(0, n * n);
                int y = r.Next(0, n * n);

                if (cells[x, y].Text != "")
                {
                    N -= 1;

                    cells[x, y].Enabled = true;
                    cells[x, y].Text = "";
                }
            }

        }

        // Функция запускающая генерацию игрового поля
        public void Go()
        {
            if (CheckAlerts() == false)
            {
                CreateInterface();
                HideCells();
            }
        }

        // Функция обработки нажатия Начать генерацию
        private void StartGeneration(object sender, EventArgs e)
        {
            Controls.Clear();

            Size = new Size(700, 600);
            GenerateMap();
        }
    }
}

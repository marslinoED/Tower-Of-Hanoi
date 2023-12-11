using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TowerOfHanoiProject
{
    public partial class Form1 : Form
    {
        private const int DefaultNumberOfDisks = 3;
        private const int MaxTowerHeight = 8;

        private const int DiskWidth = 30;
        private const int DiskHeight = 10;
        private const int TowerSpacing = 150;
        private const int TowerWidth = 15; // Adjust the tower width as needed
        private const int SpaceBetweenDisks = 5; // Adjust this value for the desired space

        private int numberOfDisks;
        private int speed_int;
        public int counting = 1;
        private Graphics graphics;

        // Keep track of disk positions on each tower
        private Dictionary<int, List<int>> towerDisks;

        // Array of hex colors for the disks
        public static string[] hexColors = { "#d95f48", "#db8437", "#eec332", "#55b59a", "#65c271", "#499ad9", "#9168b6", "#ed9ed6" };

        // TextBox to log the steps
        private TextBox logTextBox;

        // Button and TextBox
        private Button button1;
        private TextBox txtNumberOfDisks;
        private TextBox speed;
        private Label sp, nd, steps_counter, Game, A, B, C;
        public Form1()
        {
            InitializeComponent();
            towerDisks = new Dictionary<int, List<int>>();
            InitializeLogTextBox();
            InitializeButtonAndTextBox();
            this.Text = "Tower Of Hanoi";
            this.ControlBox = false;
            this.Size = new Size(680, 360);
        }

        private void InitializeLogTextBox()
        {
            logTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Location = new Point(450, 10),
                Size = new Size(210, 300),
                Font = new Font("MV Boli", 9)
            };

            Controls.Add(logTextBox);
        }

        private void InitializeButtonAndTextBox()
        {
            button1 = new Button
            {
                Location = new Point(10, 10),
                Text = "Start Game",
                Size = new Size(180, 30),

                Font = new Font("MV Boli", 10)
            };
            button1.Click += button1_Click;
            Controls.Add(button1);

            txtNumberOfDisks = new TextBox
            {
                Location = new Point(10, 50),
                Size = new Size(70, 20),
                Text = DefaultNumberOfDisks.ToString(),

                Font = new Font("MV Boli", 9)
            };
            Controls.Add(txtNumberOfDisks);

            speed = new TextBox
            {
                Location = new Point(120, 50),
                Size = new Size(70, 20),
                Text = "500",
                Font = new Font("MV Boli", 9)
            };
            Controls.Add(speed);

            sp = new Label
            {
                Location = new Point(128, 80),
                Size = new Size(50, 20),
                Text = "Speed",
                Font = new Font("MV Boli", 10),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(sp);

            nd = new Label
            {
                Location = new Point(5, 80),
                Size = new Size(95, 20),
                Text = "Disk Number",
                Font = new Font("MV Boli", 10),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(nd);

            steps_counter = new Label
            {
                Location = new Point(230, 55),
                Size = new Size(150, 27),
                Text = "Steps Count",
                Font = new Font("MV Boli", 12),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(steps_counter);

            Game = new Label
            {
                Location = new Point(200, 15),
                Size = new Size(220, 35),
                Text = "Tower Of Hanoi",
                Font = new Font("MV Boli", 20),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(Game);

            A = new Label
            {
                Location = new Point(73, 110),
                Size = new Size(20, 25),
                Text = "1",
                Font = new Font("MV Boli", 12),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(A);

            B = new Label
            {
                Location = new Point(223, 110),
                Size = new Size(20, 25),
                Text = "2",
                Font = new Font("MV Boli", 12),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(B);

            C = new Label
            {
                Location = new Point(373, 110),
                Size = new Size(20, 25),
                Text = "3",
                Font = new Font("MV Boli", 12),
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(C);
        }

        private void LogStep(string step)
        {
            logTextBox.AppendText(step + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if ((int.TryParse(txtNumberOfDisks.Text, out numberOfDisks) && numberOfDisks > 0 && numberOfDisks <= 8) && ((int.TryParse(speed.Text, out speed_int) && speed_int > 10)))
            {
                LogStep("Try Number : " + (counting++).ToString());
                LogStep(string.Empty);
                // Limit the number of disks to the maximum allowed
                numberOfDisks = Math.Min(numberOfDisks, MaxTowerHeight);
                steps_counter.Text = "Steps Count " + (Math.Pow(2, numberOfDisks) - 1).ToString();
                graphics = CreateGraphics();
                InitializeTowerDisks();
                DrawDisks(numberOfDisks, 0, ClientSize.Height - DiskHeight);
                TowerOfHanoi(numberOfDisks, 0, 2, 1); // Solve Tower of Hanoi
                LogStep(string.Empty);
            }
            else
            {
                MessageBox.Show("Please enter a valid positive integer for the number of disks or speed.");
            }
        }

        private void InitializeTowerDisks()
        {
            towerDisks.Clear();
            for (int i = 0; i < 3; i++)
            {
                towerDisks[i] = new List<int>();
            }
            for (int i = numberOfDisks; i >= 1; i--)
            {
                towerDisks[0].Add(i);
            }
        }

        private void DrawDisks(int n, int towerIndex, int baseY)
        {
            int colorIndex = 0; // Index for selecting colors from the hexColors array
            // Clear the entire tower
            graphics.FillRectangle(SystemBrushes.Control, towerIndex * TowerSpacing, 0, TowerSpacing, ClientSize.Height);
            // Draw the remaining disks on the tower
            foreach (int diskSize in towerDisks[towerIndex])
            {
                string hexColor = hexColors[colorIndex % hexColors.Length];
                Brush diskBrush = new SolidBrush(ColorTranslator.FromHtml(hexColor));
                int towerX = towerIndex * TowerSpacing + TowerSpacing / 2 - TowerWidth / 2;
                int diskWidth = DiskWidth + (diskSize - 1) * DiskWidth / 2;
                int diskX = towerX - diskWidth / 2 + DiskWidth / 2;
                int diskY = baseY - (MaxTowerHeight - diskSize) * DiskHeight; // Adjusted calculation
                graphics.FillRectangle(diskBrush, diskX, diskY, diskWidth, DiskHeight);
                baseY -= SpaceBetweenDisks + DiskHeight; // Adjust vertical position for the next disk
                colorIndex++;
            }
        }



        private void MoveDisk(int diskSize, int sourceTower, int targetTower)
        {
            // Move the disk from the source tower to the target tower
            towerDisks[sourceTower].Remove(diskSize);
            towerDisks[targetTower].Add(diskSize);

            // Draw the updated towers
            DrawDisks(numberOfDisks, sourceTower, ClientSize.Height - DiskHeight);
            DrawDisks(numberOfDisks, targetTower, ClientSize.Height - DiskHeight);

            // Log the step
            LogStep($"Disk {diskSize} from Tower {sourceTower + 1} to Tower {targetTower + 1}");

            System.Threading.Thread.Sleep(speed_int); // Add a delay for visualization
        }

        private void TowerOfHanoi(int n, int sourceTower, int targetTower, int auxiliaryTower)
        {
            if (n > 0)
            {
                TowerOfHanoi(n - 1, sourceTower, auxiliaryTower, targetTower);
                MoveDisk(n, sourceTower, targetTower);
                TowerOfHanoi(n - 1, auxiliaryTower, targetTower, sourceTower);
            }
        }


    }
}
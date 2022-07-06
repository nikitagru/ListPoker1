using ListPoker.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ListPoker.View
{
    public partial class PlayTable : Form
    {
        List<Player> players = new List<Player>();      
        // key - current step, value - player and his choice
        Dictionary<int, Dictionary<Player, List<ComboBox>>> allPlayersChoice = new Dictionary<int, Dictionary<Player, List<ComboBox>>>();
        // key - current step, value - player score
        Dictionary<int, List<Label>> playersResults = new Dictionary<int, List<Label>>();
        PlayTableController tableController = new PlayTableController();

        string[] playerInfo = new string[] { "заказ", "темная", "взятка", "итого" };
        Brush br;
        
        int currentStep = 1;
        bool printNewStep = true;

        Form1 form1;
        public PlayTable(string playerNames, Form1 form)
        {
            var nameList = playerNames.Split(' ').ToList();
            for (var i = 0; i < nameList.Count; i++)
            {
                players.Add(new Player(nameList[i]));
            }
            this.form1 = form;
            InitializeComponent();
            
            MakeTable();
            DrawPlayerNames();
            DrawDistributor();
            DrawCardCount();
            DrawPlayerRoundVariants();
            
            CreateResultLabels();
            timer1.Interval = 500;
            timer1.Start();
            timer1.Tick += Update;
            this.FormClosing += PlayTable_FormClosing;
        }

        private void PlayTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Show();
        }

        /// <summary>
        /// Event, which recalculating players score
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update(object sender, EventArgs e)
        {
            this.Invalidate();
            
            (bool, int) isCorrectUserInput = tableController.CheckPlayerInput(allPlayersChoice);
            if (!isCorrectUserInput.Item1)
            {
                br = new SolidBrush(Color.FromArgb(184, 81, 81));
                currentStep = isCorrectUserInput.Item2;
                this.Paint += Form1_Paint;
            } else
            {
                currentStep = -10;
                this.Paint += Form1_Paint;
                Dictionary<int, List<int>> results = tableController.CalculatePlayersScore(allPlayersChoice, playersResults);

                ShowResults(results);
                this.Invalidate();
            }
            if (printNewStep)
            {
                CreateRoundArea();
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (br != null)
            {
                Rectangle rec = new Rectangle(new Point(0, TableInfo.firstRowHeight + TableInfo.secondRowHeight + (currentStep - 1) * TableInfo.roundRowHeight), 
                    new Size(
                    TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + TableInfo.playerColumnWidth * players.Count,
                    TableInfo.roundRowHeight));
                
                e.Graphics.FillRectangle(br, rec);
            }
        }

        /// <summary>
        /// Creating main table (base grid)
        /// </summary>
        private void MakeTable()
        {
            PictureBox pic = DrawLine(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + players.Count * TableInfo.playerColumnWidth, 
                                        1, new Point(0, TableInfo.firstRowHeight));
            
            PictureBox pic1 = DrawLine(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + players.Count * TableInfo.playerColumnWidth, 
                                        1, new Point(0, pic.Location.Y + TableInfo.secondRowHeight));
            
            this.Controls.Add(pic);
            this.Controls.Add(pic1);
            DrawVertLines();
            DrawHorLines();
            DrawPlayerInfoTable();
        }

        /// <summary>
        /// Draw a line
        /// </summary>
        /// <param name="width">line width</param>
        /// <param name="height">line height</param>
        /// <param name="location">line position</param>
        /// <returns>returns a line with input params</returns>
        private PictureBox DrawLine(int width, int height, Point location)
        {
            PictureBox pic = new PictureBox();
            pic.BackColor = Color.Black;
            pic.Location = location;
            pic.Width = width;
            pic.Height = height;
            return pic;
        }

        /// <summary>
        /// Drawing vert lines in the table (player column, round and column with distributor name)
        /// </summary>
        private void DrawVertLines()
        {
            var maxCards = 36 / players.Count;
            PictureBox vertLine = DrawLine(1, TableInfo.firstRowHeight + TableInfo.secondRowHeight + ((maxCards - 1) * 2 + players.Count * 2) * TableInfo.roundRowHeight, 
                                                                            new Point(TableInfo.firstColumnWidth, 0));
            Label label = new Label();
            label.Text = "Раунд";
            label.Location = new Point(vertLine.Location.X - 85, TableInfo.firstRowHeight);
            label.Size = new Size(80, 29);
            label.Font = MainFont.font;
            this.Controls.Add(label);
            
            PictureBox vertLine1 = DrawLine(1, TableInfo.firstRowHeight + TableInfo.secondRowHeight + ((maxCards - 1) * 2 + players.Count * 2) * TableInfo.roundRowHeight, 
                                                                            new Point(vertLine.Location.X + TableInfo.secondColumnWidth, 0));
            Label label1 = new Label();
            label1.Text = "Раздающий";
            label1.Location = new Point(vertLine1.Location.X - 130, TableInfo.firstRowHeight);
            label1.Size = new Size(129, 29);
            label1.Font = MainFont.font;
            this.Controls.Add(label1);



            this.Controls.Add(vertLine);
            this.Controls.Add(vertLine1);
            for (var i = 1; i <= players.Count; i++)
            {
                PictureBox pic = DrawLine(2, TableInfo.firstRowHeight + TableInfo.secondRowHeight + ((maxCards - 1) * 2 + players.Count * 2) * TableInfo.roundRowHeight, 
                                                                        new Point(vertLine1.Location.X + (i * TableInfo.playerColumnWidth), 0));
                this.Controls.Add(pic);
            }
        }

        /// <summary>
        /// Drawing horizontal lines in the table (lines for every step of the game)
        /// </summary>
        private void DrawHorLines()
        {
            var maxCards = 36 / players.Count;
            for (var i = 0; i < (maxCards - 1) * 2 + players.Count * 2; i++)
            {
                PictureBox pic = DrawLine(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + players.Count * TableInfo.playerColumnWidth, 
                                                                        1, new Point(0, i * TableInfo.roundRowHeight + TableInfo.firstRowHeight + TableInfo.secondRowHeight));
                this.Controls.Add(pic);
            }
        }

        /// <summary>
        /// Divide a player column on 4 parts
        /// </summary>
        private void DrawPlayerInfoTable()
        {
            var maxCards = 36 / players.Count;
            for (var i = 0; i < players.Count; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    PictureBox pic = DrawLine(1, TableInfo.secondRowHeight + ((maxCards - 1) * 2 + players.Count * 2) * TableInfo.roundRowHeight, 
                        new Point(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + i * TableInfo.playerColumnWidth + j * TableInfo.playerInfoColumnWidth, TableInfo.firstRowHeight));
                    this.Controls.Add(pic);
                }
            }
        }

       
        private void DrawPlayerNames()
        {
            GameLabel gameLabel = new GameLabel();
            for (var i = 0; i < players.Count; i++)
            {
                this.Controls.Add(gameLabel.DrawNames(players[i].name, i));
            }
        }

        /// <summary>
        /// Draws a distributor for every step
        /// </summary>
        private void DrawDistributor()
        {
            var maxCards = 36 / players.Count;
            var counter = 0;
            GameLabel gameLabel = new GameLabel();
            for (var i = 0; i < (maxCards - 1) * 2 + players.Count * 2; i++)
            {
                this.Controls.Add(gameLabel.DrawNameDistrib(players[counter].name, i));
                counter++;
                if (counter == players.Count)
                {
                    counter = 0;
                }
            }
        }

        /// <summary>
        /// Draws information about cards for current step
        /// </summary>
        private void DrawCardCount()
        {
            var maxCards = 36 / players.Count;
            var counter = 1;
            
            GameLabel gameLabel = new GameLabel();

            for (var i = 0; i < maxCards - 1; i++)
            {
                this.Controls.Add(gameLabel.CardCount(counter, i));
                counter++;
            }
            var tempCounter = counter - 1;
            for (var i = 0; i < players.Count; i++)
            {
                this.Controls.Add(gameLabel.CardCount(counter, tempCounter));
                tempCounter++;
            }
            counter--;
            for (var i = maxCards - 1; i > 0; i--)
            {
                this.Controls.Add(gameLabel.CardCount(counter, tempCounter));
                tempCounter++;
                counter--;
            }

            for (var i = 0; i < players.Count; i++)
            {
                this.Controls.Add(gameLabel.CardCount(maxCards, tempCounter));
                tempCounter++;
            }
        }

        /// <summary>
        /// Draws player's choice variants like "заказ", "темная" and others
        /// </summary>
        private void DrawPlayerRoundVariants()
        {
            GameLabel gameLabel = new GameLabel();
            for (var i = 0; i < players.Count; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    this.Controls.Add(gameLabel.PlayerInfo(j, playerInfo[j], i));
                }
            }
        }

        /// <summary>
        /// Creates places for player's inputs
        /// </summary>
        private void CreateRoundArea()
        {
            var maxCards = 36 / players.Count;
            //var iterationCount = (maxCards - 1) * 2 + players.Count * 2;
            Dictionary<Player, List<ComboBox>> currentPlayerInfo = new Dictionary<Player, List<ComboBox>>();
            for (var i = 0; i < players.Count; i++)
            {
                List<ComboBox> playersChoices = new List<ComboBox>();
                for (var j = 0; j < 3; j++)
                {
                    // TODO: create variable with current card count
                    ComboBox playerChoice = createAreaForCurrentStep(maxCards);
                    playerChoice.Location = new Point(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + j * TableInfo.playerInfoColumnWidth + 10 + TableInfo.playerColumnWidth * i,
                                                        TableInfo.firstRowHeight + TableInfo.secondRowHeight + (currentStep - 1) * TableInfo.roundRowHeight + 6);
                    playerChoice.Size = new Size(60, 30);
                    
                    this.Controls.Add(playerChoice);
                    playersChoices.Add(playerChoice);
                }
                currentPlayerInfo.Add(players[i], playersChoices);
            }
            allPlayersChoice.Add(currentStep - 1, currentPlayerInfo);
            printNewStep = false;
        }

        private ComboBox createAreaForCurrentStep(int cardsCount)
        {
            ComboBox playerChoice = new ComboBox();
            for (var j = 0; j < 3; j++)
            {
                for (var i = 0; i <= cardsCount; i++)
                {
                    playerChoice.Items.Add(i);
                }
            }

            return playerChoice;
        }

        /// <summary>
        /// Creates places for player results
        /// </summary>
        private void CreateResultLabels()
        {
            var maxCards = 36 / players.Count;
            for (var k = 1; k <= (maxCards - 1) * 2 + players.Count * 2; k++)
            {
                List<Label> stepResults = new List<Label>();
                for (var i = 1; i <= players.Count; i++)
                {
                    Label label = new Label();
                    label.Font = MainFont.font;
                    label.Location = new Point(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + i * TableInfo.playerColumnWidth - TableInfo.playerInfoColumnWidth,
                                                TableInfo.firstRowHeight + TableInfo.secondRowHeight + (k - 1) * TableInfo.roundRowHeight);
                    this.Controls.Add(label);
                    stepResults.Add(label);
                }
                playersResults.Add(k, stepResults);
            }
        }

        /// <summary>
        /// Drawing results
        /// </summary>
        /// <param name="results">results for current step</param>
        private void ShowResults(Dictionary<int, List<int>> results)
        {
            foreach(var item in playersResults)
            {
                for (var i = 0; i < players.Count; i++)
                {
                    if (results[item.Key].Count == players.Count)
                    {
                        if (item.Key == 1)
                        {
                            playersResults[item.Key][i].Text = results[item.Key][i].ToString();
                        } else
                        {
                            playersResults[item.Key][i].Text = (int.Parse(playersResults[item.Key - 1][i].Text) + results[item.Key][i]).ToString(); 
                        }
                    }
                }
            }
        }

        //private void DrawFailureButton()
        //{
        //    foreach (var item in playersResults)
        //    {
        //        for (var i = 0; i < players.Count; i++)
        //        {
        //            Button failureButton = new Button();
        //            failureButton.Location = new Point(TableInfo.firstColumnWidth + TableInfo.secondColumnWidth + i * TableInfo.playerColumnWidth + TableInfo.secondRowHeight + 10, 0);
        //            failureButton.Size = new Size(10, 5);
        //            failureButton.Text = "Минус 10";
        //            failureButton.Click += new EventHandler(SubstractTen);
        //        }
        //    }
        //}

        //private void SubstractTen(object sender, EventArgs e)
        //{
        //    playersResults[item.Key][i].Text = (int.Parse(playersResults[item.Key][i].Text) - 10).ToString();
        //}
    }
}

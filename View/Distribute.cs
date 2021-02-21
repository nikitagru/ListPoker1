using ListPoker.Controller;
using ListPoker.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ListPoker
{
    public partial class Distribute : Form
    {
        Label lastPlayerText;       // the player role
        List<TextBox> playerName = new List<TextBox>();
        private int playersCount { get; set; }

        private Form1 form;
        public Distribute(int players, Form1 form)
        {
            InitializeComponent();
            this.playersCount = players;
            this.form = form;
        }


        private void Distribute_Load(object sender, EventArgs e)
        {
            DistributeController controller = new DistributeController();
            for (int i = 0; i < playersCount; i++)
            {
                var playerNameString = controller.drawDistribuion(i, MainFont.font);
                
                lastPlayerText = playerNameString.Item1;
                playerName.Add(playerNameString.Item2);
                
                this.Controls.Add(playerNameString.Item1);
                this.Controls.Add(playerNameString.Item2);
            }

            Button select = new Button();       // a button to confirm player names
            select.Location = new Point(lastPlayerText.Location.X + 150, lastPlayerText.Location.Y + 50);
            select.Text = "Далее";
            this.Controls.Add(select);
            select.Click += new EventHandler(SelectClick);

            Button help = new Button();     // show information about distributor
            help.Location = new Point(select.Location.X - 150, select.Location.Y);
            help.Size = new Size(150, select.Height);
            help.Text = "Кто такой раздающий?";
            this.Controls.Add(help);
            help.Click += new EventHandler(controller.HelpClick);
        }

        private void SelectClick(object sender, EventArgs e)
        {
            DistributeController controller = new DistributeController();
            var playerNames = controller.InitPlayers(playerName);

            if (playerNames != "")      // if player name is correct
            {
                PlayTable table = new PlayTable(playerNames, form);
                table.Show();
                this.Hide();
            } else
            {
                MessageBox.Show("Вы не ввели имя игрока или оно является некорректным");
            }
        }
    }
}

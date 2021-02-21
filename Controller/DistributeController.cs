using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ListPoker.Controller
{
    class DistributeController
    {
        /// <summary>
        /// Creating text boxes for player names
        /// </summary>
        /// <param name="i">number of the player</param>
        /// <param name="font">Main font</param>
        /// <returns>Label with player role and a place for input</returns>
        public (Label, TextBox) drawDistribuion(int i, Font font)
        {
            Label label = new Label();
            label.Text = i == 0 ? "Игрок (раздающий) " + (i + 1) : "Игрок " + (i + 1);
            label.Size = new Size(200, 30);
            label.Font = font;
            label.Location = new Point(20, 30 + i * 35);

            TextBox playerName = new TextBox();
            playerName.Location = new Point(label.Location.X + 200 + 50, label.Location.Y + 5);
            playerName.Size = new Size(100, 10);

            var playerNameString = (label, playerName);

            return playerNameString;
        }

        public void HelpClick(object sender, EventArgs e)
        {
            MessageBox.Show("Перед началом игры игроков перетасовывает колоду и начинает раздавать по кругу, пока не выпадет туз.\n" +
                "Игрок, которому выпал туз, является раздающим.");
        }
        
        /// <summary>
        /// Init players
        /// </summary>
        /// <param name="playerName">player names from second window</param>
        /// <returns></returns>
        public string InitPlayers(List<TextBox> playerName)
        {
            var result = "";
            Regex rgx = new Regex(@"[^\s]{0,}$");       // anyone string without spaces
            for (var i = 0; i < playerName.Count; i++)
            {
                if (playerName[i].Text != null && playerName[i].Text != "" && rgx.IsMatch(playerName[i].Text))
                {
                    result += playerName[i].Text + " ";
                } else
                {
                    result = "";
                    break;
                }
            }
            if (result != "")
            {
                result = result.Remove(result.Length - 1);
            }
            return result;
        }
    }
}

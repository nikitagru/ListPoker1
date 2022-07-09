using ListPoker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ListPoker.Controller
{
    class PlayTableController
    {
        public (bool, int) CheckPlayerInput(Dictionary<int, Dictionary<Player, List<ComboBox>>> allPlayersChoice)
        {
            TableChecker tableChecker = new TableChecker();
            return tableChecker.CheckPlayerInfo(allPlayersChoice);
        }

        public Dictionary<int, List<int>> CalculatePlayersScore(Dictionary<int, Dictionary<Player, List<ComboBox>>> allPlayersChoice, Dictionary<int, List<Label>> playersResults)
        {
            Calculator calculator = new Calculator();
            return calculator.CalculateScore(allPlayersChoice);
        }
    }
}

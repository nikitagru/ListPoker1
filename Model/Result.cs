using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ListPoker.Model
{
    class Result
    {
        private Dictionary<int, Dictionary<Player, TextBox[]>> allPlayersChoice;
        public Dictionary<int, List<Label>> playersResults;
        public Result(Dictionary<int, Dictionary<Player, TextBox[]>> allPlayersChoice, Dictionary<int, List<Label>> playersResults)
        {
            this.allPlayersChoice = allPlayersChoice;
            this.playersResults = playersResults;
        }


    }
}

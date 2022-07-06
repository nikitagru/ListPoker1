using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ListPoker.Model
{
    class TableChecker
    {
        public (bool, int) CheckPlayerInfo(Dictionary<int, Dictionary<Player, TextBox[]>> allPlayersChoice)
        {
            var isCorrect = true;
            var correctStep = 0;
            foreach(var item in allPlayersChoice)
            {
                var result = 0;
                var sum = 0;
                var playersGets = 0;
                var playerChoices = new List<int>();
                bool isString = false;
                Regex rgx = new Regex(@"^[0-9]+$");
                foreach (var item1 in item.Value)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        bool success = int.TryParse(item1.Value[i].Text, out sum);
                        if (success)
                        {
                            var currentPlayerChoice = 0;
                            if (int.TryParse(item1.Value[i].Text, out currentPlayerChoice))
                            {
                                result += currentPlayerChoice;
                                playerChoices.Add(int.Parse(item1.Value[i].Text));
                                break;
                            }
                        }
                        if (!rgx.IsMatch(item1.Value[i].Text) && item1.Value[i].Text != "")
                        {
                            isString = true;
                        }
                    }
                    if ((playerChoices.Count > item.Value.Count && item1.Value[1].Text == "0") || isString)
                    {
                        isCorrect = false;
                        return (isCorrect, item.Key);
                    }
                    var currentPlayerGet = 0;
                    var isNotEmpty = int.TryParse(item1.Value[2].Text, out currentPlayerGet);
                    if (isNotEmpty)
                    {
                        playersGets += int.Parse(item1.Value[2].Text);
                    }
                }

                if (result == item.Key || playersGets > item.Key)
                {
                    isCorrect = false;
                    return (isCorrect, item.Key);   
                } else
                {
                    correctStep = item.Key;
                }
            }
            return (isCorrect, correctStep);
            
        }

        public bool CheckNextRoundArea()
    }
}

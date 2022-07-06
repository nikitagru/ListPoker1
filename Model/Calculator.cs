using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ListPoker.Model
{
    class Calculator
    {
        
        Dictionary<int, List<int>> newResults = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> CalculateScore(Dictionary<int, Dictionary<Player, TextBox[]>> allPlayersChoice)
        {
            foreach (var item in allPlayersChoice)
            {
                List<int> results = new List<int>();
                foreach (var item1 in item.Value)       // item1.Key - player
                {
                    bool isStandartChoice;
                    if ((item1.Value[0].Text != "" || item1.Value[1].Text != "") && item1.Value[2].Text != "")
                    {
                        int playerWish;
                        isStandartChoice = int.TryParse(item1.Value[0].Text, out playerWish);
                        
                        if (!isStandartChoice)
                        {
                            playerWish = int.Parse(item1.Value[1].Text);
                        }

                        int playerGets;
                        var isNotEmpty = int.TryParse(item1.Value[2].Text, out playerGets);
                        if (isNotEmpty)
                        {
                            if (playerWish == 0 && playerGets == 0)
                            {
                                results.Add(5);
                            }
                            else if (playerWish > playerGets)
                            {
                                results.Add((playerGets - playerWish) * 10);
                            }
                            else if (playerWish == playerGets)
                            {
                                results.Add(playerGets * 10);
                            }
                            else
                            {
                                results.Add(playerGets * 2);
                            }
                            if (!isStandartChoice)
                            {
                                results[results.Count - 1] *= 2;
                            }
                        }
                    } else
                    {
                        break;
                    }
                }
                newResults.Add(item.Key, results);
            }
            return newResults;
        }
    }
}

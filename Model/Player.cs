using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ListPoker
{
    class Player
    {
        public string name { get; private set; }
        public int score { get; private set; }


       public Player(string name)
        {
            this.name = name;
            this.score = 0;
        }

        public void AddScore(int scoreToAdd)
        {
            this.score += scoreToAdd;
        }
    }
}

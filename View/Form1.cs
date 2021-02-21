using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListPoker
{
    public partial class Form1 : Form
    {
        private int playersCount { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)     // if selected the item from collection
            {
                playersCount = int.Parse((string)comboBox1.SelectedItem);

                Distribute distribute = new Distribute(playersCount, this);
                distribute.Width = 500;
                distribute.Height = playersCount * 20 + 200;
                distribute.Show();
                this.Hide();
            } else
            {
                MessageBox.Show("Вы должны выбрать количество игроков из предложенных в окне");
            }
            
        }
    }
}

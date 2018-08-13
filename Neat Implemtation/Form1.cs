using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neat_Implemtation
{
    public partial class Form1 : Form
    {
        Genome genome = new Genome();
        Random r = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            genome.addConnectionMutation(r);
            genome.addNode(r);
        }
    }
}

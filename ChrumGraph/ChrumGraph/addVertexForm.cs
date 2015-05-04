﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChrumGraph
{
    /// <summary>
    /// A dialog in which the user chooses the desired label for new Vertex
    /// </summary>
    public partial class AddVertexForm : Form
    {
        private Core core;
        /// <summary>
        /// Standard constructor
        /// </summary>
        public AddVertexForm(Core core)
        {
            this.core = core;
            InitializeComponent();
        }

        public string GetNewLabel()
        {
            return labelInput.Text;
        }

        private void addVertexForm_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            if(labelInput.Text == "")
            {
                labelError.Text = "Label cannot be empty";
                return;
            }
            if(core.VerticesDict.ContainsKey(labelInput.Text))
            {
                labelError.Text = "Vertex with this label already exists";
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

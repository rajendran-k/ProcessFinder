using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            label8.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            label10.Visible = false;
            label11.Visible = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label9.Visible = false;
            label8.Visible = true;
            label10.Visible = false;
            label11.Visible = false;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            var processName = textBox1.Text;
            if (!string.IsNullOrEmpty(processName))
            {
                await Task.Run(async () =>
                {
                    var test = new Analysis();
                    var duration = numericUpDown1.Value.ToString();
                    var sampling = numericUpDown2.Value.ToString();
                    var status = await test.SearchProcessNameAsync(processName);

                    if (status)
                    {
                        button1.Enabled = false;
                        button2.Enabled = false;

                        var data = test.GetProcessDetials(int.Parse(duration), int.Parse(sampling) * 1000, processName);
                        if (data != null)
                        {
                            test.DisplayResult(data);
                            label10.Visible = true;
                            textBox2.Text = test.Cpu +"%";
                            textBox3.Text = test.Handle;
                            textBox4.Text = test.PrivateMemory;
                            textBox5.Text = test.VirtualMemory;
                            if(test.MemoryLeak==1)
                            {
                                label11.Visible = true;
                            }
                            button1.Enabled = true;
                            button2.Enabled = true;
                            label8.Visible = false;
                        }
                    }
                    else
                    {
                        button1.Enabled = true;
                        button2.Enabled = true;
                        label8.Visible = false;
                        label9.Visible = true;
                    }
                });
            }

            else
            {
                MessageBox.Show("Enter process by that name");
                label8.Visible = false;
                label9.Visible = true;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label9.Visible = false;
            label11.Visible = false;
        }
    }
}

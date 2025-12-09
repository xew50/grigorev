using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using grigorev2.DB;

namespace grigorev2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private string Pic_Name;
        private List<Table_Motorbike> vsMotorbike = Form1.DB.Table_Motorbike.ToList();

        private void Form2_load(object sender, EventArgs e)
        {

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(
                vsMotorbike.Select(x => x.Brand).Distinct().ToArray()
            );

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void pictureBox1_click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Images|*.bmp;*.jpg;*.jpeg;*.png";

            if (d.ShowDialog() == DialogResult.OK)
            {
                Pic_Name = d.FileName;
                pictureBox1.Image = Image.FromFile(Pic_Name);
            }
        }

        private int Flplus1()
        {
            if (vsMotorbike.Count == 0) return 1;
            return vsMotorbike.Max(x => x.ID) + 1;
        }

        private void add2but_click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Марка и Модель обязательны!");
                return;
            }

            if (!float.TryParse(textBox3.Text, out float price))
            {
                MessageBox.Show("Цена должна быть числом!");
                return;
            }

            if (!int.TryParse(textBox5.Text, out int horsepower))
            {
                MessageBox.Show("ЛС должно быть целым числом!");
                return;
            }

            if (Pic_Name == null || !File.Exists(Pic_Name))
            {
                MessageBox.Show("Выберите изображение!");
                return;
            }


            Table_Motorbike NMotorbike = new Table_Motorbike();
            int newId = Flplus1();

            NMotorbike.ID = newId;
            NMotorbike.Brand = comboBox1.Text;
            NMotorbike.Model = textBox1.Text;
            NMotorbike.Price = price;
            NMotorbike.Horsepower = horsepower;
            NMotorbike.Picture = $"{newId}{Path.GetExtension(Pic_Name)}";


            File.Copy(Pic_Name, $@"Pictures\{NMotorbike.Picture}", true);

            try
            {
                Form1.DB.Table_Motorbike.Add(NMotorbike);
                Form1.DB.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Данные успешно добавлены!");

            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void backbut_click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ghuschyan2rkis.DB;

namespace grigorev3
    
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static Model1 DB = new Model1();

        private void Form1_Load(object sender, EventArgs e)
        {

            tableMotorbikeBindingSource.DataSource = DB.Table_Motorbike.ToList();

            if (DB.Table_Motorbike.Count() == 0)
                return;


            int ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            var moto = DB.Table_Motorbike.Find(ID);
            if (File.Exists($@"Pictures\{moto.Picture}"))
                pictureBox1.Image = Image.FromFile($@"Pictures\{moto.Picture}");
        }

        private void add_click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            this.Hide();
            f.Show();
        }

        private void dell_click(object sender, EventArgs e)
        {
            if (DB.Table_Motorbike.Count() == 0)
            {
                MessageBox.Show("Данные отсутствуют!");
                return;
            }

            int ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            Table_Motorbike CurrentMoto = DB.Table_Motorbike.Find(ID);

            DialogResult res = MessageBox.Show(
                $@"Удалить объект с ID = {CurrentMoto.ID}?",
                "Удаление",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                try
                {
                    DB.Table_Motorbike.Remove(CurrentMoto);
                    DB.SaveChanges();

                    if (File.Exists($@"Pictures\{CurrentMoto.Picture}"))
                        File.Delete($@"Pictures\{CurrentMoto.Picture}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    tableMotorbikeBindingSource.DataSource = DB.Table_Motorbike.ToList();
                    pictureBox1.Image = null;
                }
            }
        }

        private void dataGridView1_click(object sender, EventArgs e)
        {
            if (DB.Table_Motorbike.Count() == 0) return;

            int ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            var moto = DB.Table_Motorbike.Find(ID);

            if (File.Exists($@"Pictures\{moto.Picture}"))
                pictureBox1.Image = Image.FromFile($@"Pictures\{moto.Picture}");
        }
    }
}

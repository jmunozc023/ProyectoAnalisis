using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace parqueo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = DESKTOP-B2NAU1O; database=parqueoU; integrated security = true");
            conexion.Open();
            string cadena = "insert into Parqueo (nombre_parqueo, ubicacion)" + "values ('" + textBox1.Text + "', '" + textBox2.Text + "')";
            SqlCommand comando = new SqlCommand(cadena, conexion);
            comando.ExecuteNonQuery();
            MessageBox.Show("Aprove");
            textBox1.Text = "";
            textBox2.Text = "";
            conexion.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = DESKTOP-B2NAU1O; database=parqueoU; integrated security = true");
            conexion.Open();
            string cadena = "DELETE FROM Parqueo WHERE nombre_parqueo = '" + textBox1.Text + "'";

            SqlCommand comando = new SqlCommand(cadena, conexion);

            int cantidad_borrados = comando.ExecuteNonQuery();
            if (cantidad_borrados == 1)
            {
                MessageBox.Show("El parqueo fue borrado");
            }
            else
            {
                MessageBox.Show("El parqueo no existe");
            }
            textBox1.Text = "";
            textBox2.Text = "";
            conexion.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = DESKTOP-B2NAU1O; database=parqueoU; integrated security = true");
            conexion.Open();

            string cadena = "UPDATE Parqueo SET nombre_parqueo = '" + textBox1.Text + "', " +
                     "ubicacion = '" + textBox2.Text + "' " +
                     "WHERE nombre_parqueo = '" + textBox1.Text + "'";

            SqlCommand comando = new SqlCommand(cadena, conexion);
            int cantidad_modificados = comando.ExecuteNonQuery();
            if (cantidad_modificados == 1)
            {
                MessageBox.Show("Se modificaron los datos");
            }
            else
            {
                MessageBox.Show("No existe un Parqueo con el nombre ingresado");
            }
            conexion.Close();
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("server = DESKTOP-B2NAU1O; database=parqueoU; integrated security = true");
            conexion.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter adaptador = new SqlDataAdapter("select * from parqueo", conexion);
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
            conexion.Close();
        }
    }
}

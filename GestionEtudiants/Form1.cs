using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionEtudiants
{
    public partial class Form1 : Form
    {
        private const string connectionString = "server=localhost;database=postgres;port=5432;username=postgres;password=zili";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEtudiants();
        }
        private void LoadEtudiants()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, nom FROM etudiant";
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection))
                {
                    DataTable etudiantTable = new DataTable();
                    adapter.Fill(etudiantTable);
                    grd.DataSource = etudiantTable;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string nom = txtNom.Text.Trim();
            if (nom != "")
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO etudiant (nom) VALUES (@nom)";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nom", nom);
                        command.ExecuteNonQuery();
                    }
                }
                LoadEtudiants(); // Rafraîchir la DataGridView après l'ajout
                txtNom.Clear(); // Effacer le champ de saisie après l'ajout
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nom d'étudiant valide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using cpts321;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cpts321
{
    public partial class Form1 : Form
    {
        // Creates a new spreadsheat object
        private SpreadSheet spread = new SpreadSheet(50,26);

        public Form1()
        {
            InitializeComponent();
            SpreadSheet();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
         
        }
        private void SpreadSheet()
        {
            // Subscribes all event handlers and buttons
            dataGridView1.CellBeginEdit += CellEdit;
            spread.CellPropertyChanged += OnCellPropertyChanged;
            dataGridView1.CellEndEdit += CellEndEdit;
            button1.Click += new EventHandler(button1_Click);

            // Clears current columns
            dataGridView1.Columns.Clear();

            // Names each column with a letter
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < 26; i++)
            {
                dataGridView1.Columns.Add(letters[i].ToString(), letters[i].ToString());
              
            }
            // Creates 50 rows and numbers them
            dataGridView1.Rows.Add(50);
            for (int i = 0; i < 50; i++)
            {
                var row = dataGridView1.Rows[i];
                row.HeaderCell.Value = (i + 1).ToString();
            }
        }
        

                // Button1 runs demo
        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                int col = rand.Next(2, 25);
                int row = rand.Next(0, 49);

                spread.SetText(row, col, "Test");
            }

            for (int i = 0; i < 50; i++)
            {
                spread.SetText(i, 1, "This is cell B");

            }
        }
        
        // Cell Property Update event
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell selectedCell = sender as Cell;

            if (e.PropertyName == "Val" && selectedCell != null)
            {
                dataGridView1.Rows[selectedCell.RowIndex].Cells[selectedCell.ColumnIndex].Value = selectedCell.Value;
            }
        }

        // Handles initial cell edit
        void CellEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            Cell spreadCell = spread.GetCell(row, col);

            dataGridView1.Rows[row].Cells[col].Value = spreadCell.text;
        }

        // Ends cell edit
        void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            string input;

            Cell spreadCell = spread.GetCell(row, col);

            try
            {
                input = dataGridView1.Rows[row].Cells[col].Value.ToString();
            }
            catch (NullReferenceException)
            {
                input = "null";
            }

            spreadCell.text = input;
            dataGridView1.Rows[row].Cells[col].Value = spreadCell.Value;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

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
using SpreadEngine;
using System.IO;

namespace cpts321
{
    public partial class Form1 : Form
    {
        // Intializes new spreadsheet
        private SpreadSheet spread = new SpreadSheet(50, 26);
        private ExpressionTree eTree = new ExpressionTree();
        public UndoRedoClass UnRedo = new UndoRedoClass();
        Dictionary<string, int> letterToNum = new Dictionary<string, int>();


        public Form1()
        {
            InitializeComponent();
        }

 
        private void Form1_Load(object sender, EventArgs e)
        {
            spread.CellPropertyChanged += OnCellPropertyChanged;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            button1.Click += button1_Click;
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            changeColorToolStripMenuItem1.Click += chooseBackgroundColorToolStripMenuItem_Click;
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            CreateDict();


            dataGridView1.Columns.Clear();

            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Assigns letters to each column
            for (int i = 0; i <= 25; i++)
                dataGridView1.Columns.Add(letters[i].ToString(), letters[i].ToString());
            dataGridView1.Rows.Add(50); 
            
            // Numbers each row
            for (int i = 0; i < 50; i++) 
            {
                var row = dataGridView1.Rows[i];
                row.HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell selectedCell = sender as Cell;

            if (e.PropertyName == "Value" && selectedCell != null)
            {
                dataGridView1.Rows[selectedCell.RowIndex].Cells[selectedCell.ColumnIndex].Value = selectedCell.Value;
            }
        }

        void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex, column = e.ColumnIndex;

            Cell spreadsheetCell = spread.GetCell(row, column);

            dataGridView1.Rows[row].Cells[column].Value = spreadsheetCell.text;
        }

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex, column = e.ColumnIndex;
            string text;
            double num;

            UndoRedo[] undos = new UndoRedo[1];

            // String array used for variable assignment for column letters
            string[]  alpha = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            Cell spreadsheetCell = spread.GetCell(row, column);
            undos[0] = new RestoreText(spreadsheetCell.text, spreadsheetCell.Name);

            // Checks if cell input is a number then assigns new variable
            if (double.TryParse(dataGridView1.Rows[row].Cells[column].Value.ToString(), out num))
                {
                string var = alpha[column];
                var += (row+1).ToString();
                eTree.SetVar(var, num);
                try
                {
                    text = dataGridView1.Rows[row].Cells[column].Value.ToString();
                }
                catch (NullReferenceException)
                {
                    text = "";
                }

                spreadsheetCell.text = text;



            }
            else
            {
                string cellCont = dataGridView1.Rows[row].Cells[column].Value.ToString();
                string test = cellCont.Substring(0, 1);

                if (test == "=")
                {
                    string var = alpha[column];
                    var += (row + 1).ToString();
                    cellCont = cellCont.Remove(0, 1);
                    eTree = new ExpressionTree(cellCont);
                    double result = eTree.Eval();
                    spreadsheetCell.text = result.ToString();

                }
            }

            multiCmds tmpcmd = new multiCmds(undos, spreadsheetCell.text);
            UnRedo.AddUndos(tmpcmd);
            UnRedo.AddUndoCellLocation(spreadsheetCell.Name);

            dataGridView1.Rows[row].Cells[column].Value = spreadsheetCell.Value;

            refreshUndoRedoButtons();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            
            for (int i = 0; i < 50; i++)
            {
                int row = rand.Next(0, 49);
                int column = rand.Next(2, 25); 

                spread.array[row, column].text = "Hello World!";
            }

            for (int i = 0; i < 50; i++)
            {
                spread.SetText(i, 1, "Cell B" + (i + 1).ToString());
                int row = i + 1;
                dataGridView1.Rows[i].Cells[1].Value = spread.GetCell("B" + row).Value;
            }

         
        }

        private void chooseBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedColor = 0;

            List<UndoRedo> undos = new List<UndoRedo>();

            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color.ToArgb();

                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    Cell spreadsheetCell = spread.GetCell(cell.RowIndex, cell.ColumnIndex);

                    RestoreBGColor tempBGClass = new RestoreBGColor((int)spreadsheetCell.BGColor, spreadsheetCell.Name);

                    undos.Add(tempBGClass);

                    spreadsheetCell.BGColor = (uint)selectedColor;
                }

                multiCmds tempCmd = new multiCmds(undos, "changing cell background color");

                UnRedo.AddUndos(tempCmd);

                refreshUndoRedoButtons();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnRedo.Undo(spread);
            string cell = UnRedo.GetUndoCellLocation();

            dataGridView1.Rows[letterToNum[cell.Substring(0,1)]].Cells[Int32.Parse(cell.Substring(1,1))].Value = spread.GetCell(cell).Value;
            dataGridView1.Refresh();
            refreshUndoRedoButtons();

        }

     
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnRedo.Redo(spread);
            dataGridView1.Rows[0].Cells[0].Value = spread.GetCell("A1").Value;
            refreshUndoRedoButtons();
        }

        private void refreshUndoRedoButtons()
        {
            ToolStripItemCollection tempColl = menuStrip1.Items;
            ToolStripMenuItem tempMenus = tempColl[0] as ToolStripMenuItem;


            for (int i = 0; i < tempMenus.DropDownItems.Count; i++)
            {
                if (tempMenus.DropDownItems[i].Text[0] == 'U')
                {
                    bool undo = UnRedo.CanUndo;
                    tempMenus.DropDownItems[i].Enabled = undo;
                    tempMenus.DropDownItems[i].Text = "Undo " + UnRedo.UndoNext;
                }
                else if (tempMenus.DropDownItems[i].Text[0] == 'R')
                {
                    bool redo = UnRedo.CanRedo;
                    tempMenus.DropDownItems[i].Enabled = redo;
                    tempMenus.DropDownItems[i].Text = "Redo " + UnRedo.RedoNext;
                }
            }
        }

        private void CreateDict()
        {
            letterToNum.Add("A", 1);
            letterToNum.Add("B", 2);
            letterToNum.Add("C", 3);
            letterToNum.Add("D", 4);
            letterToNum.Add("E", 5);
            letterToNum.Add("F", 6);
            letterToNum.Add("G", 7);
            letterToNum.Add("H", 8);
            letterToNum.Add("I", 9);
            letterToNum.Add("J", 10);
            letterToNum.Add("K", 11);
            letterToNum.Add("L", 12);
            letterToNum.Add("M", 13);
            letterToNum.Add("N", 14);
            letterToNum.Add("O", 15);
            letterToNum.Add("P", 16);
            letterToNum.Add("Q", 17);
            letterToNum.Add("R", 18);
            letterToNum.Add("S", 19);
            letterToNum.Add("T", 20);
            letterToNum.Add("U", 21);
            letterToNum.Add("V", 22);
            letterToNum.Add("W", 23);
            letterToNum.Add("X", 24);
            letterToNum.Add("Y", 25);
            letterToNum.Add("Z", 26);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creates new SaveFileDialog
            var saveFile = new SaveFileDialog();
            saveFile.Filter = "XML files (*.xml)|*.xml";

            // Checks if user input was valid
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Stream output = new FileStream(saveFile.FileName, FileMode.Create, FileAccess.Write); 
                spread.Save(output);

                output.Dispose();
            }
        }

   
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creates new OpenFileDialog
            var openFile = new OpenFileDialog();
            openFile.Filter = "XML files (*.xml)|*.xml";

            // Checks if user input was valid
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Clear();

                Stream infile = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                spread.Load(infile);
                infile.Dispose();
                UnRedo.Clear(); 
            }

            refreshUndoRedoButtons();
        }

        // Clears spreadsheet
        public void Clear()
        {
            for (int i = 0; i < spread.GetRowCount; i++)
            {
                for (int j = 0; j < spread.GetColumnCount; j++)
                {
                    if (spread.array[i, j].text != "" || spread.array[i, j].Value != "" || spread.array[i, j].BGColor != 4294967295) 
                    {
                        spread.array[i, j].Clear();
                    }
                }
            }
        }
        private void chooseColorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

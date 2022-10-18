using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SpreadEngine;


namespace cpts321
{
    public class SpreadSheet
    {
        // Inherits from cell to create new initial cells
        private class CreateCell : Cell
        {
            public CreateCell(int rIndex, int cIndex) : base(rIndex, cIndex) { }

            public void SetVal(string value)
            {
                cellVal = value;
            }
        }

        // Event handler for cell changes
        public event PropertyChangedEventHandler CellPropertyChanged;
        public UndoRedoClass UndoRedo = new UndoRedoClass();


        private void setEmpty(CreateCell m_Cell, Cell cell)
        {
            m_Cell.SetVal("");
            CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

        //Conditonals to handle when cells are edited
        private void setExp(CreateCell nodeCell, Cell cell, ref bool boolFlag)
        {
            ExpressionTree exptree = new ExpressionTree(nodeCell.text.Substring(1));
            string[] variables = exptree.GetAllVariables();


            foreach (string variableName in variables)
            {
                // checks for bad reference
                if (GetCell(variableName) == null)
                {
                    nodeCell.SetVal("!(Bad Reference)");
                    CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                    boolFlag = true;
                    break;
                }

                //SetExpressionVariable
                Cell variableCell = GetCell(variableName);
                double value;

                if (string.IsNullOrEmpty(variableCell.Value))
                    exptree.SetVar(variableCell.Name, 0);

                else if (!double.TryParse(variableCell.Value, out value))
                    exptree.SetVar(variableName, 0);

                else
                    exptree.SetVar(variableName, value);

                if (variableName == nodeCell.Name)
                {
                    nodeCell.SetVal("!(Self Reference)");
                    CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                    boolFlag = true;
                    break;
                }
            }
            if (boolFlag) { return; }

            foreach (string varName in variables)
            {
                if (varName == nodeCell.Name) // directly same
                {
                    nodeCell.SetVal("!(Circular Refer)");
                    CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                    boolFlag = true;
                    break;
                }

                // Checks dic for key
                if (!dependDict.ContainsKey(nodeCell.Name)) 
                {
                    continue;
                }

                string currCell = nodeCell.Name;

                for (int i = 0; i < dependDict.Count; i++)  
                {
                    foreach (string dependentCell in dependDict[currCell]) 
                    {
                        if (varName == dependentCell)
                        {
                            nodeCell.SetVal("!(Circular Reference)");
                            CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                            boolFlag = true;
                            break;
                        }
                        if (!dependDict.ContainsKey(dependentCell))
                        {
                            continue;
                        }
                        currCell = dependentCell;
                    }
                }
            }

            if (boolFlag) { return; }

            nodeCell.SetVal(exptree.Eval().ToString());
            CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

        private void EvaluateCell(string exp)
        {
            EvaluateCell(GetCell(exp));
        }

        private void setText(CreateCell m_Cell, Cell cell)
        {
            m_Cell.SetVal(m_Cell.text);
            CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
        }

  
        private void EvaluateCell(Cell cell)
        {
            bool flag = false;
            CreateCell m_Cell = cell as CreateCell;
        

            if (string.IsNullOrEmpty(m_Cell.text)) { setEmpty(m_Cell, cell); }
       

            else if (m_Cell.text[0] == '=' && m_Cell.text.Length >= 2)
            {
                setExp(m_Cell, cell, ref flag);
                if (flag) { return; }
            }

            else { setText(m_Cell, cell); }

            if (dependDict.ContainsKey(m_Cell.Name))
            {
                foreach (var depCell in dependDict[m_Cell.Name])
                {
                    EvaluateCell(depCell);
                }
            }
        }
    

        // Cell array to handle spreadsheet cells
        public Cell[,] array;

        // Constructor for spreadsheet that creates a new spreadsheet base on x and y values
        public SpreadSheet(int rowIndex, int columnIndex)
        {
            array = new Cell[rowIndex, columnIndex];

            for (int x = 0; x < rowIndex; x++)
            {
                for (int y = 0; y < columnIndex; y++)

                {
                    array[x, y] = new CreateCell(x, y);
                    array[x, y].PropertyChanged += OnPropChanged;
                }
            }
        }

        // Handles changes to cell property
        public void OnPropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "text")
            {
                CreateCell tempCell = sender as CreateCell;
                EvaluateCell(sender as Cell);

                if(tempCell.text != "" && tempCell.text[0] == '=')
                {
                    ExpressionTree exp = new ExpressionTree(tempCell.text.Substring(1));
                    MakeDep(tempCell.Name, exp.GetAllVariables());
                }

                EvaluateCell(sender as Cell);
            }
            else if (e.PropertyName == "BGColor")
            {
                CellPropertyChanged(sender, new PropertyChangedEventArgs("BGColor"));
            }
        }
        private Dictionary<string, HashSet<string>> dependDict;

        private void RemoveDep(string cellName)
        {
            List<string> dependenciesList = new List<string>();

            foreach (string key in dependDict.Keys)
            {
                if (dependDict[key].Contains(cellName))
                    dependenciesList.Add(key);
            }

            foreach (string key in dependenciesList)
            {
                HashSet<string> hashset = dependDict[key];
                if (hashset.Contains(cellName))
                    hashset.Remove(cellName);
            }
        }


        private void MakeDep(string cellName, string[] variablesUsed)
        {
            for (int i = 0; i < variablesUsed.Length; i++)
            {
                if (dependDict.ContainsKey(variablesUsed[i]) == false)
                {
                    dependDict[variablesUsed[i]] = new HashSet<string>();
                }
                dependDict[variablesUsed[i]].Add(cellName);
            }
        }

        // Getter for individual cell
        public Cell GetCell(int rowIndex, int columnIndex)
        {
            return array[rowIndex, columnIndex];
        }

        public Cell GetCell(string exp)
        {
            char letter = exp[0];
            short number;
            Cell result;

            if (char.IsLetter(letter) == false) { return null; }

            if (short.TryParse(exp.Substring(1), out number) == false) { return null; }

            try { result = GetCell(number - 1, letter - 'A'); }
            catch { return null; }

            return result;
        }
        // Getters for cell row count and column count
        public int GetRowCount { get { return array.GetLength(0); } }

        public int GetColumnCount { get { return array.GetLength(1); } }

        // Sets the text in a cell
        public void SetText(int rowIndex, int columnIndex, string input)
        {
            array[rowIndex, columnIndex].text = input;

            if (columnIndex == 1)
            {
                array[rowIndex, 0].text = input;
            }
        }
        public void Save(Stream outFile)
        {
            XmlWriterSettings setting = new XmlWriterSettings();

            setting.Indent = true;

            XmlWriter xml = XmlWriter.Create(outFile, setting);

            xml.WriteStartElement("spreadsheet");

            // Cell formatting
            foreach (Cell cell in array)
            {
                if (cell.text != "" || cell.Value != "" || cell.BGColor != 4294967295)
                {
                    xml.WriteStartElement("cell name");

                    xml.WriteAttributeString("name", cell.Name);

                    xml.WriteElementString("bgcolor", cell.BGColor.ToString("x8"));

                    xml.WriteElementString("text", cell.text.ToString());

                    xml.WriteEndElement();
                }
            }
            xml.WriteEndElement();
            xml.Close();
        }

     
        public void Load(Stream infile)
        {
            XDocument inFile = XDocument.Load(infile);

            foreach (XElement textFile in inFile.Root.Elements("cell"))
            {
                Cell cell = this.GetCell(textFile.Attribute("name").Value);

                if (textFile.Element("text") != null)
                {
                    cell.text = textFile.Element("text").Value.ToString();
                }

                if (textFile.Element("bgcolor") != null)
                {
                    cell.BGColor = uint.Parse(textFile.Element("bgcolor").Value, System.Globalization.NumberStyles.HexNumber);  
                }
            }
        }
    }
}


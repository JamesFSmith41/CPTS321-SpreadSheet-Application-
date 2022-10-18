using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpts321
{


    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        // Variables for each cell
        private readonly int rowIndex;
        private readonly int columnIndex;
        protected string cellText = "";
        protected string cellVal = "";
        private readonly string name = "";
        private int backGroundColor = -1;
        // Cell constructor
        public Cell(int rIndex, int cIndex)
        {
            rowIndex = rIndex;
            columnIndex = cIndex;
            name += Convert.ToChar('A' + ColumnIndex);
            name += (RowIndex + 1).ToString();
        }

        // Getters and setters for cell

        public string Name { get { return name; } }

        public int ColumnIndex { get { return columnIndex; } }

        public int RowIndex { get { return rowIndex; } }

        public string text
        {
            get { return cellText; }

            set
            {
                if(cellText == value)
                {
                    return;
                }
                    cellText = value;
             
                PropertyChanged(this, new PropertyChangedEventArgs("text"));
                
            }
        }


        public string Value { get { return cellVal; } }

        public uint BGColor
        {
            get { return (uint)backGroundColor; }
            set
            {
                if (backGroundColor == value) { return; }
                backGroundColor = (int)value;
                PropertyChanged(this, new PropertyChangedEventArgs("BGColor"));
            }
        }

        public void Clear()
        {
            cellText = "";
            BGColor = 0;
        }
    }
}


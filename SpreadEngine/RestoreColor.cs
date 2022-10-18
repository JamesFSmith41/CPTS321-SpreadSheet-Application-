using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpts321;

namespace SpreadEngine
{
    public class RestoreBGColor : UndoRedo
    {
        private int m_Color;
        private string m_Name;


        public RestoreBGColor(int cellColor, string cellName)
        {
            m_Color = cellColor;
            m_Name = cellName;
        }


        public UndoRedo Exec(SpreadSheet sheet)
        {
            Cell cell = sheet.GetCell(m_Name);

            int old = (int)cell.BGColor;

            cell.BGColor = (uint)m_Color;

            RestoreBGColor oldBGClass = new RestoreBGColor(old, m_Name);

            return oldBGClass;
        }
    }
}

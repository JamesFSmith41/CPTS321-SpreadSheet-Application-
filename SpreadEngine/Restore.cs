using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpts321;

namespace SpreadEngine
{
    class Restore
    {
    }
    public class RestoreText : UndoRedo
    {
        private string nodeText, nodeName;

        public RestoreText(string cellText, string cellName)
        {
            nodeText = cellText;
            nodeName = cellName;
        }

        
        public UndoRedo Exec(SpreadSheet spread)
        {
            string old = spread.GetCell(nodeName).text;
            spread.GetCell(nodeName).text = nodeText;
            RestoreText oldTextClass = new RestoreText(old, nodeName);

            return oldTextClass;
        }

       
    }
}

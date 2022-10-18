using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpts321;

namespace SpreadEngine
{
    public interface UndoRedo
    {
        UndoRedo Exec(SpreadSheet ssheet);
    }

    public class multiCmds
    {
        private UndoRedo[] cmds;
        public string name;
        public string[] cellLoc;


        
        public multiCmds() { }

        public multiCmds(UndoRedo[] cmds, string name)
        {
            this.cmds = cmds;
            this.name = name;
        }

        
        public multiCmds(List<UndoRedo> cmds, string name)
        {
            this.cmds = cmds.ToArray(); this.name = name;
        }

  
        public multiCmds Exec(SpreadSheet sheet)
        {
            List<UndoRedo> cmd_list = new List<UndoRedo>();

            foreach (UndoRedo cmd in cmds)
            {
                UndoRedo doCmd = cmd.Exec(sheet);

                cmd_list.Add(doCmd);
            }

            multiCmds mulcmds = new multiCmds(cmd_list.ToArray(), name);

            return mulcmds;
        }

       
    }

    public class UndoRedoClass
    {
        private Stack<multiCmds> undos = new Stack<multiCmds>();

        private Stack<multiCmds> redos = new Stack<multiCmds>();

        private List<string> undoCells = new List<string>();
        private List<string> redoCells = new List<string>();


        public string GetRow(multiCmds cmds)
        {
            // string row = cmds[0];

            string row = "";

            return row;
        }
        public bool CanUndo
        {
            get
            {
                return undos.Count != 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return redos.Count != 0;
            }
        }

       
        public string UndoNext
        {
            get
            {
                if (CanUndo)   
                {
                    return undos.Peek().name;
                }
                return "";
            }
        }


        public string RedoNext
        {
            get
            {
                if (CanRedo)   
                {
                    return redos.Peek().name;
                }
                return "";
            }
        }


        public void AddUndos(multiCmds undo)
        {
            undos.Push(undo);
            redos.Clear();
        }

        public void AddUndoCellLocation(string cell)
        {
            undoCells.Add(cell);
        }

        public string GetUndoCellLocation()
        {
            string location = undoCells.Last();
            undoCells.Remove(location);
            return location;
        }

        public void AddRedoCellLocation(string cell)
        {
            redoCells.Add(cell);
        }

        public string GetRedoCellLocation()
        {
            string location = redoCells.Last();
            redoCells.Remove(location);
            return location;
        }
        public void Undo(SpreadSheet ssheet)
        {
            redos.Push(undos.Pop().Exec(ssheet));

          
        }



     
        public void Redo(SpreadSheet ssheet)
        {
            undos.Push(redos.Pop().Exec(ssheet));
        }

   
        public void Clear()
        {
            undos.Clear(); redos.Clear();
        }
    }
}

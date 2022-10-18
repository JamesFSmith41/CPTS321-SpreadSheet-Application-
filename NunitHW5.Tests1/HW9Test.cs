using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using cpts321;
using SpreadEngine;
using System.Windows.Forms;
using System.IO;

namespace NunitHW5.Tests1
{
    class HW9Test
    {
        [Test]
        public void TestSave1()
        {

            SpreadSheet spread = new SpreadSheet(50, 26);
            spread.SetText(1, 1, "45");
            var saveFile = new SaveFileDialog();
            saveFile.Filter = "XML files (*.xml)|*.xml";

            // Checks if user input was valid
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Stream output = new FileStream(saveFile.FileName, FileMode.Create, FileAccess.Write);
                spread.Save(output);
                Assert.That(output, Is.EqualTo("<spreadsheet> < cell name = B2 >< bgcolor > ffffffff </ bgcolor >< text > 45 </ text ></ cell ></ spreadsheet > "), "ould equal 3");
                output.Dispose();
            }
            
        }

        [Test]
        public void TestLoad1()
        {

            SpreadSheet spread = new SpreadSheet(50, 26);

            // Creates new OpenFileDialog
            var openFile = new OpenFileDialog();
            openFile.Filter = "XML files (*.xml)|*.xml";

            // Checks if user input was valid
            if (openFile.ShowDialog() == DialogResult.OK)
            {
            

                Stream infile = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                spread.Load(infile);
                infile.Dispose();
          
            }

        }
    }
}

// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using HW4;
using SpreadEngine;


// SpreadSheet_James_Smith
namespace HW4 { }



    
    [TestFixture]
    public class TestClass
    {
            // Sets text of a cell
        [Test]
        public void TestAdd1()
        {
        SpreadSheet spread = new SpreadSheet(2, 2);
        string test = "Val";
        spread.SetText(1, 1, test);

            Assert.That(spread.GetCell(1,1).text.ToString(), Is.EqualTo(test), "cell text wrong");
        }

                // Sets text of a cell

        [Test]
        public void TestAdd2()
        {


        SpreadSheet spread = new SpreadSheet(100, 100);
        string test = "Val";
        spread.SetText(75, 75, test);

        Assert.That(spread.GetCell(75, 75).text.ToString(), Is.EqualTo(test), "cell text wrong");
    }

    // Sets text of a cell

    [Test]
        public void TestAdd3()
        {
        


        SpreadSheet spread = new SpreadSheet(10, 10);
        string test = "Val";
        spread.SetText(9, 9, test);

        Assert.That(spread.GetCell(9, 9).Value.ToString(), Is.EqualTo(test), "cell text wrong");
    }

    // Sets text of a cell

    [Test]
        public void TestAddRow2()
        {

        SpreadSheet spread = new SpreadSheet(100, 100);
        string test = "Val";
        spread.SetText(1, 1, test);

        Assert.That(spread.GetCell(1, 1).text.ToString(), Is.EqualTo(test), "cell text wrong");
    }
 }


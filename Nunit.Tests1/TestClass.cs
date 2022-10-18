// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using cpts321;

namespace NunitHW6.Tests1
{
    [TestFixture]
    public class TestClass
    {
        // Tests addition 1
        [Test]
        public void TestTreeAddition1()
        {

            ExpressionTree eTree = new ExpressionTree("1+2");
            Assert.That(eTree.Evaluate(), Is.EqualTo(3), "Should equal 3");
        }
        // Tests addition2 
        [Test]
        public void TestTreeAddition2()
        {

            ExpressionTree eTree = new ExpressionTree("1+2+3");
            Assert.That(eTree.Evaluate(), Is.EqualTo(6), "Should equal 6");
        }
        // Tests addition 3
        [Test]
        public void TestTreeAddition3()
        {

            ExpressionTree eTree = new ExpressionTree("1+1+1+1+1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(5), "Should equal 5");
        }
        // Tests subtraction 1
        [Test]
        public void TestTreeSubtraction1()
        {

            ExpressionTree eTree = new ExpressionTree("2-1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(1), "Should equal 1");
        }
        // Tests subtraction 2

        [Test]
        public void TestTreeSubtraction2()
        {

            ExpressionTree eTree = new ExpressionTree("2-3");
            Assert.That(eTree.Evaluate(), Is.EqualTo(-1), "Should equal -1");
        }
        // Tests subtraction 3

        [Test]
        public void TestTreeSubtraction3()
        {

            ExpressionTree eTree = new ExpressionTree("4-3");
            Assert.That(eTree.Evaluate(), Is.EqualTo(1), "Should equal 1");
        }
        // Tests multiplication 1 

        [Test]
        public void TestTreeMultiplication1()
        {

            ExpressionTree eTree = new ExpressionTree("1*1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(1), "Should equal 1");
        }
        // Tests multiplication  2

        [Test]
        public void TestTreeMultiplication2()
        {

            ExpressionTree eTree = new ExpressionTree("5*5");
            Assert.That(eTree.Evaluate(), Is.EqualTo(25), "Should equal 25");
        }
        // Tests multiplication  3

        [Test]
        public void TestTreeMultiplication3()
        {

            ExpressionTree eTree = new ExpressionTree("10*4");
            Assert.That(eTree.Evaluate(), Is.EqualTo(40), "Should equal 40");
        }
        // Tests divison  1

        [Test]
        public void TestTreeDivison1()
        {

            ExpressionTree eTree = new ExpressionTree("10/5");
            Assert.That(eTree.Evaluate(), Is.EqualTo(2), "Should equal 2");
        }
        // Tests divison  2

        [Test]
        public void TestTreeDivison2()
        {

            ExpressionTree eTree = new ExpressionTree("1/1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(1), "Should equal 1");
        }
        // Tests divison  3

        [Test]
        public void TestTreeDivison3()
        {

            ExpressionTree eTree = new ExpressionTree("4/2");
            Assert.That(eTree.Evaluate(), Is.EqualTo(2), "Should equal 2");
        }
        // Tests multiple operators in an expression 1
        [Test]
        public void TestTreeExpressions1()
        {

            ExpressionTree eTree = new ExpressionTree("1+3-1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(3), "Should equal 3");
        }
        // Tests multiple operators in an expression 2

        [Test]
        public void TestTreeExpression2()
        {

            ExpressionTree eTree = new ExpressionTree("1+2-1");
            Assert.That(eTree.Evaluate(), Is.EqualTo(2), "Should equal 3");
        }
        // Tests multiple operators in an expression 3

        [Test]
        public void TestTreeExpression3()
        {

            ExpressionTree eTree = new ExpressionTree("4/2*2");
            Assert.That(eTree.Evaluate(), Is.EqualTo(4), "Should equal 2");
        }
    }
}

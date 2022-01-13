using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticAlgorithmNS;

namespace GeneticAlgorithmTests {
  [TestClass]
  public class GeneticAlgorithmTest {

    GeneticSettings geneticSettings;
    GeneticAlgorithm geneticAlgorithm;

    [TestInitialize]
    public void TestInitialize() {

      // Lav først genetic settings

      geneticSettings = new GeneticSettings()

      geneticAlgorithm = new GeneticAlgorithm();
    }


    [TestMethod]
    public void TestMethod1() {
    }


  }
}

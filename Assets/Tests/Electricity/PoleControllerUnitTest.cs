using System.Linq;
using Buildings.Interfaces;
using Electricity;
using Electricity.Controllers;
using Electricity.Interfaces;
using Moq;
using NUnit.Framework;
using Tests.Helpers;
using UnityEngine;
using Zenject;

namespace Tests.Electricity
{
	[TestFixture]
	public class PoleControllerUnitTest : ZenjectUnitTestFixture
	{
		private IElectricalPoleModel     _model;
		private ElectricalPoleController _poleController;

		public override void Setup()
		{
			base.Setup();
			_model          = ElectricityTestHelper.GetPoleModelMock().Object;
			_poleController = new ElectricalPoleController(Vector3.zero, _model);
		}

		public override void Teardown()
		{
			base.Teardown();
			_poleController = null;
		}

		[Test]
		public void CreateControllerTest()
		{
			Assert.NotNull(_poleController);
		}

		[Test]
		public void AddGeneratorTest()
		{
			_poleController.AddGenerator(new Mock<IGeneratorController>().Object);
			Assert.AreEqual(1, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void AddDoubleGeneratorTest()
		{
			var generatorController = new Mock<IGeneratorController>().Object;
			_poleController.AddGenerator(generatorController);
			_poleController.AddGenerator(generatorController);
			Assert.AreEqual(1, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void AddTwoGeneratorsTest()
		{
			_poleController.AddGenerator(new Mock<IGeneratorController>().Object);
			_poleController.AddGenerator(new Mock<IGeneratorController>().Object);
			Assert.AreEqual(2, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void AddArrayOfGeneratorsTest()
		{
			_poleController.AddGenerators(new[]
			                              {
				                              new Mock<IGeneratorController>().Object,
				                              new Mock<IGeneratorController>().Object
			                              }.ToList());
			Assert.AreEqual(2, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void AddArrayOfGeneratorsWithDoubleTest()
		{
			var first = new Mock<IGeneratorController>().Object;
			_poleController.AddGenerators(new[]
			                              {
				                              first,
				                              first
			                              }.ToList());
			Assert.AreEqual(1, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void RemoveGeneratorTest()
		{
			var first = new Mock<IGeneratorController>().Object;
			_poleController.AddGenerator(first);
			_poleController.RemoveGenerator(first);
			Assert.AreEqual(0, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void RemoveArrayOfGeneratorsTest()
		{
			var first  = new Mock<IGeneratorController>().Object;
			var second = new Mock<IGeneratorController>().Object;
			_poleController.AddGenerators(new[]
			                              {
				                              first,
				                              second
			                              }.ToList());
			_poleController.RemoveGenerators(new[]
			                                 {
				                                 first,
				                                 second
			                                 }.ToList());
			Assert.AreEqual(0, _poleController.NearlyGenerators.Count);
		}

		[Test]
		public void AddPoleTest()
		{
			_poleController.AddPole(new Mock<IElectricalPoleController>().Object);
			Assert.AreEqual(1, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void AddDoublePoleTest()
		{
			var first = new Mock<IElectricalPoleController>().Object;
			_poleController.AddPole(first);
			_poleController.AddPole(first);
			Assert.AreEqual(1, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void AddTwoPolesTest()
		{
			_poleController.AddPole(new Mock<IElectricalPoleController>().Object);
			_poleController.AddPole(new Mock<IElectricalPoleController>().Object);
			Assert.AreEqual(2, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void AddArrayOfPolesTest()
		{
			_poleController.AddPoles(new[]
			                        {
				                        new Mock<IElectricalPoleController>().Object,
				                        new Mock<IElectricalPoleController>().Object
			                        });
			Assert.AreEqual(2, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void AddArrayOfPolesWithDoubleTest()
		{
			var first = new Mock<IElectricalPoleController>().Object;
			_poleController.AddPoles(new[]
			                        {
				                        first,
				                        first
			                        });
			Assert.AreEqual(1, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void RemovePoleTest()
		{
			var first = new Mock<IElectricalPoleController>().Object;
			_poleController.AddPole(first);
			_poleController.RemovePole(first);
			Assert.AreEqual(0, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void RemoveArrayOfPolesTest()
		{
			var first  = new Mock<IElectricalPoleController>().Object;
			var second = new Mock<IElectricalPoleController>().Object;
			_poleController.AddPoles(new[]
			                        {
				                        first,
				                        second
			                        });
			_poleController.RemovePoles(new[]
			                           {
				                           first,
				                           second
			                           });
			Assert.AreEqual(0, _poleController.NearlyPoles.Count);
		}

		[Test]
		public void SetNetTest()
		{
			var net = new Mock<IElectricityNet>().Object;
			_poleController.SetNet(net);
			Assert.AreEqual(net, _poleController.Net);
		}

		[Test]
		public void SetNullNetTest()
		{
			var net = new Mock<IElectricityNet>().Object;
			_poleController.SetNet(net);
			_poleController.SetNet(null);
			Assert.IsNull(_poleController.Net);
		}

		[Test]
		public void AddBuildingTest()
		{
			_poleController.AddBuilding(new Mock<IElectricalBuildingController>().Object);
			Assert.AreEqual(1, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void AddDoubleBuildingTest()
		{
			var first = new Mock<IElectricalBuildingController>().Object;
			_poleController.AddBuilding(first);
			_poleController.AddBuilding(first);
			Assert.AreEqual(1, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void AddTwoBuildingsTest()
		{
			_poleController.AddBuilding(new Mock<IElectricalBuildingController>().Object);
			_poleController.AddBuilding(new Mock<IElectricalBuildingController>().Object);
			Assert.AreEqual(2, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void AddArrayOfBuildingsTest()
		{
			_poleController.AddBuildings(new[]
			                            {
				                            new Mock<IElectricalBuildingController>().Object,
				                            new Mock<IElectricalBuildingController>().Object
			                            }.ToList());
			Assert.AreEqual(2, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void AddArrayOfBuildingsWithDoubleTest()
		{
			var first = new Mock<IElectricalBuildingController>().Object;
			_poleController.AddBuildings(new[]
			                            {
				                            first,
				                            first
			                            }.ToList());
			Assert.AreEqual(1, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void RemoveBuildingTest()
		{
			var first = new Mock<IElectricalBuildingController>().Object;
			_poleController.AddBuilding(first);
			_poleController.RemoveBuilding(first);
			Assert.AreEqual(0, _poleController.NearlyBuildings.Count);
		}

		[Test]
		public void RemoveArrayOfBuildingsTest()
		{
			var first  = new Mock<IElectricalBuildingController>().Object;
			var second = new Mock<IElectricalBuildingController>().Object;
			_poleController.AddBuildings(new[]
			                            {
				                            first,
				                            second
			                            }.ToList());
			_poleController.RemoveBuildings(new[]
			                               {
				                               first,
				                               second
			                               }.ToList());
			Assert.AreEqual(0, _poleController.NearlyBuildings.Count);
		}
	}
}
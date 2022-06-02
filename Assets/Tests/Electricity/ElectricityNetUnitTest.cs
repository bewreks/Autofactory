using System.Collections.Generic;
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
	public class ElectricityNetUnitTest : ZenjectUnitTestFixture
	{
		private ElectricityNet _net;

		public override void Setup()
		{
			base.Setup();

			_net = new ElectricityNet();
		}

		public override void Teardown()
		{
			base.Teardown();

			_net.Dispose();
			_net = null;
		}

		[Test]
		public void SimpleInitTest()
		{
			Assert.IsTrue(_net.StopPowerUpdates_Get);
			_net.Initialize(10);
			_net.TestNet(0, 0, 0, 0);
			Assert.AreEqual(10, _net.ID);
			Assert.IsFalse(_net.StopPowerUpdates_Get);
		}

		[Test]
		public void FullInitTest()
		{
			var firstPole  = new Mock<IElectricalPoleController>();
			var secondPole = new Mock<IElectricalPoleController>();
			var generator  = new Mock<IGeneratorController>();
			var building   = new Mock<IElectricalBuildingController>();
			generator.SetupGet(controller => controller.ActualPower.Value).Returns((generator.Object, 10f));

			firstPole.SetupGet(controller => controller.NearlyPoles)
			         .Returns(new List<IElectricalPoleController>(new[] { secondPole.Object }));
			firstPole.SetupGet(controller => controller.NearlyGenerators)
			         .Returns(new List<IGeneratorController>(new[] { generator.Object }));
			firstPole.SetupGet(controller => controller.NearlyBuildings)
			         .Returns(new List<IElectricalBuildingController>());
			firstPole.Setup(controller => controller.SetNet(It.IsAny<ElectricityNet>())).Verifiable();

			secondPole.SetupGet(controller => controller.NearlyPoles)
			          .Returns(new List<IElectricalPoleController>(new[] { firstPole.Object }));
			secondPole.SetupGet(controller => controller.NearlyBuildings)
			          .Returns(new List<IElectricalBuildingController>(new[] { building.Object }));
			secondPole.SetupGet(controller => controller.NearlyGenerators)
			          .Returns(new List<IGeneratorController>(new[] { generator.Object }));
			secondPole.Setup(controller => controller.SetNet(It.IsAny<ElectricityNet>())).Verifiable();

			generator.Setup(controller => controller.AddNet(It.IsAny<ElectricityNet>())).Verifiable();
			building.Setup(controller => controller.AddNet(It.IsAny<ElectricityNet>())).Verifiable();

			Assert.IsTrue(_net.StopPowerUpdates_Get);
			_net.Initialize(10, new List<IElectricalPoleController>(new[] { firstPole.Object, secondPole.Object }));
			_net.TestNet(10, 1, 2, 1);
			firstPole.Verify(controller => controller.SetNet(_net));
			secondPole.Verify(controller => controller.SetNet(_net));
			generator.Verify(controller => controller.AddNet(_net));
			building.Verify(controller => controller.AddNet(_net));
			Assert.IsFalse(_net.StopPowerUpdates_Get);
		}

		[Test]
		public void UniteTest()
		{
			var firstPole = new ElectricalPoleController(Vector3.zero, ElectricityTestHelper.GetPoleModelMock().Object);
			var secondPole = new ElectricalPoleController(Vector3.zero,
			                                              ElectricityTestHelper.GetPoleModelMock().Object);
			var generator = new GeneratorController(Vector3.zero, ElectricityTestHelper.GetGeneratorModelMock().Object);
			var building = new ElectricalBuildingController(Vector3.zero,
			                                                ElectricityTestHelper.GetElectricalBuildingModelMock()
				                                                .Object);

			_net.Initialize(5);
			_net.AddPole(firstPole);
			_net.AddGenerator(generator);
			_net.TestNet(10, 1, 1, 0);
			var secondNet = new ElectricityNet();
			secondNet.Initialize(10);
			secondNet.AddPole(secondPole);
			secondNet.AddGenerator(generator);
			secondNet.AddBuilding(building);
			secondNet.TestNet(5, 1, 1, 1);
			_net.TestNet(5, 1, 1, 0);
			_net.Unite(secondNet);
			_net.TestNet(10, 1, 2, 1);
			Assert.AreEqual(_net, firstPole.Net);
			Assert.AreEqual(_net, secondPole.Net);
			Assert.IsTrue(generator.Nets.Contains(_net));
			Assert.IsTrue(building.Nets.Contains(_net));
		}
	}
}
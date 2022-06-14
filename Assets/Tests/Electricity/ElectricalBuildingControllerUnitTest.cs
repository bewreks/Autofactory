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
	public class ElectricalBuildingControllerUnitTest : ZenjectUnitTestFixture
	{
		private IElectricalBuildingModel     _model;
		private ElectricalBuildingController _controller;

		public override void Setup()
		{
			base.Setup();

			_model      = ElectricityTestHelper.GetElectricalBuildingModelMock().Object;
			_controller = new ElectricalBuildingController(Vector3.zero, _model);
		}

		public override void Teardown()
		{
			base.Teardown();
			_controller = null;
		}

		[Test]
		public void CreateControllerTest()
		{
			Assert.NotNull(_controller);
		}

		[Test]
		public void AddNetTest()
		{
			var netMock = new Mock<IElectricityNet>();
			_controller.AddNet(netMock.Object);
			Assert.AreEqual(1, _controller.Nets.Count);
		}

		[Test]
		public void DoubleNetTest()
		{
			var netMock = new Mock<IElectricityNet>();
			_controller.AddNet(netMock.Object);
			_controller.AddNet(netMock.Object);
			Assert.AreEqual(1, _controller.Nets.Count);
		}

		[Test]
		public void RemoveNetTest()
		{
			var netMock = new Mock<IElectricityNet>();
			_controller.AddNet(netMock.Object);
			_controller.RemoveNet(netMock.Object);
			Assert.AreEqual(0, _controller.Nets.Count);
		}

		[Test]
		public void AddTwoNetsTest()
		{
			var netMock       = new Mock<IElectricityNet>();
			var secondNetMock = new Mock<IElectricityNet>();
			_controller.AddNet(netMock.Object);
			_controller.AddNet(secondNetMock.Object);
			Assert.AreEqual(2, _controller.Nets.Count);
		}

		[Test]
		public void RemoveOneOfTwoNetsTest()
		{
			var netMock       = new Mock<IElectricityNet>();
			var secondNetMock = new Mock<IElectricityNet>();
			_controller.AddNet(netMock.Object);
			_controller.AddNet(secondNetMock.Object);
			_controller.RemoveNet(secondNetMock.Object);
			Assert.AreEqual(1, _controller.Nets.Count);
		}

		[Test]
		public void AddPoleTest()
		{
			var poleMock = new Mock<IElectricalPoleController>();
			_controller.AddPole(poleMock.Object);
			Assert.AreEqual(1, _controller.NearlyPoles.Count);
		}

		[Test]
		public void DoubleAddPoleTest()
		{
			var poleMock = new Mock<IElectricalPoleController>();
			_controller.AddPole(poleMock.Object);
			_controller.AddPole(poleMock.Object);
			Assert.AreEqual(1, _controller.NearlyPoles.Count);
		}

		[Test]
		public void RemovePolesTest()
		{
			var poleMock       = new Mock<IElectricalPoleController>();
			var secondPoleMock = new Mock<IElectricalPoleController>();
			_controller.AddPole(poleMock.Object);
			_controller.AddPole(secondPoleMock.Object);
			Assert.AreEqual(2, _controller.NearlyPoles.Count);
			_controller.RemovePoles(new []{ poleMock.Object, secondPoleMock.Object }.ToList());
			Assert.AreEqual(0, _controller.NearlyPoles.Count);
		}

		[Test]
		public void RemovePoleTest()
		{
			var poleMock = new Mock<IElectricalPoleController>();
			_controller.AddPole(poleMock.Object);
			Assert.AreEqual(1, _controller.NearlyPoles.Count);
			_controller.RemovePole(poleMock.Object);
			Assert.AreEqual(0, _controller.NearlyPoles.Count);
		}
	}
}
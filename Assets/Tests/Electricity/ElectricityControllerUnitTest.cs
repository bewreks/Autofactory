using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Electricity
{
	[TestFixture]
	public class ElectricityControllerUnitTest : ZenjectUnitTestFixture
	{
		private ElectricityController      _electricityController;
		private ElectricPoleBuildingModel  _poleModel;
		private BaseGeneratorBuildingModel _generatorModel;

		public override void Setup()
		{
			base.Setup();

			_poleModel      = Resources.Load<ElectricPoleBuildingModel>("Models/Buildings/ElectricPoleBuildingModel");
			_generatorModel = Resources.Load<BaseGeneratorBuildingModel>("Models/Buildings/BaseGeneratorBuildingModel");

			_electricityController = new ElectricityController();
		}

		public override void Teardown()
		{
			base.Teardown();
			_electricityController.Dispose();
			_electricityController = null;
		}

		[Test]
		public void AddPoleWithoutIDTest()
		{
			_electricityController.AddPole(Vector3.zero, _poleModel);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(0, net.ID);
			Assert.AreEqual(1, net.Poles.Count);
			var poleInRadius = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius), _poleModel);
			var poleNotInRadius =
				new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius + 1), _poleModel);
			Assert.IsTrue(net.IsPoleInWires(poleInRadius));
			Assert.IsFalse(net.IsPoleInWires(poleNotInRadius));
		}

		[Test]
		public void AddPoleWithIDTest()
		{
			_electricityController.AddPole(Vector3.zero, _poleModel, 10);
			Assert.Null(_electricityController.GetNet(0));
			var net = _electricityController.GetNet(10);
			Assert.NotNull(net);
			Assert.AreEqual(10, net.ID);
			Assert.AreEqual(1,  net.Poles.Count);
			var poleInRadius = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius), _poleModel);
			var poleNotInRadius =
				new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius + 1), _poleModel);
			Assert.IsTrue(net.IsPoleInWires(poleInRadius));
			Assert.IsFalse(net.IsPoleInWires(poleNotInRadius));
		}

		[Test]
		public void AddGeneratorWithoutNetTest()
		{
			_electricityController.AddGenerator(Vector3.one, _generatorModel);
			Assert.Null(_electricityController.GetNet(0));
			Assert.AreEqual(1, _electricityController.Generators.Count);
		}

		[Test]
		public void AddGeneratorWithNetTest()
		{
			_electricityController.AddPole(Vector3.zero, _poleModel);
			_electricityController.AddGenerator(Vector3.one, _generatorModel);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(0,                     _electricityController.Generators.Count);
			Assert.AreEqual(_generatorModel.Power, net.Power);
		}

		[Test]
		public void AddGeneratorBeforePoleWithNetTest()
		{
			_electricityController.AddGenerator(Vector3.one, _generatorModel);
			_electricityController.AddPole(Vector3.zero, _poleModel);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(0,                     _electricityController.Generators.Count);
			Assert.AreEqual(_generatorModel.Power, net.Power);
		}

		[Test]
		public void AddTwoPolesInOneNetTest()
		{
			_electricityController.AddPole(Vector3.zero,                             _poleModel, 0);
			_electricityController.AddPole(new Vector3(0, 0, _poleModel.WireRadius), _poleModel, 0);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(2, net.Poles.Count);
		}

		[Test]
		public void AddTwoPolesInTwoNetTest()
		{
			_electricityController.AddPole(Vector3.zero,                                 _poleModel);
			_electricityController.AddPole(new Vector3(0, 0, _poleModel.WireRadius + 1), _poleModel);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(1, net.Poles.Count);
			net = _electricityController.GetNet(1);
			Assert.NotNull(net);
			Assert.AreEqual(1, net.Poles.Count);
		}

		[Test]
		public void RemoveGeneratorFromNetTest()
		{
			_electricityController.AddPole(Vector3.zero, _poleModel);
			var generator = _electricityController.AddGenerator(Vector3.one, _generatorModel);
			_electricityController.RemoveGenerator(generator);
			var net = _electricityController.GetNet(0);
			Assert.NotNull(net);
			Assert.AreEqual(0, _electricityController.Generators.Count);
			Assert.AreEqual(0, net.Power);
		}

		[Test]
		public void RemovePoleFromNetTest()
		{
			var pole = _electricityController.AddPole(Vector3.zero, _poleModel);
			_electricityController.AddGenerator(Vector3.one, _generatorModel);
			_electricityController.RemovePole(pole);
			var net = _electricityController.GetNet(0);
			Assert.Null(net);
			Assert.AreEqual(1, _electricityController.Generators.Count);
		}
	}
}
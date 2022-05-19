using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Electricity
{
	[TestFixture]
	public class ElectricityNetUnitTest : ZenjectUnitTestFixture
	{
		private ElectricPoleBuildingModel  _poleModel;
		private BaseGeneratorBuildingModel _generatorModel;

		public override void Setup()
		{
			base.Setup();

			_poleModel      = Resources.Load<ElectricPoleBuildingModel>("Models/Buildings/ElectricPoleBuildingModel");
			_generatorModel = Resources.Load<BaseGeneratorBuildingModel>("Models/Buildings/BaseGeneratorBuildingModel");
		}

		[Test]
		public void AddPoleToNetTest()
		{
			var net = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			Assert.AreEqual(1, net.Poles.Count);
			var poleInRadius = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius), _poleModel);
			var poleNotInRadius =
				new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius + 1), _poleModel);
			Assert.IsTrue(net.IsPoleInWires(poleInRadius));
			Assert.IsFalse(net.IsPoleInWires(poleNotInRadius));
		}

		[Test]
		public void RemovePoleFromNetTest()
		{
			var net = new ElectricityNet();
			net.Initialize(0);
			var zero = new ElectricityPoleController(Vector3.zero,                                 _poleModel);
			var one  = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius),     _poleModel);
			var two  = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius * 2), _poleModel);
			net.AddPole(zero);
			net.AddPole(one);
			net.AddPole(two);
			Assert.AreEqual(3, net.Poles.Count);

			net.RemovePole(one.Position, out var nets);
			Assert.AreEqual(1,            net.Poles.Count);
			Assert.AreEqual(Vector3.zero, net.Poles[0].Position);

			Assert.AreEqual(1,            nets.Length);
			Assert.AreEqual(1,            nets[0].Poles.Count);
			Assert.AreEqual(two.Position, nets[0].Poles[0].Position);
		}

		[Test]
		public void AddGeneratorToNetTest()
		{
			var net = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(new GeneratorController(Vector3.one, _generatorModel));
			Assert.AreEqual(_generatorModel.Power, net.Power);
			Assert.AreEqual(1,                     net.Generators.Count);
		}

		[Test]
		public void AddGeneratorToNetsTest()
		{
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			var net       = new ElectricityNet();
			var secondNet = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(generator);
			Assert.AreEqual(_generatorModel.Power, net.Power);
			Assert.AreEqual(1,                     net.Generators.Count);
			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			secondNet.AddGenerator(generator);
			Assert.AreEqual(_generatorModel.Power / 2, net.Power);
			Assert.AreEqual(_generatorModel.Power / 2, secondNet.Power);
			Assert.AreEqual(1,                         secondNet.Generators.Count);
			Assert.AreEqual(2,                         generator.Nets.Count);
		}

		[Test]
		public void RemoveGeneratorFromNetTest()
		{
			var net = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			net.AddGenerator(generator);
			Assert.AreEqual(_generatorModel.Power, net.Power);
			Assert.AreEqual(1,                     net.Generators.Count);
			net.RemoveGenerator(generator);
			Assert.AreEqual(0, net.Power);
			Assert.AreEqual(0, net.Generators.Count);
		}

		[Test]
		public void RemoveGeneratorFromNetsTest()
		{
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			var net       = new ElectricityNet();
			var secondNet = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(generator);
			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			secondNet.AddGenerator(generator);
			Assert.AreEqual(_generatorModel.Power / 2, net.Power);
			Assert.AreEqual(_generatorModel.Power / 2, secondNet.Power);
			Assert.AreEqual(1,                         secondNet.Generators.Count);
			Assert.AreEqual(2,                         generator.Nets.Count);
			secondNet.RemoveGenerator(generator);
			Assert.AreEqual(_generatorModel.Power, net.Power);
			Assert.AreEqual(0,                     secondNet.Power);
			Assert.AreEqual(1,                     net.Generators.Count);
			Assert.AreEqual(0,                     secondNet.Generators.Count);
			Assert.AreEqual(1,                     generator.Nets.Count);
			net.RemoveGenerator(generator);
			Assert.AreEqual(0, net.Power);
			Assert.AreEqual(0, secondNet.Power);
			Assert.AreEqual(0, net.Generators.Count);
			Assert.AreEqual(0, secondNet.Generators.Count);
			Assert.AreEqual(0, generator.Nets.Count);
		}

		[Test]
		public void UniteNetsTest()
		{
			var net       = new ElectricityNet();
			var secondNet = new ElectricityNet();
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(new GeneratorController(Vector3.one, _generatorModel));
			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.left, _poleModel));
			secondNet.AddGenerator(new GeneratorController(-Vector3.one, _generatorModel));
			net.AddNet(secondNet);
			Assert.AreEqual(_generatorModel.Power * 2, net.Power);
			Assert.AreEqual(2,                         net.Generators.Count);
			Assert.AreEqual(2,                         net.Poles.Count);
			Assert.AreEqual(0,                         secondNet.Power);
			Assert.AreEqual(0,                         secondNet.Generators.Count);
			Assert.AreEqual(0,                         secondNet.Poles.Count);
		}
	}
}
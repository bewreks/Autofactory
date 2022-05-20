using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Factories;
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
			Container.Bind<ElectricityController>().FromNew().AsCached();
		}

		public override void Teardown()
		{
			base.Teardown();

			Container.Unbind<ElectricityController>();
		}

		[Test]
		public void AddPoleToNetTest()
		{
			var net = Factory.GetFactoryItem<ElectricityNet>(Container);

			net.Initialize(0);
			var mainPole = new ElectricityPoleController(Vector3.zero, _poleModel);
			net.AddPole(mainPole);
			net.TestNet(0, 0, 1);
			net.TestNetWire(new Vector3(0, 0, _poleModel.WireRadius),     _poleModel, true);
			net.TestNetWire(new Vector3(0, 0, _poleModel.WireRadius + 1), _poleModel, false);
			mainPole.TestPole(0, 0);
		}

		[Test]
		public void RemovePoleFromNetTest()
		{
			var net = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			var zero = new ElectricityPoleController(Vector3.zero,                                 _poleModel);
			var one  = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius),     _poleModel);
			var two  = new ElectricityPoleController(new Vector3(0, 0, _poleModel.WireRadius * 2), _poleModel);
			net.AddPole(zero);
			net.AddPole(one);
			net.AddPole(two);

			net.TestNet(0, 0, 3);
			zero.TestPole(1, 0);
			one.TestPole(2, 0);
			two.TestPole(1, 0);

			net.RemovePole(one, out var nets);

			net.TestNet(0, 0, 1);
			Assert.AreEqual(zero.Position, net.Poles[0].Position);

			Assert.AreEqual(1, nets.Length);
			nets[0].TestNet(0, 0, 1);
			Assert.AreEqual(two.Position, nets[0].Poles[0].Position);
		}

		[Test]
		public void AddGeneratorToNetTest()
		{
			var net = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			var pole      = new ElectricityPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			net.AddPole(pole);
			net.AddGenerator(generator);

			net.TestNet(_generatorModel.Power, 1, 1);
			pole.TestPole(0, 1);
		}

		[Test]
		public void AddGeneratorToNetsTest()
		{
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			var net       = Factory.GetFactoryItem<ElectricityNet>(Container);
			var secondNet = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(generator);
			net.TestNet(_generatorModel.Power, 1, 1);

			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			secondNet.AddGenerator(generator);

			net.TestNet(_generatorModel.Power / 2, 1, 1);
			secondNet.TestNet(_generatorModel.Power / 2, 1, 1);
			generator.TestGenerator(2, 2);
		}

		[Test]
		public void RemoveGeneratorFromNetTest()
		{
			var net = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			net.AddGenerator(generator);
			net.TestNet(_generatorModel.Power, 1, 1);

			net.RemoveGenerator(generator);
			net.TestNet(0, 0, 1);
		}

		[Test]
		public void RemoveGeneratorFromNetsTest()
		{
			var generator = new GeneratorController(Vector3.one, _generatorModel);
			var net       = Factory.GetFactoryItem<ElectricityNet>(Container);
			var secondNet = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(generator);
			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			secondNet.AddGenerator(generator);
			net.TestNet(_generatorModel.Power / 2, 1, 1);
			secondNet.TestNet(_generatorModel.Power / 2, 1, 1);
			generator.TestGenerator(2, 2);
			secondNet.RemoveGenerator(generator);
			net.TestNet(_generatorModel.Power, 1, 1);
			secondNet.TestNet(0, 0, 1);
			generator.TestGenerator(1, 1);

			net.RemoveGenerator(generator);
			net.TestNet(0, 0, 1);
			secondNet.TestNet(0, 0, 1);
			generator.TestGenerator(0, 0);
		}

		[Test]
		public void UniteNetsTest()
		{
			var net       = Factory.GetFactoryItem<ElectricityNet>(Container);
			var secondNet = Factory.GetFactoryItem<ElectricityNet>(Container);
			net.Initialize(0);
			net.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			net.AddGenerator(new GeneratorController(Vector3.one, _generatorModel));
			secondNet.Initialize(1);
			secondNet.AddPole(new ElectricityPoleController(Vector3.left, _poleModel));
			secondNet.AddGenerator(new GeneratorController(-Vector3.one, _generatorModel));
			net.AddNet(secondNet);
			net.TestNet(_generatorModel.Power * 2, 2, 2);
			secondNet.TestNet(0, 0, 0);
		}
	}

	internal static class ElectricityTestHelper
	{
		public static void TestNet(this ElectricityNet net, float power, int generatorsCount, int polesCount)
		{
			Assert.AreEqual(power,           net.Power);
			Assert.AreEqual(generatorsCount, net.Generators.Count);
			Assert.AreEqual(polesCount,      net.Poles.Count);
		}

		public static void TestPole(this ElectricityPoleController pole,
		                            int                            neighborPolesCount,
		                            int                            neighborGeneratorsCount)
		{
			Assert.AreEqual(neighborPolesCount,      pole.NearlyPoles.Count);
			Assert.AreEqual(neighborGeneratorsCount, pole.NearlyGenerators.Count);
		}

		public static void TestNetWire(this ElectricityNet       net,
		                               Vector3                   position,
		                               ElectricPoleBuildingModel model,
		                               bool                      result)
		{
			Assert.AreEqual(result, net.IsPoleInWires(new ElectricityPoleController(position, model)));
		}

		public static void TestGenerator(this GeneratorController generator, int netsCount, int neighbourPolesCount)
		{
			Assert.AreEqual(netsCount,           generator.Nets.Count);
			Assert.AreEqual(neighbourPolesCount, generator.NearlyPoles.Count);
		}
	}
}
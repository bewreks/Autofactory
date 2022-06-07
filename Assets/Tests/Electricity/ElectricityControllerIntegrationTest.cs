using System.Collections;
using Buildings.Interfaces;
using Electricity;
using Electricity.Controllers;
using Factories;
using NUnit.Framework;
using Tests.Helpers;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Electricity
{
	[TestFixture]
	public class ElectricityControllerIntegrationTest : ZenjectUnitTestFixture
	{
		private ElectricityController    _electricityController;
		private IElectricalPoleModel     _poleModel;
		private IGeneratorModel          _generatorModel;
		private IElectricalBuildingModel _electricalBuildingModel;

		public override void Setup()
		{
			base.Setup();

			_poleModel               = ElectricityTestHelper.GetPoleModelMock().Object;
			_generatorModel          = ElectricityTestHelper.GetGeneratorModelMock().Object;
			_electricalBuildingModel = ElectricityTestHelper.GetElectricalBuildingModelMock().Object;

			Container.Bind<ElectricityController>().FromNew().AsSingle();
			_electricityController = Container.Resolve<ElectricityController>();
		}

		public override void Teardown()
		{
			base.Teardown();
			_electricityController.Dispose();
			_electricityController = null;
			Container.Unbind<ElectricityController>();
			Factory.Clear();
		}

		[UnityTest]
		public IEnumerator AddSinglePoleTest()
		{
			var pole = new ElectricalPoleController(Vector3.zero, _poleModel);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			Assert.AreEqual(0, pole.Net.ID);
		}

		[UnityTest]
		public IEnumerator AddSingleGeneratorTest()
		{
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.TestController(0, 1, 0);
			generator.TestGenerator(0, 0);
			Assert.AreEqual(_generatorModel.Power, generator.ActualPower.Value.Item2);
		}

		[UnityTest]
		public IEnumerator AddSingleBuildingTest()
		{
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.TestController(0, 0, 1);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorTest()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 1, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorAndUniteV1Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 0);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorAndUniteV2Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 0);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorAndUniteV3Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 0);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorAndUniteV4Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 0);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleGeneratorAndUniteV5Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.AddGenerator(generator);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 0);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingAndUniteV1Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 1);
			building.TestBuilding(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingAndUniteV2Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 1);
			building.TestBuilding(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingAndUniteV3Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 1);
			building.TestBuilding(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingAndUniteV4Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 1);
			building.TestBuilding(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingAndUniteV5Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.AddBuilding(building);
			_electricityController.AddPole(pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 1);
			building.TestBuilding(1, 1);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndSingleBuildingTest()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.TestController(1, 0, 1);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator AddSinglePoleAndAndSingleGeneratorAndSingleBuildingAndUniteTest()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building  = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 1);
			building.TestBuilding(1, 1);
			generator.TestGenerator(1, 1);
		}

		[UnityTest]
		public IEnumerator CreateTwoNetWithSingleGeneratorAndSingleBuildingTest()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			yield return null;
			_electricityController.TestController(2, 0, 0);
			pole.TestPole(0, 1, 1);
			secondPole.TestPole(0, 1, 1);
			building.TestBuilding(2, 2);
			generator.TestGenerator(2, 2);
		}

		[UnityTest]
		public IEnumerator CreateTwoNetWithSingleGeneratorAndSingleBuildingAndUniteV1Test()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			_electricityController.MergePoles(pole, secondPole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 1);
			secondPole.TestPole(0, 1, 1);
			building.TestBuilding(1, 2);
			generator.TestGenerator(1, 2);
			Assert.IsTrue(_electricityController.Datas.Nets.TryGetValue(0, out var net));
			Assert.AreEqual(0, net.ID);
			net.TestNet(_generatorModel.Power, 1, 2, 1);
		}

		[UnityTest]
		public IEnumerator CreateTwoNetWithSingleGeneratorAndSingleBuildingAndUniteV2Test()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			yield return null;
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.MergePoles(pole, secondPole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 1);
			secondPole.TestPole(0, 1, 1);
			building.TestBuilding(1, 2);
			generator.TestGenerator(1, 2);
			Assert.IsTrue(_electricityController.Datas.Nets.TryGetValue(0, out var net));
			Assert.AreEqual(0, net.ID);
			net.TestNet(_generatorModel.Power, 1, 2, 1);
		}

		[UnityTest]
		public IEnumerator CreateTwoNetWithSingleGeneratorAndSingleBuildingAndUniteV3Test()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			yield return null;
			_electricityController.MergePoles(pole, secondPole);
			yield return null;
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 1);
			secondPole.TestPole(0, 1, 1);
			building.TestBuilding(1, 2);
			generator.TestGenerator(1, 2);
			Assert.IsTrue(_electricityController.Datas.Nets.TryGetValue(0, out var net));
			Assert.AreEqual(0, net.ID);
			net.TestNet(_generatorModel.Power, 1, 2, 1);
		}

		[UnityTest]
		public IEnumerator CreateTwoNetWithSingleGeneratorAndSingleBuildingAndUniteV4Test()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.MergePoles(pole, secondPole);
			yield return null;
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			yield return null;
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 1, 1);
			secondPole.TestPole(0, 1, 1);
			building.TestBuilding(1, 2);
			generator.TestGenerator(1, 2);
			Assert.IsTrue(_electricityController.Datas.Nets.TryGetValue(0, out var net));
			Assert.AreEqual(0, net.ID);
			net.TestNet(_generatorModel.Power, 1, 2, 1);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFromNetV1Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 1, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFromNetV2Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 1, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFromNetV1Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.RemoveBuildingFromNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 1);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFromNetV2Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.RemoveBuildingFromNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 1);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFromTwoNetsTest()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building   = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.AddBuildingToNet(building, secondPole);
			yield return null;
			_electricityController.RemoveBuildingFromNet(building, pole);
			_electricityController.RemoveBuildingFromNet(building, secondPole);
			yield return null;
			_electricityController.TestController(2, 0, 1);
			pole.TestPole(0, 0, 0);
			secondPole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFromTwoNetsTest()
		{
			var pole       = new ElectricalPoleController(Vector3.zero, _poleModel);
			var secondPole = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator  = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddPole(secondPole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.AddGeneratorToNet(generator, secondPole);
			yield return null;
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			_electricityController.RemoveGeneratorFromNet(generator, secondPole);
			yield return null;
			_electricityController.TestController(2, 1, 0);
			pole.TestPole(0, 0, 0);
			secondPole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFromControllerTest()
		{
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddBuilding(building);
			yield return null;
			_electricityController.RemoveBuilding(building);
			yield return null;
			_electricityController.TestController(0, 0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFromControllerTest()
		{
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddGenerator(generator);
			yield return null;
			_electricityController.RemoveGenerator(generator);
			yield return null;
			_electricityController.TestController(0, 0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFullV1Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			_electricityController.RemoveGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFullV2Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			yield return null;
			_electricityController.RemoveGenerator(generator);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFullV3Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.RemoveGenerator(generator);
			yield return null;
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveGeneratorFullV4Test()
		{
			var pole      = new ElectricalPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			_electricityController.RemoveGenerator(generator);
			yield return null;
			_electricityController.RemoveGeneratorFromNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			generator.TestGenerator(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFullV1Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.RemoveBuildingFromNet(building, pole);
			_electricityController.RemoveBuilding(building);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFullV2Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.RemoveBuildingFromNet(building, pole);
			yield return null;
			_electricityController.RemoveBuilding(building);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFullV3Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			yield return null;
			_electricityController.RemoveBuilding(building);
			yield return null;
			_electricityController.RemoveBuildingFromNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}

		[UnityTest]
		public IEnumerator RemoveBuildingFullV4Test()
		{
			var pole     = new ElectricalPoleController(Vector3.zero, _poleModel);
			var building = new ElectricalBuildingController(Vector3.zero, _electricalBuildingModel);
			_electricityController.AddPole(pole);
			_electricityController.AddBuilding(building);
			_electricityController.AddBuildingToNet(building, pole);
			_electricityController.RemoveBuilding(building);
			yield return null;
			_electricityController.RemoveBuildingFromNet(building, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			pole.TestPole(0, 0, 0);
			building.TestBuilding(0, 0);
		}
	}
}
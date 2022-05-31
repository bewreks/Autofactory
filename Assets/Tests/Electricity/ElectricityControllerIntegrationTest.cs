using System.Collections;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Factories;
using Helpers;
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

		private ElectricityController      _electricityController;
		private ElectricPoleBuildingModel  _poleModel;
		private BaseGeneratorBuildingModel _generatorModel;
		private ElectricBuildingModel      _electricBuildingModel;

		public override void Setup()
		{
			base.Setup();

			_poleModel             = Resources.Load<ElectricPoleBuildingModel>(BuildingHelper.POLE_PATH);
			_generatorModel        = Resources.Load<BaseGeneratorBuildingModel>(BuildingHelper.GENERATOR_PATH);
			_electricBuildingModel = Resources.Load<ElectricBuildingModel>(BuildingHelper.ELECTRIC_BUILDING_PATH);

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
			_electricityController.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			yield return null;
			_electricityController.Datas.Nets[0].TestNet(0, 0, 1, 0);
		}

		[UnityTest]
		public IEnumerator AddSingleGeneratorTest()
		{
			_electricityController.AddGenerator(new GeneratorController(Vector3.zero, _generatorModel));
			yield return null;
			_electricityController.TestController(0, 1, 0);
		}

		[UnityTest]
		public IEnumerator AddSingleBuildingTest()
		{
			_electricityController.AddBuilding(new ElectricalBuildingController(Vector3.zero, _electricBuildingModel));
			yield return null;
			_electricityController.TestController(0, 0, 1);
		}

		[UnityTest]
		public IEnumerator AddTwoSeparatePolesTest()
		{
			_electricityController.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			_electricityController.AddPole(new ElectricityPoleController(Vector3.zero, _poleModel));
			yield return null;
			_electricityController.TestController(2, 0, 0);
		}

		[UnityTest]
		public IEnumerator AddTwoSeparatePolesAndUniteTest()
		{
			var first  = new ElectricityPoleController(Vector3.zero, _poleModel);
			var second = new ElectricityPoleController(Vector3.zero, _poleModel);
			_electricityController.AddPole(first);
			_electricityController.AddPole(second);
			_electricityController.MergePoles(first, second);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			_electricityController.Datas.Nets[0].TestNet(0, 0, 2, 0);
			Assert.NotZero(Factory.GetCountOf<ElectricityNet>());
		}

		[UnityTest]
		public IEnumerator AddTwoSeparatePolesAndUniteAfterFrameTest()
		{
			var first  = new ElectricityPoleController(Vector3.zero, _poleModel);
			var second = new ElectricityPoleController(Vector3.zero, _poleModel);
			_electricityController.AddPole(first);
			_electricityController.AddPole(second);
			yield return null;
			_electricityController.MergePoles(first, second);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			_electricityController.Datas.Nets[0].TestNet(0, 0, 2, 0);
			Assert.NotZero(Factory.GetCountOf<ElectricityNet>());
		}

		[UnityTest]
		public IEnumerator AddPoleGeneratorAndUniteTest()
		{
			var pole      = new ElectricityPoleController(Vector3.zero, _poleModel);
			var generator = new GeneratorController(Vector3.zero, _generatorModel);
			_electricityController.AddPole(pole);
			_electricityController.AddGenerator(generator);
			_electricityController.AddGeneratorToNet(generator, pole);
			yield return null;
			_electricityController.TestController(1, 0, 0);
			_electricityController.Datas.Nets[0].TestNet(_generatorModel.Power, 1, 1, 0);
		}
	}
}
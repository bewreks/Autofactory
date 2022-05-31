using System;
using Buildings.Interfaces;
using Electricity;
using Electricity.Controllers;
using Electricity.Interfaces;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Helpers
{
	internal static class ElectricityTestHelper
	{
		public static void TestNet(this ElectricityNet net,
		                           float               power,
		                           int                 generatorsCount,
		                           int                 polesCount,
		                           int                 buildingsCount)
		{
			Assert.AreEqual(power,           net.Power,            $"Power is not {power}");
			Assert.AreEqual(generatorsCount, net.Generators.Count, $"Generators count is not {generatorsCount}");
			Assert.AreEqual(polesCount,      net.Poles.Count,      $"Poles count is not {polesCount}");
			Assert.AreEqual(buildingsCount,  net.Buildings.Count,  $"Buildings count is not {buildingsCount}");
		}

		public static void TestPole(this ElectricalPoleController pole,
		                            int                           neighborPolesCount,
		                            int                           neighborGeneratorsCount)
		{
			Assert.AreEqual(neighborPolesCount,      pole.NearlyPoles.Count);
			Assert.AreEqual(neighborGeneratorsCount, pole.NearlyGenerators.Count);
		}

		public static void TestGenerator(this GeneratorController generator, int netsCount, int neighbourPolesCount)
		{
			Assert.AreEqual(netsCount,           generator.Nets.Count);
			Assert.AreEqual(neighbourPolesCount, generator.NearlyPoles.Count);
		}

		public static void TestBuilding(this GeneratorController generator, int netsCount, int neighbourPolesCount)
		{
			Assert.AreEqual(netsCount,           generator.Nets.Count);
			Assert.AreEqual(neighbourPolesCount, generator.NearlyPoles.Count);
		}

		public static void TestController(this ElectricityController controller,
		                                  int                        netsCount,
		                                  int                        generatorsCount,
		                                  int                        buildingsCount)
		{
			Assert.AreEqual(netsCount,
			                controller.Datas.Nets.Keys.Count,
			                $"Nets count is not {netsCount}");
			Assert.AreEqual(generatorsCount,
			                controller.Datas.Generators.Count,
			                $"Generators count is not {generatorsCount}");
			Assert.AreEqual(buildingsCount,
			                controller.Datas.Buildings.Count,
			                $"Buildings count is not {buildingsCount}");
		}

		public static Mock<IElectricalPoleModel> GetPoleModelMock()
		{
			var poleModelMock = new Mock<IElectricalPoleModel>
			                    {
				                    CallBase = true
			                    };
			return poleModelMock;
		}

		public static Mock<IGeneratorModel> GetGeneratorModelMock()
		{
			var poleModelMock = new Mock<IGeneratorModel>
			                    {
				                    CallBase = true
			                    };
			poleModelMock.SetupGet(model => model.Power).Returns(10f);
			return poleModelMock;
		}

		public static Mock<IElectricalBuildingModel> GetElectricalBuildingModelMock()
		{
			var poleModelMock = new Mock<IElectricalBuildingModel>
			                    {
				                    CallBase = true
			                    };
			poleModelMock.SetupGet(model => model.ConsumptionCurve)
			             .Returns(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
			return poleModelMock;
		}
	}
}
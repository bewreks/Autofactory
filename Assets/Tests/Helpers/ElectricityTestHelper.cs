using Buildings.Models;
using Electricity;
using Electricity.Controllers;
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

		public static void TestPole(this ElectricityPoleController pole,
		                            int                            neighborPolesCount,
		                            int                            neighborGeneratorsCount)
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

		public static Mock<ElectricityPoleController> GetPoleControllerMock(Vector3 position = new Vector3())
		{
			var poleModelMock = new Mock<ElectricPoleBuildingModel>
			                    {
				                    CallBase = true
			                    };
			poleModelMock.Setup(model => model.Awake());
			return new Mock<ElectricityPoleController>(position, poleModelMock.Object);
		}
	}
}
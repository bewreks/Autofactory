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
		public static void TestNet(this IElectricityNet net,
		                           float                power,
		                           int                  generatorsCount,
		                           int                  polesCount,
		                           int                  buildingsCount)
		{
			Assert.AreEqual(power,           net.Power,            $"Power is not {power}");
			Assert.AreEqual(generatorsCount, net.Generators.Count, $"Generators count is not {generatorsCount}");
			Assert.AreEqual(polesCount,      net.Poles.Count,      $"Poles count is not {polesCount}");
			Assert.AreEqual(buildingsCount,  net.Buildings.Count,  $"Buildings count is not {buildingsCount}");
		}

		public static void TestPole(this ElectricalPoleController pole,
		                            int                           neighborPolesCount,
		                            int                           neighborGeneratorsCount,
		                            int                           neighborBuildingsCount)
		{
			Assert.AreEqual(neighborPolesCount,      
			                pole.NearlyPoles.Count, 
			                $"Pole's nearly poles count is not {neighborPolesCount}");
			
			Assert.AreEqual(neighborGeneratorsCount, 
			                pole.NearlyGenerators.Count, 
			                $"Pole's nearly generators count is not {neighborGeneratorsCount}");
			
			Assert.AreEqual(neighborBuildingsCount,  
			                pole.NearlyBuildings.Count, 
			                $"Pole's nearly buildings count is not {neighborBuildingsCount}");
		}

		public static void TestGenerator(this GeneratorController generator, int netsCount, int neighbourPolesCount)
		{
			Assert.AreEqual(netsCount, 
			                generator.Nets.Count, 
			                $"Generator's nets count is not {netsCount}");
			
			Assert.AreEqual(neighbourPolesCount, 
			                generator.NearlyPoles.Count,
			                $"Generator's nearly poles count is not {neighbourPolesCount}");
		}

		public static void TestGenerator(this GeneratorController generator, float power, int netsCount, int neighbourPolesCount)
		{
			Assert.AreEqual(netsCount, 
			                generator.Nets.Count, 
			                $"Generator's nets count is not {netsCount}");
			
			Assert.AreEqual(neighbourPolesCount, 
			                generator.NearlyPoles.Count,
			                $"Generator's nearly poles count is not {neighbourPolesCount}");
			
			Assert.AreEqual(power, 
			                generator.ActualPower.Value.Item2, 
			                $"Generator's power is not {netsCount}");
		}

		public static void TestBuilding(this ElectricalBuildingController building,
		                                int                               netsCount,
		                                int                               neighbourPolesCount)
		{
			Assert.AreEqual(netsCount,           
			                building.Nets.Count,
			                $"Building's nets count is not {netsCount}");
			
			Assert.AreEqual(neighbourPolesCount, 
			                building.NearlyPoles.Count,
			                $"Building's nearly poles count is not {neighbourPolesCount}");
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
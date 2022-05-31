using System;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Helpers;
using NUnit.Framework;
using UnityEngine;
using Zenject;
using Moq;
using Tests.Helpers;
using UniRx;

namespace Tests.Electricity
{
	[TestFixture]
	public class GeneratorUnitTest : ZenjectUnitTestFixture
	{
		private BaseGeneratorBuildingModel _model;
		private GeneratorController        _generatorController;

		public override void Setup()
		{
			base.Setup();
			_model               = Resources.Load<BaseGeneratorBuildingModel>(BuildingHelper.GENERATOR_PATH);
			_generatorController = new GeneratorController(Vector3.zero, _model);
		}

		public override void Teardown()
		{
			base.Teardown();
			_generatorController = null;
		}

		[Test]
		public void CreateControllerTest()
		{
			Assert.NotNull(_generatorController);
		}

		[Test]
		public void AddPoleTest()
		{
			var poleMock = ElectricityTestHelper.GetPoleControllerMock();
			_generatorController.AddPole(poleMock.Object);
			Assert.AreEqual(1, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void DoubleAddPoleTest()
		{
			var poleMock = ElectricityTestHelper.GetPoleControllerMock();
			_generatorController.AddPole(poleMock.Object);
			_generatorController.AddPole(poleMock.Object);
			Assert.AreEqual(1, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void AddNet()
		{
			var prevValue = 0;
			var newValue  = _model.Power;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Select(pair => pair)
			                                     .Subscribe(tuple =>
			                                     {
				                                     Assert.AreEqual(prevValue, tuple.Item1);
				                                     Assert.AreEqual(newValue,  tuple.Item2);
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			disposable.Dispose();
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void DoubleNet()
		{
			var prevValue = 0;
			var newValue  = _model.Power;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Select(pair => pair)
			                                     .Subscribe(tuple =>
			                                     {
				                                     Assert.AreEqual(prevValue, tuple.Item1);
				                                     Assert.AreEqual(newValue,  tuple.Item2);
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			_generatorController.AddNet(netMock.Object);
			Assert.IsFalse(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void RemoveNet()
		{
			var prevValue = 0;
			var newValue  = _model.Power;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Select(pair => pair)
			                                     .Subscribe(tuple =>
			                                     {
				                                     Assert.AreEqual(prevValue, tuple.Item1);
				                                     Assert.AreEqual(newValue,  tuple.Item2);
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			_generatorController.RemoveNet(netMock.Object);
			Assert.IsFalse(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void AddTwoNets()
		{
			var prevValue     = 0f;
			var newValue      = _model.Power;
			var wasCalled     = false;
			var netMock       = new Mock<ElectricityNet>();
			var secondNetMock = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Select(pair => pair)
			                                     .Subscribe(tuple =>
			                                     {
				                                     Assert.AreEqual(prevValue, tuple.Item1);
				                                     Assert.AreEqual(newValue,  tuple.Item2);
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			prevValue = _model.Power;
			newValue  = _model.Power/2;
			_generatorController.AddNet(secondNetMock.Object);
			Assert.IsTrue(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void RemoveOneOfTwoNets()
		{
			var prevValue     = 0f;
			var newValue      = _model.Power;
			var wasCalled     = false;
			var netMock       = new Mock<ElectricityNet>();
			var secondNetMock = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Select(pair => pair)
			                                     .Subscribe(tuple =>
			                                     {
				                                     Assert.AreEqual(prevValue, tuple.Item1);
				                                     Assert.AreEqual(newValue,  tuple.Item2);
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			prevValue = _model.Power;
			newValue  = _model.Power/2;
			_generatorController.AddNet(secondNetMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			prevValue = _model.Power/2;
			newValue  = _model.Power;
			_generatorController.RemoveNet(secondNetMock.Object);
			Assert.IsTrue(wasCalled);
			disposable.Dispose();
		}
	}
}
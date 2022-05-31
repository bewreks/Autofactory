using System.Linq;
using Buildings.Interfaces;
using Electricity;
using Electricity.Controllers;
using Electricity.Interfaces;
using Helpers;
using Moq;
using NUnit.Framework;
using Tests.Helpers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Tests.Electricity
{
	[TestFixture]
	public class GeneratorControllerUnitTest : ZenjectUnitTestFixture
	{
		private IGeneratorModel     _model;
		private GeneratorController _generatorController;

		public override void Setup()
		{
			base.Setup();
			_model               = ElectricityTestHelper.GetGeneratorModelMock().Object;
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
			var poleMock = new Mock<IElectricalPoleController>();
			_generatorController.AddPole(poleMock.Object);
			Assert.AreEqual(1, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void DoubleAddPoleTest()
		{
			var poleMock = new Mock<IElectricalPoleController>();
			_generatorController.AddPole(poleMock.Object);
			_generatorController.AddPole(poleMock.Object);
			Assert.AreEqual(1, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void RemovePolesTest()
		{
			var poleMock       = new Mock<IElectricalPoleController>();
			var secondPoleMock = new Mock<IElectricalPoleController>();
			_generatorController.AddPole(poleMock.Object);
			_generatorController.AddPole(secondPoleMock.Object);
			Assert.AreEqual(2, _generatorController.NearlyPoles.Count);
			_generatorController.RemovePoles(new []{ poleMock.Object, secondPoleMock.Object }.ToList());
			Assert.AreEqual(0, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void RemovePoleTest()
		{
			var poleMock = new Mock<IElectricalPoleController>();
			_generatorController.AddPole(poleMock.Object);
			Assert.AreEqual(1, _generatorController.NearlyPoles.Count);
			_generatorController.RemovePole(poleMock.Object);
			Assert.AreEqual(0, _generatorController.NearlyPoles.Count);
		}

		[Test]
		public void AddNetTest()
		{
			var prevValue = -10f;
			var newValue  = -10f;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Subscribe(tuple =>
			                                     {
				                                     prevValue = tuple.Item1;
				                                     newValue  = tuple.Item2;
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			disposable.Dispose();
			Assert.AreEqual(0,            prevValue);
			Assert.AreEqual(_model.Power, newValue);
			Assert.IsTrue(wasCalled);
		}

		[Test]
		public void DoubleNetTest()
		{
			var prevValue = -10f;
			var newValue  = -10f;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Subscribe(tuple =>
			                                     {
				                                     prevValue = tuple.Item1;
				                                     newValue  = tuple.Item2;
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			Assert.IsTrue(wasCalled);
			wasCalled = false;
			_generatorController.AddNet(netMock.Object);
			Assert.AreEqual(0,            prevValue);
			Assert.AreEqual(_model.Power, newValue);
			Assert.IsFalse(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void RemoveNetTest()
		{
			var prevValue = -10f;
			var newValue  = -10f;
			var wasCalled = false;
			var netMock   = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Subscribe(tuple =>
			                                     {
				                                     prevValue = tuple.Item1;
				                                     newValue  = tuple.Item2;
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			wasCalled = false;
			_generatorController.RemoveNet(netMock.Object);
			Assert.AreEqual(0,            prevValue);
			Assert.AreEqual(_model.Power, newValue);
			Assert.IsFalse(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void AddTwoNetsTest()
		{
			var prevValue     = -10f;
			var newValue      = -10f;
			var wasCalled     = false;
			var netMock       = new Mock<ElectricityNet>();
			var secondNetMock = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Subscribe(tuple =>
			                                     {
				                                     prevValue = tuple.Item1;
				                                     newValue  = tuple.Item2;
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			_generatorController.AddNet(secondNetMock.Object);
			Assert.AreEqual(_model.Power,     prevValue);
			Assert.AreEqual(_model.Power / 2, newValue);
			Assert.IsTrue(wasCalled);
			disposable.Dispose();
		}

		[Test]
		public void RemoveOneOfTwoNetsTest()
		{
			var prevValue     = -10f;
			var newValue      = -10f;
			var wasCalled     = false;
			var netMock       = new Mock<ElectricityNet>();
			var secondNetMock = new Mock<ElectricityNet>();
			var disposable = _generatorController.ActualPower
			                                     .PairWithPrevious()
			                                     .Subscribe(tuple =>
			                                     {
				                                     prevValue = tuple.Item1;
				                                     newValue  = tuple.Item2;
				                                     wasCalled = true;
			                                     });

			_generatorController.AddNet(netMock.Object);
			_generatorController.AddNet(secondNetMock.Object);
			_generatorController.RemoveNet(secondNetMock.Object);
			Assert.AreEqual(_model.Power / 2, prevValue);
			Assert.AreEqual(_model.Power,     newValue);
			Assert.IsTrue(wasCalled);
			disposable.Dispose();
		}
	}
}
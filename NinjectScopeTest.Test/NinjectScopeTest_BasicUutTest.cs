﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NinjectScopeTest.Test
{
    // Derive your Unit test class from NinjectScopeTest<T>
    // where T is the type of your Scope object
    [TestClass]
    public class NinjectScopeTest_BasicUutTest :
        NinjectScopeTest<NinjectScopeTest_BasicUutTest.TestScope>
    {
        // create your own scope object, which is used to 
        // hold the data that your test class needs
        // to perform the test.  Any properties of type Mock<T>
        // where T is an interface or a non-sealed class
        // will be automatically instantiated and bound
        // to the Ninject Kernel on NinjectScope
        public class TestScope : NinjectScope
        {
            public Mock<ICloneable> CloneableMock { get; set; }
            public object ClonedObject { get; set; }
            public Mock<IComparable> ComparableMock { get; set; }

            // manditory Initialize method to do any
            // custom setup that you'd like to perform.
            public override void Initialize()
            {
                ClonedObject = new object();
                CloneableMock.Setup(x => x.Clone())
                    .Returns(ClonedObject);
            }
        }

        // An example Unit Under Test, used solely for this example.
        // This class can really be anything.
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestDependencyInjection
        {
            private readonly ICloneable _cloneable;

            // dependencies will be automatically resolved when Get<T>
            // is called on NinjectScopeTest.  In this case, T would
            // be TestDependencyInjection
            public TestDependencyInjection(
                ICloneable cloneable,
                // unused dependency, but here just for show
                IComparable unusedComparable)
            {
                _cloneable = cloneable;
            }

            public object CloneWrapper()
            {
                return _cloneable.Clone();
            }
        }

        [TestMethod]
        public void NinjectScopeTest_BasicDependencyTest()
        {
            // this is all you need in your test to get you
            // auto-resolved mocked dependencies for your
            // unit under test.
            var uut = Get<TestDependencyInjection>();

            // perform action on your UUT
            var cloned = uut.CloneWrapper();

            // assert that results were as expected
            Assert.AreEqual(Scope.ClonedObject, cloned);
        }
    }
}
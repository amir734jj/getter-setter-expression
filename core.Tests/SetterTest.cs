using System;
using AutoFixture;
using core.Tests.Models;
using GetterSetterExpr;
using Xunit;

namespace core.Tests
{

    public class SetterTest
    {
        private readonly Fixture _fixture;

        public SetterTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Flat()
        {
            // Arrange
            var updatedValue = _fixture.Create<int>();
            var person = _fixture.Build<Human>()
                .With(x => x.GrandParent, new Human())
                .Create();
            
            // Act
            GetSetUtility.Set((Human x) => x.Age)(person, updatedValue);

            // Assert
            Assert.Equal(updatedValue, person.Age);
        }
        
        [Fact]
        public void Test__Nested()
        {
            // Arrange
            var updatedValue = _fixture.Create<int>();
            var person = _fixture.Build<Human>()
                .With(x => x.GrandParent, new Human
                {
                    Name = _fixture.Create<string>(),
                    Age = _fixture.Create<int>(),
                    GrandParent = null
                })
                .Create();
            
            // Act
            GetSetUtility.Set((Human x) => x.GrandParent.Age)(person, updatedValue);

            // Assert
            Assert.Equal(updatedValue, person.GrandParent.Age);
        }
    }
}
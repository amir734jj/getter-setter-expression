using System;
using AutoFixture;
using GetterSetterExpr;
using Xunit;

namespace core.Tests
{
    public class Human
    {
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        public Human GrandParent { get; set; }
    }
    
    public class GetterTest
    {
        private readonly Fixture _fixture;

        public GetterTest()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Test__Flat()
        {
            // Arrange
            var person = _fixture.Build<Human>()
                .With(x => x.GrandParent, new Human())
                .Create();
            
            // Act
            var result = GetSetUtility.Get((Human x) => x.Age)(person);

            // Assert
            Assert.Equal(person.Age, result);
        }
        
        [Fact]
        public void Test__Nested()
        {
            // Arrange
            var person = _fixture.Build<Human>()
                .With(x => x.GrandParent, new Human
                {
                    Name = _fixture.Create<string>(),
                    Age = _fixture.Create<int>(),
                    GrandParent = null
                })
                .Create();
            
            // Act
            var result = GetSetUtility.Get((Human x) => x.GrandParent.Age)(person);

            // Assert
            Assert.Equal(person.GrandParent.Age, result);
        }
    }
}
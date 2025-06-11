using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Tests.Shared
{
    internal class TestValueObject : ValueObject
    {
        public double TestDecimal { get; }
        public string TestString { get; }
        public bool TestBoolean { get; }
        public int TestInt { get; }

        public TestValueObject(double testDecimal, string testString, bool testBoolean, int testInt)
        {
            TestDecimal = testDecimal;
            TestString = testString;
            TestBoolean = testBoolean;
            TestInt = testInt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TestDecimal;
            yield return TestString;
            yield return TestBoolean;
            yield return TestInt;
        }
    }

    public class ValueObjectTests
    {
        [Fact]
        public void Equals_ShouldReturnTrue_WhenObjectsHaveSameValues()
        {
            // Arrange
            var testValueObject1 = new TestValueObject(.1, "test", true, 1);
            var testValueObject2 = new TestValueObject(.1, "test", true, 1);

            // Act && Assert
            Assert.Equal(testValueObject1, testValueObject2);
            Assert.True(testValueObject1.Equals(testValueObject2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenObjectsHaveDifferentValues()
        {
            // Arrange
            var testValueObject1 = new TestValueObject(.2, "test2", false, 2);
            var testValueObject2 = new TestValueObject(.1, "test", true, 1);

            // Act && Assert
            Assert.NotEqual(testValueObject1, testValueObject2);
            Assert.False(testValueObject1.Equals(testValueObject2));
        }

        [Fact]
        public void Equals_ShouldReturnFalse_WhenComparedWithNull()
        {
            var testValueObject1 = new TestValueObject(.2, "test2", false, 2);
            var notValueObject = "Not a Value Object";

            // Act & Assert
            Assert.False(testValueObject1.Equals(notValueObject));
        }

        [Fact]
        public void GetHashCode_ShouldReturnSameHashCode_ForEqualObjects()
        {
            // Arrange
            var testValueObject1 = new TestValueObject(.1, "test", true, 1);
            var testValueObject2 = new TestValueObject(.1, "test", true, 1);

            // Act & Assert
            Assert.Equal(testValueObject1.GetHashCode(), testValueObject2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldReturnDifferentHashCode_ForDifferentObjects()
        {
            // Arrange
            var testValueObject1 = new TestValueObject(.2, "test2", false, 2);
            var testValueObject2 = new TestValueObject(.1, "test", true, 1);

            // Act & Assert
            Assert.NotEqual(testValueObject1.GetHashCode(), testValueObject2.GetHashCode());
        }
    }
}

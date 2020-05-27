﻿using Cosmos.Judgments;
using Cosmos.Numeric;
using Xunit;

namespace Cosmos.Test.Judgements
{
    public class NumericJudgementTest
    {
        [Theory]
        [InlineData("1")]
        [InlineData("0")]
        [InlineData("-100")]
        [InlineData("+100")]
        public void IsInt32Test_True(string str)
        {
            Assert.True(NumericJudgment.IsInt32(str));
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("1.")]
        [InlineData("10L")]
        [InlineData("1000000000000000000000000000000000000000")]
        [InlineData("LOVE")]
        [InlineData("")]
        [InlineData(null)]
        public void IsInt32Test_False(string str)
        {
            Assert.False(NumericJudgment.IsInt32(str));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-0")]
        [InlineData("+0")]
        [InlineData("1")]
        [InlineData("1.1")]
        [InlineData("-1")]
        [InlineData("-1.1")]
        [InlineData("+1")]
        [InlineData("+1.1")]
        [InlineData("1.")]
        public void IsNumericTest_True(string str)
        {
            Assert.True(NumericJudgment.IsNumeric(str));
        }

        [Theory]
        [InlineData("1.1D")]
        [InlineData("10L")]
        [InlineData("1000000000000000000000000000000000000000")]
        [InlineData("LOVE")]
        [InlineData("")]
        [InlineData(null)]
        public void IsNumericTest_False(string str)
        {
            Assert.False(NumericJudgment.IsNumeric(str));
        }
    }
}

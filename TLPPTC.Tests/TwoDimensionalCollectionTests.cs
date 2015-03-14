using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class TwoDimensionalCollectionTests
	{
		private class TestValue
		{
		}

		[Fact]
		public void X_outside_range_throws_exception()
		{
			var sut = new TwoDimensionalCollection<TestValue>();
			sut[0, 0] = new TestValue();

			sut[1, 0].Should().Be(null);
		}

		[Fact]
		public void Y_outside_range_gives_null()
		{
			var sut = new TwoDimensionalCollection<TestValue>();
			sut[0, 0] = new TestValue();

			sut[0, 1].Should().Be(null);
		}

		[Fact]
		public void Retrieves_value_at_coordinate()
		{
			var sut = new TwoDimensionalCollection<TestValue>();
			var testValue = new TestValue();
			sut[0, 0] = testValue;

			sut[0, 0].Should().Be(testValue);
		}
	}
}

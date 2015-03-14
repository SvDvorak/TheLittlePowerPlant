using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class NonStupidLookupTests
	{
		[Fact]
		public void Append_single_value_adds_it()
		{
			var sut = new NonStupidLookup<string, int>();

			sut.Append("HI", 1);

			sut["HI"].Should().HaveCount(1);
			sut["HI"].Single().Should().Be(1);
		}

		[Fact]
		public void Append_multiple_values_adds_them_all()
		{
			var sut = new NonStupidLookup<string, int>();

			sut.Append("HI", new[] { 1, 2, 3 });

			var values = sut["HI"].ToList();
			values.Should().HaveCount(3);
			values[0].Should().Be(1);
			values[1].Should().Be(2);
			values[2].Should().Be(3);
		}

		[Fact]
		public void Has_key_when_it_has_been_added()
		{
			var sut = new NonStupidLookup<string, int>();

			sut.Append("HI", new[] { 1, 2, 3 });

			sut.HasKey("HI").Should().BeTrue();
		}


		[Fact]
		public void Does_not_have_key_when_it_hasnt_been_added()
		{
			var sut = new NonStupidLookup<string, int>();

			sut.HasKey("HI").Should().BeFalse();
		}

		[Fact]
		public void Retrieves_values_for_key_using_index()
		{
			var sut = new NonStupidLookup<string, int>();
			sut.Append("1", 1);
			sut.Append("2", 2);

			var keyGroup = sut.GetKeyGroupByIndex(1);

			keyGroup.ShouldAllBeEquivalentTo(new[] { 1 });
		}
	}
}

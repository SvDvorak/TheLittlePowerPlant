using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace TLPPTC.Tests
{
	public class ConnectionsFinderTests
	{
		private readonly ConnectionsFinder _sut;

		public ConnectionsFinderTests()
		{
			_sut = new ConnectionsFinder();
		}

		[Fact]
		public void Returns_existing_connections()
		{
			var connections = _sut.FindConnections(name => new object());

			connections[0].Should().Be("11111111");
		}

		[Fact]
		public void Returns_missing_connections()
		{
			var connections = _sut.FindConnections(name => null);

			connections[0].Should().Be("00000000");
		}

		[Fact]
		public void Separates_between_left_and_right_connections()
		{
			var connections = _sut.FindConnections(name => name.Contains("_L") ? new object() : null);

			connections[0].Should().Be("10101010");
		}

		[Fact]
		public void Subdivides_and_rotates_connections()
		{
			var connections = _sut.FindConnections(name => name.Contains("_S_") || name.Contains("_N_") ? new object() : null);

			connections.ShouldAllBeEquivalentTo(new List<string>()
				{
					"11001100",
					"00110011",
					"11001100",
					"00110011",
					"110011",
					"001100",
					"110011",
					"001100",
					"1100",
					"0011",
					"1100",
					"0011",
					"11",
					"00",
					"11",
					"00"
				});
		}
	}
}

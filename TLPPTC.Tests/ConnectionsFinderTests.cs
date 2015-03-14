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
		private TestExitRetriever _testExitRetriever;

		public ConnectionsFinderTests()
		{
			_testExitRetriever = new TestExitRetriever();
			_sut = new ConnectionsFinder(_testExitRetriever);
		}

		[Fact]
		public void Returns_existing_connections()
		{
			_testExitRetriever.SetExitCondition(name => new object());
			var connections = _sut.FindConnectionSets(null).ToList();

			connections[0].Connections.Should().Be("11111111");
		}

		[Fact]
		public void Returns_missing_connections()
		{
			_testExitRetriever.SetExitCondition(name => null);
			var connections = _sut.FindConnectionSets(null).ToList();

			connections[0].Connections.Should().Be("00000000");
		}

		[Fact]
		public void Separates_between_left_and_right_connections()
		{
			_testExitRetriever.SetExitCondition(name => name.Contains("_L") ? new object() : null);
			var connectionSets = _sut.FindConnectionSets(null).ToList();

			connectionSets[0].Connections.Should().Be("10101010");
		}

		[Fact]
		public void Subdivides_and_rotates_connections()
		{
			_testExitRetriever.SetExitCondition(name => name.Contains("_S_") || name.Contains("_N_") ? new object() : null);
			var connectionSets = _sut.FindConnectionSets(null);

			connectionSets.Select(x => x.Connections).ShouldAllBeEquivalentTo(new List<string>()
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

			connectionSets.Select(x => x.Rotation).Should().ContainInOrder(new List<int>
				{
					0,
					3,
					2,
					1,
					0,
					3,
					2,
					1,
					0,
					3,
					2,
					1,
					0,
					3,
					2,
					1,
				});
		}

		[Fact]
		public void Retrieves_complete_connections_for_tile()
		{
			_testExitRetriever.SetExitCondition(name => name.Contains("_S_") || name.Contains("_N_") ? new object() : null);
			var connections = _sut.GetCompleteConnectionsOriented(null, 0);

			connections.Should().Be("11001100");
		}
	}

	public class TestExitRetriever : IExitRetriever
	{
		private Func<string, object> _conditionFunc;

		public void SetExitCondition(Func<string, object> conditionFunc)
		{
			_conditionFunc = conditionFunc;
		}

		public object GetExits(object tile, string name)
		{
			return _conditionFunc(name);
		}
	}
}

using System;
using Microsoft.Extensions.Internal;

namespace UnitTests.Mocks
{
	public class SystemClockMock : ISystemClock
	{
		#region Properties

		public virtual DateTimeOffset UtcNow { get; set; }

		#endregion
	}
}
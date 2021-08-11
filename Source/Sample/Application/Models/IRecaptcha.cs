using System;

namespace Application.Models
{
	public interface IRecaptcha
	{
		#region Properties

		bool Enabled { get; }
		Uri ScriptUrl { get; }
		string SiteKey { get; }
		string TokenParameterName { get; }

		#endregion
	}
}
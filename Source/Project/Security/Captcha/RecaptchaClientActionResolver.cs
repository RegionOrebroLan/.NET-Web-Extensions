using System.Text.RegularExpressions;
using RegionOrebroLan.ServiceLocation;

namespace RegionOrebroLan.Web.Security.Captcha
{
	[ServiceConfiguration(InstanceMode = InstanceMode.Singleton, ServiceType = typeof(IRecaptchaClientActionResolver))]
	public class RecaptchaClientActionResolver : IRecaptchaClientActionResolver
	{
		#region Fields

		/// <summary>
		/// [ : beginning of character group
		/// ^ : not matching the characters in the group, https://docs.microsoft.com/en-us/dotnet/standard/base-types/character-classes-in-regular-expressions#negative-character-group-
		/// a-z : any lowercase letter
		/// A-Z : any uppercase letter
		/// _ : underscore
		/// ] : end of character group
		/// </summary>
		private const string _pattern = "[^a-zA-Z_]";

		private Regex _regularExpression;

		#endregion

		#region Properties

		protected internal virtual Regex RegularExpression => this._regularExpression ?? (this._regularExpression = new Regex(_pattern, RegexOptions.Compiled));

		#endregion

		#region Methods

		/// <summary>
		/// The action-value in:
		/// grecaptcha.execute("[SITE-KEY]", { action: action })
		/// can only contain a-z, A-Z and _
		/// It can not be longer than 100 characters.
		/// </summary>
		public virtual string Resolve(string action)
		{
			if(string.IsNullOrEmpty(action))
				return action;

			var resolvedAction = this.RegularExpression.Replace(action, "_");

			if(resolvedAction.Length > 100)
				resolvedAction = resolvedAction.Substring(0, 100);

			return resolvedAction;
		}

		#endregion
	}
}
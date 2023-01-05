using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Models.Extensions
{
	[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
	public static class UriExtension
	{
		#region Methods

		public static string PathAndQueryAndFragment(this Uri uri)
		{
			if(uri == null)
				throw new ArgumentNullException(nameof(uri));

			if(!uri.IsAbsoluteUri)
				throw new ArgumentException("The uri must be absolute.", nameof(uri));

			return uri.PathAndQuery + uri.Fragment;
		}

		#endregion
	}
}
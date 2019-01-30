using System;

namespace SampleApplication.Business.Extensions
{
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
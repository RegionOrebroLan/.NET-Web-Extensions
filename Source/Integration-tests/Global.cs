using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Web.Security.Captcha;

namespace RegionOrebroLan.Web.IntegrationTests
{
	// ReSharper disable PossibleNullReferenceException
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		public static readonly string ProjectDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
		[CLSCompliant(false)] public static readonly IConfiguration Configuration = new ConfigurationBuilder().SetBasePath(ProjectDirectoryPath).AddJsonFile("AppSettings.json").Build();
		public static readonly IRecaptchaSettings RecaptchaSettings = Configuration.GetSection(typeof(RecaptchaSettings).Name).Get<RecaptchaSettings>();

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static void Cleanup() { }

		[AssemblyInitialize]
		[SuppressMessage("Usage", "CA1801:Review unused parameters")]
		public static void Initialize(TestContext testContext) { }

		#endregion
	}

	// ReSharper restore PossibleNullReferenceException
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
	// ReSharper disable PossibleNullReferenceException
	[TestClass]
	[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
	public static class Global
	{
		#region Fields

		public static readonly string ProjectDirectoryPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

		#endregion

		#region Methods

		[AssemblyCleanup]
		public static void Cleanup() { }

		public static IConfiguration CreateConfiguration(params string[] jsonFilePaths)
		{
			return CreateConfiguration(false, jsonFilePaths);
		}

		public static IConfiguration CreateConfiguration(bool optional, params string[] jsonFilePaths)
		{
			var configurationBuilder = CreateConfigurationBuilder(optional, jsonFilePaths);

			return configurationBuilder.Build();
		}

		public static IConfigurationBuilder CreateConfigurationBuilder(params string[] jsonFilePaths)
		{
			return CreateConfigurationBuilder(false, jsonFilePaths);
		}

		public static IConfigurationBuilder CreateConfigurationBuilder(bool optional, params string[] jsonFilePaths)
		{
			var configurationBuilder = new ConfigurationBuilder().SetBasePath(ProjectDirectoryPath);

			foreach(var path in jsonFilePaths)
			{
				configurationBuilder.AddJsonFile(path, optional, true);
			}

			return configurationBuilder;
		}

		public static IConfigurationBuilder CreateDefaultConfigurationBuilder()
		{
			return CreateConfigurationBuilder("appsettings.json");
		}

		public static IServiceCollection CreateServices(IConfiguration configuration)
		{
			var services = new ServiceCollection();

			services.AddSingleton(configuration);
			services.AddSingleton<LoggerFactory>();
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
			});

			return services;
		}

		[AssemblyInitialize]
		public static void Initialize(TestContext testContext)
		{
			if(testContext == null)
				throw new ArgumentNullException(nameof(testContext));
		}

		#endregion
	}

	// ReSharper restore PossibleNullReferenceException
}
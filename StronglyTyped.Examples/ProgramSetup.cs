using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using StronglyTyped.GuidIds;

namespace ExampleService
{
	public class ProgramSetup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonOptions(FixJsonCamelCasing);
			services.AddLogging();
			services.AddEntityFrameworkNpgsql().AddDbContext<EntityFrameworkContext>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}

		private void FixJsonCamelCasing(JsonOptions options)
		{
			options.JsonSerializerOptions.PropertyNamingPolicy = null;
		}
	}
}

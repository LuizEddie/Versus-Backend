

using RazorLight;

namespace Versus.Services
{
    public class EmailTemplateService
    {
        private readonly RazorLightEngine _engine;

        public EmailTemplateService()
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
                .UseMemoryCachingProvider().Build();
        }

        public async Task<string> RenderTemplateAsync<T>(string templateName, T model)
        {
            return await _engine.CompileRenderAsync(templateName, model);
        }
    }
}

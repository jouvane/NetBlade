using RazorLight;
using System;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.RazorTemplateEngine
{
    public class RazorTemplateEngine
    {
        private readonly RazorLightEngine _engine;

        public RazorTemplateEngine()
        {
            this._engine = new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build();
        }

        public RazorTemplateEngine(Type type)
        {
            this._engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(type)
                .UseMemoryCachingProvider()
                .Build();
        }

        public RazorTemplateEngine(Assembly assembly, string rootNamespace = null)
        {
            this._engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(assembly, rootNamespace)
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> Parse(string templateKey, string template, object data, ExpandoObject viewBag = null)
        {
            return await this._engine.CompileRenderStringAsync(templateKey, template, data, viewBag);
        }

        public async Task<string> Parse(string templateKey, object data, ExpandoObject viewBag = null)
        {
            return await this._engine.CompileRenderAsync(templateKey, data, viewBag);
        }
    }
}

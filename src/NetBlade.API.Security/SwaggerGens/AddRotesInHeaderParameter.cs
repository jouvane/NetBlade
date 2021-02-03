using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetBlade.API.Security.Options;
using NetBlade.Core.Security.TokenManager.Jwt;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetBlade.API.Security.SwaggerGens
{
    public class AddRotesInHeaderParameter : IOperationFilter
    {
        private readonly IOptions<AddRotesInHeaderParameterOption> _addRotesInHeaderParameterOption;
        private readonly JwtSecurityTokenManager _jwtSecurityTokenManager;

        public AddRotesInHeaderParameter(IOptions<AddRotesInHeaderParameterOption> addRotesInHeaderParameterOption, JwtSecurityTokenManager jwtSecurityTokenManager)
        {
            this._addRotesInHeaderParameterOption = addRotesInHeaderParameterOption;
            this._jwtSecurityTokenManager = jwtSecurityTokenManager;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            IEnumerable<Claim> claims = this._addRotesInHeaderParameterOption.Value.Roles?.Select(s => new Claim(ClaimTypes.Role, s));
            string token;
            if (claims != null && claims.Any())
            {
                token = this._jwtSecurityTokenManager.GetToken(claims);
            }
            else
            {
                token = string.Empty;
            }

            operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = $"Roles",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(token),
                    Description = $"Roles",
                }
            });
        }
    }
}

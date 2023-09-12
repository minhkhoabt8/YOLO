using Microsoft.AspNetCore.Mvc;
using SharedLib.Authorization;

namespace SharedLib.Attributes;

public class AuthorizeInternalServiceAttribute: TypeFilterAttribute
{
    public AuthorizeInternalServiceAttribute() : base(typeof(AuthorizeInternalServiceFilter))
    {
    }
}
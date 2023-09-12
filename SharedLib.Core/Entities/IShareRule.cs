using System.Security.Claims;

namespace SharedLib.Core.Entities;

public interface IShareRule
{
    bool IsEligible(ClaimsPrincipal principal);
}
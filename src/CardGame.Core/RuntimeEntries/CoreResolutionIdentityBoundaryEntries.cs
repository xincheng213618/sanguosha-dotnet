#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreResolutionIdentityBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _resolve;
    private CoreResolutionIdentityBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreResolutionIdentityBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed partial class CoreResolutionIdentityBoundaryEntries
{
    private readonly Func<EngineOwners.CoreResolutionIdentityBoundaryRuntime> _resolve;
    private CoreResolutionIdentityBoundaryEntries(Func<EngineOwners.CoreResolutionIdentityBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreResolutionIdentityBoundaryEntries Create(Func<EngineOwners.CoreResolutionIdentityBoundaryRuntime> resolve) => new(resolve);
}

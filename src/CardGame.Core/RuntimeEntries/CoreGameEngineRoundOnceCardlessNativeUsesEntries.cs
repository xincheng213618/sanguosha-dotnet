#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessNativeUsesEntries(Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessNativeUsesEntries Create(Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> resolve) => new(resolve);
}

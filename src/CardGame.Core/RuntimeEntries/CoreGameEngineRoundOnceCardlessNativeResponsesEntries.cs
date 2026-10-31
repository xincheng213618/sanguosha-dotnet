#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessNativeResponsesEntries(Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessNativeResponsesEntries Create(Func<EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> resolve) => new(resolve);
}

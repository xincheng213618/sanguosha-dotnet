#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessNativeResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessNativeResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessNativeUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessNativeUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> resolve) => new(resolve);
}

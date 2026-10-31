#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessProgramRequestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessProgramRequestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> resolve) => new(resolve);
}

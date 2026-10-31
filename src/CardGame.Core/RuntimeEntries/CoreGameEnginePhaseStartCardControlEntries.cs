#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime))]
internal sealed partial class CoreGameEnginePhaseStartCardControlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> _resolve;
    private CoreGameEnginePhaseStartCardControlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartCardControlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> resolve) => new(resolve);
}

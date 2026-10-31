#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime))]
internal sealed partial class CoreGameEngineRoundDistinctBasicUseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicUseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicUseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> resolve) => new(resolve);
}

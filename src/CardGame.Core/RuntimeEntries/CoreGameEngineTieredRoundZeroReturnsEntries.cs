#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> resolve) => new(resolve);
}

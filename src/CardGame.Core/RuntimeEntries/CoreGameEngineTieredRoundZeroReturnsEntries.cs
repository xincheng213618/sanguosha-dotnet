#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroReturnsEntries(Func<EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroReturnsEntries Create(Func<EngineOwners.CoreGameEngineTieredRoundZeroReturnsRuntime> resolve) => new(resolve);
}

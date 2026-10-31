#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEndRefillNextTurnRuntime))]
internal sealed partial class CoreGameEngineEndRefillNextTurnEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> _resolve;
    private CoreGameEngineEndRefillNextTurnEntries(Func<EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndRefillNextTurnEntries Create(Func<EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> resolve) => new(resolve);
}

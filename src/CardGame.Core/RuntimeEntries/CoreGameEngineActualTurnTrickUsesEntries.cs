#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnTrickUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> _resolve;
    private CoreGameEngineActualTurnTrickUsesEntries(Func<EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnTrickUsesEntries Create(Func<EngineOwners.CoreGameEngineActualTurnTrickUsesRuntime> resolve) => new(resolve);
}

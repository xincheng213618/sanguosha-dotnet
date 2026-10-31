#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHulaoCardsRuntime))]
internal sealed partial class CoreGameEngineHulaoCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHulaoCardsRuntime> _resolve;
    private CoreGameEngineHulaoCardsEntries(Func<EngineOwners.CoreGameEngineHulaoCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHulaoCardsEntries Create(Func<EngineOwners.CoreGameEngineHulaoCardsRuntime> resolve) => new(resolve);
}

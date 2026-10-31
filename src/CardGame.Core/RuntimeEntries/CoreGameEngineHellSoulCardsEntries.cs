#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed partial class CoreGameEngineHellSoulCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHellSoulCardsRuntime> _resolve;
    private CoreGameEngineHellSoulCardsEntries(Func<EngineOwners.CoreGameEngineHellSoulCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHellSoulCardsEntries Create(Func<EngineOwners.CoreGameEngineHellSoulCardsRuntime> resolve) => new(resolve);
}

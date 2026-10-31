#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLiushiActivationCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> _resolve;
    private CoreGameEngineLiushiActivationCardsEntries(Func<EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiushiActivationCardsEntries Create(Func<EngineOwners.CoreGameEngineLiushiActivationCardsRuntime> resolve) => new(resolve);
}

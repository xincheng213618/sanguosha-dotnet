#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateCardOffersRuntime))]
internal sealed partial class CoreGameEnginePrivateCardOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateCardOffersRuntime> _resolve;
    private CoreGameEnginePrivateCardOffersEntries(Func<EngineOwners.CoreGameEnginePrivateCardOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateCardOffersEntries Create(Func<EngineOwners.CoreGameEnginePrivateCardOffersRuntime> resolve) => new(resolve);
}

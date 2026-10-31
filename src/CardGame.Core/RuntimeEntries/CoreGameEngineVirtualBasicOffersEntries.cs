#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed partial class CoreGameEngineVirtualBasicOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _resolve;
    private CoreGameEngineVirtualBasicOffersEntries(Func<EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineVirtualBasicOffersEntries Create(Func<EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> resolve) => new(resolve);
}

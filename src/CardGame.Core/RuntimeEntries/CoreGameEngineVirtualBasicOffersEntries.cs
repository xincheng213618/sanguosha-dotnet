#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed partial class CoreGameEngineVirtualBasicOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _resolve;
    private CoreGameEngineVirtualBasicOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineVirtualBasicOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> resolve) => new(resolve);
}

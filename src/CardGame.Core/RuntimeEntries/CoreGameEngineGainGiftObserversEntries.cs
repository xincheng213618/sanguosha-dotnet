#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftObserversRuntime))]
internal sealed partial class CoreGameEngineGainGiftObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftObserversRuntime> _resolve;
    private CoreGameEngineGainGiftObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGainGiftObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftObserversRuntime> resolve) => new(resolve);
}

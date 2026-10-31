#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGainGiftObserversRuntime))]
internal sealed partial class CoreGameEngineGainGiftObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGainGiftObserversRuntime> _resolve;
    private CoreGameEngineGainGiftObserversEntries(Func<EngineOwners.CoreGameEngineGainGiftObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGainGiftObserversEntries Create(Func<EngineOwners.CoreGameEngineGainGiftObserversRuntime> resolve) => new(resolve);
}

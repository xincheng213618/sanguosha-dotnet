#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedCardGiftsRuntime))]
internal sealed partial class CoreGameEngineCompletedCardGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> _resolve;
    private CoreGameEngineCompletedCardGiftsEntries(Func<EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedCardGiftsEntries Create(Func<EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> resolve) => new(resolve);
}

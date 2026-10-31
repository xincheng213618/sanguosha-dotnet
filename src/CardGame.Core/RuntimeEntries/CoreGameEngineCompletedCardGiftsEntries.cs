#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime))]
internal sealed partial class CoreGameEngineCompletedCardGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> _resolve;
    private CoreGameEngineCompletedCardGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedCardGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCardGiftsRuntime> resolve) => new(resolve);
}

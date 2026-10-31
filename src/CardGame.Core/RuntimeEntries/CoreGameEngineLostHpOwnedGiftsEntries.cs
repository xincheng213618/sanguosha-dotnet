#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLostHpOwnedGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _resolve;
    private CoreGameEngineLostHpOwnedGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLostHpOwnedGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> resolve) => new(resolve);
}

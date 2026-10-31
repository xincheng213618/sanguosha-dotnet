#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> _resolve;
    private CoreGameEngineFinalTargetGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> resolve) => new(resolve);
}

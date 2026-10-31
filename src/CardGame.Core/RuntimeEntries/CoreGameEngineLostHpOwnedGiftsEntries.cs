#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime))]
internal sealed partial class CoreGameEngineLostHpOwnedGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> _resolve;
    private CoreGameEngineLostHpOwnedGiftsEntries(Func<EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLostHpOwnedGiftsEntries Create(Func<EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime> resolve) => new(resolve);
}

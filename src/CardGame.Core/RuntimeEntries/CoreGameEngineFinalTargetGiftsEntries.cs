#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFinalTargetGiftsRuntime))]
internal sealed partial class CoreGameEngineFinalTargetGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> _resolve;
    private CoreGameEngineFinalTargetGiftsEntries(Func<EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetGiftsEntries Create(Func<EngineOwners.CoreGameEngineFinalTargetGiftsRuntime> resolve) => new(resolve);
}

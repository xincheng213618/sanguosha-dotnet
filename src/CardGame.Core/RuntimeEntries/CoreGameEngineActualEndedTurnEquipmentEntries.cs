#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime))]
internal sealed partial class CoreGameEngineActualEndedTurnEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> _resolve;
    private CoreGameEngineActualEndedTurnEquipmentEntries(Func<EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualEndedTurnEquipmentEntries Create(Func<EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> resolve) => new(resolve);
}

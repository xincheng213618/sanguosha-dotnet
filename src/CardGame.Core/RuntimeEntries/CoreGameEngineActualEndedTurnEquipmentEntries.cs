#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime))]
internal sealed partial class CoreGameEngineActualEndedTurnEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> _resolve;
    private CoreGameEngineActualEndedTurnEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualEndedTurnEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualEndedTurnEquipmentRuntime> resolve) => new(resolve);
}

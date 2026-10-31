#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed partial class CoreGameEngineEquipmentSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _resolve;
    private CoreGameEngineEquipmentSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> resolve) => new(resolve);
}

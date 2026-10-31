#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed partial class CoreGameEngineEquipmentSkillsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _resolve;
    private CoreGameEngineEquipmentSkillsEntries(Func<EngineOwners.CoreGameEngineEquipmentSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSkillsEntries Create(Func<EngineOwners.CoreGameEngineEquipmentSkillsRuntime> resolve) => new(resolve);
}

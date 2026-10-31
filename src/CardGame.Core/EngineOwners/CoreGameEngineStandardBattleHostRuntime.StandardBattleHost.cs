#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineStandardBattleHostRuntime
{
    internal IStandardBattleHost CreateStandardBattleHost()
    {
        if (_coreMatchDefinitionOptionsAccess._options.ShanheBattle is not null && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(ShanheSourceDongdangRules.SkillId, out var definition))
            ShanheSourceDongdangRules.ValidateDefinition(definition);
        return _kernelCoreGameEngineStandardBattleHostOperations.CreateStandardBattleHost();
    }
}

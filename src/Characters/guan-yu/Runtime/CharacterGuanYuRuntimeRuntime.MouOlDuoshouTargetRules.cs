#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 50)]
    internal bool GuanFirstBasicSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => GuanFirstBasicAvailable(query.Actor, query.Kind);
}

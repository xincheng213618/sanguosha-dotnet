#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQualification, 10)]
    internal bool MoGangquanSlashTargetQualified(ProgramSlashTargetQuery query) => MoGangquanTargetAllowed(query.Actor, query.Target, query.Kind, query.Source, query.PhysicalCardIds);
}

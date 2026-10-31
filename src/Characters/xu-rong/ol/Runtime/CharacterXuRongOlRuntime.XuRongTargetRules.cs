#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuRongOlRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 60)]
    internal bool XuRongDirectedTargetProhibited(ProgramDirectedTargetQuery query) => IsXuRongSlashTargetForbidden(query.ActorSeat, query.TargetSeat, query.Kind);
}

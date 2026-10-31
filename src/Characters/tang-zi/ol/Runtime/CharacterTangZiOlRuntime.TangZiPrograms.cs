#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangZiOlRuntime
{
    internal bool MatchesTangZiWoundedGrant(CharacterState owner, SkillProgramHpSkillGrant rule) => XingzhaoRuntime.MatchesWoundedGrant(owner.Seat, rule, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
    bool ISelectionWoundedQueries.IdentityConcealed(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IdentityConcealed;
    bool ISelectionWoundedQueries.IsRuleWounded(int seat) => _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]);
}

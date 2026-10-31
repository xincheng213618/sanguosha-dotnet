#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangZiOlRuntime
{
    internal bool MatchesTangZiWoundedGrant(CharacterState owner, SkillProgramHpSkillGrant rule) => XingzhaoRuntime.MatchesWoundedGrant(owner.Seat, rule, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
    bool global::CardGame.Core.ISelectionWoundedQueries.IdentityConcealed(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IdentityConcealed;
    bool global::CardGame.Core.ISelectionWoundedQueries.IsRuleWounded(int seat) => _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[seat]);
}

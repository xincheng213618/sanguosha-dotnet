#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTeamActivitiesRuntime
{
    internal bool SamePublicTeam(CharacterState a, CharacterState b) => _coreModeAllegianceOperations.ModeAllegiancePolicy.SamePublicTeam(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(a), global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(b));
    internal bool HasActivitySkill(CharacterState player, string id) => player.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(player).HasSkill(id);
    internal (CardUseEffectSource Source, int Cap)? JiangeDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (_modeRuntimeOperations._modeRuntime.Find<IJiangeBattleFeature>()?.AlliedDamageCap(attack.TargetSeat)is not { } plan)
            return null;
        var owner = _coreMatchParticipantsPlayersAccess._players[plan.OwnerSeat];
        var grant = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.First(g => g.SkillId == plan.SkillId);
        return (new CardUseEffectSource(grant.SkillId, plan.BindingId, owner.Seat, grant.SkillInstanceId), plan.Maximum);
    }
}

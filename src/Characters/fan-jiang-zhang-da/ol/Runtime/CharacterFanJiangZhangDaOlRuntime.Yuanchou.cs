#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanJiangZhangDaOlRuntime
{
    internal bool HasYuanchouArmorBypass(long useId, int targetSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { Type: CardActionType.Use, EffectiveIsRed: false } action } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action) != false || !use.TargetSeats.Contains(targetSeat) || action.ActorSeat != use.SourceSeat || action.ActorSeat == targetSeat)
            return false;
        return _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections) || _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[targetSeat], SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections);
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanJiangZhangDaOlRuntime
{
    internal bool HasYuanchouArmorBypass(long useId, int targetSeat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { Type: CardActionType.Use, EffectiveIsRed: false } action } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action) != false || !use.TargetSeats.Contains(targetSeat) || action.ActorSeat != use.SourceSeat || action.ActorSeat == targetSeat)
            return false;
        return _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections) || _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[targetSeat], SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections);
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal bool HasCaoYuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGongjieRoundFirstGifts) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXiangxuMinimumTransition) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXiangzuDyingGiftAndRecovery);

    internal static CardConversionSource CaoYuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_cao_yu_ol_CaoYuLedger.CaoYuSource(f);
    internal bool CaoYuActualTurnOpen(int turn, int owner) => turn > 0 && turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber && owner == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == turn && e.OwnerSeat == owner);
    internal bool CaoYuHistory(int owner, string skill, int recipient) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoYuRecipientHistoryEvent>().Any(e => e.OwnerSeat == owner && e.SkillId == skill && e.RecipientSeat == recipient);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27440)]
    internal bool QualifyCaoYu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [{ } e] || !CaoYuComposition.Handles(e.Op))
            return true;
        return e.Op switch
        {
            SkillProgramEffectOp.ResolveGongjieRoundFirstGifts => CaoYuRoundFirst(c, context),
            SkillProgramEffectOp.ResolveXiangxuMinimumTransition => IsXiangxuMinimumCandidate(c, context),
            SkillProgramEffectOp.ResolveXiangxuIssuedTurnEndAdjustment => IsIssuedXiangxuEndingCandidate(c, context),
            _ => context.Window == SkillProgramTriggerWindow.DyingEntering && context.TargetSeat == c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && !XiangzuSpent(c.OwnerSeat) && CaoYuGiftCards(c.OwnerSeat, c.OwnerSeat).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat)};
    }
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaDaiMobileRuntime
{
    SkillProgramStepOutcome IMobileMaDaiProgramHost.ReduceDamageTargetMaximum(ProgramSkillFrame frame) => ReduceMobileMaDaiTargetMaximum(frame);
    // Before-damage facts have no captured damage-distance value: qualify the actual
    // source here, once, before issuing the original owner judgment.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 72901, SkillProgramEffectOp.ReduceMobileMaDaiDamageTargetMaximumHp)]
    internal bool CanOfferMobileMaDaiQianxi(ProgramTriggerLegalityContext gate) => gate.SkillId == MobileMaDaiComposition.Skill && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, IsChainPropagation: false, DamageWasApplied: false, Card: not null, EffectiveCardKind: { } kind } attack && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.SourceSeat == gate.OwnerSeat && attack.CardUserSeat == gate.OwnerSeat && attack.TargetSeat != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat] is { IsAlive: true } && _coreNativeRuleQueriesOperations.GetCombatDistance(gate.OwnerSeat, attack.TargetSeat) == 1;
    internal SkillProgramStepOutcome ReduceMobileMaDaiTargetMaximum(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileMaDaiComposition.Skill || frame.InstructionIndex != 3 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } targetSeat } context || context.SourceSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not BeforeDamageProgramWindowFrame { Prevented: true } window || window.SourceSeat != frame.OwnerSeat || window.TargetSeat != targetSeat || window.RedirectedTargetSeat is not null || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, IsChainPropagation: false, DamageWasApplied: false } attack || attack.SourceSeat != frame.OwnerSeat || attack.TargetSeat != targetSeat || frame.JudgmentOutcomeBindings is not [{ Name: "qianxi-outcome", TargetSeat: var judgedSeat, Suit: Suit.Spade or Suit.Club or Suit.Diamond } outcome] || judgedSeat != frame.OwnerSeat || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(frame, outcome) || _coreMatchParticipantsPlayersAccess._players[targetSeat] is not { IsAlive: true } target || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileMaDaiMaximumReducedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Mobile Qianxi requires its exact prevented original Slash and finalized non-heart owner judgment.");
        var before = target.MaxHp;
        var after = _coreWushuangTacticRulesOperations.HasWushuangTactic(targetSeat, 2020) ? 7 : target.HasShenPangTongKunyu ? 1 : Math.Max(0, checked(before - 1));
        if (!_characterDaQiaoXiaoQiaoHappyGodOperations.TryApplyHappyQiaoMaximumHp(frame.Id, targetSeat, after, frame.SkillId))
        {
            var hp = target.Hp;
            target.MaxHp = after;
            target.Hp = Math.Min(hp, after);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(targetSeat, after - before, after, frame.SkillId) { HpBeforeClamp = hp });
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaDaiMaximumReducedEvent(frame.Id, window.Id, attack.ResolutionId, outcome.JudgmentFrameId, frame.OwnerSeat, targetSeat, before, after));
        // The executor has committed this instruction before native HP/replacement
        // children run. Their typed Program return resumes cleanup, never repayment.
        if (after == 0 && target.IsAlive)
        {
            _coreNativePlayerDeathOperations.BeginPlayerDeath(frame.Id, target, null, null, null, causingProgramSkillFrameId: frame.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }
}

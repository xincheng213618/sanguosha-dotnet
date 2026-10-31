#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceHuojieRuntime
{
    internal SkillProgramStepOutcome StartSourceHuojie(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var count = _coreSourceBeigongOperations.SourceDingxiCount(f.OwnerSeat);
        if (count <= _coreMatchParticipantsPlayersAccess._players.Count)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceHuojie = new(count, 0, SourceHuojieStage.JudgmentChildren) });
        ContinueSourceHuojie(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool CanBeginSourceHuojieJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.SkillId == SourceBeigongIds.Huojie && f.SourceHuojie is { Stage: SourceHuojieStage.Judging, JudgmentId: > 0 } && f.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && target == f.OwnerSeat && reason == JudgmentReasons.Lightning && bind == "source-huojie-lightning" && visibility == SkillProgramCardSetVisibility.Public;
    internal bool IsSourceHuojieJudgment(JudgmentFrame j, ProgramSkillFrame f) => CanBeginSourceHuojieJudgment(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private) && f.SourceHuojie!.JudgmentId == j.Id && j.ParentFrameId == f.Id && j.SourceSeat == f.OwnerSeat && j.Continuation == JudgmentContinuationKind.ProgramSkill;
    internal bool IsSourceHuojieLightningJudgment(JudgmentFrame j) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == j.ParentFrameId)is { } f && IsSourceHuojieJudgment(j, f);
    internal bool ReturnSourceHuojieJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceHuojie: { Stage: SourceHuojieStage.Judging } r } f || j.ParentFrameId != f.Id)
            return false;
        if (!IsSourceHuojieJudgment(j, f))
            throw new InvalidOperationException("Source3603 lost its exact current Lightning judgment return.");
        var result = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Single(e => e.ResolutionId == j.Id && e.ParentResolutionId == f.Id && e.Reason == JudgmentReasons.Lightning);
        var handoff = result.CardId is { } resultId && _cardZonesOperations._cardZones.GetLocation(resultId) == CardLocation.Processing ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == resultId) : null;
        if (handoff is not null && (handoff.From != CardLocation.Judgment(f.OwnerSeat) || handoff.To != CardLocation.Processing || handoff.Reason.Value != "skill-program.judgment.result"))
            throw new InvalidOperationException("Source3603 cannot retire an unrelated judgment entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.JudgmentChildren, Hit = result.Succeeded, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, JudgmentCardId = handoff?.CardId ?? 0, JudgmentHandoffSequence = handoff?.Sequence ?? 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (result.CardId is { } id && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceHuojie = f.SourceHuojie!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
        return true;
    }

    internal void ContinueSourceHuojie(ProgramSkillFrame f)
    {
        var r = f.SourceHuojie!;
        if (r.Stage == SourceHuojieStage.Judging)
            return;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreSourceBeigongOperations.FinishSourceBeigong(f);
            return;
        }

        if (r.Stage == SourceHuojieStage.GainChildren)
        {
            _coreSourceBeigongOperations.FinishSourceBeigong(f);
            return;
        }

        if (r.Stage == SourceHuojieStage.DamageChildren)
        {
            var received = _coreProgramEventHistoryOperations.ProgramEventHistory<SourceHuojieReceivedDamageEvent>().Any(e => e.ProgramFrameId == f.Id && e.JudgmentId == r.JudgmentId);
            if (received)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.GainChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                foreach (var pile in _coreSourceBeigongOperations.SourceDingxiPiles(f.OwnerSeat))
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(_corePublicPileSourcesOperations.PublicPileCards(pile).Select(c => c.Id).ToArray(), CardLocation.Hand(f.OwnerSeat), new("skill-program.source-huojie.obtain"));
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceHuojie = f.SourceHuojie!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceHuojieCompletedEvent(f.Id, f.OwnerSeat, r.Done, true));
                _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
                return;
            }
        }
        else if (r.Hit)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.DamageChildren } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 3, nature: DamageNature.Thunder, sourceLess: true);
            _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
            return;
        }

        if (r.Done >= r.MaximumJudgments)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceHuojieCompletedEvent(f.Id, f.OwnerSeat, r.Done, false));
            _coreSourceBeigongOperations.FinishSourceBeigong(f);
            return;
        }

        var judgeId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceHuojie = r with { Stage = SourceHuojieStage.Judging, Done = r.Done + 1, JudgmentId = judgeId, Hit = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceHuojieJudgmentIssuedEvent(f.Id, judgeId, f.OwnerSeat, r.Done + 1, r.MaximumJudgments));
        _ = _coreNativeJudgmentOperations.BeginJudgment(null, f.OwnerSeat, JudgmentReasons.Lightning, f.Id, CardKind.Lightning, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "source-huojie-lightning", programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 24512)]
    internal void SourceHuojieActualReceivedDamage(DamageAppliedEvent fact)
    {
        if (!fact.SourceLess || fact.Nature != DamageNature.Thunder || fact.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attempt)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == attempt.ResolutionId && p.SkillId == SourceBeigongIds.Huojie && p.SourceHuojie is { Stage: SourceHuojieStage.DamageChildren });
        if (f is null || fact.TargetSeat != f.OwnerSeat || attempt.TargetSeat != f.OwnerSeat || attempt.SourceSeat != f.OwnerSeat || f.AttackAttempt is not { SourceLess: true, Nature: DamageNature.Thunder })
            return;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<SourceHuojieReceivedDamageEvent>().Any(e => e.ProgramFrameId == f.Id && e.JudgmentId == f.SourceHuojie!.JudgmentId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceHuojieReceivedDamageEvent(f.Id, f.SourceHuojie!.JudgmentId, f.OwnerSeat, fact.Amount));
    }
}

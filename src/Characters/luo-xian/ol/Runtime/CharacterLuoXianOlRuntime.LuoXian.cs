#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuoXianOlRuntime
{
    internal static CardConversionSource DailiSource(ProgramSkillFrame frame) => global::CardGame.Core.RuntimeSkills.RuntimeRules_luo_xian_ol_LuoXian.DailiSource(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26420)]
    internal bool CanOfferDaili(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveDailiTurnEnd) || context.Window == SkillProgramTriggerWindow.AfterTurnEnded && PubliclyRevealedHandCount(candidate.OwnerSeat) % 2 == 0;
    internal SkillProgramStepOutcome ResolveDailiTurnEnd(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.Daili is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DeferredTurnEndFrame parent || parent.Id != context.ParentFrameId || !_coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, frame, context) || !CanOfferDaili(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(frame), context))
            throw new InvalidOperationException("Daili requires its exact unissued actual completed-turn entitlement.");
        var receipt = new DailiTurnEndReceipt(DailiSource(frame), frame.GameplayHash, parent.Id, parent.TurnNumber, parent.OwnerSeat, frame.InstructionIndex, PubliclyRevealedHandCount(frame.OwnerSeat), _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsFaceDown);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Daili = new(receipt, DailiStage.FlipChildren) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DailiTurnEndIssuedEvent(frame.Id, receipt));
        _coreProgramLifecycleOperations.TurnOverProgramTarget(frame.Id, frame.OwnerSeat, frame.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DailiTurnedOverEvent(frame.Id, receipt.WasFaceDown, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsFaceDown));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainDailiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26420)]
    internal bool ResumeDaili(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Daili: { } pending } frame || frame.Id != id)
            return false;
        RequireDaili(frame);
        if (DrainDailiChildren(id))
            return true;
        if (pending.Stage == DailiStage.FlipChildren && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Daili = pending with { Stage = DailiStage.DrawChildren, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], 3, true, new("skill-program.daili.draw"));
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Daili = frame.Daili!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = drawn.Count } });
            foreach (var cardId in drawn)
            {
                var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.CardId == cardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.daili.draw");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DailiCardDrawnEvent(id, frame.OwnerSeat, movement.Sequence));
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DailiDrawPaidEvent(id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, drawn.Count));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        var reveals = pending.Stage == DailiStage.DrawChildren && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive ? DailiDrawnMovements(frame).Where(m => CurrentHandEntry(frame.OwnerSeat, m.CardId) == m.Sequence).Select(m => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId))).ToArray() : [];
        if (reveals.Length > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(frame.Id, Array.AsReadOnly(reveals)));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(frame.Id, "daili-drawn-hand"))
            return true;
        CompleteDailiAfterPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return true;
    }

    [PublicHandRevealReturn("daili-drawn-hand")]
    internal void CompleteDailiAfterPublicReveal(ProgramSkillFrame frame)
    {
        var pending = frame.Daili ?? throw new InvalidOperationException("Daili lost its issued Draw/reveal receipt.");
        var count = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardsRevealedEvent>().LastOrDefault(e => e.ResolutionId == frame.Id)?.Cards.Count ?? 0;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DailiTurnEndCompletedEvent(frame.Id, pending.Count, count));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Daili = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    internal IEnumerable<CardMovementRecord> DailiDrawnMovements(ProgramSkillFrame frame) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DailiCardDrawnEvent>().Where(e => e.FrameId == frame.Id).Select(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence == e.MovementSequence));
    [ProgramContinuation(ProgramContinuationKind.Movement, 26420)]
    internal bool ReturnDailiMovement(ProgramSkillFrame frame) => frame.Daili is not null && ResumeDaili(frame.Id);
    SkillProgramStepOutcome global::CardGame.Core.ILuoXianProgramHost.ResolveDailiTurnEnd(ProgramSkillFrame frame) => ResolveDailiTurnEnd(frame);
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreWenheActivityProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IWenheActivityProgramHost.ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => ExecuteWenheActivity(frame, op);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 41000)]
    internal bool AllowsWenheActivation(ProgramActivationLegalityContext gate) => _coreActivityBattleRulesOperations.WenheRules?.AllowsActivation(gate.SkillId, gate.OwnerSeat) ?? gate.SkillId is not ("wenhe:chaos-resolution" or "wenhe:champion-mark");
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41000)]
    internal bool AllowsWenheRuleTrigger(ProgramTriggerLegalityContext gate) => _coreActivityBattleRulesOperations.WenheRules?.AllowsTrigger(new(gate.SkillId, gate.Candidate.BindingId, gate.OwnerSeat, gate.WindowContext?.Window, gate.WindowContext?.SourceSeat, gate.WindowContext?.TargetSeat)) ?? gate.SkillId != "wenhe:rules";
    internal bool HasWenheDeadOwnerEnding(int actualOwner) => _coreActivityBattleRulesOperations.WenheRules?.HasDeadOwnerEnding(actualOwner) ?? false;
    internal bool IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int actualOwner) => _coreActivityBattleRulesOperations.WenheRules?.IsDeadOwnerEndingCandidate(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, actualOwner) ?? false;
    internal SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op)
    {
        if (frame.WenheRuleFlow is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WenheRuleFlowIssuedEvent>().Any(fact => fact.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Wenhe requires its exact once-issued activity operation.");
        var plan = (_coreActivityBattleRulesOperations.WenheRules ?? throw new InvalidOperationException("The activity rule feature is not installed.")).IssueFlow(frame.Id, op, frame.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { WenheRuleFlow = new(op, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, plan.Amount, 0, 0) { Seats = plan.Seats } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheRuleFlowIssuedEvent(frame.Id, op, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, frame.OwnerSeat, plan.Seats.Count, plan.Amount));
        for (var index = 0; index < plan.Seats.Count; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheRuleParticipantEvent(frame.Id, index, plan.Seats[index]));
        if (plan.BeginsChaos)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { WenheRuleFlow = frame.WenheRuleFlow!with { Stage = 3 } });
            _coreWenheChaosOperations.BeginWenheChaos(frame, null);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return ResumeWenheRuleFlow(frame.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41000)]
    internal bool ResumeWenheRuleFlow(long id) => _coreActivityBattleRulesOperations.WenheRules?.ResumeFlow(id) ?? false;
    internal bool TryBeginWenheRuleChildren(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardWenheKiller, SkillProgramEffectOp.ResolveWenheTurnStart, SkillProgramEffectOp.UseWenheChampionMark)]
    internal void ReturnWenheRuleDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        AssertWenheRuleFlow(f);
        var r = f.WenheRuleFlow!;
        var count = (_coreActivityBattleRulesOperations.WenheRules ?? throw new InvalidOperationException("The activity rule feature is not installed.")).DrawCount(r.Operation, r.Amount);
        if (r.Stage != 2 || outcome.RecipientSeat != r.Seats[r.Index] || outcome.RequestedCount != count)
            throw new InvalidOperationException("Wenhe draw lost its already paid participant.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(f.Id, outcome.RecipientSeat, null, SkillProgramCardSetVisibility.Private, new("wenhe.rule.draw"), 0, outcome.ActualDrawIds);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41000)]
    internal bool RetainWenheIssuedFlow(ProgramSkillFrame f) => f.WenheRuleFlow is not null || f.WenheChaos is not null && _coreWenheChaosOperations.IsExactWenheChaos(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 41000)]
    internal void AssertWenheRuleFlow(ProgramSkillFrame f)
    {
        if (f.WenheRuleFlow is not { } r)
            return;
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.WenheActivity is null || r.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.Index < 0 || r.Index > r.Seats.Count || r.Stage is < 0 or > 3 || r.Seats is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<WenheRuleFlowIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Operation == r.Operation && e.RoundNumber == r.RoundNumber && e.TurnNumber == r.TurnNumber && e.Count == r.Seats.Count && e.Amount == r.Amount && e.OwnerSeat == f.OwnerSeat) != 1 || r.Seats.Where((seat, index) => _coreProgramEventHistoryOperations.ProgramEventHistory<WenheRuleParticipantEvent>().Count(e => e.ProgramFrameId == f.Id && e.Index == index && e.Seat == seat) != 1).Any())
            throw new InvalidOperationException("Wenhe rule lost its frozen participants, paid cursor or originating fact.");
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.DeadOwnerEnding, 10)]
    internal bool ProveIsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate) => IsWenheDeadOwnerEndingCandidate(candidate, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
}

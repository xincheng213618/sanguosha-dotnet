#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeathGrowthSkillsRuntime
{
    internal const string DeathGrowthDrawReason = "skill-program.other-death-growth.draw";
    internal CardConversionSource DeathGrowthSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 310)]
    internal bool CanOfferDeathGrowthSkill(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [{ Op: SkillProgramEffectOp.DrawAfterOtherDeathThenChooseMissingSkill }])
            return true;
        if (context.Window != SkillProgramTriggerWindow.CharacterDied || context.TargetSeat is not { } victim || candidate.OwnerSeat == victim || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || _coreMatchParticipantsPlayersAccess._players[victim].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.VictimSeat != victim || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || window.Contexts[window.CandidateIndex] != context)
            return false;
        return _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == window.DeathFrameId)is { } death && death.VictimSeat == victim && !death.KillerProgramsResolved;
    }

    internal SkillProgramStepOutcome BeginDeathGrowthSkill(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.DeathGrowthSkill is not null || f.WindowContext is not { } c || !CanOfferDeathGrowthSkill(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Death growth lost its exact other-death candidate before the draw.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single(w => w.Id == c.ParentFrameId);
        var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(d => d.Id == window.DeathFrameId);
        var dying = death.ReturnKind == DeathReturnKind.Dying ? _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Single(d => d.Id == death.ParentFrameId) : null;
        var damage = dying?.Continuation == DyingContinuationKind.Damage ? _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Single(d => d.Id == dying.ParentFrameId) : null;
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DeathGrowthSkill = new(f.InstructionIndex, window.Id, window.DeathFrameId, window.VictimSeat, DeathGrowthSource(f), f.GameplayHash, Array.AsReadOnly(effect.SkillIds.ToArray()), effect.Amount, before, before, 0, OriginalDeathHash: global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime.DeathGrowthCursorHash(death), OriginalDyingFrameId: dying?.Id, OriginalDyingHash: dying is null ? null : global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime.DeathGrowthCursorHash(dying), OriginalDamageHash: damage is null ? null : global::CardGame.Core.EngineOwners.CoreGameEngineDeathGrowthSkillObserversRuntime.DeathGrowthCursorHash(damage)), PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], effect.Amount, true, new(DeathGrowthDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DeathGrowthSkill = f.DeathGrowthSkill!with { MovementAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence, DrawCount = drawn.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeathGrowthDrawIssuedEvent(f.Id, f.DeathGrowthSkill!));
        if (!DrainDeathGrowthChildren(f))
            AdvanceDeathGrowthChoice(f);
        return _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null ? SkillProgramStepOutcome.AwaitChoice : SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainDeathGrowthChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void AdvanceDeathGrowthChoice(ProgramSkillFrame f)
    {
        _coreDeathGrowthSkillObserversOperations.RequireDeathGrowthSkill(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishDeathGrowthSkill(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DeathGrowthSkill = f.DeathGrowthSkill!with { Stage = DeathGrowthSkillStage.ChooseSkill }, PendingMovementContinuation = null, Step = ResolutionFrameStep.AwaitingResponse });
        var choices = DeathGrowthChoices(f);
        if (choices.Count == 0)
        {
            FinishDeathGrowthSkill(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择获得一个尚未拥有的技能。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal IReadOnlyList<PromptChoice> DeathGrowthChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.DeathGrowthSkill!.SkillIds.Where(id => !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Any(g => g.IsEnabled && g.SkillId == id)).Select(id => new PromptChoice(new($"death-growth.{(f.Id)}.{(id)}"), $"获得【{(_contentRegistryOperations._contentRegistry.GetSkill(id).Name)}】", [], [], new Dictionary<string, string> { ["program-action"] = "death-growth-skill", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["skill-id"] = id })).ToArray());
    [ProgramAnswer("death-growth-skill")]
    internal void ResolveDeathGrowthChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A death-growth choice lost its owning frame.");
        _coreDeathGrowthSkillObserversOperations.RequireDeathGrowthSkill(f);
        var r = f.DeathGrowthSkill!;
        if (r.Stage != DeathGrowthSkillStage.ChooseSkill || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], DeathGrowthChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The selected death-growth skill is stale, already owned or belongs to another chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var id = selected.Parameters["skill-id"];
        _coreProgramSkillChangesOperations.GrantProgramSkills(f, [id]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeathGrowthSkillGrantedEvent(f.Id, r.DeathFrameId, f.OwnerSeat, r.VictimSeat, r.Source, id));
        FinishDeathGrowthSkill(f);
    }

    internal void FinishDeathGrowthSkill(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DeathGrowthSkill = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 21400)]
    internal bool ResumeDeathGrowthSkill(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DeathGrowthSkill.Stage: DeathGrowthSkillStage.DrawChildren } f || f.Id != id)
            return false;
        _coreDeathGrowthSkillObserversOperations.RequireDeathGrowthSkill(f);
        if (!DrainDeathGrowthChildren(f))
            AdvanceDeathGrowthChoice(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 21400)]
    internal bool ReturnDeathGrowthMovement(ProgramSkillFrame f)
    {
        if (f.DeathGrowthSkill?.Stage != DeathGrowthSkillStage.DrawChildren)
            return false;
        _coreDeathGrowthSkillObserversOperations.RequireDeathGrowthSkill(f);
        if (!DrainDeathGrowthChildren(f))
            AdvanceDeathGrowthChoice(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30)]
    internal bool CanContinueDeathGrowthSkill(ProgramSkillFrame f) => f.DeathGrowthSkill is not null && _coreDeathGrowthSkillObserversOperations.ValidDeathGrowthSkill(f);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 40)]
    internal bool IsDeathGrowthMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.DeathGrowthSkill is { Stage: DeathGrowthSkillStage.DrawChildren } r && r.InstructionIndex == f.InstructionIndex && effect?.Op == SkillProgramEffectOp.DrawAfterOtherDeathThenChooseMissingSkill && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    SkillProgramStepOutcome global::CardGame.Core.IDeathGrowthSkillHost.DrawAfterOtherDeathThenChooseMissingSkill(ProgramSkillFrame f, SkillProgramEffect e) => BeginDeathGrowthSkill(f, e);
}

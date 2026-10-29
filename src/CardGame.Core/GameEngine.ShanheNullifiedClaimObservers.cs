using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 62033)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 62033)]
    private bool IsShanheNullifiedClaimProgramDying() => ActiveDying is { } dying &&
        ActiveNullificationWindow is { } window &&
        _resolutionStack.OfType<DyingFrame>().Any(frame => frame.Id == dying.FrameId && frame.ParentFrameId == dying.ParentFrameId) &&
        TryGetShanheNullifiedClaimRide(window, out _);

    private bool TryGetShanheNullifiedClaimRide(NullificationWindowFrame window, out int[] childProcessingCosts)
    {
        childProcessingCosts = [];
        if (_options.ShanheBattle is null || _modeDefinition.Id != ShanheBattleSetup.BattleModeId ||
            !window.EffectNullified || !IsShanheNullifiedClaimAwaitingMovement(window)) return false;
        var index = _resolutionStack.FindIndex(frame => frame.Id == window.Id);
        if (index < 0 || index + 1 >= _resolutionStack.Count ||
            _resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame root ||
            root.ResumeShanheNullifiedClaimWindowId != window.Id || !HasOnlyShanheNullifiedClaimMovementReturn(root) ||
            !IsShanheNullifiedClaimMovement(window, root.Batch) ||
            _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Count(frame => frame.ResumeShanheNullifiedClaimWindowId == window.Id) != 1)
            return false;
        if (window.ShanheDelayedNullifiedTrickClaim is { } delayed)
            ValidateShanheDelayedNullifiedTrickClaim(window, delayed);
        else
        {
            if (LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is not { } receipt ||
                receipt.WindowFrameId != window.Id || receipt.ChainDepth != window.ChainDepth) return false;
            AssertShanheNullifiedTrickClaims();
        }

        var rootIndex = index + 1;
        for (index += 2; index < _resolutionStack.Count; index++)
        {
            var parent = _resolutionStack[index - 1]; var child = _resolutionStack[index];
            // Frozen candidate context and the active candidate's exact source
            // are both required; a matching frame type or parent id is not enough.
            if (parent is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame && child is ProgramSkillFrame)
            { if (!TypedDamagePostEventEdge(parent, child)) return false; continue; }
            if (parent is ProgramSkillFrame producer && child is CardsMovedTriggerWindowFrame moved)
            { if (!ProgramMovementReturnMatches(producer, moved) || HasUnlistedShanheClaimMovementReturn(moved)) return false; continue; }
            if (parent is ProgramSkillFrame healthProducer && child is HpChangedTriggerWindowFrame hp)
            { if (!ProgramHpReturnMatches(healthProducer, hp)) return false; continue; }
            if (child is RecoveryReplacementFrame && RecoveryReplacementFrameRidesOn(child, parent)) continue;
            if (child is CardsMovedTriggerWindowFrame recoveryMovement && parent is RecoveryReplacementFrame recovery &&
                recoveryMovement.ResumeRecoveryReplacementFrameId == recovery.Id && recoveryMovement.ResumeProgramFrameId is null &&
                !HasAlternateProgramMovementReturn(recoveryMovement with { ResumeRecoveryReplacementFrameId = null }) &&
                !HasUnlistedShanheClaimMovementReturn(recoveryMovement) && DamageFrameRidesOn(child, parent)) continue;
            if (child is HpChangedTriggerWindowFrame && parent is RecoveryReplacementFrame && DamageFrameRidesOn(child, parent)) continue;
            if (parent is ProgramSkillFrame { AttackAttempt: not null } && child is BeforeDamageProgramWindowFrame or DamageFrame ||
                parent is BeforeDamageProgramWindowFrame && child is ProgramSkillFrame ||
                parent is DamageFrame && child is DamageTriggerWindowFrame or DyingFrame ||
                parent is DamageTriggerWindowFrame && child is ProgramSkillFrame)
            { if (!ProgramChildResolutionEdge(index)) return false; continue; }

            if (child is DyingFrame dying && parent is ProgramSkillFrame losing && ShanheClaimProgramHpLossDying(losing, dying)) continue;
            if (parent is DyingFrame dyingParent)
            {
                // These existing helpers prove the whole rescue suffix,
                // including its real conversion/material payment and return.
                if (IsPaidHandRepaymentRescueRide(index - 1, dyingParent) ||
                    IsPaidHandRepaymentProgramAlcoholRide(index - 1, dyingParent) ||
                    PolicyCounterspellVirtualAlcoholRide(index - 1, dyingParent) ||
                    PaidObserverDamageVirtualAlcoholRide(index - 1, dyingParent))
                { childProcessingCosts = ShanheClaimProvenChildMaterials(rootIndex); return true; }
                if (child is ProgramSkillFrame response &&
                    (PolicyCounterspellDyingProgramRidesOn(response, dyingParent) || PaidObserverDamageDyingProgramMatches(response, dyingParent))) continue;
                if (ParticipantHandDyingEntryRide(child, parent, dyingParent)) continue;
            }
            if (index >= 2 && _resolutionStack[index - 2] is DyingFrame entryDying &&
                ParticipantHandDyingEntryRide(child, parent, entryDying) && child is ProgramSkillFrame entryProgram &&
                parent is ProgramLifecycleTriggerWindowFrame entry &&
                MountObserverCandidateMatches(entryProgram, entry.Candidates[entry.CandidateIndex])) continue;
            if (PolicyCounterspellDyingFaceEdge(index) || PaidObserverDamageDyingFaceEdge(index) ||
                RandomEquipmentFrameRidesOn(child, parent) || PileEquipmentFrameRidesOn(child, parent)) continue;
            return false;
        }
        childProcessingCosts = ShanheClaimProvenChildMaterials(rootIndex);
        return true;
    }

    private bool ShanheClaimProgramHpLossDying(ProgramSkillFrame producer, DyingFrame dying) =>
        dying.Continuation == DyingContinuationKind.ProgramSkill && dying.ParentFrameId == producer.Id &&
        dying.KillerSeat is null && _contentRegistry.Skills.GetValueOrDefault(producer.SkillId)?.Program is { } program &&
        program.GameplayHash == producer.GameplayHash && producer.InstructionIndex > 0 &&
        ProgramInstructionResolver.Default.Resolve(producer, program).GetPausedInstruction(producer.InstructionIndex).Effect is
            { Op: SkillProgramEffectOp.LoseHp } effect && ResolveProgramEffectTarget(producer, effect.Target) == dying.VictimSeat &&
        ProgramEventHistory<ProgramSkillHpLostEvent>().Any(fact => fact.FrameId == producer.Id && fact.SkillId == producer.SkillId &&
            fact.TargetSeat == dying.VictimSeat && fact.Amount > 0 && fact.RemainingHp <= 0);

    // Called only after every contiguous edge (or an existing whole-suffix
    // rescue proof) succeeded. No unrelated live use can contribute materials.
    private int[] ShanheClaimProvenChildMaterials(int rootIndex) => _resolutionStack.Skip(rootIndex + 1)
        .OfType<CardUseFrame>().Where(use => use.Action is { Type: CardActionType.Use } action &&
            action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(use.PhysicalCardIds ?? [use.CardId]))
        .SelectMany(use => use.Action!.PhysicalCards).Where(cost => _cardZones.GetLocation(cost.CardId) == CardLocation.Processing)
        .Select(cost => cost.CardId).Distinct().Order().ToArray();
}

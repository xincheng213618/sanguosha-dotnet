#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheNullifiedClaimObserversRuntime
{
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 62033)]
    internal bool IsShanheNullifiedClaimProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } window && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(frame => frame.Id == dying.FrameId && frame.ParentFrameId == dying.ParentFrameId) && TryGetShanheNullifiedClaimRide(window, out _);
    internal bool TryGetShanheNullifiedClaimRide(NullificationWindowFrame window, out int[] childProcessingCosts)
    {
        childProcessingCosts = [];
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || !window.EffectNullified || !_coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimAwaitingMovement(window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == window.Id);
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame root || root.ResumeShanheNullifiedClaimWindowId != window.Id || !CoreGameEngineShanheNullifiedTrickClaimsRuntime.HasOnlyShanheNullifiedClaimMovementReturn(root) || !_coreShanheNullifiedTrickClaimsOperations.IsShanheNullifiedClaimMovement(window, root.Batch) || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Count(frame => frame.ResumeShanheNullifiedClaimWindowId == window.Id) != 1)
            return false;
        if (window.ShanheDelayedNullifiedTrickClaim is { } delayed)
            _coreShanheDelayedNullifiedTrickClaimsOperations.Value.ValidateShanheDelayedNullifiedTrickClaim(window, delayed);
        else
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is not { } receipt || receipt.WindowFrameId != window.Id || receipt.ChainDepth != window.ChainDepth)
                return false;
            _coreShanheNullifiedTrickClaimsOperations.AssertShanheNullifiedTrickClaims();
        }

        var rootIndex = index + 1;
        for (index += 2; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            var child = _resolutionStackOperations._resolutionStack[index];
            // Frozen candidate context and the active candidate's exact source
            // are both required; a matching frame type or parent id is not enough.
            if (parent is CardsMovedTriggerWindowFrame or HpChangedTriggerWindowFrame && child is ProgramSkillFrame)
            {
                if (!_coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child))
                    return false;
                continue;
            }

            if (parent is ProgramSkillFrame producer && child is CardsMovedTriggerWindowFrame moved)
            {
                if (!CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(producer, moved) || CoreGameEngineShanheNullifiedTrickClaimsRuntime.HasUnlistedShanheClaimMovementReturn(moved))
                    return false;
                continue;
            }

            if (parent is ProgramSkillFrame healthProducer && child is HpChangedTriggerWindowFrame hp)
            {
                if (!CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(healthProducer, hp))
                    return false;
                continue;
            }

            if (child is RecoveryReplacementFrame && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent))
                continue;
            if (child is CardsMovedTriggerWindowFrame recoveryMovement && parent is RecoveryReplacementFrame recovery && recoveryMovement.ResumeRecoveryReplacementFrameId == recovery.Id && recoveryMovement.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(recoveryMovement with { ResumeRecoveryReplacementFrameId = null }) && !CoreGameEngineShanheNullifiedTrickClaimsRuntime.HasUnlistedShanheClaimMovementReturn(recoveryMovement) && _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent))
                continue;
            if (child is HpChangedTriggerWindowFrame && parent is RecoveryReplacementFrame && _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent))
                continue;
            if (parent is ProgramSkillFrame { AttackAttempt: not null } && child is BeforeDamageProgramWindowFrame or DamageFrame || parent is BeforeDamageProgramWindowFrame && child is ProgramSkillFrame || parent is DamageFrame && child is DamageTriggerWindowFrame or DyingFrame || parent is DamageTriggerWindowFrame && child is ProgramSkillFrame)
            {
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index))
                    return false;
                continue;
            }

            if (child is DyingFrame dying && parent is ProgramSkillFrame losing && ShanheClaimProgramHpLossDying(losing, dying))
                continue;
            if (parent is DyingFrame dyingParent)
            {
                // These existing helpers prove the whole rescue suffix,
                // including its real conversion/material payment and return.
                if (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(index - 1, dyingParent) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index - 1, dyingParent) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(index - 1, dyingParent) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(index - 1, dyingParent))
                {
                    childProcessingCosts = ShanheClaimProvenChildMaterials(rootIndex);
                    return true;
                }

                if (child is ProgramSkillFrame response && (_coreUnrespondableCounterspellsOperations.PolicyCounterspellDyingProgramRidesOn(response, dyingParent) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageDyingProgramMatches(response, dyingParent)))
                    continue;
                if (CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, dyingParent))
                    continue;
            }

            if (index >= 2 && _resolutionStackOperations._resolutionStack[index - 2] is DyingFrame entryDying && CoreParticipantHandDamageRidesRuntime.ParticipantHandDyingEntryRide(child, parent, entryDying) && child is ProgramSkillFrame entryProgram && parent is ProgramLifecycleTriggerWindowFrame entry && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(entryProgram, entry.Candidates[entry.CandidateIndex]))
                continue;
            if (_coreUnrespondableCounterspellsOperations.PolicyCounterspellDyingFaceEdge(index) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageDyingFaceEdge(index) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(child, parent))
                continue;
            return false;
        }

        childProcessingCosts = ShanheClaimProvenChildMaterials(rootIndex);
        return true;
    }

    internal bool ShanheClaimProgramHpLossDying(ProgramSkillFrame producer, DyingFrame dying) => dying.Continuation == DyingContinuationKind.ProgramSkill && dying.ParentFrameId == producer.Id && dying.KillerSeat is null && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(producer.SkillId)?.Program is { } program && program.GameplayHash == producer.GameplayHash && producer.InstructionIndex > 0 && ProgramInstructionResolver.Default.Resolve(producer, program).GetPausedInstruction(producer.InstructionIndex).Effect is { Op: SkillProgramEffectOp.LoseHp } effect && CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(producer, effect.Target) == dying.VictimSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(fact => fact.FrameId == producer.Id && fact.SkillId == producer.SkillId && fact.TargetSeat == dying.VictimSeat && fact.Amount > 0 && fact.RemainingHp <= 0);
    internal int[] ShanheClaimProvenChildMaterials(int rootIndex) => _resolutionStackOperations._resolutionStack.Skip(rootIndex + 1).OfType<CardUseFrame>().Where(use => use.Action is { Type: CardActionType.Use } action && action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual(use.PhysicalCardIds ?? [use.CardId])).SelectMany(use => use.Action!.PhysicalCards).Where(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.Processing).Select(cost => cost.CardId).Distinct().Order().ToArray();
}

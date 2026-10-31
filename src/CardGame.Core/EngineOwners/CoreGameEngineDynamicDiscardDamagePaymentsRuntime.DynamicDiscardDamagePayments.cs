#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDynamicDiscardDamagePaymentsRuntime
{
    internal const string DynamicDiscardDamageBlocked = "dynamic-discard-damage-blocked";
    internal const string DynamicDiscardDamageReason = "program.dynamic-target-hp.discard";
    SkillProgramStepOutcome global::CardGame.Core.IDynamicDiscardDamageHost.DiscardTargetHpCardsAndDamage(ProgramSkillFrame f) => BeginDynamicDiscardDamage(f);
    internal bool DynamicDiscardDamageSourceValid(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledActivationPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId);
    internal Card[] DynamicDiscardDamageCards(int owner, string skill, string instance) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, instance, c)).ToArray();
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 50, SkillProgramEffectOp.DiscardTargetHpCardsAndDamage)]
    internal bool CanStartDynamicDiscardDamage(CharacterState owner, string skill) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skill, DynamicDiscardDamageBlocked, SkillUsageScope.Turn) == 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => CanSelectDynamicDiscardDamageTarget(owner, skill, p.Seat));
    internal bool CanSelectDynamicDiscardDamageTarget(CharacterState owner, string skill, int target) => owner.IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && target != owner.Seat && _coreMatchParticipantsPlayersAccess._players[target].Hp > 0 && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner.Seat, target) && DynamicDiscardDamageCards(owner.Seat, skill, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, skill)).Length >= _coreMatchParticipantsPlayersAccess._players[target].Hp;
    internal SkillProgramStepOutcome BeginDynamicDiscardDamage(ProgramSkillFrame f)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (active.DynamicDiscardDamage is not null || active.SelectedTargetSeats is not [var target] || !DynamicDiscardDamageSourceValid(active) || !CanStartDynamicDiscardDamage(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId) || !CanSelectDynamicDiscardDamageTarget(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, target))
            throw new InvalidOperationException("The variable discard attack requires one current legal target and unpaid source.");
        var r = new DynamicDiscardDamageReceipt(active.InstructionIndex, target, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[target].Hp), _coreNativeRuleQueriesOperations.GetCombatDistance(active.OwnerSeat, target), _coreNativeRuleQueriesOperations.GetAttackRange(active.OwnerSeat), _coreActualTurnProgressTurnNumberAccess._turnNumber, DynamicDiscardDamageStage.Choosing, [], []);
        _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { DynamicDiscardDamage = r });
        PublishDynamicDiscardDamage(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DynamicDiscardDamageChoices(ProgramSkillFrame f)
    {
        var r = f.DynamicDiscardDamage!;
        var choices = DynamicDiscardDamageCards(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Where(c => !r.CardIds.Contains(c.Id)).Select(c => new PromptChoice(new($"target-hp.{(f.Id)}.{(c.Id)}"), $"弃置【{(c.DisplayName)}】（{(r.CardIds.Count + 1)}/{(r.FrozenTargetHp)}）", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "dynamic-discard-damage", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "card" })).ToList();
        choices.Add(new(new($"target-hp.{(f.Id)}.cancel"), "取消本次发动", [], [], new Dictionary<string, string> { ["program-action"] = "dynamic-discard-damage", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "cancel" }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishDynamicDiscardDamage(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = DynamicDiscardDamageChoices(f);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"{(skill.Name)}：弃置{(f.DynamicDiscardDamage!.FrozenTargetHp)}张牌。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("dynamic-discard-damage")]
    internal void ResolveDynamicDiscardDamageChoice(PromptChoice c)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Variable payment lost its parent.");
        var r = f.DynamicDiscardDamage ?? throw new InvalidOperationException("Variable payment lost its unpaid receipt.");
        if (r.Stage != DynamicDiscardDamageStage.Choosing || !DynamicDiscardDamageSourceValid(f) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], DynamicDiscardDamageChoices(f).Where(x => x.Id == c.Id).ToArray()))
            throw new InvalidOperationException("Variable payment no longer matches its published source or choice.");
        if (c.Parameters["branch"] == "cancel")
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DynamicDiscardDamage = null });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), false);
            return;
        }

        if (c.Cards is not [var id])
            throw new InvalidOperationException("Select one exact owned discard material.");
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var ids = r.CardIds.Append(id).ToArray();
        var locations = r.OriginalLocations.Append(from).ToArray();
        if (ids.Length < r.FrozenTargetHp)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DynamicDiscardDamage = r with { CardIds = ids, OriginalLocations = locations } });
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            PublishDynamicDiscardDamage(f);
            return;
        }

        if (ids.Length != r.FrozenTargetHp || ids.Distinct().Count() != ids.Length || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Hp != r.FrozenTargetHp || _coreNativeRuleQueriesOperations.GetCombatDistance(f.OwnerSeat, r.TargetSeat) != r.FrozenDistance || _coreNativeRuleQueriesOperations.GetAttackRange(f.OwnerSeat) != r.FrozenAttackRange || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(f.OwnerSeat, r.TargetSeat) || ids.Select((value, index) => (value, index)).Any(x => _cardZonesOperations._cardZones.GetLocation(x.value) != locations[x.index] || !DynamicDiscardDamageCards(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Any(card => card.Id == x.value)))
            throw new InvalidOperationException("The unpaid target HP, range or complete HE cost changed.");
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DynamicDiscardDamage = r with { Stage = DynamicDiscardDamageStage.Paid, CardIds = ids, OriginalLocations = locations, PaymentSequenceBefore = before } });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(DynamicDiscardDamageReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { DynamicDiscardDamage = f.DynamicDiscardDamage!with { PaymentSequenceAfter = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DynamicDiscardDamagePaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.FrozenTargetHp, r.ActualTurn, ids));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 450)]
    internal bool ReturnDynamicDiscardDamageMovement(ProgramSkillFrame f)
    {
        if (f.DynamicDiscardDamage is null)
            return false;
        if (f.DynamicDiscardDamage.Stage != DynamicDiscardDamageStage.Paid || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat || !ValidDynamicDiscardDamagePayments(f))
            throw new InvalidOperationException("The paid target-HP cost lost its exact owning movement return.");
        if (TryDrainDynamicDiscardDamageCost(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        // A committed cost survives source suppression/death. The already
        // frozen living target receives the attack; a winner/dead target ends it.
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool TryDrainDynamicDiscardDamageCost(ProgramSkillFrame f)
    {
        if (f.DynamicDiscardDamage is not { Stage: DynamicDiscardDamageStage.Paid } || !IsDynamicDiscardDamageMovement(f, ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect, f.PendingMovementContinuation ?? throw new InvalidOperationException("The paid discard drain requires its movement return.")))
            throw new InvalidOperationException("The discard drain lost its exact paid instruction and ledger.");
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 30)]
    internal bool IsDynamicDiscardDamageMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => effect?.Op == SkillProgramEffectOp.DiscardTargetHpCardsAndDamage && f.DynamicDiscardDamage is { Stage: DynamicDiscardDamageStage.Paid } r && f.InstructionIndex == r.InstructionIndex && f.SelectedTargetSeats is [var target] && target == r.TargetSeat && movement.SubjectSeat == f.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null && ValidDynamicDiscardDamagePayments(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 470)]
    internal bool ResumeDynamicDiscardDamage(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.DynamicDiscardDamage is not { } r)
            return false;
        if (r.Stage == DynamicDiscardDamageStage.Choosing)
            return true;
        if (r.Stage == DynamicDiscardDamageStage.Paid)
        {
            if (f.PendingMovementContinuation is not null)
            {
                if (TryDrainDynamicDiscardDamageCost(f))
                    return true;
                ReturnDynamicDiscardDamageMovement(f);
                return true;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DynamicDiscardDamage = r with { Stage = DynamicDiscardDamageStage.DamageIssued } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1);
            return true;
        }

        if (r.Stage == DynamicDiscardDamageStage.DamageIssued)
        {
            if (f.AttackAttempt is not null)
            {
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id));
                return true;
            }

            throw new InvalidOperationException("Paid variable damage lost its typed attack completion.");
        }

        if (r.Stage == DynamicDiscardDamageStage.DamageCompleted)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program))
                return true;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
                return true;
            }

            if ((f.SkillId == "wushuang:skill-3062" || global::CardGame.Core.EngineOwners.CharacterZhugeKeMobileRuntime.IsMobileZhugeKeDuwu(f.SkillId)) ? r.DyingFrameId is null : r.DyingSurvived != true)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            if (r.DamageFrameId is not { } damageId || r.DyingFrameId is not { } dyingId)
                throw new InvalidOperationException("Survival cost lost exact damage/Dying.");
            if (_skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, DynamicDiscardDamageBlocked, SkillUsageScope.Turn) == 0)
            {
                if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(f.OwnerSeat, f.SkillId, DynamicDiscardDamageBlocked, SkillUsageScope.Turn, 1))
                    throw new InvalidOperationException("Survival prohibition already consumed.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(f.OwnerSeat, f.SkillId, DynamicDiscardDamageBlocked, SkillUsageScope.Turn, 1));
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DynamicDiscardDamage = r with { Stage = DynamicDiscardDamageStage.PenaltyIssued } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DynamicDiscardDamagePenaltyEvent(f.Id, f.OwnerSeat, r.ActualDamageTargetSeat ?? r.TargetSeat, damageId, dyingId, r.ActualTurn));
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp > 0 && _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1) == SkillProgramStepOutcome.AwaitChild)
                return true;
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == DynamicDiscardDamageStage.PenaltyIssued)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program))
                return true;
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
            return true;
        }

        throw new InvalidOperationException("Invalid variable discard-damage stage.");
    }

    internal void CaptureDynamicDiscardDamageFrame(long damageId, long ownerId, int target)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == ownerId)is not { DynamicDiscardDamage.Stage: DynamicDiscardDamageStage.DamageIssued } f)
            return;
        if (f.DynamicDiscardDamage!.DamageFrameId is not null)
            throw new InvalidOperationException("Variable attack unexpectedly issued another native damage frame.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { DynamicDiscardDamage = f.DynamicDiscardDamage with { DamageFrameId = damageId, ActualDamageTargetSeat = target } });
    }

    internal void CaptureDynamicDiscardDamageDying(long dyingId, long damageId, int victim)
    {
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.DynamicDiscardDamage is { } r && r.DamageFrameId == damageId);
        if (f?.DynamicDiscardDamage is not { } r)
            return;
        if (r.ActualDamageTargetSeat != victim || r.DyingFrameId is not null)
            throw new InvalidOperationException("Variable attack changed its exact Dying recipient.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { DynamicDiscardDamage = r with { DyingFrameId = dyingId } });
    }

    internal void CaptureDynamicDiscardDamageSurvival(global::CardGame.Core.GameEngine.DyingCompletionReceipt completion, bool survived)
    {
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.DynamicDiscardDamage?.DyingFrameId == completion.FrameId);
        if (f?.DynamicDiscardDamage is not { } r)
            return;
        if (completion.Continuation != DyingContinuationKind.Damage || completion.ParentFrameId != r.DamageFrameId || completion.VictimSeat != r.ActualDamageTargetSeat || r.DyingSurvived is not null)
            throw new InvalidOperationException("Variable attack Dying returned from a different native producer.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { DynamicDiscardDamage = r with { DyingSurvived = survived } });
    }

    internal ProgramSkillFrame CaptureDynamicDiscardDamageCompleted(ProgramSkillFrame f)
    {
        if (f.DynamicDiscardDamage is not { Stage: DynamicDiscardDamageStage.DamageIssued } r)
            return f;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DynamicDiscardDamageCompletedEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.DamageFrameId, r.ActualDamageTargetSeat, r.DyingFrameId, r.DyingSurvived, r.ActualTurn));
        return f with
        {
            DynamicDiscardDamage = r with
            {
                Stage = DynamicDiscardDamageStage.DamageCompleted
            }
        };
    }

    internal bool ValidDynamicDiscardDamagePayments(ProgramSkillFrame f)
    {
        var r = f.DynamicDiscardDamage!;
        if (r.CardIds.Count != r.FrozenTargetHp || r.CardIds.Count != r.OriginalLocations.Count || r.CardIds.Distinct().Count() != r.CardIds.Count)
            return false;
        return r.CardIds.Select((id, i) => (id, i)).All(x => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.CardId == x.id && m.Sequence > r.PaymentSequenceBefore && m.Sequence <= r.PaymentSequenceAfter && m.From == r.OriginalLocations[x.i] && m.Reason.Value == DynamicDiscardDamageReason && m.To == CardLocation.DiscardPile) == 1);
    }
}

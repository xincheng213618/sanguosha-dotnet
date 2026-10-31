#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSignedDamagePaymentsRuntime
{
    internal bool HasSignedDamagePayments => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage);

    internal SkillProgramEffect SignedDamageEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal global::CardGame.Core.GameEngine.SignedDamageProfile CaptureSignedDamageProfile(int ownerSeat, int otherSeat, SkillProgramEffect effect)
    {
        var p = effect.PublicDeathDamageCost!;
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        // Prove public death before exposing its role to the mode policy.
        // National-war's placeholder Role is not an identity death benefit.
        var roles = _coreMatchParticipantsPlayersAccess._players.Where(c => !c.IsAlive && c.RoleRevealed).Select(c => _coreModeAllegianceOperations.ModeAllegiancePolicy.PublicIdentityDeathRole(new(c.Role, c.IsAlive, c.RoleRevealed))).OfType<Role>().Distinct().ToArray();
        var enabled = _coreNationalSkillsOperations.HasRuntimeSkill(owner, p.QualifierSkillId);
        return new(owner.Hp, _coreMatchParticipantsPlayersAccess._players[otherSeat].Hp, enabled && roles.Any(p.WaiveHpDeadRoles.Contains), enabled && roles.Any(p.AnyColorDeadRoles.Contains), enabled && roles.Any(p.AllowEquipmentDeadRoles.Contains), roles.Aggregate(0, (mask, role) => mask | (1 << (int)role)), enabled ? _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, p.QualifierSkillId) : null);
    }

    internal IReadOnlyList<Card> SignedDamageCosts(int ownerSeat, string skillId, string instance, SkillProgramEffect e, global::CardGame.Core.GameEngine.SignedDamageProfile p) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(p.AllowEquipment ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat]) : []).Where(c => (p.AnyColor || e.Suits.Contains(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], c))) && !c.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, instance, c) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 880)]
    internal bool CanOfferSignedDamagePayment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage))
            return true;
        var e = trigger.Effects.Single();
        if (c is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0 } || source == target || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || candidate.OwnerSeat != (e.Amount > 0 ? source : target) || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { Prevented: false, Amount: > 0 } parent || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.ResolutionId != (parent.ContinuationAttackResolutionId ?? parent.ParentFrameId) || attack.SourceSeat != source || attack.TargetSeat != target || attack.DamageAmount != parent.Amount || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != parent.Nature)
            return false;
        var p = CaptureSignedDamageProfile(candidate.OwnerSeat, e.Amount > 0 ? target : source, e);
        return (p.WaiveHp || p.OtherHp >= p.OwnerHp) && SignedDamageCosts(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, e, p).Count > 0;
    }

    internal SkillProgramStepOutcome BeginSignedDamagePayment(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var e = SignedDamageEffect(f);
        if (f.SignedDamagePayment is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target } c || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame w || w.Id != c.ParentFrameId || w.Prevented || w.Amount <= 0 || source == target || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.ResolutionId != (w.ContinuationAttackResolutionId ?? w.ParentFrameId) || _coreNativeCardClassificationsOperations.GetDamageNature(attack) != w.Nature || attack.DamageAmount != w.Amount || f.OwnerSeat != (e.Amount > 0 ? source : target))
            throw new InvalidOperationException("Signed damage payment lost its exact before-damage owner.");
        var p = CaptureSignedDamageProfile(f.OwnerSeat, e.Amount > 0 ? target : source, e);
        if (!p.WaiveHp && p.OtherHp < p.OwnerHp || SignedDamageCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, e, p).Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "伤害调整没有合法未付款费用，原伤害继续。");
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SignedDamagePayment = new(f.InstructionIndex, w.Id, attack.ResolutionId, source, target, w.Nature, w.Amount, e.Amount, p.OwnerHp, p.OtherHp, p.WaiveHp, p.AnyColor, p.AllowEquipment, p.DeadRoleMask, p.QualifierInstanceId, SignedDamagePaymentStage.ChoosingCost) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSignedDamageOfferedEvent(f.Id, w.Id, attack.ResolutionId, f.OwnerSeat, source, target, w.Nature, w.Amount, e.Amount, p.OwnerHp, p.OtherHp, p.WaiveHp, p.AnyColor, p.AllowEquipment, p.DeadRoleMask, p.QualifierInstanceId));
        PublishSignedDamagePayment(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static global::CardGame.Core.GameEngine.SignedDamageProfile SignedDamageFrozenProfile(ProgramSignedDamagePaymentReceipt r) => new(r.OwnerHp, r.OtherHp, r.WaiveHp, r.AnyColor, r.AllowEquipment, r.PublicDeadRoleMask, r.QualifierInstanceId);
    internal IReadOnlyList<PromptChoice> SignedDamagePaymentChoices(ProgramSkillFrame f) => Array.AsReadOnly(SignedDamageCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, SignedDamageEffect(f), SignedDamageFrozenProfile(f.SignedDamagePayment!)).Select(card => new PromptChoice(new($"signed-damage.{(f.Id)}.{(card.Id)}"), $"弃置【{(card.DisplayName)}】令此伤害{((f.SignedDamagePayment!.Delta > 0 ? "+1" : "-1"))}", Array.AsReadOnly(new[] { card.Id }), [], new Dictionary<string, string> { ["program-action"] = "signed-damage-payment", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    internal void PublishSignedDamagePayment(ProgramSkillFrame f)
    {
        var choices = SignedDamagePaymentChoices(f);
        if (choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "未付款费用失效，原伤害继续。");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择一张符合当前费用条件的牌。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).ToArray()), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("signed-damage-payment")]
    internal void ResolveSignedDamagePayment(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SignedDamagePayment.Stage: SignedDamagePaymentStage.ChoosingCost } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [SignedDamagePaymentChoices(f).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("Signed damage answer lost its exact private cost choice.");
        var r = f.SignedDamagePayment!;
        var e = SignedDamageEffect(f);
        var p = CaptureSignedDamageProfile(f.OwnerSeat, e.Amount > 0 ? r.TargetSeat : r.SourceSeat, e);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || p != SignedDamageFrozenProfile(r) || !p.WaiveHp && p.OtherHp < p.OwnerHp)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "付款前资格或原来源失效，原伤害继续。");
            return;
        }

        var card = SignedDamageCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, e, p).Single(c => c.Id == choice.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), SignedDamagePayment = r with { Stage = SignedDamagePaymentStage.Paid, CostCardId = card.Id, CostFrom = from, CostEffectiveSuit = suit } });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage)}"), moved =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { SignedDamagePayment = active.SignedDamagePayment!with { CostMovementSequence = moved.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSignedDamagePaidEvent(f.Id, r.BeforeDamageFrameId, r.OriginalAttackFrameId, f.OwnerSeat, r.SourceSeat, r.TargetSeat, r.Delta, card.Id, from, suit, moved.Sequence, r.WaiveHp, r.AnyColor, r.AllowEquipment, r.PublicDeadRoleMask, r.QualifierInstanceId));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 480)]
    internal bool ResumeSignedDamagePayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.SignedDamagePayment is not { } r)
            return false;
        if (r.Stage == SignedDamagePaymentStage.ChoosingCost)
            return true;
        if (_coreSuitDamagePreventionBenefitsOperations.DrainSuitPlacementChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.SignedDamagePayment!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == SignedDamagePaymentStage.Adjusted)
        {
            // The standalone paid instruction has completed. Its exact receipt
            // was validated above; source loss cannot cancel the committed tail.
            _coreSkillProgramsOperations.FinishProgramSkill(f, completed: !r.Cancelled);
            return true;
        }

        var w = _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single(p => p.Id == r.BeforeDamageFrameId);
        var cancel = _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive;
        var after = cancel ? w.Amount : Math.Max(0, checked(w.Amount + r.Delta));
        if (!cancel)
        {
            _coreRecipientScopedDamageOperations.AdjustExactRecipientDamage(f, r, after);
            _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { Amount = after, Prevented = after == 0 });
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { SignedDamagePayment = r with { Stage = SignedDamagePaymentStage.Adjusted, FinalAmount = after, Cancelled = cancel } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSignedDamageAdjustedEvent(id, r.BeforeDamageFrameId, r.OriginalAttackFrameId, r.SourceSeat, r.TargetSeat, r.OriginalAmount, after, r.Delta, cancel));
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 460)]
    internal bool ReturnSignedDamageMovement(ProgramSkillFrame f)
    {
        if (f.SignedDamagePayment is null)
            return false;
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Signed damage cost lost its typed movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage)]
    internal PromptChoice SelectAiSignedDamagePayment(PendingDecision d, ProgramSkillFrame f) => d.Choices.OrderBy(c => CardCatalog.Get(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()).Kind).HandKeepValue).ThenBy(c => c.Cards.Single()).First();
    internal bool IsValidSignedDamagePayment(ProgramSkillFrame f, bool requirePaid)
    {
        if (f.SignedDamagePayment is not { } r || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } c || r.InstructionIndex != f.InstructionIndex || SignedDamageEffect(f).Op != SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage || r.Delta != SignedDamageEffect(f).Amount || c.ParentFrameId != r.BeforeDamageFrameId || c.SourceSeat != r.SourceSeat || c.TargetSeat != r.TargetSeat || c.Amount != r.OriginalAmount || r.SourceSeat == r.TargetSeat || r.OriginalAmount <= 0 || f.OwnerSeat != (r.Delta > 0 ? r.SourceSeat : r.TargetSeat) || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(p => p.Id == r.BeforeDamageFrameId)is not { } w || (w.ContinuationAttackResolutionId ?? w.ParentFrameId) != r.OriginalAttackFrameId || w.SourceSeat != r.SourceSeat || w.TargetSeat != r.TargetSeat || w.Nature != r.Nature || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex].Candidate) || !_resolutionStackOperations._resolutionStack.Any(p => p.Id == r.OriginalAttackFrameId) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSignedDamageOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.BeforeDamageFrameId == w.Id && e.OriginalAttackFrameId == r.OriginalAttackFrameId && e.OwnerSeat == f.OwnerSeat && e.SourceSeat == r.SourceSeat && e.TargetSeat == r.TargetSeat && e.Nature == r.Nature && e.Amount == r.OriginalAmount && e.Delta == r.Delta && e.OwnerHp == r.OwnerHp && e.OtherHp == r.OtherHp && e.WaiveHp == r.WaiveHp && e.AnyColor == r.AnyColor && e.AllowEquipment == r.AllowEquipment && e.PublicDeadRoleMask == r.PublicDeadRoleMask && e.QualifierInstanceId == r.QualifierInstanceId) != 1)
            return false;
        var paid = r.Stage != SignedDamagePaymentStage.ChoosingCost;
        if (requirePaid && !paid || !paid && (r.CostCardId is not null || r.CostFrom is not null || r.CostEffectiveSuit is not null || r.CostMovementSequence != 0 || r.FinalAmount is not null))
            return false;
        if (r.Stage != SignedDamagePaymentStage.Adjusted && (w.Amount != r.OriginalAmount || w.Prevented))
            return false;
        if (paid && (r.CostCardId is not { } cardId || r.CostFrom is not { } from || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || from.Zone == CardZoneKind.Equipment && !r.AllowEquipment || r.CostEffectiveSuit is not { } suit || !r.AnyColor && !SignedDamageEffect(f).Suits.Contains(suit) || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == r.CostMovementSequence && m.CardId == cardId && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage)}") != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSignedDamagePaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.BeforeDamageFrameId == w.Id && e.OriginalAttackFrameId == r.OriginalAttackFrameId && e.OwnerSeat == f.OwnerSeat && e.SourceSeat == r.SourceSeat && e.TargetSeat == r.TargetSeat && e.Delta == r.Delta && e.CostCardId == cardId && e.CostFrom == from && e.EffectiveSuit == suit && e.MovementSequence == r.CostMovementSequence && e.WaiveHp == r.WaiveHp && e.AnyColor == r.AnyColor && e.AllowEquipment == r.AllowEquipment && e.PublicDeadRoleMask == r.PublicDeadRoleMask && e.QualifierInstanceId == r.QualifierInstanceId) != 1))
            return false;
        if (r.Stage == SignedDamagePaymentStage.Adjusted && (r.FinalAmount != w.Amount || !r.Cancelled && w.Prevented != (w.Amount == 0) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSignedDamageAdjustedEvent>().Count(e => e.ProgramFrameId == f.Id && e.BeforeDamageFrameId == w.Id && e.OriginalAttackFrameId == r.OriginalAttackFrameId && e.SourceSeat == r.SourceSeat && e.TargetSeat == r.TargetSeat && e.BeforeAmount == r.OriginalAmount && e.AfterAmount == r.FinalAmount && e.Delta == r.Delta && e.Cancelled == r.Cancelled) != 1))
            return false;
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.ISignedDamagePaymentHost.DiscardOwnedCardToAdjustCurrentDamage(ProgramSkillFrame f) => BeginSignedDamagePayment(f);
}

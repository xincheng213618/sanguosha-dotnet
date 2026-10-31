#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSuitDamagePreventionBenefitsRuntime
{
    internal SkillProgramEffect SuitBenefitEffect(ProgramSkillFrame f) => ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal IReadOnlyList<Card> SuitBenefitCosts(int ownerSeat, string skillId, string instance, IReadOnlyList<Suit>? suits = null) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat])).Where(c => (skillId != "boundary:tianxiang-mobile" || _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Hand(ownerSeat)) && (suits is null || suits.Contains(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], c))) && !c.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, instance, c) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 870)]
    internal bool CanOfferSuitPreventionBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit))
            return true;
        return context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target, Amount: > 0 } && target == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(w => w.Id == context.ParentFrameId && !w.Prevented && w.TargetSeat == target) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != target) && SuitBenefitCosts(target, candidate.SkillId, candidate.SkillInstanceId, trigger.Effects[0].Suits).Count > 0;
    }

    internal SkillProgramStepOutcome BeginSuitPreventionBenefit(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SuitPreventionBenefit is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target } context || target != f.OwnerSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame parent || parent.Id != context.ParentFrameId || parent.Prevented || parent.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != (parent.ContinuationAttackResolutionId ?? parent.ParentFrameId))
            throw new InvalidOperationException("Suit prevention lost its exact pending damage owner.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = new(f.InstructionIndex, parent.Id, attack.ResolutionId, parent.SourceSeat, context.SourceSeat is null, target, parent.Amount, SuitPreventionBenefitStage.ChoosingCost) });
        PublishSuitPreventionBenefit(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SuitPreventionBenefitChoices(ProgramSkillFrame f)
    {
        var r = f.SuitPreventionBenefit!;
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Params(string branch) => new()
        {
            ["program-action"] = "suit-prevention-benefit",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == SuitPreventionBenefitStage.ChoosingCost)
            foreach (var card in SuitBenefitCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, SuitBenefitEffect(f).Suits))
                choices.Add(new(new($"suit-benefit.{(f.Id)}.cost.{(card.Id)}"), $"弃置【{(card.DisplayName)}】", Array.AsReadOnly(new[] { card.Id }), [], Params("cost")));
        else if (r.Stage == SuitPreventionBenefitStage.ChoosingRecipient)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => p.Seat))
            {
                var targets = Array.AsReadOnly(new[] { target.Seat });
                choices.Add(new(new($"suit-benefit.{(f.Id)}.damage.{(target.Seat)}"), $"令 {(target.Name)} 受到来源的{(SuitBenefitEffect(f).Amount)}点伤害后按已损失体力摸牌", [], targets, Params("damage-draw")));
                choices.Add(new(new($"suit-benefit.{(f.Id)}.loss.{(target.Seat)}"), $"令 {(target.Name)} 失去{(SuitBenefitEffect(f).Amount)}点体力后获得原弃牌", [], targets, Params("lose-hp-gift")));
            }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSuitPreventionBenefit(ProgramSkillFrame f)
    {
        var choices = SuitPreventionBenefitChoices(f);
        if (choices.Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "没有合法未付款选择，原伤害继续。");
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.SuitPreventionBenefit!.Stage == SuitPreventionBenefitStage.ChoosingCost ? "选择一张符合花色的手牌或装备牌。" : "选择受益角色与防伤后的分支。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("suit-prevention-benefit")]
    internal void ResolveSuitPreventionBenefit(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.SuitPreventionBenefit is not { } r || r.Stage is not (SuitPreventionBenefitStage.ChoosingCost or SuitPreventionBenefitStage.ChoosingRecipient) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [SuitPreventionBenefitChoices(f).Single(c => c.Id == choice.Id)]))
            throw new InvalidOperationException("Suit prevention answer lost its private owning choice.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "防伤付款前来源失效，原伤害继续。");
            return;
        }

        if (r.Stage == SuitPreventionBenefitStage.ChoosingCost)
        {
            var card = SuitBenefitCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, SuitBenefitEffect(f).Suits).Single(c => c.Id == choice.Cards.Single());
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = r with { CostCardId = card.Id, CostFrom = _cardZonesOperations._cardZones.GetLocation(card.Id), CostEffectiveSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), Stage = SuitPreventionBenefitStage.ChoosingRecipient } });
            PublishSuitPreventionBenefit(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var recipient = choice.Targets.Single();
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || recipient == f.OwnerSeat || r.CostCardId is not { } id || r.CostFrom is not { } from || _cardZonesOperations._cardZones.GetLocation(id) != from || !SuitBenefitCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, SuitBenefitEffect(f).Suits).Any(c => c.Id == id))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原费用或受益角色在付款前失效。");
            return;
        }

        var branch = choice.Parameters["branch"] switch
        {
            "damage-draw" => SuitPreventionBenefitBranch.DamageAndDraw,
            "lose-hp-gift" => SuitPreventionBenefitBranch.LoseHpAndGift,
            _ => throw new InvalidOperationException("Unknown suit prevention branch.")};
        var cost = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), SuitPreventionBenefit = r with { RecipientSeat = recipient, Branch = branch, Stage = SuitPreventionBenefitStage.CostPaid } });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, cost, from, new($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit)}"), moved =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { SuitPreventionBenefit = active.SuitPreventionBenefit!with { CostMovementSequence = moved.Sequence } });
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionPaymentEvent(f.Id, r.BeforeDamageFrameId, r.OriginalAttackFrameId, f.OwnerSeat, id, from, r.CostEffectiveSuit!.Value, recipient, branch, moved.Sequence));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainSuitPlacementChildren(ProgramSkillFrame f)
    {
        var continuation = f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement;
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, continuation) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, continuation) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 500)]
    internal bool ResumeSuitPreventionBenefit(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.SuitPreventionBenefit is not { } r)
            return false;
        if (r.Stage is SuitPreventionBenefitStage.ChoosingCost or SuitPreventionBenefitStage.ChoosingRecipient)
            return true;
        if (f.AttackAttempt is not null)
            return true; // Only its actual damage pipeline may complete this paid instruction.
        if (DrainSuitPlacementChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.SuitPreventionBenefit!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        var recipient = _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat!.Value];
        var effect = SuitBenefitEffect(f);
        if (r.Stage == SuitPreventionBenefitStage.CostPaid)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !recipient.IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = r with { Stage = SuitPreventionBenefitStage.Finished } });
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            if (r.Branch == SuitPreventionBenefitBranch.DamageAndDraw)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = r with { Stage = SuitPreventionBenefitStage.Damaging } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionBenefitIssuedEvent(f.Id, recipient.Seat, r.Branch.Value, r.SourceLess ? null : r.SourceSeat, effect.Amount));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), recipient.Seat, effect.Amount, r.SourceLess ? null : new ProgramParticipantReference(ProgramParticipantRef.EventSource), DamageNature.Normal, r.SourceLess);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = r with { Stage = SuitPreventionBenefitStage.LosingHp, HpBefore = recipient.Hp } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionBenefitIssuedEvent(f.Id, recipient.Seat, r.Branch!.Value, null, effect.Amount));
            if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, recipient.Seat, effect.Amount) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == SuitPreventionBenefitStage.Damaging)
        {
            var count = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && recipient.IsAlive ? Math.Min(effect.MaximumCards, Math.Max(0, recipient.MaxHp - recipient.Hp)) : 0;
            var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), SuitPreventionBenefit = r with { Stage = SuitPreventionBenefitStage.Drawing, RequestedDrawCount = count, MovementSequenceBefore = before } });
            if (count > 0)
                _corePhysicalDrawFacadesOperations.DrawCards(recipient, count, true, new($"skill-program.{(f.SkillId)}.suit-prevention-draw"));
            var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
            var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient.Seat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.suit-prevention-draw");
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { SuitPreventionBenefit = active.SuitPreventionBenefit!with { MovementSequenceAfter = after, ActualDrawCount = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionDrawEvent(id, recipient.Seat, count, actual, before, after));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (r.Stage == SuitPreventionBenefitStage.LosingHp)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = r with { Stage = SuitPreventionBenefitStage.Gifting }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && recipient.IsAlive && _cardZonesOperations._cardZones.GetLocation(r.CostCardId!.Value) == CardLocation.DiscardPile)
            {
                var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == r.CostCardId);
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(recipient.Seat), new($"skill-program.{(f.SkillId)}.suit-prevention-gift"), moved =>
                {
                    var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(active with { SuitPreventionBenefit = active.SuitPreventionBenefit!with { GiftMovementSequence = moved.Sequence } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionGiftEvent(id, recipient.Seat, card.Id, moved.Sequence, false));
                });
            }
            else
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { SuitPreventionBenefit = active.SuitPreventionBenefit!with { GiftUnavailable = true } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionGiftEvent(id, recipient.Seat, r.CostCardId!.Value, 0, true));
            }

            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SuitPreventionBenefit = null });
        return false;
    }

    internal void CaptureSuitPreventionDamageCompleted(ProgramSkillFrame f, bool applied)
    {
        if (f.SuitPreventionBenefit is not { Stage: SuitPreventionBenefitStage.Damaging, Branch: SuitPreventionBenefitBranch.DamageAndDraw } r)
            return;
        if (!IsValidSuitPreventionBenefit(f, true) || f.AttackReturn?.ParentAttackOwnerFrameId != r.OriginalAttackFrameId)
            throw new InvalidOperationException("Suit benefit damage returned to a different original owner.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSuitPreventionDamageCompletedEvent(f.Id, r.OriginalAttackFrameId, r.RecipientSeat!.Value, applied));
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 480)]
    internal bool ReturnSuitPlacementMovement(ProgramSkillFrame f)
    {
        if (f.SuitPreventionBenefit is null && f.MatchedJudgmentPlacement is null)
            return false;
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Suit placement lost its typed movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit)]
    internal PromptChoice SelectAiSuitPreventionBenefit(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.SuitPreventionBenefit!.Stage == SuitPreventionBenefitStage.ChoosingCost)
            return decision.Choices.OrderBy(c => CardCatalog.Get(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(c.Cards[0])).Single(x => x.Id == c.Cards[0]).Kind).HandKeepValue).ThenBy(c => c.Cards[0]).First();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(view, c.Targets.Single(), new SkillProgramAiHint(0, 0, 0, c.Parameters["branch"] == "damage-draw" ? Math.Min(5, _coreMatchParticipantsPlayersAccess._players[c.Targets[0]].MaxHp - _coreMatchParticipantsPlayersAccess._players[c.Targets[0]].Hp + SuitBenefitEffect(f).Amount) : 1, 0, SuitBenefitEffect(f).Amount, false, false))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    internal bool IsValidSuitPreventionBenefit(ProgramSkillFrame f, bool requirePayment)
    {
        if (f.SuitPreventionBenefit is not { } r || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || r.InstructionIndex != f.InstructionIndex || SuitBenefitEffect(f).Op != SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit || context.ParentFrameId != r.BeforeDamageFrameId || context.TargetSeat != f.OwnerSeat || context.Amount != r.OriginalAmount || context.SourceSeat != (r.SourceLess ? null : r.SourceSeat) || r.OriginalTargetSeat != f.OwnerSeat || r.OriginalAmount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.SourceSeat) || !_resolutionStackOperations._resolutionStack.Any(p => p.Id == r.OriginalAttackFrameId) || _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == r.BeforeDamageFrameId)is not { } parent || (parent.ContinuationAttackResolutionId ?? parent.ParentFrameId) != r.OriginalAttackFrameId || parent.SourceSeat != r.SourceSeat || parent.TargetSeat != f.OwnerSeat || parent.Amount != r.OriginalAmount || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].Candidate))
            return false;
        var paid = r.Stage is not (SuitPreventionBenefitStage.ChoosingCost or SuitPreventionBenefitStage.ChoosingRecipient);
        if (parent.Prevented != paid || requirePayment && !paid)
            return false;
        if (!paid)
            return r.CostMovementSequence == 0 && r.RecipientSeat is null && r.Branch is null && (r.Stage == SuitPreventionBenefitStage.ChoosingCost ? r.CostCardId is null && r.CostFrom is null && r.CostEffectiveSuit is null : r.CostCardId is { } chosen && r.CostFrom is { } held && held.OwnerSeat == f.OwnerSeat && held.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(chosen) == held && SuitBenefitEffect(f).Suits.Contains(r.CostEffectiveSuit!.Value));
        if (r.CostCardId is not { } cardId || r.CostFrom is not { } from || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !SuitBenefitEffect(f).Suits.Contains(r.CostEffectiveSuit!.Value) || r.RecipientSeat is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == f.OwnerSeat || r.Branch is null || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.CostMovementSequence && m.CardId == cardId && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardSuitPreventDamageAndBenefit)}"))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSuitPreventionPaymentEvent>().Count(e => e.ProgramFrameId == f.Id && e.BeforeDamageFrameId == parent.Id && e.OriginalAttackFrameId == r.OriginalAttackFrameId && e.OwnerSeat == f.OwnerSeat && e.CostCardId == cardId && e.CostFrom == from && e.EffectiveSuit == r.CostEffectiveSuit && e.RecipientSeat == seat && e.Branch == r.Branch && e.MovementSequence == r.CostMovementSequence) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDamagePreventedEvent>().Count(e => e.FrameId == parent.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.OwnerSeat == f.OwnerSeat && e.Amount == parent.Amount) == 1;
    }

    SkillProgramStepOutcome global::CardGame.Core.ISuitPreventionAndJudgmentPlacementHost.DiscardSuitPreventDamageAndBenefit(ProgramSkillFrame f) => BeginSuitPreventionBenefit(f);
    SkillProgramStepOutcome global::CardGame.Core.ISuitPreventionAndJudgmentPlacementHost.PlaceMatchedJudgmentCard(ProgramSkillFrame f, string bind, IReadOnlyList<Suit> suits) => _coreMatchedJudgmentPlacementsOperations.BeginMatchedJudgmentPlacement(f, bind, suits);
}

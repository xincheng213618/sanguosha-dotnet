#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGrantedEntityPhasesRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IGrantedEntityPhaseProgramHost.InsertGrantedEntityPlayPhase(ProgramSkillFrame f) => BeginGrantedEntityPlayPhase(f);
    SkillProgramStepOutcome global::CardGame.Core.IGrantedEntityPhaseProgramHost.ClaimGrantedPhaseSlash(ProgramSkillFrame f) => BeginGrantedPhaseSlashClaim(f);
    internal SkillProgramStepOutcome BeginGrantedEntityPlayPhase(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.IssuedEntityPhase is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("An inserted entity phase cannot be issued twice.");
        f = f with
        {
            IssuedEntityPhase = new(new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, 0, GrantedEntityPhaseStage.Scheduled)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        return _coreProgramLifecycleOperations.ScheduleProgramPhase(f, TurnPhase.Play, SkillProgramPhaseContinuation.BeforeNormalPreparation);
    }

    internal void IssueGrantedEntityPhaseBoundary(CharacterState actor)
    {
        if (_programPhaseScheduleOperations._programPhaseSchedule is not { Phase: TurnPhase.Play } schedule || schedule.Frame.IssuedEntityPhase is not { Stage: GrantedEntityPhaseStage.Scheduled } receipt)
            return;
        if (actor.Seat != schedule.Frame.OwnerSeat || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || receipt.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("The granted entity phase lost its actual inserted actor/turn.");
        receipt = receipt with
        {
            Stage = GrantedEntityPhaseStage.Active,
            PhaseInstanceId = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId
        };
        _programPhaseScheduleOperations._programPhaseSchedule = schedule with
        {
            Frame = schedule.Frame with
            {
                IssuedEntityPhase = receipt
            }
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GrantedEntityPhaseStartedEvent(schedule.Frame.Id, receipt.Source, receipt.TurnNumber, receipt.PhaseInstanceId));
    }

    internal bool IsGrantedEntityPhaseActive(int owner) => _programPhaseScheduleOperations._programPhaseSchedule is { Phase: TurnPhase.Play } schedule && schedule.Frame.OwnerSeat == owner && schedule.Frame.IssuedEntityPhase is { Stage: GrantedEntityPhaseStage.Active } receipt && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner && receipt.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && receipt.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterCondition, 10)]
    internal bool CanOfferGrantedPhaseSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ClaimGrantedPhaseSlash))
            return true;
        return IsGrantedEntityPhaseActive(candidate.OwnerSeat) && _programPhaseScheduleOperations._programPhaseSchedule!.Frame.IssuedEntityPhase is { CardId: null } receipt && receipt.Source.SkillId == candidate.SkillId && receipt.Source.SkillInstanceId == candidate.SkillInstanceId;
    }

    internal SkillProgramStepOutcome BeginGrantedPhaseSlashClaim(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.GrantedPhaseSlashClaim is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting || !IsGrantedEntityPhaseActive(f.OwnerSeat) || _programPhaseScheduleOperations._programPhaseSchedule!.Frame.IssuedEntityPhase is not { } phase || phase.Source.SkillId != f.SkillId || phase.Source.SkillInstanceId != f.SkillInstanceId)
            throw new InvalidOperationException("A phase Slash choice requires its exact newly issued extra phase.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GrantedPhaseSlashClaim = new(f.InstructionIndex, _programPhaseScheduleOperations._programPhaseSchedule.Frame.Id, phase.PhaseInstanceId, false) });
        PublishGrantedPhaseSlash(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> GrantedPhaseSlashChoices(ProgramSkillFrame f)
    {
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string option, string text, IReadOnlyList<int>? ids = null) => new(new($"granted-phase-slash.{(f.Id)}.{(option)}"), text, ids ?? [], [], new Dictionary<string, string> { ["program-action"] = "granted-phase-slash", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = option });
        // The deck offers only a source, never its unrevealed identity set.
        if (_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Any(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)))
            choices.Add(Choice("deck", "从牌堆获得一张杀"));
        foreach (var card in _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)))
            choices.Add(Choice("discard-" + card.Id, $"获得弃牌堆的【{(card.DisplayName)}】（{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))} {(card.RankText)}）", [card.Id]));
        choices.Add(Choice("skip", "不获得杀"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishGrantedPhaseSlash(ProgramSkillFrame f)
    {
        var choices = GrantedPhaseSlashChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "可以从牌堆或弃牌堆获得一张杀。本额外阶段未造成伤害，结束时仍对自己造成1点伤害。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("granted-phase-slash")]
    internal void ResolveGrantedPhaseSlashChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A phase claim lost its current program.");
        var receipt = f.GrantedPhaseSlashClaim ?? throw new InvalidOperationException("A phase claim lost its unpaid receipt.");
        if (receipt.Paid || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || !GrantedPhaseSlashChoices(f).Any(c => c.Id == choice.Id))
            throw new InvalidOperationException("A phase claim choice is not currently published.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !IsGrantedEntityPhaseActive(f.OwnerSeat) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || choice.Parameters["option"] == "skip")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { GrantedPhaseSlashClaim = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var from = choice.Parameters["option"] == "deck" ? CardLocation.DrawPile : CardLocation.DiscardPile;
        var card = from == CardLocation.DrawPile ? _cardZonesOperations._cardZones.CardsAt(from).Reverse().FirstOrDefault(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)) : choice.Cards is [var id] ? _cardZonesOperations._cardZones.CardsAt(from).SingleOrDefault(c => c.Id == id && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)) : null;
        if (card is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { GrantedPhaseSlashClaim = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { GrantedPhaseSlashClaim = receipt with { Paid = true, CardId = card.Id } });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new("program.granted-phase-slash.claim"), beforeFact: moved =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { GrantedPhaseSlashClaim = active.GrantedPhaseSlashClaim!with { ClaimMovementSequence = moved.Sequence } });
            var schedule = _programPhaseScheduleOperations._programPhaseSchedule!;
            _programPhaseScheduleOperations._programPhaseSchedule = schedule with
            {
                Frame = schedule.Frame with
                {
                    IssuedEntityPhase = schedule.Frame.IssuedEntityPhase!with
                    {
                        CardId = card.Id,
                        ClaimMovementSequence = moved.Sequence
                    }
                }
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GrantedPhaseSlashClaimedEvent(schedule.Frame.Id, f.Id, schedule.Frame.IssuedEntityPhase!.Source, receipt.PhaseInstanceId, card.Id, moved.Sequence));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 620)]
    internal bool ResumeGrantedPhaseSlashClaim(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.GrantedPhaseSlashClaim is not { } receipt)
            return false;
        if (!receipt.Paid)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { GrantedPhaseSlashClaim = null });
        return false;
    }

    internal ProgramSkillFrame FreezeGrantedEntityPhaseEnd(ProgramSkillFrame f)
    {
        if (f.IssuedEntityPhase is not { Stage: GrantedEntityPhaseStage.Active } receipt)
            return f;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || receipt.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || receipt.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("A granted extra phase cannot end through another phase's counter.");
        var dealt = _coreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess._playPhaseDamageDealtByCurrentPlayer;
        receipt = receipt with
        {
            Stage = GrantedEntityPhaseStage.Returning,
            DamageDealt = dealt
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GrantedEntityPhaseEndedEvent(f.Id, receipt.Source, receipt.TurnNumber, receipt.PhaseInstanceId, dealt));
        return f with
        {
            IssuedEntityPhase = receipt
        };
    }

    internal bool IsReturningGrantedEntityPhase(ProgramSkillFrame f) => f.IssuedEntityPhase is { Stage: GrantedEntityPhaseStage.Returning } r && r.DamageDealt is not null && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GrantedEntityPhaseEndedEvent>().Any(e => e.FrameId == f.Id && e.Source == r.Source && e.TurnNumber == r.TurnNumber && e.PhaseInstanceId == r.PhaseInstanceId && e.DamageDealt == r.DamageDealt);
    [ProgramContinuation(ProgramContinuationKind.Resume, 610)]
    internal bool ResumeGrantedEntityPhaseTrailer(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.IssuedEntityPhase is not { } receipt || receipt.Stage is not (GrantedEntityPhaseStage.Returning or GrantedEntityPhaseStage.DamagePaid))
            return false;
        if (receipt.Stage == GrantedEntityPhaseStage.Returning)
        {
            if (!IsReturningGrantedEntityPhase(f))
                throw new InvalidOperationException("The exact completed extra phase lost its issued trailer.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { IssuedEntityPhase = receipt with { Stage = GrantedEntityPhaseStage.DamagePaid } });
            if (receipt.DamageDealt == 0 && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, f.OwnerSeat, 1);
                return true;
            }
        }

        if (f.AttackAttempt is not null || _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(f.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { IssuedEntityPhase = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).IssuedEntityPhase!with { Stage = GrantedEntityPhaseStage.Complete } });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive);
        return true;
    }

    internal bool HasGrantedPhaseEntityDistance(CharacterState owner, IReadOnlyList<int>? ids)
    {
        if (ids is not [var id] || !IsGrantedEntityPhaseActive(owner.Seat) || _programPhaseScheduleOperations._programPhaseSchedule!.Frame.IssuedEntityPhase is not { CardId: { } claimed, ClaimMovementSequence: { } sequence } receipt || claimed != id)
            return false;
        if (!_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == sequence && m.CardId == id && m.To == CardLocation.Hand(owner.Seat)))
            return false;
        var outside = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.CardId == id && m.Sequence > sequence && !global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime.IsProvenanceOwnedZone(m.To, owner.Seat)).ToArray();
        if (global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceCardPoliciesRuntime.IsProvenanceOwnedZone(_cardZonesOperations._cardZones.GetLocation(id), owner.Seat))
            return outside.Length == 0;
        // An already-paid successful declaration retains its exact original Hand
        // material; no unrelated Processing entity can revive the phase grant.
        if (_coreCardDeclarationsOperations.UnclaimedDeclarationPayment(owner.Seat, id)is not { } payment || payment.ActorSeat != owner.Seat || payment.Source.OwnerSeat != owner.Seat || payment.Cost.From != CardLocation.Hand(owner.Seat) || outside is not [var entry] || entry.From != payment.Cost.From || entry.To != CardLocation.Processing || entry.Reason.Value != "conversion.declaration.pay" || entry.CardKind != payment.Cost.CardKind || _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(m => m.CardId == id).Sequence != entry.Sequence || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardDeclarationCommittedEvent>().Any(e => e.DeclarationId == payment.DeclarationId && e.OwnerSeat == owner.Seat && e.ActorSeat == owner.Seat && e.DeclaredKind == payment.DeclaredKind))
            return false;
        return _resolutionStackOperations._resolutionStack.Any(frame => frame.Id == payment.OwnerFrameId && (frame.AcceptedDeclarationPayment == payment || frame is CardDeclarationFrame { Stage: CardDeclarationStage.Applying, Succeeded: true } declaration && declaration.Id == payment.DeclarationId && declaration.Payment == payment));
    }

    internal void IssueGrantedPhaseEntityUseDistance(long frameId, CardActionContext? action)
    {
        if (action is not { Type: CardActionType.Use } || action.ActorSeat != action.ProviderSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || !HasGrantedPhaseEntityDistance(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action.PhysicalCards.Select(c => c.CardId).ToArray()))
            return;
        var schedule = _programPhaseScheduleOperations._programPhaseSchedule!;
        var receipt = schedule.Frame.IssuedEntityPhase!;
        var policy = new GrantedEntityDistanceUseIssued(frameId, action.ActionId, schedule.Frame.Id, receipt.Source, receipt.PhaseInstanceId, receipt.CardId!.Value);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(frameId, f => f with { GrantedEntityDistance = policy });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GrantedEntityDistanceUseIssuedEvent(policy));
    }

    internal bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor) => useId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { GrantedEntityDistance: { } policy, Action: { } action } && policy.CardUseFrameId == id && action.ActorSeat == actor && action.ProviderSeat == actor && policy.Source.OwnerSeat == actor && action.ActionId == policy.CardActionId && action.PhysicalCards is [var cost] && cost.CardId == policy.CardId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind);
    internal bool HasPotentialGrantedPhaseSlash(CharacterState owner) => IsGrantedEntityPhaseActive(owner.Seat) && _coreActionQueriesOperations.GetSlashUseCards(owner).Any(card => HasGrantedPhaseEntityDistance(owner, [card.Id]));
    internal long? GrantedEntityTrailerHealthOwner(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent applied || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] is not DamageFrame damage || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { IssuedEntityPhase: { Stage: GrantedEntityPhaseStage.DamagePaid, DamageDealt: 0 } phase, AttackAttempt: { DamageWasApplied: true } attack } owner || damage.ParentFrameId != owner.Id || phase.Source.OwnerSeat != owner.OwnerSeat || attack.SourceSeat != owner.OwnerSeat || attack.SourceLess || applied.SourceLess || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || damage.Nature != applied.Nature || attack.SourceSeat != applied.SourceSeat || attack.TargetSeat != applied.TargetSeat)
            return null;
        return owner.Id;
    }
}

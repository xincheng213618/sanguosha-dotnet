#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMobileRuntime
{
    internal const string HpDifferencePileDiscard = "skill-program.public-pile.hp-difference-payment";
    internal bool? HpDifferencePileCardColor(int owner, Card card) => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card));
    internal int HpDifferencePileX(int owner, int target) => _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && _coreMatchParticipantsPlayersAccess._players[target].HasHpValue ? Math.Max(1, Math.Abs(_coreMatchParticipantsPlayersAccess._players[owner].Hp - _coreMatchParticipantsPlayersAccess._players[target].Hp)) : int.MaxValue;
    internal Card[] HpDifferencePileCosts(int owner, string skill, string instance, bool red) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => HpDifferencePileCardColor(owner, c) == red && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skill, instance, c) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).OrderBy(c => c.Id).ToArray();
    internal IReadOnlyList<int> HpDifferencePileDamageTargets(int owner, string skill, string instance, SkillProgramEffect effect) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, SkillProgramTargetKind.OtherLivingInAttackRange).Where(target => _corePublicPileColorsOperations.SameGrantColorPiles(owner, effect.SkillIds.Single(), instance).Any(p => HpDifferencePileCardColor(owner, _corePublicPileSourcesOperations.PublicPileCards(p).Single())is { } red && HpDifferencePileCosts(owner, skill, instance, red).Length >= HpDifferencePileX(owner, target))).ToArray();
    internal SkillProgramStepOutcome StartHpDifferencePileDamage(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (frame.HpDifferencePileDamage is not null || frame.TriggerId is not null || frame.InstructionIndex != 1 || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(frame.OwnerSeat, frame.SkillId) != SkillPolarity.Yang || HpDifferencePileDamageTargets(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect).Count == 0)
            throw new InvalidOperationException("HP-difference pile damage requires its original payable Yang activation.");
        var ren = _corePublicPileColorsOperations.SameGrantColorPiles(frame.OwnerSeat, effect.SkillIds.Single(), frame.SkillInstanceId).First(p => HpDifferencePileCardColor(frame.OwnerSeat, _corePublicPileSourcesOperations.PublicPileCards(p).Single())is { } red && _coreProgramLifecycleOperations.GetProgramTargetSeats(frame.OwnerSeat, SkillProgramTargetKind.OtherLivingInAttackRange).Any(target => HpDifferencePileCosts(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, red).Length >= HpDifferencePileX(frame.OwnerSeat, target)));
        var card = _corePublicPileSourcesOperations.PublicPileCards(ren).Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HpDifferencePileDamage = new(ren, card.Id, HpDifferencePileCardColor(frame.OwnerSeat, card)!.Value) });
        PublishHpDifferencePileDamage(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice HpDifferencePileChoice(ProgramSkillFrame f, string branch, int value, string label) => new(new($"pile-hp-difference.{(f.Id)}.{(f.HpDifferencePileDamage!.CardIds.Count)}.{(branch)}.{(value)}"), label, branch == "card" ? [value] : [], branch == "target" ? [value] : [], new Dictionary<string, string> { ["program-action"] = "pile-hp-difference", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> HpDifferencePileChoices(ProgramSkillFrame f)
    {
        var r = f.HpDifferencePileDamage!;
        if (r.Stage == HpDifferencePileDamageStage.Cards)
            return HpDifferencePileCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, r.IsRed).Where(c => !r.CardIds.Contains(c.Id)).Select(c => HpDifferencePileChoice(f, "card", c.Id, $"弃置【{(c.DisplayName)}】（{(r.CardIds.Count + 1)}/{(r.Required)}）")).ToArray();
        return _coreProgramLifecycleOperations.GetProgramTargetSeats(f.OwnerSeat, SkillProgramTargetKind.OtherLivingInAttackRange).Where(seat => HpDifferencePileCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, r.IsRed).Length >= HpDifferencePileX(f.OwnerSeat, seat)).Select(seat => HpDifferencePileChoice(f, "target", seat, $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}：弃置{(HpDifferencePileX(f.OwnerSeat, seat))}张同色牌，造成1点伤害")).ToArray();
    }

    internal void PublishHpDifferencePileDamage(ProgramSkillFrame f)
    {
        var choices = HpDifferencePileChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("HP-difference discard lost its payable target or owned cost.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("pile-hp-difference")]
    internal void AnswerHpDifferencePileDamage(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var r = f.HpDifferencePileDamage!;
        if (!HpDifferencePileChoices(f).Any(c => c.Id == choice.Id) || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture))
            throw new InvalidOperationException("HP-difference damage accepts only its current exact choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == HpDifferencePileDamageStage.Target)
        {
            var target = choice.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HpDifferencePileDamage = r with { Stage = HpDifferencePileDamageStage.Cards, TargetSeat = target, Required = HpDifferencePileX(f.OwnerSeat, target), OwnerHp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, TargetHp = _coreMatchParticipantsPlayersAccess._players[target].Hp } });
            PublishHpDifferencePileDamage(f);
            return;
        }

        var ids = r.CardIds.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HpDifferencePileDamage = r = r with { CardIds = ids } });
        if (ids.Length < r.Required)
        {
            PublishHpDifferencePileDamage(f);
            return;
        }

        if (_coreProgramConversionPolarityOperations.GetProgramConversionPolarity(f.OwnerSeat, f.SkillId) != SkillPolarity.Yang || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || ids.Any(id => !HpDifferencePileCosts(f.OwnerSeat, f.SkillId, f.SkillInstanceId, r.IsRed).Any(c => c.Id == id)))
            throw new InvalidOperationException("HP-difference damage lost its original exact same-color cost before payment.");
        _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HpDifferencePileDamage = r with { Stage = HpDifferencePileDamageStage.Movement, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(HpDifferencePileDiscard));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HpDifferencePileDamage = f.HpDifferencePileDamage!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpDifferencePileDamagePaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, r.Required, r.OwnerHp, r.TargetHp, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueHpDifferencePileDamage(f.Id);
    }

    internal void ContinueHpDifferencePileDamage(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HpDifferencePileDamage: { Stage: HpDifferencePileDamageStage.Movement } r } f || f.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, HpDifferencePileDamage = r with { Stage = HpDifferencePileDamageStage.Damage } });
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1, new(ProgramParticipantRef.Owner));
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 81400)]
    internal bool ResumeHpDifferencePileDamage(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HpDifferencePileDamage: { } r } f || f.Id != id)
            return false;
        if (r.Stage != HpDifferencePileDamageStage.Movement)
            return false;
        ContinueHpDifferencePileDamage(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 81400)]
    internal bool ReturnHpDifferencePileDamage(ProgramSkillFrame f) => f.HpDifferencePileDamage is { Stage: HpDifferencePileDamageStage.Movement } && ResumeHpDifferencePileDamage(f.Id);
    internal bool MobileLuZhiCompletion(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool committed = false)
    {
        if (candidate.SkillId != MobileLuZhiComposition.Zhenliang || context.CardUse is not { } card || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat == candidate.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber < 1 || !committed && _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(candidate.OwnerSeat, candidate.SkillId) != SkillPolarity.Yin || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || window.Action.ActionId != card.CardActionId)
            return false;
        var action = context.Window == SkillProgramTriggerWindow.CardResponseCompleted ? CoreGameEngineResponseCompletionRuntime.ResponseCompletionActorAction(window) : window.Action;
        if (action.ActorSeat != candidate.OwnerSeat || !_corePublicPileColorsOperations.SameGrantColorPiles(candidate.OwnerSeat, MobileLuZhiComposition.Mingren, candidate.SkillInstanceId).Any(p => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_corePublicPileSourcesOperations.PublicPileCards(p).Single().Kind) == CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind)))
            return false;
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted ? action.Type == CardActionType.Use || window.CompletedResponseReturn is { IsCommitted: false } && _coreResponseUseCompletionOperations.IsCompletedResponseParentConsistent(window) : context.Window == SkillProgramTriggerWindow.CardResponseCompleted && window.ResponseCompletion is { CostsDrained: true } && _coreResponseCompletionOperations.ValidResponseCompletionWindow(window) && !IsMobileLuZhiTrueResponseUse(action);
    }

    internal bool IsMobileLuZhiTrueResponseUse(CardActionContext action) => _coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Nullification) || _coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, ProgramCompletedResponseKind.Dodge) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } kind } attack } && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && attack.TargetSeat == action.ActorSeat;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 81400, SkillProgramEffectOp.CommitConversionPolarity)]
    internal bool CanOfferMobileLuZhiCompletion(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId != MobileLuZhiComposition.Zhenliang || MobileLuZhiCompletion(candidate, context);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 81400)]
    internal void CaptureMobileLuZhiYin(ProgramConversionPolarityCommittedEvent fact)
    {
        if (fact.SkillId != MobileLuZhiComposition.Zhenliang || fact.PreviousState != SkillPolarity.Yin)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(fact.FrameId);
        var context = f.WindowContext ?? throw new InvalidOperationException("Mobile Zhenliang Yin lost its original completion.");
        if (!MobileLuZhiCompletion(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), context, true))
            throw new InvalidOperationException("Mobile Zhenliang Yin cannot commit without its actual matching-category completion.");
        var ren = _corePublicPileColorsOperations.SameGrantColorPiles(f.OwnerSeat, MobileLuZhiComposition.Mingren, f.SkillInstanceId).Single();
        var renCard = _corePublicPileSourcesOperations.PublicPileCards(ren).Single();
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MobileLuZhiYin = new(context.CardUse!.CardActionId, context.ParentFrameId, context.Window, renCard.Id, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(renCard.Kind), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) });
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81400)]
    internal bool KeepMobileLuZhiPaid(ProgramSkillFrame f) => f.SkillId == MobileLuZhiComposition.Zhenliang && (f.HpDifferencePileDamage is { Stage: HpDifferencePileDamageStage.Movement or HpDifferencePileDamageStage.Damage } && f.ConversionPreviousPolarity == SkillPolarity.Yang || f.MobileLuZhiYin is { } r && f.ConversionPreviousPolarity == SkillPolarity.Yin && f.WindowContext?.ParentFrameId == r.ParentWindowId && f.InstructionIndex >= 3);
}

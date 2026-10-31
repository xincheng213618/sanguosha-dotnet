#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsRuntime
{
    internal bool MatchesProgramJudgmentReplacement(CharacterState owner, SkillProgramTrigger trigger, int targetSeat, string reason) => trigger.Window == SkillProgramTriggerWindow.JudgmentReplacing && CoreGameEngineProgramMarkerPaymentsRuntime.CanPayProgramMarkerCost(owner, trigger.MarkerCost) && (trigger.Subject == SkillProgramTriggerSubject.Any || owner.Seat == targetSeat) && !trigger.ExcludedReasons.Contains(reason, StringComparer.Ordinal) && trigger.Effects[0].Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner));
    internal IReadOnlyList<Card> GetProgramJudgmentReplacementCards(CharacterState owner, SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.ReplaceJudgmentFromPublicRice }])
            return _coreRiceJudgmentOperations.RiceJudgmentReplacementCards(owner, trigger);
        if (CharacterWangYiMobileRuntime.IsDeckTopJudgmentReplacement(trigger))
            return _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(1).ToArray();
        if (trigger.Window != SkillProgramTriggerWindow.JudgmentReplacing || trigger.Effects.Count == 0 || trigger.Effects[0] is not { Op: SkillProgramEffectOp.ReplaceJudgment or SkillProgramEffectOp.DelegateJudgmentReplacement } replacement || !replacement.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)))
            return[];
        // Collection preserves the opportunity: an earlier replacement can change
        // the current color. Published choices and payment always recheck it.
        bool? currentColor = null;
        if (replacement.RequireDifferentColor == true && !ignoreCurrentColor)
        {
            if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is not { } current || current.TargetSeat != judgmentSubjectSeat || !string.Equals(current.Reason, judgmentReason, StringComparison.Ordinal) || _coreNativeDamageHandlesOperations.GetJudgmentCard(current)is not { } currentCard)
                return[];
            currentColor = CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[judgmentSubjectSeat], currentCard));
            if (currentColor is null)
                return[];
        }

        var cards = new List<Card>();
        if (replacement.Zones.Contains(CardZoneKind.Hand))
            cards.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(owner));
        if (replacement.Zones.Contains(CardZoneKind.Equipment))
            cards.AddRange(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner));
        return cards.Where(card => !_coreDelegatedJudgmentChoicesOperations.IsDelegatedJudgment(trigger) || !card.IsGeneralWeapon).Where(card => !_coreNativeJudgmentOperations.IsProtectedJudgmentSourceEquipment(owner, card, judgmentSubjectSeat, judgmentReason)).Where(card => replacement.Suits.Contains(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card))).Where(card => replacement.RequireDifferentColor != true || CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card))is { } candidateColor && (ignoreCurrentColor || candidateColor != currentColor)).OrderBy(card => card.Id).ToArray();
    }

    internal (SkillProgram Program, SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(JudgmentTriggerCandidate candidate)
    {
        if (string.IsNullOrWhiteSpace(candidate.ProgramId) || string.IsNullOrWhiteSpace(candidate.ProgramTriggerId) || string.IsNullOrWhiteSpace(candidate.SkillInstanceId) || string.IsNullOrWhiteSpace(candidate.GameplayHash))
            throw new InvalidOperationException("The judgment replacement candidate has no configured identity.");
        var program = _contentRegistryOperations._contentRegistry.Skills[candidate.ProgramId].Program ?? throw new InvalidOperationException("The judgment replacement program is unavailable.");
        if (!string.Equals(program.GameplayHash, candidate.GameplayHash, StringComparison.Ordinal))
            throw new InvalidOperationException("A running judgment replacement definition changed.");
        var trigger = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, candidate.ProgramTriggerId).Trigger!;
        if (trigger.Window != SkillProgramTriggerWindow.JudgmentReplacing)
            throw new InvalidOperationException("The configured judgment candidate no longer replaces judgments.");
        return (program, trigger);
    }

    internal void BeginProgramJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate)
    {
        if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment?.Id != pending.Id || !ReferenceEquals(_coreNativeJudgmentOperations.CurrentJudgmentCandidate(pending), candidate) || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)is not { } judgmentCard)
            throw new InvalidOperationException("The configured judgment replacement is not current.");
        var(program, trigger) = GetProgramJudgmentReplacement(candidate);
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var enabled = owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, program.Id, candidate.SkillInstanceId) && MatchesProgramJudgmentReplacement(owner, trigger, pending.TargetSeat, pending.Reason);
        if (CharacterWangYiMobileRuntime.IsDeckTopJudgmentReplacement(trigger))
        {
            if (enabled && _characterWangYiMobileOperations.HasDeckTopJudgmentReplacementMaterial())
                _characterWangYiMobileOperations.BeginDeckTopJudgmentReplacementChoice(pending, candidate, program, trigger);
            else
                _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
            return;
        }

        var replacementCards = enabled ? GetProgramJudgmentReplacementCards(owner, trigger, pending.TargetSeat, pending.Reason) : [];
        if (replacementCards.Count == 0)
        {
            _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
            return;
        }

        if (_coreDelegatedJudgmentChoicesOperations.IsDelegatedJudgment(trigger))
        {
            _coreDelegatedJudgmentChoicesOperations.BeginDelegatedJudgmentChoice(pending, candidate, program, trigger, replacementCards);
            return;
        }

        _coreNativeJudgmentOperations.SetJudgmentFrameStep(pending.Id, ResolutionFrameStep.AwaitingResponse);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementRequestedEvent(pending.Id, pending.Id, pending.TargetSeat, owner.Seat, pending.Reason, judgmentCard.Id, judgmentCard.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], judgmentCard)));
        var choices = replacementCards.Select(card => new PromptChoice(new ChoiceId($"program-judgment-replace.{(program.Id)}.{(trigger.Id)}.card-{(card.Id)}"), $"用【{(card.DisplayName)}】替换 {(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].Name)} 的判定牌。", [card.Id], [], new Dictionary<string, string> { ["action"] = "program-judgment-replace", ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        if (trigger.Optional)
            choices.Add(new PromptChoice(new ChoiceId($"program-judgment-replace.{(program.Id)}.{(trigger.Id)}.skip"), "不发动技能，保留当前判定牌。", [], [], new Dictionary<string, string> { ["action"] = "program-judgment-replace-skip" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramJudgmentReplacement, owner.Seat, $"【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】：是否替换当前判定牌？", replacementCards.Select(card => card.Id).ToArray(), [], SourceSeat: pending.TargetSeat, IncomingCard: judgmentCard.Kind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = pending.TargetSeat,
            Choices = choices.ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal CommandResult SubmitProgramJudgmentReplacementAnswer(int actorSeat, PromptId prompt, ChoiceId choice)
    {
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(actorSeat, DecisionKind.ProgramJudgmentReplacement, prompt, CommandErrorCode.IllegalAction);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(error.Code, error.Message);
        var selected = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.SingleOrDefault(item => item.Id == choice);
        if (selected is null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The judgment replacement choice is not available.");
        return _kernelCoreMainOperations.Accept(() =>
        {
            ResolveProgramJudgmentReplacementChoice(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void ResolveProgramJudgmentReplacementChoice(PromptChoice selected)
    {
        if (_coreDelegatedJudgmentChoicesOperations.TryResolveDelegatedJudgmentChoice(selected))
            return;
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("The configured judgment replacement continuation is missing.");
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(pending) ?? throw new InvalidOperationException("The configured judgment replacement candidate is missing.");
        var(program, trigger) = GetProgramJudgmentReplacement(candidate);
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var oldJudgmentCard = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The configured judgment replacement lost its current card.");
        if (_characterWangYiMobileOperations.TryResolveDeckTopJudgmentReplacement(selected, pending, candidate, program, trigger))
            return;
        int? selectedCardId = selected.Parameters.GetValueOrDefault("action") switch
        {
            "program-judgment-replace" => int.Parse(selected.Parameters["card-id"], System.Globalization.CultureInfo.InvariantCulture),
            "program-judgment-replace-skip" when trigger.Optional => null,
            _ => throw new InvalidOperationException("Unsupported configured judgment replacement choice.")};
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selectedCardId is not { } cardId)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementResolvedEvent(pending.Id, pending.Id, pending.TargetSeat, owner.Seat, pending.Reason, Used: false, oldJudgmentCard.Id, null, null, null, null));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentReplacementResolvedEvent(pending.Id, program.Id, trigger.Id, owner.Seat, pending.TargetSeat, Activated: false, oldJudgmentCard.Id, null, trigger.Effects[0].OldCardDestination!.Value, 0, 0));
            _coreMatchLogOperations.AddLog("JudgmentReplacementSkipped", $"{(owner.Name)} 选择不发动【{(_contentRegistryOperations._contentRegistry!.Skills[program.Id].Name)}】。", owner.Seat, pending.TargetSeat);
            _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
            return;
        }

        var replacement = GetProgramJudgmentReplacementCards(owner, trigger, pending.TargetSeat, pending.Reason).SingleOrDefault(card => card.Id == cardId) ?? throw new InvalidOperationException("The selected configured replacement card is no longer legal.");
        var binding = _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, SkillProgramTriggerWindow.JudgmentReplacing).Single(item => item.SkillId == program.Id && item.Trigger.Id == trigger.Id);
        var shared = new ProgramTriggerCandidate(owner.Seat, program.Id, trigger.Id, binding.SkillInstanceId, program.GameplayHash, trigger.Priority);
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.JudgmentReplacing, pending.Id, owner.Seat, SourceSeat: pending.SourceSeat, TargetSeat: pending.TargetSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner), JudgmentReplacement: new ProgramJudgmentReplacementContext(pending.Id, pending.TargetSeat, pending.Reason, oldJudgmentCard.Id, replacement.Id));
        _coreProgramLifecycleOperations.BeginProgramBinding(shared, context);
    }

    internal void ReplaceProgramJudgment(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var context = frame.WindowContext?.JudgmentReplacement ?? throw new InvalidOperationException("Replacement primitive requires its frozen judgment choice.");
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("Replacement primitive lost its judgment continuation.");
        if (pending.Id != context.JudgmentFrameId || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)?.Id != context.OldCardId || pending.TargetSeat != context.SubjectSeat || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Replacement primitive no longer matches its judgment.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(frame);
        var replacement = GetProgramJudgmentReplacementCards(owner, trigger, pending.TargetSeat, pending.Reason).SingleOrDefault(card => card.Id == context.ReplacementCardId) ?? throw new InvalidOperationException("The selected replacement card is no longer legal.");
        var old = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The replacement primitive lost its judgment card.");
        var committedSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], replacement);
        var destination = effect.OldCardDestination ?? throw new InvalidOperationException("Replacement has no old-card destination.");
        CommitProgramJudgmentReplacement(pending, owner, old, replacement, committedSuit, destination);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { WindowContext = active.WindowContext!with { JudgmentReplacement = context with { ReplacementSuit = committedSuit, ReplacementRank = replacement.Rank } } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementResolvedEvent(pending.Id, pending.Id, pending.TargetSeat, owner.Seat, pending.Reason, Used: true, old.Id, replacement.Id, replacement.Kind, committedSuit, replacement.Rank));
        _coreMatchLogOperations.AddLog("JudgmentReplaced", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.Skills[frame.SkillId].Name)}】替换了判定牌。", owner.Seat, pending.TargetSeat);
    }

    internal void CompleteProgramJudgmentReplacementBinding(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext?.JudgmentReplacement ?? throw new InvalidOperationException("Replacement program lost its frozen judgment context.");
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("Replacement program lost its judgment continuation.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame parent || parent.Id != context.JudgmentFrameId || pending.Id != parent.Id)
            throw new InvalidOperationException("Replacement program did not return to its judgment frame.");
        var effect = _coreProgramLifecycleOperations.GetProgramTrigger(frame).Effects[0];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentReplacementResolvedEvent(parent.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, context.SubjectSeat, Activated: context.ReplacementSuit is not null, context.OldCardId, context.ReplacementSuit is not null ? context.ReplacementCardId : null, effect.OldCardDestination!.Value, context.DrawnCards, context.RecoveredHp));
        _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
    }

    internal void CommitProgramJudgmentReplacement(JudgmentFrame pending, CharacterState owner, Card oldJudgmentCard, Card replacement, Suit committedSuit, SkillProgramOldJudgmentCardDestination oldDestination, CardLocation? exactRiceLocation = null)
    {
        var oldFrom = CardLocation.Judgment(pending.TargetSeat);
        if (_cardZonesOperations._cardZones.GetLocation(oldJudgmentCard.Id) != oldFrom)
            throw new InvalidOperationException("The current judgment card is no longer in its judgment zone.");
        var topInput = exactRiceLocation is null && _characterWangYiMobileOperations.IsDeckTopJudgmentReplacementInput(pending, owner, replacement);
        var replacementFrom = exactRiceLocation ?? (topInput ? CardLocation.DrawPile : _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(owner, replacement));
        if (exactRiceLocation is not null ? replacementFrom.Zone != CardZoneKind.PublicPersistentPile || replacementFrom.OwnerSeat != owner.Seat || _cardZonesOperations._cardZones.GetLocation(replacement.Id) != replacementFrom || !_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Values.Any(p => p.Location == replacementFrom && _coreRicePileOperations.RiceProducer(p)) : !topInput && replacementFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("The configured replacement card is not in an owned input zone.");
        var requestedOldTo = oldDestination == SkillProgramOldJudgmentCardDestination.OwnerHand ? CardLocation.Hand(owner.Seat) : CardLocation.DiscardPile;
        var oldGainDestination = _coreHappyWeiYanGainDestinationsOperations.PlanHappyWeiYanGainDestination(oldJudgmentCard, oldFrom, requestedOldTo, CardMoveReasons.ProgramJudgmentOldCard);
        var oldTo = oldGainDestination?.ActualTo ?? requestedOldTo;
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([oldFrom, replacementFrom, CardLocation.Processing], [oldTo, CardLocation.Processing, oldFrom]);
        var movements = new List<CardMovementRecord>(3);
        var committed = false;
        try
        {
            _cardZonesOperations._cardZones.MoveBatch([new CardTransfer(oldJudgmentCard.Id, oldFrom, oldTo), new CardTransfer(replacement.Id, replacementFrom, CardLocation.Processing)]);
            _cardZonesOperations._cardZones.Move(replacement.Id, CardLocation.Processing, oldFrom);
            _coreNativeJudgmentOperations.ReplaceJudgmentFrame(_coreNativeJudgmentOperations.GetJudgmentFrame(pending.Id)with { CardId = replacement.Id, CardKind = replacement.Kind, Suit = committedSuit });
            movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(oldJudgmentCard, oldFrom, oldTo, CardMoveReasons.ProgramJudgmentOldCard, happyWeiYanGainDestination: oldGainDestination));
            _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(oldJudgmentCard, oldFrom, oldTo);
            movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(replacement, replacementFrom, CardLocation.Processing, CardMoveReasons.ProgramJudgmentReplace));
            _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(replacement, replacementFrom, CardLocation.Processing);
            if (pending.DelegatedReplacement is not null)
            {
                movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(replacement, CardLocation.Processing, oldFrom, CardMoveReasons.ProgramJudgmentReplace));
                _coreGainGiftProgramsOperations.RecordDelegatedJudgmentPayment(pending, batch.Id);
                _coreEquipmentSkillsOperations.ResolveEquipmentSkillGrant(replacement, replacementFrom, CardLocation.Processing);
                _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(replacement, replacementFrom, CardMoveReasons.ProgramJudgmentReplace);
                _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(replacement, replacementFrom, CardLocation.Processing);
            }
            else
            {
                _corePhysicalMovementFacadesOperations.ResolveSilverLionRemoval(replacement, replacementFrom, CardMoveReasons.ProgramJudgmentReplace);
                _corePhysicalMovementFacadesOperations.ResolveWoodenOxMove(replacement, replacementFrom, CardLocation.Processing);
                movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(replacement, CardLocation.Processing, oldFrom, CardMoveReasons.ProgramJudgmentReplace));
                _corePhysicalMovementFacadesOperations.ClearJudgmentEffectiveKindAfterMove(replacement, CardLocation.Processing, oldFrom);
            }

            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, movements, committed);
        }
    }

    internal bool IsAiProgramJudgmentReplacementPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramJudgmentReplacement, PlayerSeat: var playerSeat } && !_coreMatchParticipantsPlayersAccess._players[playerSeat].IsHuman;
    internal void ResolvePendingAiProgramJudgmentReplacement()
    {
        if (_coreDelegatedJudgmentChoicesOperations.ResolveAiDelegatedJudgmentChoice())
            return;
        if (_characterWangYiMobileOperations.TryResolveAiDeckTopJudgmentReplacement())
            return;
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("AI configured judgment replacement is missing.");
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI configured judgment replacement prompt is missing.");
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(pending) ?? throw new InvalidOperationException("AI configured judgment candidate is missing.");
        var(_, trigger) = GetProgramJudgmentReplacement(candidate);
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var judgmentCard = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("AI configured judgment replacement lost its current card.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == pending.ParentFrameId);
        var successSuits = parent is null ? null : _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program?.Triggers.SingleOrDefault(item => item.Id == parent.TriggerId)?.Effects.Select(effect => effect.Condition).FirstOrDefault(condition => condition.Kind == SkillProgramConditionKind.BoundCardsMatchSuits && condition.SourceBind == pending.ProgramResultBind)?.Suits;
        var(cardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ChooseJudgmentReplacement(_corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat), pending.TargetSeat, pending.Reason, decision.ValidCardIds, judgmentCard.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], judgmentCard), judgmentCard.Rank, successSuits, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        var selected = cardId is { } id ? decision.Choices.Single(choice => choice.Cards.SequenceEqual([id])) : trigger.Optional ? decision.Choices.Single(choice => choice.Cards.Count == 0) : decision.Choices.First(choice => choice.Cards.Count == 1);
        ResolveProgramJudgmentReplacementChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}

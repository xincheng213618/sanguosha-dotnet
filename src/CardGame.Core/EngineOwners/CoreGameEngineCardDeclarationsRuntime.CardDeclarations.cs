#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardDeclarationsRuntime
{
    internal const string DeclarationTurnUsage = "declaration";
    internal DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId)
    {
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
        {
            var frame = _resolutionStackOperations._resolutionStack[index];
            var payment = frame is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (payment is { Claimed: false } && payment.ProviderSeat == provider && payment.Cost.CardId == cardId && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing)
                return payment;
        }

        return null;
    }

    internal Card? AvailableDeclarationCard(CharacterState owner)
    {
        // Applying declarations form the last segment of the original lookup,
        // so even an older declaration takes precedence over accepted receipts.
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
            if (_resolutionStackOperations._resolutionStack[index] is CardDeclarationFrame { Stage: CardDeclarationStage.Applying, Payment: { Claimed: false } receipt } && receipt.ProviderSeat == owner.Seat)
                return _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(card => card.Id == receipt.Cost.CardId);
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
            if (_resolutionStackOperations._resolutionStack[index].AcceptedDeclarationPayment is { Claimed: false } acceptedReceipt && acceptedReceipt.ProviderSeat == owner.Seat)
                return _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(card => card.Id == acceptedReceipt.Cost.CardId);
        return null;
    }

    internal DeclaredCardPayment? TransferDeclarationToCardUse(long useId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration)
            return null;
        _coreRuntimeOperations.PopResolutionFrame(declaration.Id, ResolutionFrameKind.CardDeclaration);
        return declaration.Payment with
        {
            OwnerFrameId = useId
        };
    }

    internal bool ClaimDeclarationPayment(Card card, CardLocation from, CardLocation to)
    {
        if (from.OwnerSeat is not { } provider || to != CardLocation.Processing || UnclaimedDeclarationPayment(provider, card.Id)is not { } payment || payment.Cost.From != from)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.Single(frame => frame.Id == payment.OwnerFrameId);
        if (parent.AcceptedDeclarationPayment != payment)
            throw new InvalidOperationException("A declaration payment lost its actual continuation owner.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { AcceptedDeclarationPayment = payment with { Claimed = true } });
        _coreNativeUsedDiscardOriginsOperations.Value.CaptureClaimedDeclarationTrueUse(payment);
        _coreShanheStartingResourcesOperations.CaptureShanheClaimedDeclarationUse(payment);
        // A Dodge/Played response may be owned by the incoming card's frame.
        // Its receipt is not a payment for that frame's attacking Use action.
        if (payment.ReturnPurpose is not (CardDeclarationPurpose.Dodge or CardDeclarationPurpose.Duel or CardDeclarationPurpose.Group or CardDeclarationPurpose.Counterspell or CardDeclarationPurpose.FactionDefense or CardDeclarationPurpose.FactionSlash or CardDeclarationPurpose.QizhengResponse or CardDeclarationPurpose.MouLuSuAnniversaryPlayed or CardDeclarationPurpose.MouSimaYiPlayed))
            _characterBianFuRenOlOperations.CaptureBianFuRenClaimedDeclarationPayment(parent.Id);
        return true;
    }

    internal CardLocation CapturedDeclarationOrigin(int cardId, CardLocation actual)
    {
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
        {
            var frame = _resolutionStackOperations._resolutionStack[index];
            var payment = frame is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (payment is { Claimed: false } && payment.Cost.CardId == cardId)
                return payment.Cost.From;
        }

        return actual;
    }

    internal string PublicDeclarationDescription(Card card, string ordinaryDescription)
    {
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
        {
            var frame = _resolutionStackOperations._resolutionStack[index];
            var payment = frame is CardDeclarationFrame declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (payment is { IsRevealed: false } && payment.Cost.CardId == card.Id)
                return _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing ? "声明【" + CardCatalog.Get(payment.DeclaredKind).DisplayName + "】" : ordinaryDescription;
        }

        return ordinaryDescription;
    }

    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 30)]
    internal void AddDeclarationSlashActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable)
    {
        actions.RemoveAll(action => action.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null && (action.Kind == LegalActionKind.Recast || CoreNativeCardClassificationsRuntime.IsSlashCard(action.PlayedCardKind ?? CardKind.Dodge)));
        if (!_coreNationalSkillsOperations.GetSkillBindingShard(actor).ViewAsPrograms.Any(program => program.ViewAs.Any(rule => rule.DeclarationValidation is not null)))
            return;
        foreach (var card in playable.Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card)))
            foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false).Where(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null))
                {
                    var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: kind);
                    foreach (var target in targets)
                        actions.Add(new LegalAction(LegalActionKind.Slash, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, "声明【" + CardCatalog.Get(kind).DisplayName + "】"), PlayedCardKind: kind) { ConversionSource = source });
                    _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(actions, actor, card, targets, CardCatalog.Get(kind).DisplayName, kind, source);
                    _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, CardCatalog.Get(kind).DisplayName, kind, source);
                }
    }

    internal DeclaredCardPayment? LiveDeclarationPayment(int cardId)
    {
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
        {
            var frame = _resolutionStackOperations._resolutionStack[index];
            var payment = frame is CardDeclarationFrame declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (payment?.Cost.CardId == cardId && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing)
                return payment;
        }

        return null;
    }

    internal bool CanDeclareCard(CharacterState owner, string skillId) => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, skillId, DeclarationTurnUsage, SkillUsageScope.Turn) == 0;
    internal CardConversionSource? PeekDeclarationConversion(CharacterState owner, Card card, CardKind kind, bool use, CardConversionSource? explicitSource = null)
    {
        if (UnclaimedDeclarationPayment(owner.Seat, card.Id)is { } paid)
            return paid.Source;
        var hasPublishedSource = explicitSource is not null || _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice || use && _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice;
        var selected = explicitSource ?? (use && _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice ? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion : _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice ? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion : use ? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion : _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion);
        if (selected is not null)
            return _corePlayCardHistoryOperations.ViewAsRule(selected)?.DeclarationValidation is not null ? selected : null;
        if (hasPublishedSource || CoreNativeCardClassificationsRuntime.MatchesRequiredCard(card.Kind, kind))
            return null;
        return _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, kind, !use, _coreDyingCursorRuntimeOperations.Value.ActiveDying is not null).FirstOrDefault(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null);
    }

    internal CardDeclarationParentReturn DeclarationReturn(CardDeclarationPurpose purpose, int actor, LegalAction? action = null, int? target = null, int recovery = 1, IReadOnlyList<ProgramRecoveryPolicySource>? policies = null, DyingResponseEvent? dying = null, bool fan = false, CardKind? finalKind = null)
    {
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        return new(purpose, parent?.Id ?? 0, parent?.Kind, parent?.Step, actor, action, target, recovery, policies is null ? null : Array.AsReadOnly(policies.ToArray()), dying, fan, finalKind);
    }

    internal bool TryBeginCardDeclaration(CharacterState provider, Card card, CardKind kind, CardConversionSource? source, CardDeclarationParentReturn parent, IReadOnlyList<int> targets)
    {
        if (source is null || _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is null || UnclaimedDeclarationPayment(provider.Seat, card.Id)is not null)
            return false;
        if (source.OwnerSeat != provider.Seat)
            throw new InvalidOperationException("A declaration requires its actual provider's own selected source.");
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Hand(provider.Seat) || !CanDeclareCard(provider, source.SkillId))
            throw new InvalidOperationException("A declaration requires a new legal hand payment in the actual turn.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var declared = _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind;
        var payment = new DeclaredCardPayment(id, id, provider.Seat, new(card.Id, card.Kind, CardLocation.Hand(provider.Seat), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(provider.Seat, card)), source, declared, ActorSeat: parent.ActorSeat, FrozenSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(provider, card))
        {
            JuewuEffectiveRank = _characterGuanYuWumiaoOperations.JuewuForeignHandRank(provider, card),
            ReturnPurpose = parent.Purpose,
            OriginalRank = _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(provider, card)
        };
        payment = _characterCaiWenJiLeOperations.CaptureBeifenDeclarationPayment(payment, parent);
        payment = _characterHuangGaiMouMobileOperations.CaptureMouZhaJiangDeclaration(payment, parent);
        payment = _characterLuKangWumiaoOperations.CaptureWuLuKangDixianDeclaration(payment, parent);
        payment = _characterGuanYuWumiaoOperations.CaptureWuyouDeclarationPayment(payment, parent);
        payment = _characterXiaoQiaoLeOperations.CaptureWeiwanDeclarationPayment(payment, parent);
        payment = _characterZhouYuOlMouOperations.CaptureMouGuquDeclaration(payment, parent);
        payment = _characterDiaoChanLeOperations.CaptureDiouDeclaration(payment, parent);
        payment = _characterSunQuanOlMagicOperations.CaptureMoQuanyuDeclarationQuota(payment, parent, targets);
        payment = _characterSimaYiMouOperations.CaptureMouSmDeclaration(payment, parent);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice = false;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = false;
        _coreRuntimeOperations.PushRuntimeFrame(new CardDeclarationFrame(id, provider.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, payment, parent, Array.AsReadOnly(targets.ToArray())));
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(provider.Seat, source.SkillId, DeclarationTurnUsage, SkillUsageScope.Turn, 1))
            throw new InvalidOperationException("Declaration quota changed during acceptance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(provider.Seat, source.SkillId, DeclarationTurnUsage, SkillUsageScope.Turn, 1));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(provider.Seat), CardLocation.Processing, new("conversion.declaration.pay"), movement =>
        {
            var current = (CardDeclarationFrame)_resolutionStackOperations._resolutionStack.Single(frame => frame.Id == id);
            if (movement.CardId != current.Payment.Cost.CardId || movement.CardKind != current.Payment.Cost.CardKind || movement.From != current.Payment.Cost.From || movement.To != CardLocation.Processing || current.Payment.BianFuRenPaymentSequence is { } prior && prior != movement.Sequence)
                throw new InvalidOperationException("A declaration must retain its exact original physical payment.");
            // Reuse the existing declaration payment sequence, even in a small
            // registry without a Yuejian observer; this is the same real move.
            _coreRuntimeOperations.ReplaceRuntimeFrame(id, current with { Payment = current.Payment with { BianFuRenPaymentSequence = movement.Sequence } });
        });
        _characterSunQuanOlMagicOperations.RecordMoQuanyuDeclarationPayment(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardDeclarationCommittedEvent(id, provider.Seat, parent.ActorSeat, declared, Array.AsReadOnly(targets.ToArray())));
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }

    internal bool CanChallengeDeclaration(int seat, int owner) => seat != owner && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Any(instance => instance.Program.CannotChallengeDeclarations);
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterDeclarationChallenge, 0, NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(CardDeclarationFrame), 190)]
    internal void ContinueCardDeclaration(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardDeclarationFrame frame || frame.Id != id)
            return;
        if (frame.Stage == CardDeclarationStage.Paying)
        {
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(frame.Payment.Cost.CardId) != CardLocation.Processing)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = CardDeclarationStage.Returning, Succeeded = false });
                ContinueCardDeclaration(id);
                return;
            }

            var seats = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, offset)).Where(seat => CanChallengeDeclaration(seat, frame.OwnerSeat)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = CardDeclarationStage.Challenging });
            var child = new CardDeclarationChallengeFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, id, Array.AsReadOnly(seats));
            _coreRuntimeOperations.PushRuntimeFrame(child);
            ContinueCardDeclarationChallenge(child.Id);
            return;
        }

        if (frame.Stage == CardDeclarationStage.Granting)
        {
            if (_coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = CardDeclarationStage.Returning });
            ContinueCardDeclaration(id);
            return;
        }

        if (frame.Stage == CardDeclarationStage.Cleaning)
        {
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = CardDeclarationStage.Returning });
            ContinueCardDeclaration(id);
            return;
        }

        if (frame.Stage == CardDeclarationStage.Returning)
            ReturnCardDeclaration(frame);
    }

    [NativeFrameRoute(typeof(CardDeclarationChallengeFrame), 200)]
    internal void ContinueCardDeclarationChallenge(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardDeclarationChallengeFrame child || child.Id != id)
            return;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardDeclarationFrame>().Single(parent => parent.Id == child.ParentFrameId);
        while (child.SeatIndex < child.EligibleSeats.Count && !CanChallengeDeclaration(child.EligibleSeats[child.SeatIndex], frame.OwnerSeat))
        {
            child = child with
            {
                SeatIndex = child.SeatIndex + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(child);
        }

        if (child.SeatIndex == child.EligibleSeats.Count)
        {
            _coreRuntimeOperations.PopResolutionFrame(id, ResolutionFrameKind.CardDeclarationChallenge);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Succeeded = true, Stage = CardDeclarationStage.Returning });
            ContinueCardDeclaration(frame.Id);
            return;
        }

        var seat = child.EligibleSeats[child.SeatIndex];
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.RespondDodge, seat, $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 声明【{(CardCatalog.Get(frame.Payment.DeclaredKind).DisplayName)}】，是否质疑？", [], [], SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(new[] { new PromptChoice(new ChoiceId("declaration.pass"), "不质疑", [], [], new Dictionary<string, string> { { "declaration-answer", "pass" } }), new PromptChoice(new ChoiceId("declaration.challenge"), "质疑并翻牌", [], [], new Dictionary<string, string> { { "declaration-answer", "challenge" } }) })
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void AnswerCardDeclaration(PromptChoice choice)
    {
        var child = _resolutionStackOperations._resolutionStack.LastOrDefault() as CardDeclarationChallengeFrame ?? throw new InvalidOperationException("The declaration challenge has no owner.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != child.EligibleSeats[child.SeatIndex])
            throw new InvalidOperationException("The challenge responder changed.");
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardDeclarationFrame>().Single(parent => parent.Id == child.ParentFrameId);
        var seat = child.EligibleSeats[child.SeatIndex];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters.GetValueOrDefault("declaration-answer") == "pass")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(child with { SeatIndex = child.SeatIndex + 1 });
            ContinueCardDeclarationChallenge(child.Id);
            return;
        }

        if (choice.Parameters.GetValueOrDefault("declaration-answer") != "challenge" || !CanChallengeDeclaration(seat, frame.OwnerSeat))
            throw new InvalidOperationException("The challenge is no longer legal.");
        _coreRuntimeOperations.PopResolutionFrame(child.Id, ResolutionFrameKind.CardDeclarationChallenge);
        var success = frame.Payment.Cost.CardKind == frame.Payment.DeclaredKind;
        frame = frame with
        {
            Payment = frame.Payment with
            {
                IsRevealed = true
            },
            IsRevealed = true,
            ChallengerSeat = seat,
            Succeeded = success,
            Stage = success ? CardDeclarationStage.Granting : CardDeclarationStage.Cleaning
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == frame.Payment.Cost.CardId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardDeclarationRevealedEvent(frame.Id, seat, _coreSnapshotValueProjectionOperations.ToSnapshot(card), success));
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        if (success)
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[seat], $"declaration:{(frame.Payment.Source.SkillId)}:{(frame.Payment.Source.SkillInstanceId)}", [_corePlayCardHistoryOperations.ViewAsRule(frame.Payment.Source)!.DeclarationValidation!.ChallengeGrantSkillId]);
        else
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, new("conversion.declaration.failed"));
        ContinueCardDeclaration(frame.Id);
    }

    internal IReadOnlyList<CardDeclarationSnapshot>? GetCardDeclarationSnapshots(int viewer, bool revealAll)
    {
        var values = _resolutionStackOperations._resolutionStack.OfType<CardDeclarationFrame>().Select(frame =>
        {
            var cost = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(frame.Payment.Cost.CardId)).Single(card => card.Id == frame.Payment.Cost.CardId);
            return new CardDeclarationSnapshot(frame.Id, frame.OwnerSeat, frame.Return.ActorSeat, frame.Payment.DeclaredKind, _characterSimaYiMouOperations.MouSmDeclarationTargets(frame, viewer, revealAll), frame.IsRevealed, frame.IsRevealed ? _coreSnapshotValueProjectionOperations.ToSnapshot(cost) : null, revealAll || viewer == frame.OwnerSeat ? _coreSnapshotValueProjectionOperations.ToSnapshot(cost) : null);
        }).ToArray();
        return values.Length == 0 ? null : Array.AsReadOnly(values);
    }

    internal void ReturnCardDeclaration(CardDeclarationFrame frame)
    {
        var parent = frame.Return;
        var parentExists = parent.ParentFrameId == 0 || _resolutionStackOperations._resolutionStack.Any(candidate => candidate.Id == parent.ParentFrameId && candidate.Kind == parent.ParentKind);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(frame.Payment.Cost.CardId)).Single(card => card.Id == frame.Payment.Cost.CardId);
        var cancelled = !parentExists || !owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None;
        var success = frame.Succeeded == true && !cancelled && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing;
        if (!success && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Stage = CardDeclarationStage.Cleaning, Succeeded = false });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, new("conversion.declaration.cancelled"));
            ContinueCardDeclaration(frame.Id);
            return;
        }

        if (success && parent.Purpose == CardDeclarationPurpose.Use)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Stage = CardDeclarationStage.Applying });
            if (_characterLvKuangLvXiangOlOperations.TryApplyQigongDeclaration(frame))
                return;
            _coreNativeActionDispatchOperations.ExecuteAction(owner, parent.UseAction!);
            return;
        }

        if (success && parent.Purpose == CardDeclarationPurpose.Recovery && parent.ParentFrameId == 0 && parent.DyingResponse is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Stage = CardDeclarationStage.Applying });
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, _coreMatchParticipantsPlayersAccess._players[parent.TargetSeat!.Value], card, CardCatalog.Get(frame.Payment.DeclaredKind).DisplayName, frame.Payment.DeclaredKind, parent.RecoveryAmount, parent.RecoveryPolicies?.Select(policy => (policy.SkillId, policy.PolicyId)).ToArray(), frame.Payment.Source);
            return;
        }

        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.CardDeclaration);
        if (_characterLvKuangLvXiangOlOperations.TryReturnQigongDeclaration(frame, success))
            return;
        if (_coreJianjiDeclarationsOperations.Value.TryReturnJianjiDeclaration(frame, success))
            return;
        if (_characterSunHuanAnniversaryOperations.Value.TryReturnAnniversarySunHuanDeclaration(frame, success))
            return;
        if (_characterGuoYuanMobileShiOperations.TryReturnShiGyDeclaration(frame, success))
            return;
        if (_characterGuoYouZhiMouMobileOperations.Value.ReturnMouGyzDeclaration(frame, success))
            return;
        if (_characterKeBiNengOlOperations.TryReturnKeBiNengPingduanDeclaration(frame, success))
            return;
        if (_characterSimaYiMouOperations.ReturnMouSmPlayedDeclaration(frame, success))
            return;
        if (_characterJuShouMouOperations.ReturnMouJsDeclaration(frame, success) || _characterDaQiaoXiaoQiaoHappyGodOperations.ReturnHappyQiaoShutuDeclaration(frame, success))
            return;
        if (_characterLuSuMouOperations.Value.ReturnMouLuSuPlayedDeclaration(frame, success))
            return;
        if (cancelled)
            return;
        if (success)
        {
            var actualParent = _resolutionStackOperations._resolutionStack.LastOrDefault(candidate => candidate.Id == parent.ParentFrameId)!;
            if (actualParent is ResponseWindowFrame response)
                actualParent = _resolutionStackOperations._resolutionStack.Single(candidate => candidate.Id == response.ParentFrameId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(actualParent.Id, actualParent with { AcceptedDeclarationPayment = frame.Payment with { OwnerFrameId = actualParent.Id } });
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = frame.Payment.Source;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = true;
        }

        switch (parent.Purpose)
        {
            case CardDeclarationPurpose.Use:
                break;
            case CardDeclarationPurpose.Dodge:
                if (success)
                    _coreNativeDodgeOperations.ResolveDodgeResponse(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack!, owner, card);
                else
                {
                    var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack!;
                    if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == attack.ResolutionId)
                        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
                    _coreNativePromptBoundaryOperations.ClearPendingDecision();
                    if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                }

                break;
            case CardDeclarationPurpose.Duel:
                _coreNativeDuelOperations.ResolveDuelResponse(_coreNativeCardContinuationCursorsOperations.ActiveDuel!, owner, success ? card : null);
                break;
            case CardDeclarationPurpose.QizhengResponse:
                if (success)
                    _characterXunYuMobileGodOperations.PayQizhengSingleResponse(parent.ParentFrameId, owner, card, parent.FinalEffectiveKind ?? frame.Payment.DeclaredKind);
                else
                    _characterXunYuMobileGodOperations.ResolveQizhengAfterResponse(parent.ParentFrameId);
                break;
            case CardDeclarationPurpose.Group:
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard!, owner, success ? card : null);
                break;
            case CardDeclarationPurpose.Counterspell:
                _coreNativeNullificationOperations.ResolveNullificationChoice(_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow!, owner, success ? card : null, success ? frame.Payment.Source : null);
                break;
            case CardDeclarationPurpose.Recovery:
                if (success)
                    _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, _coreMatchParticipantsPlayersAccess._players[parent.TargetSeat!.Value], card, CardCatalog.Get(frame.Payment.DeclaredKind).DisplayName, frame.Payment.DeclaredKind, parent.RecoveryAmount, parent.RecoveryPolicies?.Select(policy => (policy.SkillId, policy.PolicyId)).ToArray(), frame.Payment.Source, parent.DyingResponse);
                else if (parent.DyingResponse is { } dying)
                {
                    _coreNativeDyingOperations.CompleteDyingCardResponse(dying with { UsedPeach = false, PeachCardId = null, UsedAlcohol = false, AlcoholCardId = null });
                }

                break;
            case CardDeclarationPurpose.FactionDefense:
                if (success)
                    _coreNativeFactionDefenseOperations.CompleteFactionDefenseResponse(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense!, owner, card, false);
                else
                {
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense!.CandidateIndex++;
                    _coreNativeFactionDefenseOperations.AdvanceFactionDefenseCandidate();
                }

                break;
            case CardDeclarationPurpose.FactionSlash:
                if (success)
                    _coreNativeFactionSlashOperations.CompleteFactionSlashResponse(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!, owner, card);
                else
                {
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.CandidateIndex++;
                    _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
                }

                break;
            case CardDeclarationPurpose.ProvidedSlash:
                if (success)
                    _coreNativeFactionSlashOperations.BeginProvidedFactionSlashSlash(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!, owner, [card], parent.FinalEffectiveKind ?? frame.Payment.DeclaredKind, parent.UsesZhuqueFan, frame.Payment.Source);
                else
                {
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.CandidateIndex++;
                    _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
                }

                break;
            case CardDeclarationPurpose.BorrowedSwordProvidedSlash:
                if (success)
                    _coreNativeFactionSlashOperations.BeginBorrowedSwordFactionSlashSlash(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!, owner, [card], parent.FinalEffectiveKind ?? frame.Payment.DeclaredKind, parent.UsesZhuqueFan, frame.Payment.Source);
                else
                {
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.AwaitingProviders = true;
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.CandidateIndex++;
                    _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
                }

                break;
            case CardDeclarationPurpose.QinglongProvidedSlash:
                if (success)
                    _coreNativeFactionSlashOperations.BeginQinglongCrescentBladeFactionSlashSlash(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!, owner, [card], parent.FinalEffectiveKind ?? frame.Payment.DeclaredKind, parent.UsesZhuqueFan, frame.Payment.Source);
                else
                {
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.AwaitingProviders = true;
                    _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest!.CandidateIndex++;
                    _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
                }

                break;
            case CardDeclarationPurpose.BorrowedSword:
                if (success)
                    _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordSlashChoice(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, card, parent.FinalEffectiveKind ?? frame.Payment.DeclaredKind);
                else
                    _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, transferWeapon: true);
                break;
        }
    }
}

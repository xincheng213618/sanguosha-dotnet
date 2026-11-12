#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    internal static bool SameQigongNativeReceipt(QigongNativeReceipt? left, QigongNativeReceipt? right) => RuntimeSkills.RuntimeRules_lv_kuang_lv_xiang_ol_LvKuangLvXiangNativeUses.SameQigongNativeReceipt(left, right);
    internal IReadOnlyList<PromptChoice> QigongSlashChoices(ProgramSkillFrame f)
    {
        var r = f.LvKuangLvXiang!;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var result = new List<PromptChoice>();
        bool Legal(Card c, CardKind k, CardConversionSource? source = null, IReadOnlyList<int>? ids = null) => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, c, source, k, ignoreDistance: true, noEffectiveRank: ids?.Count > 1, physicalCardIds: ids) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, ids ?? [c.Id]);
        void Add(string key, IReadOnlyList<int> cards, CardKind kind, CardConversionSource? source = null, string? equipment = null, bool cardless = false)
        {
            var args = new Dictionary<string, string>
            {
                {
                    "effective-kind",
                    kind.ToString()
                }
            };
            if (source is not null)
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(args, source);
            if (equipment is not null)
                args["equipment"] = equipment;
            if (cardless)
                args["round-once-cardless"] = "true";
            result.Add(LvChoice(f, "slash", key, "对" + target.Name + "使用杀", cards, [target.Seat], args));
        }

        foreach (var card in _coreActionQueriesOperations.GetSlashUseCards(actor))
            foreach (var kind in _coreNativeTargetLegalityOperations.GetSlashUseKinds(actor, CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) ? card.Kind : CardKind.Slash))
            {
                if (!Legal(card, kind))
                    continue;
                foreach (var choice in _coreCardConversionsOperations.CreateConversionChoiceVariants(actor, card, kind, false, $"qigong.{(f.Id)}.{(card.Id)}.{(kind)}", "使用杀", [card.Id], [r.Target], new Dictionary<string, string> { { "effective-kind", kind.ToString() } }))
                    if (!CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source) || source is null || _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is null)
                        result.Add(LvChoice(f, "slash", choice.Id.Value, choice.Description, choice.Cards, choice.Targets, choice.Parameters));
            }

        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(actor)).DistinctBy(c => c.Id))
            foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Where(k => k != CardKind.Slash))
                foreach (var source in _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, false))
                    if (_corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is null && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, card) && Legal(card, kind, source))
                        Add($"typed-{(card.Id)}-{(kind)}-{(source.SkillInstanceId)}-{(source.BindingId)}", [card.Id], kind, source);
        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.EnumerateZhangbaSlashPairs(actor))
        {
            var ids = pair.Select(c => c.Id).ToArray();
            if (Legal(pair[0], CardKind.Slash, ids: ids))
                Add("zhangba-" + string.Join('-', ids), ids, CardKind.Slash, equipment: CardKind.ZhangbaSerpentSpear.ToString());
        }

        foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false))
            {
                var ids = selection.Cards.Select(c => c.Id).ToArray();
                if (_corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.DeclarationValidation is null && Legal(selection.Cards[0], kind, selection.Source, ids))
                    Add($"multi-{(kind)}-{(selection.Source.SkillInstanceId)}-{(selection.Source.BindingId)}-" + string.Join('-', ids), ids, kind, selection.Source);
            }

        if (_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundOnceCardlessBasics)
            foreach (var(source, _)in _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(actor, CardKind.Slash))
                if (_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new(0, CardKind.Slash, Suit.None, 0), source, CardKind.Slash, ignoreDistance: true, noEffectiveRank: true, physicalCardIds: []))
                    Add("cardless-" + source.SkillInstanceId + "-" + source.BindingId, [], CardKind.Slash, source, cardless: true);
        AddQigongNativeAdapterChoices(f, result);
        result.Add(LvChoice(f, "decline", "decline", "不使用杀"));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void IssueQigongSlash(ProgramSkillFrame f, PromptChoice choice)
    {
        if (TryBeginQigongNativeAdapter(f, choice))
            return;
        var r = f.LvKuangLvXiang!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var kind = Enum.Parse<CardKind>(choice.Parameters["effective-kind"]);
        var requestAcceptance = _slashRequests.Accept(f, r.Actor, r.Target);
        f = _requestExecution.PrepareAcceptedUse(f, requestAcceptance);
        r = f.LvKuangLvXiang!;
        if (choice.Cards.Count == 0)
        {
            if (choice.Parameters.GetValueOrDefault("round-once-cardless") != "true" || !CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source) || source is null)
                throw new InvalidOperationException("Qigong cardless use requires its independent true paid native source.");
            var paid = _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Stage = "issuing", IssuingIds = [], IssuingKind = CardKind.Slash, RoundOncePayment = paid } });
            IssueQigongCardless(f, source, CardKind.Slash, requestAcceptance);
            return;
        }

        var selection = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LvKuangLvXiang = r with { Stage = "issuing", IssuingIds = selection.Cards.Select(c => c.Id).ToArray(), IssuingKind = selection.Kind } });
        _coreNativeSlashUseOperations.ResolveSlashCore(actor, target, selection.Cards[0], selection.Kind, actor.Seat, physicalCards: selection.Cards, countsTowardSlashLimit: false, usesZhuqueFan: selection.Kind == CardKind.FireSlash && selection.Cards[0].Kind == CardKind.Slash && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), conversionSource: selection.Conversion, programSkillCardUseFrameId: f.Id, programSlashRequest: requestAcceptance);
    }

    internal void IssueQigongCardless(ProgramSkillFrame f, CardConversionSource source, CardKind kind, ProgramSlashRequestAcceptance? programSlashRequest = null)
    {
        var r = f.LvKuangLvXiang!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        programSlashRequest ??= _requestExecution.GetAcceptance(f, r.Actor, r.Target);
        if (programSlashRequest is null)
            throw new InvalidOperationException("Qigong's paid native use lost its accepted request.");
        var logical = new Card(0, kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(logical, actor.Seat, [target.Seat], kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), physicalCardIds: [], conversionSource: source, programSlashRequest: programSlashRequest);
        if (r.RoundOncePayment is not null)
            _coreRoundOnceCardlessNativeUsesOperations.IssueRoundOnceCardlessNativeUse(id, source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        CompleteQigongNativePayment(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void BindQigongNativeUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id) is not { Action: { Type: CardActionType.Use } action, ProgramSlashRequestExecution: { } execution } use || use.Qigong is not null ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == execution.ProgramFrameId) is not ProgramSkillFrame { LvKuangLvXiang: { } r } parent)
            return;
        var acceptance = execution.Acceptance;
        if (!_slashRequests.OwnsAcceptance(parent, acceptance, r.Actor, r.Target) || acceptance.Identity.Source != r.Source ||
            acceptance.Identity.GameplayHash != r.GameplayHash || r.Native is not null ||
            r.OriginalUseId is not { } originalUse || r.OriginalActionId is not { } originalAction || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind))
            throw new InvalidOperationException("Qigong native issuance lost its accepted original request.");
        var before = r.Declaration?.Before ?? _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (use.AcceptedDeclarationPayment is { } declaration && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(move =>
            move.CardId == declaration.Cost.CardId && move.From == declaration.Cost.From && move.To == CardLocation.Processing && move.Reason.Value == "conversion.declaration.pay") is { } declarationDeparture)
            before = Math.Min(before, declarationDeparture.Sequence - 1);
        var receipt = new QigongNativeReceipt(parent.Id, r.Source, r.GameplayHash, r.Instruction, r.Turn,
            originalUse, originalAction, r.Actor, r.Target, use.Id, action.ActionId, action, before, before)
        {
            RoundOncePayment = r.RoundOncePayment ?? use.RoundOnceCardlessBasicUse?.Payment,
            DrawFundedPayment = use.DrawFundedDistinctBasicUse?.Payment ?? r.DrawFundedPayment,
            ChainedPayment = use.ChainedStateBasicUse?.Payment ?? r.ChainedPayment,
            DeclarationPayment = use.AcceptedDeclarationPayment
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LvKuangLvXiang = r with
        {
            Stage = "native", Native = receipt, IssuingKind = action.EffectiveKind,
            IssuingProvider = action.ProviderSeat, IssuingIds = action.PhysicalCards.Select(cost => cost.CardId).ToArray()
        } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { Qigong = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QigongNativeIssuedEvent(receipt));
        if (action.PhysicalCards.Count == 0)
            CompleteQigongNativePayment(id);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashPaymentCompleted, 30)]
    internal void CompleteQigongNativePayment(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Qigong: { } r } use || r.Paid)
            return;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == r.ProgramId);
        if (!SameQigongNativeReceipt(parent.LvKuangLvXiang?.Native, r))
            throw new InvalidOperationException("Qigong payment lost its owning native issuer.");
        var paid = r with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Paid = true
        };
        if (!QigongMaterialPaymentMatches(use, paid))
            throw new InvalidOperationException("Qigong must pay exactly its chosen original native entities or independent state cost.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LvKuangLvXiang = parent.LvKuangLvXiang!with { Native = paid } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { Qigong = paid });
        _coreCardUseLifecycleOperations.IssueResponseRule(id, new(forbid: true, audience: ResponseRuleAudience.All));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QigongNativePaidEvent(paid));
    }

    internal bool QigongMaterialPaymentMatches(CardUseFrame use, QigongNativeReceipt r)
    {
        var action = r.OriginalAction;
        if (action.PhysicalCards.Count == 0)
            return use.ProgramSlashRequestExecution is { } execution && execution.ProgramFrameId == r.ProgramId &&
                execution.UseFrameId == use.Id && execution.CardActionId == action.ActionId &&
                _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramId) is ProgramSkillFrame parent &&
                _slashRequests.OwnsAcceptance(parent, execution.Acceptance, r.Actor, r.Target);
        return action.PhysicalCards.All(cost => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move =>
            move.CardId == cost.CardId && move.From == cost.From && move.Sequence > r.Before && move.Sequence <= r.After &&
            (move.Reason == CardMoveReasons.Use || r.DeclarationPayment is { } declaration && declaration.Cost == cost &&
                declaration.ProviderSeat == action.ProviderSeat && declaration.DeclaredKind == action.EffectiveKind &&
                action.ConversionChain.Contains(declaration.Source) && move.To == CardLocation.Processing && move.Reason.Value == "conversion.declaration.pay")) == 1);
    }

    internal bool ValidQigongNativeUse(CardUseFrame use)
    {
        if (use.Qigong is not { } receipt || use.Action is not { Type: CardActionType.Use } action ||
            use.ProgramSlashRequestExecution is not { } execution || execution.ProgramFrameId != receipt.ProgramId ||
            execution.UseFrameId != use.Id || execution.CardActionId != action.ActionId || receipt.UseId != use.Id || receipt.ActionId != action.ActionId ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == receipt.ProgramId) is not ProgramSkillFrame { LvKuangLvXiang: { } parentReceipt } parent ||
            !_slashRequests.OwnsAcceptance(parent, execution.Acceptance, receipt.Actor, receipt.Target) ||
            execution.Acceptance.Identity.Source != receipt.Source || receipt.Source != parentReceipt.Source || receipt.GameplayHash != parent.GameplayHash ||
            receipt.Instruction != parent.InstructionIndex || receipt.OriginalUseId != parentReceipt.OriginalUseId || receipt.OriginalActionId != parentReceipt.OriginalActionId ||
            !SameQigongNativeReceipt(parentReceipt.Native, receipt) || action.ProviderSeat != receipt.OriginalAction.ProviderSeat ||
            !action.PhysicalCards.SequenceEqual(receipt.OriginalAction.PhysicalCards) || !action.ConversionChain.SequenceEqual(receipt.OriginalAction.ConversionChain) ||
            use.PhysicalCardIds is null || !use.PhysicalCardIds.SequenceEqual(action.PhysicalCards.Select(cost => cost.CardId)) || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return false;
        return receipt.Paid ? QigongMaterialPaymentMatches(use, receipt) : receipt.Before == receipt.After;
    }

    internal bool IsQigongIssuedDistance(long? id, int actor) => id is { } useId && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Qigong: { Paid: true }, Action: { } action } use && action.ActorSeat == actor && ValidQigongNativeUse(use);
    internal bool QigongCardUseRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.Qigong?.ProgramId == parent.Id && ValidQigongNativeUse(use);
    [NativeUseValidation(80)]
    internal void ValidateQigongNativeUseReturn(ref CardUseFrame use)
    {
        AssertQigongNativeUse(use);
        if (use.Qigong is not null)
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
    }

    internal void AssertQigongNativeUse(CardUseFrame use)
    {
        if (use.Qigong is null)
            return;
        CompleteQigongNativePayment(use.Id);
        if (!ValidQigongNativeUse(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!))
            throw new InvalidOperationException("Qigong lost its exact paid native child before retirement.");
    }

    [NativeUseReturn(20)]
    internal bool CompleteQigongUseReturn(CardUseFrame use)
    {
        if (use.Qigong is null)
            return false;
        _coreDrawFundedDistinctBasicReturnsOperations.ReturnDrawFundedDistinctBasicUse(use);
        _coreChainedStateBasicReturnsOperations.ReturnChainedStateBasicUse(use);
        return true;
    }

    [NativeUsePreparation(30)]
    internal bool ReturnQigongNativeUse(CardUseFrame use)
    {
        if (use.Qigong is not { } r)
            return false;
        if (!ValidQigongNativeUse(use) || !r.Paid || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == r.ProgramId).LvKuangLvXiang!.Returned)
            throw new InvalidOperationException("Qigong native child cannot return another or twice-paid invoice.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == r.ProgramId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LvKuangLvXiang = parent.LvKuangLvXiang!with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QigongNativeReturnedEvent(parent.Id, use.Id, r.ActionId));
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 130)]
    internal bool ReturnRegisteredCompleteQigongSlash(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.Qigong is null)
            return false;
        CompleteQigongSlash(completion);
        return true;
    }

    internal void CompleteQigongSlash(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.Qigong is not { } r || !r.Paid || completion.ResolutionId != r.UseId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LvKuangLvXiang: { Returned: true, Native: { } paid } } parent || parent.Id != r.ProgramId || !SameQigongNativeReceipt(paid, r) || _resolutionStackOperations._resolutionStack.Any(f => f.Id == r.UseId))
            throw new InvalidOperationException("Qigong attack completion lost its once-consumed original native return.");
        RestoreQigongParentAttack(parent);
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
    }

    internal void RestoreQigongParentAttack(ProgramSkillFrame f)
    {
        if (f.LvKuangLvXiang is not { OriginalUseId: { } id, OriginalActionId: { } actionId } r)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { } action, CardAttack: { } attack } original || action.ActionId != actionId || original.TargetSeats is not [var target] || target != r.Target || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || attack.RequiredDodgeResponses < 1)
            throw new InvalidOperationException("Qigong must retain the actual original fully-dodged attack until its own child returns.");
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = _kernelCoreMainOperations.CreateCardAttackHandle2(id);
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 70)]
    internal bool DistancePhysicalSlash70IsQigongIssuedDistance(ProgramSlashDistanceQuery q) => IsQigongIssuedDistance(q.ExistingUseFrameId, q.Actor.Seat);
}

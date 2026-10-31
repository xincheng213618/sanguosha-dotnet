#nullable enable

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunYuMobileGodRuntime
{
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 30300)]
    internal void AddQizhengNaturalActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        foreach (var card in playable.Where(card => card.Kind == CardKind.QizhengXiangsheng && (selectedPhysicalCardId is null || selectedPhysicalCardId == card.Id) && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)))
            foreach (GameEngine.ProgramOrdinaryTrickUseOption option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.QizhengXiangsheng, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), enforceUsePermission: true, physicalCardIds: [card.Id]))
                actions.Add(new(option.ActionKind, card.Id, option.TargetSeats.Single(), option.Description, TargetSeats: option.TargetSeats));
    }

    internal bool TryExecuteQizhengNaturalUse(CharacterState actor, Card card, LegalAction action)
    {
        if (card.Kind != CardKind.QizhengXiangsheng || action.Kind != LegalActionKind.QizhengXiangsheng || action.ConversionSource is not null)
            return false;
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: card.Id).Any(a => a.Kind == action.Kind && a.CardId == card.Id && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("Qizheng lost its current legal designation.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, LegalActionKind.QizhengXiangsheng);
        return true;
    }

    internal void ResolveQizhengXiangshengEffect(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Action: { Type: CardActionType.Use } action, Qizheng: null } use || use.Id != pending.ParentFrameId || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.QizhengXiangsheng || pending.ActionKind != LegalActionKind.QizhengXiangsheng || CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending)is not { } target)
            throw new InvalidOperationException("Qizheng requires its original native ordinary-trick return.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CardKind.QizhengXiangsheng);
            return;
        }

        var r = new QizhengReceipt(pending.Id, action.ActionId, target, use.TargetIndex, _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(use.Id), QizhengStage.Designation);
        _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { Qizheng = r, Step = ResolutionFrameStep.AwaitingResponse });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengEffectStartedEvent(use.Id, r.NullificationId, r.ActionId, target, r.TargetIndex, r.EffectOrdinal));
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(use.CardId, CardKind.QizhengXiangsheng, use.SourceSeat, target));
        PublishQizheng(use);
    }

    internal static string QizhengCommitment(long use, long nullification, bool ambush, string nonce) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{(use)}:{(nullification)}:{(ambush)}:{(nonce)}")));
    internal Dictionary<string, string> QizhengParameters(string branch, string value = "") => new()
    {
        ["program-action"] = "qizheng",
        ["branch"] = branch,
        ["value"] = value
    };
    internal IReadOnlyList<PromptChoice> QizhengChoices(CardUseFrame use)
    {
        var r = use.Qizheng!;
        var choices = new List<PromptChoice>();
        var responder = _coreMatchParticipantsPlayersAccess._players[r.Target];
        string Id(string branch) => $"qizheng.{(use.Id)}.{(r.NullificationId)}.{(branch)}";
        if (r.Stage == QizhengStage.Designation)
        {
            choices.Add(new(new(Id("ambush")), "暗中指定奇兵", [], [r.Target], QizhengParameters("designation", "ambush")));
            choices.Add(new(new(Id("regular")), "暗中指定正兵", [], [r.Target], QizhengParameters("designation", "regular")));
        }
        else if (r.Stage == QizhengStage.Response)
        {
            if (!_characterLiuQiOlOperations.IsWenjiResponseForbidden2(use.Id, responder.Seat) && !_coreShanheTrickResponseOperations.IsShanheTrickResponseProhibited(use.SourceSeat, responder.Seat, CardKind.QizhengXiangsheng, use.TargetSeats))
                foreach (var required in new[]
                {
                    CardKind.Slash,
                    CardKind.Dodge
                }

                )
                {
                    foreach (var card in _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, required).Where(card => _coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(responder, card, required)))
                    {
                        var kind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, required);
                        var parameters = QizhengParameters("response", kind.ToString());
                        parameters["response-card-kind"] = kind.ToString();
                        parameters["response"] = required == CardKind.Slash ? "slash" : "dodge";
                        choices.AddRange(_coreCardConversionsOperations.CreateConversionChoiceVariants(responder, card, kind, true, Id($"respond.{(kind)}.{(card.Id)}"), $"打出【{(CardCatalog.Get(kind).DisplayName)}】", [card.Id], [], parameters));
                    }

                    foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, required, true))
                    {
                        var parameters = QizhengParameters("multi", required.ToString());
                        CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
                        choices.Add(new(new(Id($"multi.{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.{(string.Join('-', selection.Cards.Select(c => c.Id)))}")), $"将{(selection.Cards.Count)}张牌当【{(CardCatalog.Get(required).DisplayName)}】打出", selection.Cards.Select(c => c.Id).ToArray(), [], parameters));
                    }

                    if (required == CardKind.Slash)
                        foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(responder))
                            choices.Add(new(new(Id($"zhangba.{(string.Join('-', pair.Select(c => c.Id)))}")), "发动丈八蛇矛，将两张手牌当杀打出", pair.Select(c => c.Id).ToArray(), [], QizhengParameters("zhangba", CardKind.Slash.ToString())));
                }

            choices.Add(new(new(Id("pass")), "不打出牌", [], [], QizhengParameters("pass")));
        }
        else if (r.Stage == QizhengStage.Gain)
        {
            foreach (var slot in Enumerable.Range(0, _corePhysicalOwnedCardQueriesOperations.GetHand(responder).Count))
                choices.Add(new(new(Id($"hand.{(slot)}")), $"获得一张手牌（牌位{(slot + 1)}）", [], [r.Target], QizhengParameters("hand", slot.ToString(CultureInfo.InvariantCulture))));
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(responder).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(use.SourceSeat, c, CardLocation.Equipment(r.Target), OwnedCardMoveIntent.Obtain)))
                choices.Add(new(new(Id($"equipment.{(card.Id)}")), $"获得装备【{(_coreSnapshotValueProjectionOperations.ToSnapshot(card).DisplayName)}】", [card.Id], [r.Target], QizhengParameters("equipment", card.Id.ToString(CultureInfo.InvariantCulture))));
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishQizheng(CardUseFrame use)
    {
        var r = use.Qizheng!;
        var choices = QizhengChoices(use);
        if (choices.Count == 0)
        {
            FinishQizhengWithoutDamage(use);
            return;
        }

        var seat = r.Stage == QizhengStage.Response ? r.Target : use.SourceSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, r.Stage == QizhengStage.Designation ? "【奇正相生】请暗中指定奇兵或正兵。" : r.Stage == QizhengStage.Response ? "【奇正相生】你可以打出一张杀或闪，或不打出牌。" : "【奇正相生】请选择获得的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.Target], use.SourceSeat, CardKind.QizhengXiangsheng)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            TargetSeat = r.Target
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool HasQizhengPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { Qizheng.Stage: QizhengStage.Designation or QizhengStage.Response or QizhengStage.Gain } && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IncomingCard: CardKind.QizhengXiangsheng };
    [ProgramAnswer("qizheng")]
    internal void AnswerQizheng(PromptChoice selected)
    {
        if (!HasQizhengPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Qizheng: { } r } use || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], QizhengChoices(use).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Qizheng requires its current participant's exact private choice.");
        AssertQizheng(use);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = selected.Parameters["branch"];
        if (r.Stage == QizhengStage.Designation)
        {
            var ambush = selected.Parameters["value"] == "ambush";
            var nonce = $"{(_randomOperations._random.Next(int.MaxValue)):X8}{(_randomOperations._random.Next(int.MaxValue)):X8}";
            var commitment = QizhengCommitment(use.Id, r.NullificationId, ambush, nonce);
            _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { Qizheng = r with { Stage = QizhengStage.Response, Ambush = ambush, Nonce = nonce, Commitment = commitment } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengSecretCommittedEvent(use.Id, r.NullificationId, commitment));
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true);
            NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, r.Target, card, playedCardKind: CardKind.QizhengXiangsheng, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(use.Id));
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            PublishQizheng(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!);
            return;
        }

        if (r.Stage == QizhengStage.Gain)
        {
            var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
            var card = branch == "hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(target)[int.Parse(selected.Parameters["value"], CultureInfo.InvariantCulture)] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Single(c => c.Id == int.Parse(selected.Parameters["value"], CultureInfo.InvariantCulture));
            var from = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(target, card);
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(use.SourceSeat), new("card.effect.qizheng.obtain"), disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
            FinishQizhengWithoutDamage(use);
            return;
        }

        if (branch == "pass")
        {
            ResolveQizhengAfterResponse(use.Id);
            return;
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var kind = Enum.Parse<CardKind>(selected.Parameters["value"]);
        _coreCardConversionsOperations.CaptureSelectedResponseConversion(selected);
        if (branch == "zhangba")
        {
            var pair = _coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(responder, selected.Cards) ?? throw new InvalidOperationException("Qizheng lost its exact native Zhangba response materials.");
            var costs = pair.Select(card => new CardActionCost(card.Id, card.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, card))).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { Qizheng = r with { Stage = QizhengStage.PaidResponse, ResponseKind = kind } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengResponsePaidEvent(use.Id, r.NullificationId, kind));
            _coreNativeGroupResponsesOperations.MoveZhangbaResponseCards(responder, pair, use.Id, use.SourceSeat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, CardKind.Slash);
            NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle2(use.Id);
            if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, responder, responder, null, use.SourceSeat, kind, costs, ProgramCardContinuation.QizhengResponse))
                return;
            _coreNativeGroupResponsesOperations.FinishZhangbaResponseCards(pair);
            ResolveQizhengAfterResponse(use.Id);
            return;
        }

        if (branch == "multi")
        {
            if (!CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var source) || source is null)
                throw new InvalidOperationException("Qizheng multi-response lost its exact conversion.");
            var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(responder, selected.Cards, kind, true, source)!;
            _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { Qizheng = r with { Stage = QizhengStage.PaidResponse, ResponseKind = kind } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengResponsePaidEvent(use.Id, r.NullificationId, kind));
            var action = _coreCardConversionsOperations.MoveProgramMultiCardResponse(responder, selection, use.Id, use.SourceSeat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, kind);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { Qizheng = f.Qizheng!with { ResponseActionId = action.ActionId } });
            NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle2(use.Id);
            if (_coreCommittedResponseUsesOperations.Value.TryBeginCommittedResponseUsePrograms(attack, action, ProgramCardContinuation.QizhengResponse) || _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [use.SourceSeat], ProgramCardContinuation.QizhengResponse))
                return;
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.QizhengResponse);
            return;
        }

        var physical = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? CardKind.Slash : kind).Single(c => c.Id == selected.Cards.Single());
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(responder, physical, kind, _coreCardDeclarationsOperations.PeekDeclarationConversion(responder, physical, kind, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.QizhengResponse, responder.Seat, finalKind: kind), [use.SourceSeat]))
            return;
        PayQizhengSingleResponse(use.Id, responder, physical, kind);
    }

    internal void PayQizhengSingleResponse(long id, CharacterState responder, Card card, CardKind kind)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = use.Qizheng!;
        var conversion = _coreCardConversionsOperations.GetSelectedResponseConversion(responder, card, kind);
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(responder, card, kind, conversion);
        var cost = new CardActionCost(card.Id, card.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, card));
        var suit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(responder, [card], kind);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { Qizheng = r with { Stage = QizhengStage.PaidResponse, ResponseKind = kind } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengResponsePaidEvent(id, r.NullificationId, kind));
        _coreAlternativeViewAsCostsOperations.PaySingleCardResponse(card, responder, kind, conversion);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, kind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(card.Id, responder.Seat, use.SourceSeat, kind));
        NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle2(id);
        if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, responder, responder, null, use.SourceSeat, kind, [cost], ProgramCardContinuation.QizhengResponse, conversion, suit))
            return;
        _coreAlternativeViewAsCostsOperations.FinishSingleCardResponse(card, conversion);
        ResolveQizhengAfterResponse(id);
    }

    internal bool TryContinueQizhengResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (continuation != ProgramCardContinuation.QizhengResponse)
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Qizheng: { Stage: QizhengStage.PaidResponse } r } use || action.ActorSeat != r.Target || action.OpponentSeat != use.SourceSeat || action.EffectiveKind != r.ResponseKind || action.ParentActionId != use.Action?.ActionId)
            throw new InvalidOperationException("Qizheng response lost its original action parent.");
        ResolveQizhengAfterResponse(use.Id);
        return true;
    }

    internal void ResolveQizhengAfterResponse(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var r = use.Qizheng!;
        if (_characterSimaYiMobileGodOperations.TryBeginMobileSimaRenjieQizheng(use))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QizhengResolvedEvent(id, r.NullificationId, r.Target, r.Ambush, r.ResponseKind));
        var answered = r.Ambush ? r.ResponseKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) : r.ResponseKind == CardKind.Dodge;
        if (answered || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || !r.Ambush && !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive)
        {
            FinishQizhengWithoutDamage(use);
            return;
        }

        if (!r.Ambush)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreRuntimeOperations.ReplaceRuntimeTop(use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!with { Qizheng = r with { Stage = QizhengStage.Gain } });
            PublishQizheng(use);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(use with { Qizheng = r with { Stage = QizhengStage.Damage } });
        NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle2(id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void FinishQizhengWithoutDamage(CardUseFrame supplied)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { Qizheng = null });
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true);
        _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(use.Id, card);
        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, card, CardKind.QizhengXiangsheng);
    }

    internal bool TryCompleteQizhengAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Qizheng: { } r } use)
            return false;
        if (r.Stage != QizhengStage.Damage)
            throw new InvalidOperationException("Qizheng completed before its exact damage return.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { Qizheng = null });
        return false;
    }

    // Complete this typed zero-material ordinary trick before unrelated native
    // Slash finishers ask for their physical-card representation. The common
    // attack-completion prelude still runs before this issued-use dispatch.
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 5)]
    internal bool FinishVirtualQizhengAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { CardId: 0, CardKind: CardKind.QizhengXiangsheng, VirtualOrdinaryTrickOrigin: not null })
            return false;
        if (!_coreVirtualOrdinaryTrickUsesOperations.FinishProgramVirtualOrdinaryAttack(attack, out paused))
            throw new InvalidOperationException("A virtual Qizheng completion lost its exact native ordinary-trick origin.");
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 51, typeof(QizhengRoutes), "HasReceipt")]
    internal void ContinueQizheng(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Qizheng.Stage: QizhengStage.Designation or QizhengStage.Response or QizhengStage.Gain } use)
            PublishQizheng(use);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 30300)]
    internal bool AdvanceQizhengAi()
    {
        if (!HasQizhengPrompt() || _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat].IsHuman)
            return false;
        var choices = _corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices;
        var choice = _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { Qizheng.Stage: QizhengStage.Designation } ? choices[_randomOperations._random.Next(choices.Count)] : choices.FirstOrDefault(c => c.Parameters["branch"] != "pass") ?? choices[0];
        AnswerQizheng(choice);
        return true;
    }

    internal void AssertQizheng(CardUseFrame use)
    {
        if (use.Qizheng is not { } r)
            return;
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.QizhengXiangsheng || use.Action?.ActionId != r.ActionId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || r.TargetIndex != use.TargetIndex || r.EffectOrdinal != _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(use.Id) || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != r.Target || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<QizhengEffectStartedEvent>().Count(e => e == new QizhengEffectStartedEvent(use.Id, r.NullificationId, r.ActionId, r.Target, r.TargetIndex, r.EffectOrdinal)) != 1)
            throw new InvalidOperationException("Qizheng lost its original typed native effect receipt.");
        if (r.Stage != QizhengStage.Designation && (r.Nonce.Length != 16 || r.Commitment != QizhengCommitment(use.Id, r.NullificationId, r.Ambush, r.Nonce) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<QizhengSecretCommittedEvent>().Count(e => e.UseId == use.Id && e.NullificationId == r.NullificationId && e.Commitment == r.Commitment) != 1))
            throw new InvalidOperationException("Qizheng's private designation no longer matches its issued commitment.");
        if (r.ResponseKind is { } kind && (kind != CardKind.Dodge && !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<QizhengResponsePaidEvent>().Count(e => e.UseId == use.Id && e.NullificationId == r.NullificationId && e.Kind == kind) != 1))
            throw new InvalidOperationException("Qizheng lost its actual chosen response kind.");
    }
}

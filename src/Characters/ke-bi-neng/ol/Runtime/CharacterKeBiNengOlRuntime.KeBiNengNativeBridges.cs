#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKeBiNengOlRuntime
{
    internal IReadOnlyList<LegalAction> BuildKeBiNengPingduanBasicActions(ProgramSkillFrame frame)
    {
        if (frame.KeBiNengPingduan is not { } receipt || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.ActorSeat))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[receipt.ActorSeat];
        if (!IsKeBiNengPingduanBasicQuery(actor))
            return[];
        var result = new List<LegalAction>();
        foreach (var action in _coreNativeLegalActionsOperations.BuildLegalActions(actor, includeProgramActions: false))
        {
            if (action.Kind is LegalActionKind.Slash or LegalActionKind.Peach or LegalActionKind.Alcohol)
                result.Add(action);
        }

        foreach (var cards in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor))
            foreach (var kind in new[]
            {
                CardKind.Slash
            }

            )
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(target => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards), _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards), cards.Select(c => c.Id).ToArray()) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards))))
                    result.Add(new LegalAction(LegalActionKind.UseEquipmentEffect, null, target.Seat, $"发动【丈八蛇矛】对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】", kind, TargetSeats: [target.Seat], MinCardCount: 2, MaxCardCount: 2, MinTargetCount: 1, MaxTargetCount: 1, EquipmentKind: CardKind.ZhangbaSerpentSpear) { SelectableCardIds = cards.Select(c => c.Id).ToArray(), SelectableTargetSeats = [target.Seat] });
        var selections = new List<ProgramMultiCardViewAsSelection>();
        foreach (var kind in new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.Peach,
            CardKind.Alcohol
        }

        )
            selections.AddRange(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, kind, false));
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs.Where(rule => rule.VariableInputCount && rule.AllHandCardsOfSelectedSuit && rule.ForPlay && (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(rule.OutputKind) || rule.OutputKind is CardKind.Peach or CardKind.Alcohol)))
            {
                var source = new CardConversionSource(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId);
                var cards = rule.SourceZones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, actor.Seat))).DistinctBy(c => c.Id).ToArray();
                // A whole-suit request is an independent equipment conversion.
                // Latent viewAs rules owned by a Play-only activation must retain
                // that activation's timing, costs and remaining program effects.
                IEnumerable<IReadOnlyList<Card>> combinations = source.SkillId == "equipment:xuanjian" && _coreNationalSkillsOperations.HasRuntimeSkill(actor, "friend:gongli-xu-shu") && _coreFriendTrioOperations.HasFriendTrioAlly(actor, "zhuge-liang") ? cards.Select(card => (IReadOnlyList<Card>)new[] { card }) : cards.GroupBy(card => _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)).Select(group => (IReadOnlyList<Card>)group.ToArray());
                foreach (var combination in combinations)
                    if (_coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, combination.Select(c => c.Id).ToArray(), rule.OutputKind, false, source)is { } selected)
                        selections.Add(selected);
            }

        foreach (var selection in selections.DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join(',', s.Cards.Select(c => c.Id).Order()))))
        {
            var kind = selection.OutputKind;
            var rule = _corePlayCardHistoryOperations.ViewAsRule(selection.Source)!;
            var cards = selection.Cards;
            IReadOnlyList<int> targets = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? _coreMatchParticipantsPlayersAccess._players.Where(target => _coreRuleQueriesOperations.CanSpendSlashUse(actor, target, _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, selection.Source), kind, physicalCardIds: cards.Select(c => c.Id).ToArray()) && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, target, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards), physicalCardIds: cards.Select(c => c.Id).ToArray(), ignoreDistance: rule.DistanceUnlimited || _coreFriendTrioOperations.FriendXuanjianIgnoresDistance(actor, selection.Source)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards))).Select(t => t.Seat).ToArray() : kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards)) ? [actor.Seat] : kind == CardKind.Alcohol && _coreRoundDistinctBasicPlayOperations.CanUseRoundDistinctBasicAlcohol(actor, cards) ? [actor.Seat] : [];
            foreach (var target in targets)
                result.Add(new LegalAction(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? LegalActionKind.Slash : kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, cards[0].Id, kind == CardKind.Alcohol ? null : target, _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将所选牌当【{(CardCatalog.Get(kind).DisplayName)}】使用"), kind, TargetSeats: kind == CardKind.Alcohol ? [] : [target], MinCardCount: cards.Count, MaxCardCount: cards.Count) { ConversionSource = selection.Source, SelectableCardIds = cards.Select(c => c.Id).ToArray() });
        }

        return Array.AsReadOnly(result.Select(KeBiNengPingduanReceipt.FreezeAction).ToArray());
    }

    internal void BeginKeBiNengPingduanBasicUse(ProgramSkillFrame frame, LegalAction selected)
    {
        if (frame.KeBiNengPingduan is not { Stage: KeBiNengPingduanStage.BasicIssuing } receipt || !_characterMaXiuMaTieOlOperations.HasAcceptedKenshangKeAction(frame, selected) && !_characterChengJiChengCuiOlOperations.HasAcceptedTousuiKeAction(frame, selected) && !_characterGuoTuOlOperations.HasAcceptedWeijieKeAction(frame, selected) && !_characterLiuFengBoundaryOperations.HasAcceptedBoundaryLiuFengKeAction(frame, selected) && !(_characterLiuZhangOlOperations.HasAcceptedZonghuKeAction(frame, selected) || _characterHuanShuJunOlOperations.HasAcceptedHuanpeiKeAction(frame, selected)) && !BuildKeBiNengPingduanBasicActions(frame).Any(a => SameKeBiNengAction(a, selected)))
            throw new InvalidOperationException("Pingduan lost its exact currently legal basic-card selection.");
        var actor = _coreMatchParticipantsPlayersAccess._players[receipt.ActorSeat];
        var original = _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.DrawFundedDistinctBasicUse is not null)
        {
            _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasic(actor, selected.ConversionSource!, selected.PlayedCardKind!.Value, selected.TargetSeats, original, new(DrawFundedDistinctBasicIntent.Play, frame.Id, frame.Id, null, frame.InstructionIndex, selected.TargetSeat));
            return;
        }

        if (selected.ChainedStateBasicUse is not null)
        {
            _coreChainedStateBasicConversionsOperations.BeginChainedStateBasic(actor, selected.ConversionSource!, selected.PlayedCardKind!.Value, selected.TargetSeats, original, new(ChainedStateBasicIntent.Play, frame.Id, frame.Id, null, frame.InstructionIndex, selected.TargetSeat));
            return;
        }

        if (selected.Kind == LegalActionKind.UseEquipmentEffect && selected.EquipmentKind == CardKind.ZhangbaSerpentSpear)
        {
            var cards = selected.SelectableCardIds.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray();
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[selected.TargetSeats.Single()], cards[0], selected.PlayedCardKind!.Value, actor.Seat, physicalCards: cards);
            return;
        }

        if (selected.SelectableCardIds.Count > 0 && selected.ConversionSource is { } source && (_corePlayCardHistoryOperations.ViewAsRule(source)?.InputCount > 1 || _corePlayCardHistoryOperations.ViewAsRule(source)?.VariableInputCount == true))
        {
            var kind = selected.PlayedCardKind!.Value;
            var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, selected.SelectableCardIds, kind, false, source) ?? throw new InvalidOperationException("Pingduan multi-card conversion lost its exact native material set.");
            if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
                _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[selected.TargetSeats.Single()], selection.Cards[0], kind, actor.Seat, physicalCards: selection.Cards, conversionSource: source);
            else if (kind == CardKind.Peach)
                _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, selection.Cards[0], "桃", kind, 1 + (_corePlayCardHistoryOperations.ViewAsRule(source)?.RecoveryBonus ?? 0), conversionSource: source, physicalCards: selection.Cards);
            else
            {
                var id = _coreNativeCardUseOperations.BeginCardUse(selection.Cards[0], actor.Seat, [], kind, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: source);
                foreach (var card in selection.Cards)
                    _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use);
                if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, actor.Seat))
                    _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(selection.Cards[0].Id, SimpleCardUseEffect.Alcohol));
            }

            return;
        }

        // Youlong owns a second, real slot-payment program and must retain
        // the accepted request's prompt identity while that program is issued.
        if (selected.TieredRoundZeroUse is not null && selected.ConversionSource is { } tieredSource && _characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(tieredSource.SkillId))
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = original;
            // The payment frame owns this future use. Do not publish a global
            // selected conversion while its movement and dying children run.
            if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedPlay(actor, selected))
                return;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeActionDispatchOperations.ExecuteAction(actor, selected);
    }

    internal bool IsKeBiNengPingduanPaymentFrame(DrawFundedDistinctBasicFrame paid) => paid.Payment.Intent == DrawFundedDistinctBasicIntent.Play && IsKeBiNengPingduanPaymentParent(paid.Id, paid.Payment.ParentFrameId, paid.Payment.RequestFrameId, paid.Payment.Cursor, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal bool IsKeBiNengPingduanPaymentFrame(ChainedStateBasicFrame paid) => paid.Payment.Intent == ChainedStateBasicIntent.Play && IsKeBiNengPingduanPaymentParent(paid.Id, paid.Payment.ParentFrameId, paid.Payment.RequestFrameId, paid.Payment.Cursor, paid.Payment.Source, paid.Payment.EffectiveKind, paid.TargetSeats);
    internal bool IsKeBiNengPingduanPaymentParent(long paymentId, long? parentId, long? requestId, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == paymentId);
        return index > 0 && parentId == requestId && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame parent && parent.Id == parentId && parent.InstructionIndex == cursor && parent.KeBiNengPingduan is { Stage: KeBiNengPingduanStage.BasicIssuing, SelectedBasicAction: { } action } receipt && receipt.InstructionIndex == cursor && receipt.ActorSeat == source.OwnerSeat && receipt.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && receipt.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && action.ConversionSource == source && action.PlayedCardKind == kind && action.TargetSeats.SequenceEqual(targets) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanStartedEvent>().Count(e => e.FrameId == parent.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.ActorSeat == receipt.ActorSeat) == 1;
    }
}

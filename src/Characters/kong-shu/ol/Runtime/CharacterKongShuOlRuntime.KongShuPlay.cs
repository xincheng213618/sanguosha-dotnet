#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal const string LeiluanPlayPrefix = "leiluan-native.";
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 27550)]
    internal IEnumerable<LegalAction> BuildLeiluanPlayActions(CharacterState owner)
    {
        if (!HasLeiluanCapability || !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(owner))
            yield break;
        foreach (var selection in new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.Peach,
            CardKind.Alcohol
        }.SelectMany(k => LeiluanSelections(owner, k, false)).DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join('-', s.Cards.Select(c => c.Id)))))
        {
            var kind = selection.OutputKind;
            var targets = LeiluanPlayTargets(owner, selection);
            if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && targets.Count == 0 || kind == CardKind.Peach && targets.Count == 0 || kind == CardKind.Alcohol && !LeiluanAlcoholAllowed(owner, selection.Cards))
                continue;
            var ids = Array.AsReadOnly(selection.Cards.Select(c => c.Id).Order().ToArray());
            var targetCount = kind == CardKind.Alcohol ? 0 : 1;
            yield return new(LegalActionKind.UseProgramSkill, null, null, _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将{(ids.Count)}张牌当【{(CardCatalog.Get(kind).DisplayName)}】使用"), kind, MinCardCount: ids.Count, MaxCardCount: ids.Count, MinTargetCount: targetCount, MaxTargetCount: targetCount)
            {
                ProgramSkillId = selection.Source.SkillId,
                ProgramActivationId = LeiluanPlayPrefix + selection.Source.BindingId + "." + string.Join('-', ids),
                ConversionSource = selection.Source,
                SelectableCardIds = ids,
                SelectableTargetSeats = Array.AsReadOnly(targets.ToArray()),
                ProgramAiHint = new(0, kind == CardKind.Peach ? 1 : 0, 0, 0, 0, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? 1 : 0, false, false)
                {
                    ValueAdjustment = kind == CardKind.Alcohol ? 10 : 0
                }
            };
        }
    }

    internal IReadOnlyList<int> LeiluanPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        var kind = selection.OutputKind;
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards);
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, p, kind, suit, effectiveColor: color, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray())).Select(p => p.Seat).ToArray();
        return kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(owner) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit) ? [owner.Seat] : [];
    }

    internal bool LeiluanAlcoholAllowed(CharacterState owner, IReadOnlyList<Card> cards) => _coreRoundDistinctBasicPlayOperations.CanUseRoundDistinctBasicAlcohol(owner, cards);
    internal CommandError? ValidateLeiluanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, ids, targets)is { } error)
            return error;
        if (action.ConversionSource is not { } source || action.PlayedCardKind is not { } kind || !IsLeiluanSource(source) || LeiluanSelections(owner, kind, false).SingleOrDefault(s => s.Source == source && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()))is not { } selection)
            return new(CommandErrorCode.InvalidCard, "累卵的真实连环人数、材料、轮牌名或技能资格已变化。");
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind == CardKind.Peach)
        {
            if (targets.Count != 1 || !LeiluanPlayTargets(owner, selection).Contains(targets[0]))
                return new(CommandErrorCode.InvalidTarget, "累卵目标或原生杀次数不合法。");
        }
        else if (kind != CardKind.Alcohol || targets.Count != 0 || !LeiluanAlcoholAllowed(owner, selection.Cards))
            return new(CommandErrorCode.InvalidTarget, "累卵原生酒不合法。");
        return null;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 27550)]
    internal bool TrySubmitLeiluanPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(LeiluanPlayPrefix, StringComparison.Ordinal))
            return false;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = BuildLeiluanPlayActions(actor).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (command.SkillOwnerSeat is not null || action is null)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "累卵真实使用菜单已失效。");
            return true;
        }

        if (ValidateLeiluanPlay(actor, action, command.CardIds, command.TargetSeats)is { } invalid)
        {
            result = _kernelCoreMainOperations.Reject(invalid.Code, invalid.Message);
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            TryExecuteLeiluanPlay(actor, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 27550)]
    internal bool TryExecuteLeiluanPlay(CharacterState actor, LegalAction selected, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (selected.ProgramActivationId?.StartsWith(LeiluanPlayPrefix, StringComparison.Ordinal) != true || selected.ConversionSource is not { } source || !IsLeiluanSource(source))
            return false;
        if (ValidateLeiluanPlay(actor, selected, ids, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var kind = selected.PlayedCardKind!.Value;
        var selection = LeiluanSelections(actor, kind, false).Single(s => s.Source == source && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()));
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], selection.Cards[0], kind, actor.Seat, physicalCards: selection.Cards, conversionSource: source);
        else if (kind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, selection.Cards[0], "桃", kind, conversionSource: source, physicalCards: selection.Cards);
        else
        {
            var use = _coreNativeCardUseOperations.BeginCardUse(selection.Cards[0], actor.Seat, [], kind, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: source);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selection.Cards.Select(c => c.Id).ToArray(), CardLocation.Processing, CardMoveReasons.Use);
            if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(use, actor.Seat))
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(use, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(use, new(selection.Cards[0].Id, SimpleCardUseEffect.Alcohol));
        }

        return true;
    }

    internal bool TryResolveLeiluanDyingResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        if (selection.OutputKind != CardKind.Alcohol || !IsLeiluanSelection(selection))
            return false;
        if (!LeiluanSelections(owner, CardKind.Alcohol, true).Any(s => s.Source == selection.Source && s.Cards.Select(c => c.Id).Order().SequenceEqual(selection.Cards.Select(c => c.Id).Order())) || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.VictimSeat != owner.Seat)
            throw new InvalidOperationException("累卵酒必须保留真实自救与未使用牌名资格。");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, selection.Cards[0], "酒", CardKind.Alcohol, conversionSource: selection.Source, dyingResponse: new(dying.FrameId, owner.Seat, false, null, true, selection.Cards[0].Id), physicalCards: selection.Cards);
        return true;
    }

    internal bool TryFinishLeiluanAlcoholCost(CardUseFrame use, Card card)
    {
        if (use.CardKind != CardKind.Alcohol || use.LeiluanPayments?.Any(p => p.ActionId == use.Action?.ActionId && p.Paid && !p.ResponseUse) != true)
            return false;
        AssertLeiluanPayments(use);
        foreach (var id in use.PhysicalCardIds ?? [])
            if (_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(c => c.Id == id)is { } cost)
                _corePhysicalMovementFacadesOperations.MoveCard(cost, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
        return true;
    }
}

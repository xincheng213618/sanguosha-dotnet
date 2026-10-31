#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryRuntime
{
    internal const string AnniversaryYizanPlayPrefix = "anniversary-yizan-pair.";
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 44600)]
    internal IEnumerable<LegalAction> AnniversaryYizanPairActions(CharacterState owner)
    {
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkill(owner, AnniversaryZhaoTongZhaoGuangComposition.Yizan) || AnniversaryYizanModified(owner.Seat))
            yield break;
        foreach (var selection in AnniversaryZhaoTongZhaoGuangComposition.BasicKinds.Where(k => k != CardKind.Dodge).SelectMany(k => _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, k, false)).Where(IsAnniversaryYizanSelection))
        {
            var kind = selection.OutputKind;
            var targets = AnniversaryYizanPlayTargets(owner, selection);
            if (kind == CardKind.Alcohol ? !AnniversaryYizanAlcoholAllowed(owner, selection) : targets.Count == 0)
                continue;
            var ids = Array.AsReadOnly(selection.Cards.Select(c => c.Id).Order().ToArray());
            var targetCount = kind == CardKind.Alcohol ? 0 : 1;
            yield return new(LegalActionKind.UseProgramSkill, null, null, _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将两张牌当【{(CardCatalog.Get(kind).DisplayName)}】使用"), kind, MinCardCount: 2, MaxCardCount: 2, MinTargetCount: targetCount, MaxTargetCount: targetCount)
            {
                ProgramSkillId = selection.Source.SkillId,
                ProgramActivationId = AnniversaryYizanPlayPrefix + selection.Source.BindingId + "." + string.Join('-', ids),
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

    internal IReadOnlyList<int> AnniversaryYizanPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        var kind = selection.OutputKind;
        var ids = selection.Cards.Select(c => c.Id).ToArray();
        var suit = _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, selection);
        var color = _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, selection);
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, p, kind, physicalSuit: suit, effectiveColor: color, physicalCardIds: ids, conversionSource: selection.Source) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, p.Seat, suit)).Select(p => p.Seat).ToArray();
        return kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(owner) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit) ? [owner.Seat] : [];
    }

    internal bool AnniversaryYizanAlcoholAllowed(CharacterState owner, ProgramMultiCardViewAsSelection s) => !owner.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(owner, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(owner.Seat, owner.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(owner) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(owner) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(owner) || _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(owner, CardKind.Alcohol, s.Cards.Select(c => c.Id).ToArray())) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Alcohol, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, s), _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, s)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, s));
    internal CommandError? ValidateAnniversaryYizanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, ids, targets)is { } invalid)
            return invalid;
        if (action.ConversionSource is not { } source || action.PlayedCardKind is not { } kind || source.SkillId != AnniversaryZhaoTongZhaoGuangComposition.Yizan || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, ids, kind, false, source)is not { } selection)
            return new(CommandErrorCode.InvalidCard, "翊赞的原生牌名、两张实际材料或基本牌条件已变化。");
        if (kind == CardKind.Alcohol ? targets.Count != 0 || !AnniversaryYizanAlcoholAllowed(owner, selection) : targets.Count != 1 || !AnniversaryYizanPlayTargets(owner, selection).Contains(targets[0]))
            return new(CommandErrorCode.InvalidTarget, "翊赞使用的实际目标、距离或本回合次数不合法。");
        return null;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 44600)]
    internal bool SubmitAnniversaryYizanPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(AnniversaryYizanPlayPrefix, StringComparison.Ordinal))
            return false;
        var invalid = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (invalid is not null)
        {
            result = _kernelCoreMainOperations.Reject(invalid.Code, invalid.Message);
            return true;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = AnniversaryYizanPairActions(owner).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (command.SkillOwnerSeat is not null || action is null)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "翊赞的双牌使用菜单已失效。");
            return true;
        }

        if (ValidateAnniversaryYizanPlay(owner, action, command.CardIds, command.TargetSeats)is { } error)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            ExecuteAnniversaryYizanPlay(owner, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 44600)]
    internal bool ExecuteAnniversaryYizanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (action.ProgramActivationId?.StartsWith(AnniversaryYizanPlayPrefix, StringComparison.Ordinal) != true || action.ConversionSource is not { SkillId: AnniversaryZhaoTongZhaoGuangComposition.Yizan } source)
            return false;
        if (ValidateAnniversaryYizanPlay(owner, action, ids, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var kind = action.PlayedCardKind!.Value;
        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, ids, kind, false, source)!;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(owner, _coreMatchParticipantsPlayersAccess._players[targets[0]], selection.Cards[0], kind, owner.Seat, physicalCards: selection.Cards, conversionSource: source);
        else if (kind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, selection.Cards[0], "桃", kind, conversionSource: source, physicalCards: selection.Cards);
        else
        {
            var id = _coreNativeCardUseOperations.BeginCardUse(selection.Cards[0], owner.Seat, [], kind, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: source);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selection.Cards.Select(c => c.Id).ToArray(), CardLocation.Processing, CardMoveReasons.Use);
            if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, owner.Seat))
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, owner);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(selection.Cards[0].Id, SimpleCardUseEffect.Alcohol));
        }

        return true;
    }
}

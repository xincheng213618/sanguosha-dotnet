#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool HasMoQuanyuProspectiveQuota(CharacterState actor, CharacterState target, CardKind kind) => TryGetMoQuanyuDeclarationQuota(actor, target, kind, out var frozen) ? frozen : HasMoQuanyuSlash && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && actor.IsAlive && target.IsAlive && actor.Seat != target.Seat && TryGetMoQuanyuSource(actor.Seat, out _, out _) && (MoQuanyuSlashMask(actor.Seat, target.Seat) & MoQuanyuBit(MoQuanyuEffect.Liuxing)) != 0;
    internal LegalAction? RestrictMoQuanyuQuotaAction(CharacterState actor, LegalAction action, CardKind kind, Card? nativeOriginal)
    {
        if (!HasMoQuanyuSlash || !TryGetMoQuanyuSource(actor.Seat, out _, out _) || action.Kind != LegalActionKind.Slash || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            return action;
        var targets = action.TargetSeats.Count > 0 ? action.TargetSeats : action.TargetSeat is { } t ? [t] : Array.Empty<int>();
        if (targets.Count == 0)
            return action;
        var card = nativeOriginal ?? (action.CardId is { } id && id != 0 ? _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).SingleOrDefault(c => c.Id == id) : null);
        var ids = action.CardId is { } physical && physical != 0 ? new[]
        {
            physical
        }

        : Array.Empty<int>();
        if (card is not null)
            card = _characterWuJingOlOperations.WuJingLiubingUseCard(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, action.ConversionSource), kind, action.ConversionSource, ids);
        // Test the existing quota paths without this target-dependent allowance.
        // An independent unlimited-use policy may still authorize a multi-target Slash.
        var ordinarilyLegal = targets.All(t => _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[t], _coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, action.ConversionSource), kind, card, physicalCardIds: ids, ignoreMoQuanyu: true));
        if (ordinarilyLegal)
            return action;
        if (targets.Count != 1 || !HasMoQuanyuProspectiveQuota(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind))
            return null;
        return action.MaxTargetCount > 1 ? action with
        {
            MaxTargetCount = 1
        }

        : action;
    }

    internal bool IsMoQuanyuCompleteZhangbaQuotaLegal(CharacterState actor, IReadOnlyList<int> targets, IReadOnlyList<Card> cards) => !HasMoQuanyuSlash || !TryGetMoQuanyuSource(actor.Seat, out _, out _) || targets.Count <= 1 || targets.All(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[t], ignoresCount: _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards)), CardKind.Slash, effectiveRank: _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards), physicalCardIds: cards.Select(c => c.Id).ToArray(), ignoreMoQuanyu: true));
    internal LegalAction RestrictMoQuanyuZhangbaQuotaAction(CharacterState actor, LegalAction action)
    {
        if (!HasMoQuanyuSlash || !TryGetMoQuanyuSource(actor.Seat, out _, out _) || action is not { Kind: LegalActionKind.UseEquipmentEffect, EquipmentKind: CardKind.ZhangbaSerpentSpear, MaxTargetCount: > 1 } || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat)
            return action;
        var maximum = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Select(cards =>
        {
            var ids = cards.Select(c => c.Id).ToArray();
            var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(actor, cards);
            var rank = _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards);
            return action.SelectableTargetSeats.Count(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[t], CardKind.Slash, suit, _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(actor, cards), rank, ids) && _coreRuleQueriesOperations.CanSpendSlashUse(actor, _coreMatchParticipantsPlayersAccess._players[t], ignoresCount: _coreAlternatingSuitAndCompletedTopOperations.HasPhaseSuitAllowance(actor.Seat, suit), CardKind.Slash, effectiveRank: rank, physicalCardIds: ids, ignoreMoQuanyu: true));
        }).DefaultIfEmpty(0).Max();
        return action with
        {
            MaxTargetCount = Math.Min(action.MaxTargetCount, Math.Max(1, maximum))
        };
    }

    internal void CaptureMoQuanyuQuota(CardUseFrame use)
    {
        if (TryInheritMoQuanyuDeclarationQuota(use))
            return;
        if (!HasMoQuanyuSlash || use.Action is not { Type: CardActionType.Use } a || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.TargetSeats is not [var target] || !HasMoQuanyuProspectiveQuota(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target], use.CardKind) || !TryGetMoQuanyuSource(use.SourceSeat, out var source, out var hash))
            return;
        var r = new MoQuanyuQuotaReceipt(use.Id, a.ActionId, use.SourceSeat, target, use.CardKind, source, hash, MoQuanyuSlashMask(use.SourceSeat, target), MoQiangangEntered(use.SourceSeat));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuQuota = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuQuotaAcceptedEvent(r));
    }

    [ProgramCardLegality(ProgramCardLegalityStage.LegalActionProjection, 10)]
    internal LegalAction? ProjectMoQuanyuLegalAction(ProgramLegalActionQuery query) => query.Action is { Kind: LegalActionKind.UseEquipmentEffect, EquipmentKind: CardKind.ZhangbaSerpentSpear } ? RestrictMoQuanyuZhangbaQuotaAction(query.Actor, query.Action) : _coreCardUseModulesOperations.TryGetLegalActionEffectiveCardKind(query.Actor, query.Action, out var kind, query.NativeOriginalCard) ? RestrictMoQuanyuQuotaAction(query.Actor, query.Action, kind, query.NativeOriginalCard) : query.Action;
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 120)]
    internal bool MoQuanyuSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => !query.IgnoreMoQuanyu && HasMoQuanyuProspectiveQuota(query.Actor, query.Target, query.Kind);
}

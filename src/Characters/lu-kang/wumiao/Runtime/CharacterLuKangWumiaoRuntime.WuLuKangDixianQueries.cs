#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal bool ValidWuLuKangDixianGrant(WuLuKangDixianGrant g) => g.Rank is >= 1 and <= 13 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Source.OwnerSeat) && g.Source.SkillId == WuLuKangComposition.Dixian && g.Source.BindingId == "declare-rank" && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(g.Source.SkillId)?.Program is { } p && p.GameplayHash == g.GameplayHash && WuLuKangComposition.IsDixian(p) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianStartedEvent>().Count(e => e.FrameId == g.ProgramFrameId && e.Source == g.Source && e.GameplayHash == g.GameplayHash && e.Phase == g.Phase) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianRankChosenEvent>().Count(e => e.FrameId == g.ProgramFrameId && e.Rank == g.Rank && e.AllAtLeast && e.MaterialCount == 0) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianDrawReturnedEvent>().Count(e => e.FrameId == g.ProgramFrameId && e.RequestedCount == g.Rank) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianGrantedEvent>().Count(e => e.Grant == g) == 1;
    internal WuLuKangDixianGrant? WuLuKangDixianGrantFor(int actorSeat) => !HasWuLuKangDixianCapability ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianGrantedEvent>().Select(e => e.Grant).SingleOrDefault(g => g.Source.OwnerSeat == actorSeat && ValidWuLuKangDixianGrant(g));
    internal bool HasWuLuKangDixianGrant(int actorSeat) => WuLuKangDixianGrantFor(actorSeat)is not null;
    internal WuLuKangDixianGrant? WuLuKangDixianProspectiveGrantFor(int actorSeat)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var p = top is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration ? declaration.Payment : top?.AcceptedDeclarationPayment;
        return p is { Claimed: false, WuLuKangDixian: { } r } && r.ActorSeat == actorSeat && ValidWuLuKangDixianDeclaration(r) ? r.Grant : WuLuKangDixianGrantFor(actorSeat);
    }

    internal bool HasWuLuKangDixianRankPrivilege(int actorSeat, int? rank, long? useId = null) => useId is { } id ? HasIssuedWuLuKangDixianUse(id, actorSeat) : rank is> 0 && WuLuKangDixianProspectiveGrantFor(actorSeat)is { } g && rank <= g.Rank;
    internal int? WuLuKangDixianQueryRank(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, CardConversionSource? source = null, bool isTrueZhangba = false)
    {
        if (!HasWuLuKangDixianGrant(actor.Seat) || ids is not { Count: > 0 } || ids.Any(id => id <= 0))
            return null;
        if (source is not null && _characterGuanHaiOlOperations.IsXieweiSource(source))
            return 0;
        var cards = ids.Select(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id)).ToArray();
        var id = ids[0];
        var card = cards[0];
        var provider = _coreCardDeclarationsOperations.CapturedDeclarationOrigin(id, _cardZonesOperations._cardZones.GetLocation(id)).OwnerSeat is { } seat ? _coreMatchParticipantsPlayersAccess._players[seat] : actor;
        if (ids.Count == 1 && _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(provider.Seat, id)?.WuLuKangDixian is { } paid && ValidWuLuKangDixianDeclaration(paid))
            return paid.EffectiveRank;
        if ((_characterGuanYuWumiaoOperations.CaptureJuewuActionRank(provider.Seat, ids) ?? _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholActionRank(provider.Seat, kind, ids))is { } nativeRank)
            return nativeRank;
        if (ids.Count != 1)
            return isTrueZhangba && source is null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && ids.Count == 2 && _coreNativeDamageHandlesOperations.UsesFormalZhangbaSerpentSpear && _coreAdvancedWeaponsOperations.HasWeaponAbility(provider, CardKind.ZhangbaSerpentSpear) ? Math.Min(13, cards.Sum(c => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(provider, c) ?? c.Rank)) : null;
        // Use the same native appearance transformations as the accepted action.
        // A zero-point conversion stays zero; no printed-rank fallback is used.
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(provider, card, source);
        return appearance.Rank;
    }

    internal bool HasWuLuKangDixianMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, CardConversionSource? source = null) => HasWuLuKangDixianRankPrivilege(actor.Seat, WuLuKangDixianQueryRank(actor, kind, ids, source));
    internal string? WuLuKangDixianPublicDescription(int actorSeat) => WuLuKangDixianGrantFor(actorSeat)is { } g ? $"本局使用有点数且点数≤{(g.Rank)}的牌无距离和次数限制；不占通常【杀】和【酒】次数。" : null;
    internal bool ValidWuLuKangDixianDeclaration(WuLuKangDixianDeclarationReceipt r) => r.DeclarationId > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ProviderSeat) && r.CardId > 0 && r.EffectiveRank is >= 0 and <= 13 && (r.Grant is null || r.Grant.Source.OwnerSeat == r.ActorSeat && ValidWuLuKangDixianGrant(r.Grant)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianDeclarationAdmittedEvent>().Count(e => e.Receipt == r) == 1;
    internal DeclaredCardPayment CaptureWuLuKangDixianDeclaration(DeclaredCardPayment payment, CardDeclarationParentReturn parent)
    {
        if (!HasWuLuKangDixianCapability || !global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime.BeifenDeclarationIsTrueUse(parent) || payment.ActorSeat != parent.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.ActorSeat) || payment.Claimed || payment.WuLuKangDixian is not null || _cardZonesOperations._cardZones.GetLocation(payment.Cost.CardId) != payment.Cost.From)
            return payment;
        var provider = _coreMatchParticipantsPlayersAccess._players[payment.ProviderSeat];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(payment.Cost.CardId);
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(provider, card, payment.Source);
        var rank = _characterGuanYuWumiaoOperations.CaptureJuewuActionRank(provider.Seat, [card.Id]) ?? _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholActionRank(provider.Seat, payment.DeclaredKind, [card.Id]) ?? appearance.Rank;
        var r = new WuLuKangDixianDeclarationReceipt(payment.DeclarationId, parent.ActorSeat, provider.Seat, card.Id, payment.DeclaredKind, rank, WuLuKangDixianGrantFor(parent.ActorSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianDeclarationAdmittedEvent(r));
        return payment with
        {
            WuLuKangDixian = r
        };
    }

    internal int? CaptureWuLuKangDixianDeclarationRank(int provider, IReadOnlyList<int> ids) => HasWuLuKangDixianCapability && ids is [var id] && _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(provider, id)?.WuLuKangDixian is { } r && ValidWuLuKangDixianDeclaration(r) ? r.EffectiveRank : null;
    internal void IssueWuLuKangDixianUse(CardUseFrame use)
    {
        if (!HasWuLuKangDixianCapability || use.Action is not { Type: CardActionType.Use } a || use.WuLuKangDixianUse is { } old && old.ActionId == a.ActionId && old.ActorSeat == a.ActorSeat)
            return;
        var payment = use.AcceptedDeclarationPayment;
        if (payment is null && a.PhysicalCards is [var cost])
            payment = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(a.ProviderSeat, cost.CardId);
        var grant = WuLuKangDixianGrantFor(a.ActorSeat);
        if (payment?.WuLuKangDixian is { } paid && paid.ActorSeat == a.ActorSeat)
        {
            if (!ValidWuLuKangDixianDeclaration(paid) || paid.ProviderSeat != a.ProviderSeat || paid.EffectiveRank != (a.EffectiveRank ?? 0) || a.PhysicalCards is not [var material] || material.CardId != paid.CardId || material.From != payment.Cost.From)
                throw new InvalidOperationException("Dixian lost its prepayment declaration's exact native rank and actor.");
            grant = paid.Grant;
        }

        var r = new WuLuKangDixianUseReceipt(use.Id, a.ActionId, a.ActorSeat, a.ProviderSeat, a.EffectiveKind, a.EffectiveRank, grant, grant is not null && a.EffectiveRank is> 0 && a.EffectiveRank <= grant.Rank);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { WuLuKangDixianUse = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangDixianUseAdmittedEvent(r));
    }

    internal bool HasIssuedWuLuKangDixianUse(long useId, int actorSeat) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { Type: CardActionType.Use } a, WuLuKangDixianUse: { Applies: true, Grant: { } g } r } use && r.UseId == use.Id && r.ActionId == a.ActionId && r.ActorSeat == a.ActorSeat && r.ActorSeat == actorSeat && r.ProviderSeat == a.ProviderSeat && r.EffectiveRank is> 0 && r.EffectiveRank <= g.Rank && r.EffectiveRank == a.EffectiveRank && ValidWuLuKangDixianGrant(g) && g.Source.OwnerSeat == actorSeat && (r.Kind == a.EffectiveKind || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.Kind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianUseAdmittedEvent>().Count(e => e.Receipt == r) == 1;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29701)]
    internal void ObserveWuLuKangDixianUse(IGameEvent fact)
    {
        if (!HasWuLuKangDixianCapability)
            return;
        if (fact is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use)
            IssueWuLuKangDixianUse(use);
        if (fact is ProgramCardUseActorReplacedEvent replaced && _coreCardUseLifecycleOperations.LifecycleCardUse(replaced.CardUseFrameId)is { } reassigned)
            IssueWuLuKangDixianUse(reassigned);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 29701)]
    internal void AssertWuLuKangDixianUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(f => f.WuLuKangDixianUse is not null))
        {
            var r = use.WuLuKangDixianUse!;
            if (use.Action is not { Type: CardActionType.Use } a || r.UseId != use.Id || r.ActionId != a.ActionId || r.ActorSeat != a.ActorSeat || r.ProviderSeat != a.ProviderSeat || r.EffectiveRank != a.EffectiveRank || r.Kind != a.EffectiveKind && !(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.Kind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use)) || r.Grant is { } g && (g.Source.OwnerSeat != r.ActorSeat || !ValidWuLuKangDixianGrant(g)) || r.Applies != (r.Grant is { } grant && r.EffectiveRank is> 0 && r.EffectiveRank <= grant.Rank) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianUseAdmittedEvent>().Count(e => e.Receipt == r) != 1)
                throw new InvalidOperationException("Dixian changed its frozen actual Use actor, effective rank or issued permanent admission.");
        }
    }

    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 30)]
    internal bool DistanceSpecificSlashExemption30HasWuLuKangDixianRankPrivilege(ProgramSlashDistanceQuery q) => HasWuLuKangDixianRankPrivilege(q.Actor.Seat, q.EffectiveRank, q.ExistingUseFrameId);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 80)]
    internal bool DixianRankSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasWuLuKangDixianRankPrivilege(query.Actor.Seat, query.EffectiveRank ?? (query.PhysicalCard is not null ? _coreFinalTargetSlashOperations.SpecificSlashRank(query.Actor, query.PhysicalCard, query.Kind) : WuLuKangDixianQueryRank(query.Actor, query.Kind, query.PhysicalCardIds)));
}

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiAnniversaryGodRuntime
{
    internal bool HasShenZfXunshi(CharacterState p) => _coreNationalSkillsOperations.HasRuntimeSkill(p, ShenZhangFeiComposition.Xunshi);
    internal bool IsShenZfIdentitySource(CharacterState owner, Card card, CardConversionSource? source) => source is { SkillId: ShenZhangFeiComposition.Xunshi, BindingId: "multi-trick-slash" } && source.OwnerSeat == owner.Seat && card.Id > 0 && _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, card).Any(m => m.Source == source && m.Identity.Id == "multi-trick-slash");
    internal bool ShenZfAttackIsColorless(NativeOwners.CardAttackHandle attack) => _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { ShenZhangFeiColorless: { } r, Action: { Type: CardActionType.Use } a } u && CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) && attack.EffectiveCardKind == u.CardKind && u.SourceSeat == a.ActorSeat && a.ActorSeat == attack.CardUserSeat && r.ActionId == a.ActionId && r.Source is { SkillId: ShenZhangFeiComposition.Xunshi, BindingId: "colorless-use" } && r.Source.OwnerSeat == a.ActorSeat && !string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) && r.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShenZhangFeiComposition.Xunshi).Program!.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenZhangFeiColorlessIssuedEvent>().Count(e => e.UseId == u.Id && e.Receipt == r) == 1 && ShenZfColorless(a);
    internal Suit? ShenZfIdentitySuit(CharacterState p, Card c, CardLocation? identityLocation = null)
    {
        if (c.Id <= 0 || !HasShenZfXunshi(p) || !ShenZhangFeiComposition.MultiTricks.Contains(c.Kind))
            return null;
        var location = identityLocation ?? _cardZonesOperations._cardZones.GetLocation(c.Id);
        return (location is { Zone: CardZoneKind.Hand or CardZoneKind.WoodenOxGrain } && location.OwnerSeat == p.Seat || location == CardLocation.Processing) ? Suit.None : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.HandCardProjection, 40)]
    internal CardSnapshot ProjectShenZfHandCard(CharacterState p, Card c, CardSnapshot snapshot) => ShenZfIdentitySuit(p, c) == Suit.None ? snapshot with
    {
        Suit = Suit.None
    }

    : snapshot;
    internal static bool ShenZfColorless(CardActionContext a) => (a.EffectiveSuit == Suit.None || a.EffectiveIsRed is null && a.EffectiveSuit is null && (a.PhysicalCards.Count == 0 || a.PhysicalCards.Select(c => c.EffectiveIsRed).Distinct().Count() > 1 || a.PhysicalCards.All(c => c.EffectiveIsRed is null)));
    internal CardActionContext? ShenZfUseAction(ProgramSkillWindowContext w)
    {
        var p = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId);
        return p is not null && (p.Action.Type == CardActionType.Use || p.CompletedResponseReturn is { IsCommitted: true } typed && _coreResponseUseCompletionOperations.IsCompletedResponseUse2(p.Action, typed.Kind)) ? p.Action : null;
    }

    internal bool ShenZfMaterialsColorless(CharacterState p, IReadOnlyList<int>? ids)
    {
        if (!HasShenZfXunshi(p) || ids is null)
            return false;
        if (ids.Count == 0)
            return true;
        var colors = ids.Select(id => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(p, _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).Distinct().ToArray();
        return colors.Length > 1 || colors is [null ];
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 38600)]
    internal void CaptureShenZfColorless(IGameEvent payload)
    {
        if (payload is not CardUseDeclaredEvent e)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { Action: { } a } use || !ShenZfColorless(a) || !HasShenZfXunshi(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat]))
            return;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(ShenZhangFeiComposition.Xunshi).Program!;
        var receipt = new ShenZhangFeiColorlessUse(new(p.Id, "colorless-use", a.ActorSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], p.Id)), p.GameplayHash, a.ActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ShenZhangFeiColorless = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenZhangFeiColorlessIssuedEvent(use.Id, receipt));
    }

    internal bool ShenZfIssuedUse(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } a } u && a.ActorSeat == actor && ShenZfColorless(a) && (u.ShenZhangFeiColorless is { } r && r.ActionId == a.ActionId && r.Source.OwnerSeat == actor && r.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShenZhangFeiComposition.Xunshi).Program!.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenZhangFeiColorlessIssuedEvent>().Count(e => e.UseId == id && e.Receipt == r) == 1 || HasShenZfXunshi(_coreMatchParticipantsPlayersAccess._players[actor]));
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 38600)]
    internal bool ShenZfCardDistance(ProgramCardDistanceQuery q) => q.CardUseFrameId is { } id ? ShenZfIssuedUse(id, q.Actor.Seat) : ShenZfMaterialsColorless(q.Actor, q.PhysicalCardIds);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 38600)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 38600)]
    internal bool ShenZfSlashDistance(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is { } id ? ShenZfIssuedUse(id, q.Actor.Seat) : ShenZfMaterialsColorless(q.Actor, q.PhysicalCardIds ?? (q.PhysicalCard is { Id: > 0 } card ? [card.Id] : q.PhysicalSuit == Suit.None ? [] : null));
    [ProgramDistanceRule(ProgramDistanceStage.ConvertedSlashExemption, 38600)]
    internal bool ShenZfConvertedDistance(ProgramSlashConversionDistanceQuery q) => q.Source.SkillId == ShenZhangFeiComposition.Xunshi && q.Source.BindingId == "multi-trick-slash" && q.Source.OwnerSeat == q.Owner.Seat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(q.Owner, q.Source.SkillId, q.Source.SkillInstanceId);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 38600)]
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 38600)]
    internal bool ShenZfSlashQuota(ProgramSlashQuotaQuery q) => ShenZfMaterialsColorless(q.Actor, q.PhysicalCardIds ?? (q.PhysicalCard is { Id: > 0 } card ? [card.Id] : null));
    [ProgramTargetRule(ProgramTargetRulePoint.MaterialPrivilegeAllowance, 38600)]
    internal bool ShenZfMaterialPrivilege(ProgramNativeUseMaterialQuery q) => q.ExistingUseFrameId is { } id ? ShenZfIssuedUse(id, q.ActorSeat) : ShenZfMaterialsColorless(_coreMatchParticipantsPlayersAccess._players[q.ActorSeat], q.PhysicalCardIds);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 38600)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AfterSlashDebitIdentity, 38600)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 38600)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeAlcoholDebit, 38600)]
    internal bool ShenZfQuotaPayment(long id, int actor) => ShenZfIssuedUse(id, actor);
    [ProgramTargetRule(ProgramTargetRulePoint.ExtraSlashTargets, 38600)]
    internal ProgramSlashTargetExpansion? ShenZfSlashTargets(ProgramSlashTargetExpansionQuery q)
    {
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) || !HasShenZfXunshi(_coreMatchParticipantsPlayersAccess._players[q.ActorSeat]) || q.EffectiveSuit != Suit.None && (q.EffectiveIsRed is not null || q.EffectiveSuit is Suit.Heart or Suit.Diamond or Suit.Spade or Suit.Club))
            return null;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(ShenZhangFeiComposition.Xunshi).Program!;
        var source = new CardUseEffectSource(p.Id, "colorless-use", q.ActorSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[q.ActorSeat], p.Id));
        return new(source, p.GameplayHash)
        {
            TargetSeats = q.LegalTargetSeats.Where(t => t != q.ActorSeat).ToArray()
        };
    }

    [ProgramTargetRule(ProgramTargetRulePoint.ExtraSlashTargetSource, 38600)]
    internal bool ShenZfSlashTargetSource(ProgramSlashTargetSourceQuery q) => q.Source.SkillId == ShenZhangFeiComposition.Xunshi && q.Source.BindingId == "colorless-use" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(q.Source.OwnerSeat) && !string.IsNullOrWhiteSpace(q.Source.SkillInstanceId) && q.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(ShenZhangFeiComposition.Xunshi).Program!.GameplayHash;
    internal bool ShenZfCannotRespond(int source, int responder, CardKind incoming, IReadOnlyList<int> targets) => CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && targets.Contains(responder) && ShenZfMark(responder, PlayerMarkerKind.ShencaiZhang) > 0 && ShenZfIssued(responder);
    internal IEnumerable<RuleQueryContribution> ShenZfHandLimitContributions(CharacterState p)
    {
        if (p.IsAlive && ShenZfMark(p.Seat, PlayerMarkerKind.ShencaiSi)is> 0 and var count && ShenZfIssued(p.Seat))
            yield return new FiniteRuleQueryContribution($"issued:shencai-si:{(p.Seat)}", SkillRuleOperation.Add, -count);
    }
}

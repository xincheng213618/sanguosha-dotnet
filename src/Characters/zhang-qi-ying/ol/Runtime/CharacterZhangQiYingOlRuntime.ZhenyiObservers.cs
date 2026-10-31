#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangQiYingOlRuntime
{
    internal bool IsZhenyiHoutu(CardConversionSource source) => HasZhangQiYingCapability && _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei).Contains(source.SkillId, StringComparer.Ordinal) && _corePlayCardHistoryOperations.ViewAsRule(source)is { OutputKind: CardKind.Peach, InputCount: 1 };
    internal bool CanPayZhenyiHoutu(CharacterState owner, CardConversionSource source) => !IsZhenyiHoutu(source) || source.OwnerSeat == owner.Seat && owner.IsAlive && owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && owner.Markers.GetValueOrDefault(PlayerMarkerKind.Houtu) == 1 && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(p => p.Id == source.SkillId);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 20)]
    internal void ObserveAcceptedZhenyiHoutu(CardActionAcceptedEvent accepted)
    {
        if (accepted.Action.ConversionChain.Count != 0)
            PayZhenyiHoutu(accepted.Action);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AcceptedConversionCost, 20)]
    internal void PayZhenyiHoutu(CardActionContext action)
    {
        if (!HasZhangQiYingCapability || action.ConversionChain.Count == 0)
            return;
        var sources = action.ConversionChain.Where(IsZhenyiHoutu).ToArray();
        if (sources.Length == 0)
            return;
        var prior = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhenyiHoutuPaidEvent>().Where(e => e.ActionId == action.ActionId).ToArray();
        if (prior is [var paid] && sources is [var repeated] && paid.Source == repeated && paid.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && paid.Actor == action.ActorSeat && paid.Provider == action.ProviderSeat && action.PhysicalCards.SequenceEqual([paid.Material]) && action.EffectiveKind == CardKind.Peach && paid.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(repeated.SkillId).Program?.GameplayHash)
            return;
        if (prior.Length != 0 || sources is not [var source] || source.OwnerSeat != action.ProviderSeat || action.ActorSeat != action.ProviderSeat || action.Type != CardActionType.Use || action.EffectiveKind != CardKind.Peach || action.PhysicalCards is not [var material] || material.From.OwnerSeat != action.ProviderSeat || material.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain) || !CanPayZhenyiHoutu(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat], source) || _coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } dying || !action.TargetSeats.SequenceEqual([dying.VictimSeat]))
            throw new InvalidOperationException("Houtu requires one exact out-of-turn native rescue Peach and its real HE/approved grain material.");
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ProviderSeat];
        owner.Markers.Remove(PlayerMarkerKind.Houtu);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(action.ActionId, owner.Seat, PlayerMarkerKind.Houtu, -1, 0, owner.Seat, "zhenyi.houtu"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhenyiHoutuPaidEvent(action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, action.ActorSeat, action.ProviderSeat, material));
    }

    internal Suit EffectiveZhenyiJudgmentSuit(JudgmentFrame frame, Suit fallback)
    {
        AssertZhenyiJudgment(frame);
        return _characterXiaoQiaoBoundaryMobileOperations.EffectiveBoundaryXiaoQiaoJudgmentSuit(frame, _coreShanheS5XuncanOperations.ShanheS5FenchaiJudgmentSuit(frame, _coreHellDizangProgramsOperations.EffectiveHellDayuanSuit(frame, frame.ZhenyiOverride?.Suit ?? fallback)));
    }

    internal int EffectiveZhenyiJudgmentRank(JudgmentFrame frame, int fallback)
    {
        AssertZhenyiJudgment(frame);
        return _coreHellDizangProgramsOperations.EffectiveHellDayuanRank(frame, frame.ZhenyiOverride?.Rank ?? fallback);
    }

    internal void AssertZhenyiJudgment(JudgmentFrame frame)
    {
        if (frame.ZhenyiOverride is not { } r)
            return;
        if (r.JudgmentId != frame.Id || r.CardId != frame.CardId || r.Rank != 5 || r.Suit is not (Suit.Spade or Suit.Heart) || r.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhenyiJudgmentOverriddenEvent>().Count(e => e.Receipt == r) != 1)
            throw new InvalidOperationException("Zhenyi effective judgment lost its exact unchanged physical card and paid result.");
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 20)]
    internal bool ZhenyiHoutuConversionUnavailable(ProgramViewAsSourceQuery query) => !CanPayZhenyiHoutu(query.Owner, query.Source);
}

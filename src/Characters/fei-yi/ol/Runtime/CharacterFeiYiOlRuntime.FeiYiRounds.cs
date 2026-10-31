#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    internal ExtraCardEffectState? NativeExtraEffectState(CardUseFrame use) => use.ShanheSchoolTrick is { Active: true } school ? school.Native : use.MouJiangJiRounds is { Active: true } jj ? jj.Native : use.MouHqRounds is { Active: true } hq ? hq.Native : use.SoleIncomingEffects is { Active: true } incoming ? incoming.Native : use.MouBingcaiRounds is { Active: true } bingcai ? bingcai.Native : use.MoQuanyuExtraRound is { Started: true } mo ? mo.Native : use.ZixiEffectRounds is { Started: > 0 } z ? z.Native : use.FeiYiRounds is { Started: > 0 } r ? r.Native : use.ExtraCardEffect ?? use.FeiYiRounds?.Native ?? use.ZixiEffectRounds?.Native ?? use.MoQuanyuExtraRound?.Native ?? use.MouBingcaiRounds?.Native ?? use.SoleIncomingEffects?.Native ?? use.MouHqRounds?.Native ?? use.MouJiangJiRounds?.Native ?? use.ShanheSchoolTrick?.Native;
    internal void UpdateNativeExtraEffectState(long id, Func<ExtraCardEffectState, ExtraCardEffectState> update)
    {
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use.ShanheSchoolTrick is { Active: true } school ? use with { ShanheSchoolTrick = school with { Native = update(school.Native) } } : use.MouJiangJiRounds is { Active: true } jj ? use with { MouJiangJiRounds = jj with { Native = update(jj.Native) } } : use.MouHqRounds is { Active: true } hq ? use with { MouHqRounds = hq with { Native = update(hq.Native) } } : use.SoleIncomingEffects is { Active: true } incoming ? use with { SoleIncomingEffects = incoming with { Native = update(incoming.Native) } } : use.MouBingcaiRounds is { Active: true } bingcai ? use with { MouBingcaiRounds = bingcai with { Native = update(bingcai.Native) } } : use.MoQuanyuExtraRound is { Started: true } mo ? use with { MoQuanyuExtraRound = mo with { Native = update(mo.Native) } } : use.ZixiEffectRounds is { Started: > 0 } z ? use with { ZixiEffectRounds = z with { Native = update(z.Native) } } : use.FeiYiRounds is { Started: > 0 } r ? use with { FeiYiRounds = r with { Native = update(r.Native) } } : use.ExtraCardEffect is { } old ? use with { ExtraCardEffect = update(old) } : use.FeiYiRounds is { } pending ? use with { FeiYiRounds = pending with { Native = update(pending.Native) } } : use.ZixiEffectRounds is { } zixi ? use with { ZixiEffectRounds = zixi with { Native = update(zixi.Native) } } : use.MoQuanyuExtraRound is { } pendingMo ? use with { MoQuanyuExtraRound = pendingMo with { Native = update(pendingMo.Native) } } : use.SoleIncomingEffects is { } pendingIncoming ? use with { SoleIncomingEffects = pendingIncoming with { Native = update(pendingIncoming.Native) } } : use.MouBingcaiRounds is { } pendingBingcai ? use with { MouBingcaiRounds = pendingBingcai with { Native = update(pendingBingcai.Native) } } : use.MouHqRounds is { } pendingHq ? use with { MouHqRounds = pendingHq with { Native = update(pendingHq.Native) } } : use.MouJiangJiRounds is { } pendingJj ? use with { MouJiangJiRounds = pendingJj with { Native = update(pendingJj.Native) } } : use.ShanheSchoolTrick is { } pendingSchool ? use with { ShanheSchoolTrick = pendingSchool with { Native = update(pendingSchool.Native) } } : throw new InvalidOperationException("A native repeated effect needs its owning issued receipt."));
    }

    internal void BindFeiYiRankEffects(long id, ProgramTrickContinuation trick)
    {
        if (!HasFeiYiHezhong || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } action } use || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || use.CardKind == CardKind.Nullification || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || action.EffectiveRank is not { } rank || use.FeiYiRounds is { Started: > 0 } || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            return;
        var acceptedActor = FeiYiAcceptedActor(use);
        if (acceptedActor is not { } originalActor)
            throw new InvalidOperationException("Hezhong lost the original accepted actor or its exact native replacement chain.");
        var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiRankEffectIssuedEvent>().Where(g => g.Source.OwnerSeat == originalActor && g.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && g.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && (g.Greater ? rank > g.Rank : rank < g.Rank) && !_coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiRankEffectBoundEvent>().Any(e => e.GrantId == g.FrameId)).ToArray();
        if (grants.Length == 0)
            return;
        var old = use.FeiYiRounds;
        if (old is not null && (old.ActionId != action.ActionId || old.AcceptedActorSeat != originalActor || old.Trick != trick || old.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || old.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat))
            throw new InvalidOperationException("Hezhong cannot append grants to a different accepted effect.");
        var native = old?.Native ?? new ExtraCardEffectState
        {
            Source = grants[0].Source,
            ProgramFrameId = grants[0].FrameId,
            ActionId = action.ActionId,
            PaidCardId = 0,
            Trick = trick
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { FeiYiRounds = new(action.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, originalActor, trick, native) { Grants = (old?.Grants ?? []).Concat(grants.Select(g => g.FrameId)).ToArray() } });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeiYiRankEffectBoundEvent(grant.FrameId, id, action.ActionId, rank, originalActor));
    }

    internal int? FeiYiAcceptedActor(CardUseFrame use)
    {
        var declared = _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind).ToArray();
        return declared is [var original] && use.Action is { Type: CardActionType.Use } action && action.ActorSeat == use.SourceSeat && _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, action.ProviderSeat) ? original.SourceSeat : null;
    }

    internal bool HasUnstartedFeiYiExtraEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { FeiYiRounds: { } r, Action: { } a } use && r.ActionId == a.ActionId && r.Started < r.Grants.Count && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
    internal bool IsFeiYiExtraEffectRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.FeiYiRounds is { Started: > 0 };
    internal int NativeExtraEffectRoundOrdinal(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        var old = use?.ExtraCardEffect?.WholeGroupRepeated == true ? 1 : 0;
        var feiyi = use?.FeiYiRounds is { Started: > 0 } r ? r.Started : 0;
        return checked((use?.ShanheSchoolTrick?.Started == true ? 1 : 0) + (use?.MouJiangJiRounds?.Started ?? 0) + (use?.MouHqRounds?.Started ?? 0) + (use?.SoleIncomingEffects?.Started ?? 0) + old + feiyi + (use?.ZixiEffectRounds?.Started ?? 0) + (use?.MoQuanyuExtraRound?.Started == true ? 1 : 0) + (use?.MouBingcaiRounds?.Started ?? 0));
    }

    internal bool TryBeginFeiYiExtraEffectRound(long id, bool afterAttack)
    {
        if (!HasUnstartedFeiYiExtraEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { FeiYiRounds: { } r, Action: { } action } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || r.Trick is null || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Hezhong cannot pass an unfinished native child.");
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = r.Started == 0 && use.ExtraCardEffect is { } fengzi ? fengzi.MaterialClaims : r.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = r.Started == 0 ? use.ExtraCardEffect?.FrozenGroupDamageSourceSeat ?? r.Native.FrozenGroupDamageSourceSeat : r.Native.FrozenGroupDamageSourceSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { FeiYiRounds = r with { Started = r.Started + 1, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeiYiEffectRoundStartedEvent(id, action.ActionId, r.Started + 1));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
        _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, r.Trick.ActionKind, null, r.Trick.RequiredCardKind, use.CardKind);
        return true;
    }
}

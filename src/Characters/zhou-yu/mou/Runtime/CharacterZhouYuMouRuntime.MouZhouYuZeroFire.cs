#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouRuntime
{
    internal bool IsMouZyZeroFireUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, CardKind: CardKind.FireAttack, PhysicalCardIds.Count: 0, MouZhouYuYingmou: { CardId: 0, Kind: CardKind.FireAttack } n, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveIsRed: not true, RequesterSeat: null } a } u && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { MouZhouYuAnniversary: { Yin: false } } p && n.Actor == p.OwnerSeat && a.ActorSeat == p.OwnerSeat && a.ProviderSeat == p.OwnerSeat && (a.EffectiveSuit is null or Suit.None) && (a.EffectiveRank is null or 0) && a.TargetSeats.SequenceEqual(u.TargetSeats) && MouZyNativeRidesOn(u, p);
    internal IReadOnlyList<int>? MouZyNativeAncestorProcessing(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouZhouYuYingmou: { } n } use || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } parent || !MouZyNativeRidesOn(use, parent))
            return null;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        var ownerAt = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        if (at != ownerAt + 1 || !_resolutionStackOperations._resolutionStack.Take(ownerAt).OfType<CardUseFrame>().Any(u => u.Action?.ActionId == parent.MouZhouYuAnniversary!.OriginalAction))
            return null;
        var own = use.PhysicalCardIds.ToHashSet();
        return Array.AsReadOnly(_resolutionStackOperations._resolutionStack.Take(at).OfType<CardUseFrame>().SelectMany(u => u.Action?.PhysicalCards.Select(m => m.CardId) ?? u.PhysicalCardIds).Where(card => card > 0 && !own.Contains(card) && _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.Processing).Distinct().ToArray());
    }

    internal bool IsMouZyZeroFireAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (!IsMouZyZeroFireUse(attack.ResolutionId) || attack.Card is not null || attack.PhysicalCards.Count != 0 || attack.EffectiveCardKind != CardKind.FireAttack || attack.IsSourceLess || attack.IsDelayedJudgmentDamage || attack.ProgramSkillFrameId is not null || attack.ProgramJudgmentFrameId is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || attack.SourceSeat != use.SourceSeat || attack.CardUserSeat != use.SourceSeat || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } active || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(active, attack))
            return false;
        var ancestors = MouZyNativeAncestorProcessing(use.Id);
        return ancestors is not null && processing.All(card => ancestors.Contains(card.Id));
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 36000)]
    internal bool FinishMouZyZeroFireAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || attack.EffectiveCardKind != CardKind.FireAttack || !IsMouZyZeroFireUse(attack.ResolutionId))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, CardKind.FireAttack));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }
}

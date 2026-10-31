#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouRuntime
{
    internal bool IsMouZyZeroFireUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, CardKind: CardKind.FireAttack, PhysicalCardIds.Count: 0, MouZhouYuYingmou: { CardId: 0, Kind: CardKind.FireAttack } n, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveIsRed: not true, RequesterSeat: null } a } u && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { MouZhouYuAnniversary: { Yin: false } } p && n.Actor == p.OwnerSeat && a.ActorSeat == p.OwnerSeat && a.ProviderSeat == p.OwnerSeat && (a.EffectiveSuit is null or Suit.None) && (a.EffectiveRank is null or 0) && a.TargetSeats.SequenceEqual(u.TargetSeats) && MouZyNativeRidesOn(u, p);

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

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal bool IsZeroEntityUse(long id) => _characterDongChengMouOperations.IsMouDcZeroUse(id) || _characterZhouYuMouOperations.IsMouZyZeroFireUse(id) || _coreWushuangSkillsBOperations.IsWushuangBZeroUse(id) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(id) || _characterTianChouOlOperations.IsTianChouShandaoUse(id) || _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) || IsMouZhijueZeroUse(id) || _characterBaoSanNiangOlOperations.IsChosenAssaultUse(id) || _characterQinLangOlOperations.IsQinLangXianyingUse(id) || _characterAHuiNanOlOperations.IsAHuiNanJuemanUse(id) || _characterDiaoChanLeOperations.IsDiouZeroUse(id) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(id) || _characterLiuChengMouMobileOperations.IsMouLiuChengZeroUse(id) || _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(id) || _characterWenChouMouOlOperations.IsMouWenChouZeroDuel(id) || _characterLvYiMobileShiOperations.IsShiLySongyanDrawTwo(id);
    internal bool IsZeroEntityProcessingParent(long id, int cardId) => cardId == 0 && IsZeroEntityUse(id);
    internal bool MatchesMouZhijueUseAction(long id, CardActionContext action, int cardId) => cardId == 0 && IsMouZhijueZeroUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action is { } issued && action.ActionId == issued.ActionId && action.Type == CardActionType.Use && action.EffectiveKind == issued.EffectiveKind && action.PhysicalCards.Count == 0 && action.ConversionChain.Count == 0 && action.ActorSeat == issued.ActorSeat && action.ProviderSeat == issued.ProviderSeat;
    internal bool IsMouZhijueZeroAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (!IsMouZhijueZeroUse(attack.ResolutionId) || attack.Card is not null || attack.PhysicalCards.Count != 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || attack.EffectiveCardKind != use.CardKind)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind is CardKind.Duel or CardKind.FireAttack)
            return processing.Count == 0 && attack.SourceSeat == use.SourceSeat && (use.CardKind != CardKind.FireAttack || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FireAttackResolvedEvent>().Any(fact => fact.ResolutionId == use.Id && fact.CausedDamage && fact.MatchingDiscardCardId > 0));
        return processing.Count == 0 && use.CardKind is CardKind.BarbarianAssault or CardKind.ArrowBarrage && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId == use.Id && group.DamageSourceSeat == attack.SourceSeat && group.CurrentAttack?.ResolutionId == use.Id;
    }

    internal bool IsMouZhijueBorrowedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing) => _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: { } child } borrowed && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(child, attack) && IsMouZhijueZeroUse(borrowed.ResolutionId) && attack.Card is not null && processing.Select(c => c.Id).Order().SequenceEqual(attack.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing).Select(c => c.Id).Order());
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 120)]
    internal bool FinishMouZhijueZeroAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (!IsMouZhijueZeroUse(attack.ResolutionId) || attack.Card is not null || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind is not (CardKind.Duel or CardKind.FireAttack))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }
}

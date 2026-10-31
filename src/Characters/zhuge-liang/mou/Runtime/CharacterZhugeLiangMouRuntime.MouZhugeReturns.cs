#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeLiangMouRuntime
{
    internal bool IsZeroEntityUse(long id) => _characterDongChengMouOperations.IsMouDcZeroUse(id) || _characterZhouYuMouOperations.IsMouZyZeroFireUse(id) || _coreWushuangSkillsBOperations.IsWushuangBZeroUse(id) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(id) || _characterTianChouOlOperations.IsTianChouShandaoUse(id) || _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) || IsMouZhijueZeroUse(id) || _characterBaoSanNiangOlOperations.IsChosenAssaultUse(id) || _characterQinLangOlOperations.IsQinLangXianyingUse(id) || _characterAHuiNanOlOperations.Value.IsAHuiNanJuemanUse(id) || _characterDiaoChanLeOperations.IsDiouZeroUse(id) || _characterXiaoQiaoMouOlOperations.IsMouXqJueyanUse(id) || _characterLiuChengMouMobileOperations.IsMouLiuChengZeroUse(id) || _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(id) || _characterWenChouMouOlOperations.IsMouWenChouZeroDuel(id) || _characterLvYiMobileShiOperations.Value.IsShiLySongyanDrawTwo(id);
    internal bool MatchesMouZhijueUseAction(long id, CardActionContext action, int cardId) => cardId == 0 && IsMouZhijueZeroUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action is { } issued && action.ActionId == issued.ActionId && action.Type == CardActionType.Use && action.EffectiveKind == issued.EffectiveKind && action.PhysicalCards.Count == 0 && action.ConversionChain.Count == 0 && action.ActorSeat == issued.ActorSeat && action.ProviderSeat == issued.ProviderSeat;
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 120)]
    internal bool FinishMouZhijueZeroAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (!IsMouZhijueZeroUse(attack.ResolutionId) || attack.Card is not null || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind is not (CardKind.Duel or CardKind.FireAttack))
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

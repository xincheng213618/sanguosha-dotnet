#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardAttackStateRuntime
{
    internal int GetCardUseTargetIndex(long id) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == id).TargetIndex;
    internal Card GetAttackCard(int id) => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id);
    internal CardAttackState GetCardAttackState(long id) => _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == id) switch
    {
        CardUseFrame { CardAttack: { } state } => state,
        ProgramSkillFrame { CardAttack: { } state } => state,
        JudgmentFrame { CardAttack: { } state } => state,
        _ => throw new InvalidOperationException($"Card attack owner {(id)} lost its attempt.")};
    internal void InitializeCardAttackState(long id, CardAttackState state)
    {
        var owner = _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == id);
        if (owner is not (CardUseFrame or ProgramSkillFrame or JudgmentFrame))
            throw new InvalidOperationException($"Card attack owner {(id)} is unavailable.");
        UpdateCardAttackState(id, _ => state);
    }

    internal void UpdateCardAttackState(long id, Func<CardAttackState?, CardAttackState> update)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
        if (index < 0)
            throw new InvalidOperationException($"Card attack owner {(id)} is unavailable.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, _resolutionStackOperations._resolutionStack[index] switch
        {
            CardUseFrame use => use with { CardAttack = update(use.CardAttack) },
            ProgramSkillFrame program => program with { CardAttack = update(program.CardAttack) },
            JudgmentFrame judgment => judgment with { CardAttack = update(judgment.CardAttack) },
            _ => throw new InvalidOperationException($"Frame {(id)} cannot own a card attack.")});
    }

    internal global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => new(attack.ResolutionId, attack.IsDelayedJudgmentDamage, attack.ProgramSkillFrameId ?? attack.ProgramSkillCardUseFrameId ?? _coreNextActualUseTargetAdjustmentsOperations.NextActualUseAdjustedSlashParent(attack) ?? _coreHpLossSlashMaterialsOperations.HpLossMaterialSlashParent(attack), attack.IsDelayedJudgmentDamage ? attack.DelayedJudgmentSeat ?? attack.SourceSeat : null, attack.ProgramSkillFrameId is null && attack.ProgramSkillCardUseFrameId is not null, attack.DamageWasApplied, attack.TargetSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DamageTargetDuelOrigin, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DualColorDuelOrigin, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.SharedSlashBenefit, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.SharedSlashBenefit is not null && attack.CardUseCausedDamage, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ForeignTurnContestSlashReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.PindianWinnerSlashReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ConditionalDiscardDuelOrigin, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ShiQuanDuelOrigin, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.EndingHistoricalUseReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.InspectedHandSlashReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.KuangfuSlashReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ForeignSelectedCardSlashReturn, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.FriendRecastDuel, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.CaoChunSlash, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.DenglouNative, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Qigong, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ZhenyingDuel, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.HeJinMouzhu, _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.WuAnGuoLiyong);
    internal Card ReadCardAppearance(CardAppearanceReference appearance) => GetAttackCard(appearance.Id)with
    {
        Kind = appearance.Kind,
        Suit = appearance.Suit,
        Rank = appearance.Rank
    };
    internal CardResolutionContinuations GetCardContinuations(long id) => _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == id) switch
    {
        CardUseFrame use => use.Continuations,
        ProgramSkillFrame program => program.Continuations,
        JudgmentFrame judgment => judgment.Continuations,
        _ => throw new InvalidOperationException($"Continuation owner {(id)} is unavailable.")};
    internal void UpdateCardContinuations(long id, Func<CardResolutionContinuations, CardResolutionContinuations> update)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
        if (index < 0)
            throw new InvalidOperationException($"Continuation owner {(id)} is unavailable.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, _resolutionStackOperations._resolutionStack[index] switch
        {
            CardUseFrame use => use with { Continuations = update(use.Continuations) },
            ProgramSkillFrame program => program with { Continuations = update(program.Continuations) },
            JudgmentFrame judgment => judgment with { Continuations = update(judgment.Continuations) },
            _ => throw new InvalidOperationException($"Frame {(id)} cannot own a card continuation.")});
    }

    internal long? FindCardContinuationOwner(Func<CardResolutionContinuations, bool> matches) => _resolutionStackOperations._resolutionStack.LastOrDefault(frame => frame switch
    {
        CardUseFrame use => matches(use.Continuations) && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(use.Id) && !_characterGuoSiOlOperations.IsSuspendedGuoSiCompletedParent(use) && !_characterDiaoChanLeOperations.IsDiouSuspendedCompletedAttack(use) && !_characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCompletedAttack(use) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(use),
        ProgramSkillFrame program => matches(program.Continuations) && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(program.Id) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(program),
        JudgmentFrame judgment => matches(judgment.Continuations) && !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(judgment.Id) && !_characterDiaoChanLeOperations.IsDiouSuspendedRescueAttack(judgment),
        _ => false
    })?.Id;
    internal static SuspendedDecisionState CaptureSuspendedDecision(PendingDecision prompt) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CardAttackState.CaptureSuspendedDecision(prompt);
    internal static PendingDecision RestoreSuspendedDecision(SuspendedDecisionState state) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CardAttackState.RestoreSuspendedDecision(state);
    internal bool GetCardUseCausedDamage(long id) => _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == id) switch
    {
        CardUseFrame use => use.CausedDamage,
        ProgramSkillFrame program => program.CardAttack?.DamageWasApplied == true,
        JudgmentFrame judgment => judgment.CardAttack?.DamageWasApplied == true,
        _ => throw new InvalidOperationException("A completed card use lost its damage facts.")};
    internal void SetCardUseCausedDamage(long id, bool value)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id);
        if (index < 0)
            throw new InvalidOperationException("A card use lost its damage facts.");
        if (_resolutionStackOperations._resolutionStack[index] is CardUseFrame use)
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, use with { CausedDamage = value });
    }
}

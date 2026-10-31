#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraCardEffectRoundsRuntime
{
    internal void ApplyExtraCapableAlcoholEffect(long id, CharacterState target)
    {
        if (IsExtraCardEffectRound(id))
            target.AlcoholDamageBonus = checked(target.AlcoholDamageBonus + 1);
        else
            target.HasAlcoholEffect = true;
    }

    internal bool RestoreExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is not { } use || _characterFeiYiOlOperations.NativeExtraEffectState(use)is not { WholeGroupRepeated: true, FrozenGroupDamageSourceSeat: { } source })
            return false;
        group.DamageSourceSeat = source;
        return true;
    }

    internal void CaptureExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(group.ResolutionId)is not { } use || _characterFeiYiOlOperations.NativeExtraEffectState(use)is not { WholeGroupRepeated: false })
            return;
        _characterFeiYiOlOperations.UpdateNativeExtraEffectState(group.ResolutionId, r => r with { FrozenGroupDamageSourceSeat = group.DamageSourceSeat });
    }

    internal bool IsExtraCardEffectRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.ShanheSchoolTrick?.Started == true || _characterJiangJiMouOperations.IsMouJjEffectRound(id) || _characterHuangQuanMouOperations.IsMouHqRound(id) || _coreSoleIncomingEffectRepeatOperations.IsSoleIncomingExtraRound(id) || _characterHuangYueyingOlMouOperations.IsMouBingcaiExtraRound(id) || _characterSunQuanOlMagicOperations.IsMoQuanyuExtraRound(id) || _characterDaQiaoLeOperations.IsZixiExtraEffectRound(id) || _characterFeiYiOlOperations.IsFeiYiExtraEffectRound(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.ExtraCardEffect?.WholeGroupRepeated == true;
    internal bool HasUnstartedExtraCardEffect(long id) => _coreShanheSchoolTrickOperations.HasUnstartedShanheSchoolTrick(id) || _characterJiangJiMouOperations.HasUnstartedMouJjEffect(id) || _characterHuangQuanMouOperations.HasUnstartedMouHqRound(id) || _coreSoleIncomingEffectRepeatOperations.HasUnstartedSoleIncomingEffect(id) || _characterHuangYueyingOlMouOperations.HasUnstartedMouBingcaiExtraEffect(id) || _characterSunQuanOlMagicOperations.HasUnstartedMoQuanyuExtraRound(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ExtraCardEffect: { WholeGroupRepeated: false } } use && use.Action is { } action && use.ExtraCardEffect.ActionId == action.ActionId && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    internal void CaptureExtraCardEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        _characterHuangQuanMouOperations.CaptureMouHqAttack(attack);
        _coreSoleIncomingEffectRepeatOperations.CaptureSoleIncomingAttack(attack);
        _characterHuangYueyingOlMouOperations.CaptureMouBingcaiExtraEffectAttack(attack);
        _characterDaQiaoLeOperations.CaptureZixiExtraEffectAttack(attack);
        _characterSunQuanOlMagicOperations.CaptureMoQuanyuExtraAttack(attack);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ExtraCardEffect: { WholeGroupRepeated: false } r, CardAttack: { } original } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || r.OriginalAttacks.Any(s => s.TargetIndex == use.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ExtraCardEffect = r with { OriginalAttacks = r.OriginalAttacks.Append(new(use.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? r.Fangtian } });
    }

    internal bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false)
    {
        _coreShanheSchoolTrickOperations.CompleteShanheSchoolTrick(id);
        _characterJiangJiMouOperations.CompleteMouJjEffect(id);
        _characterHuangQuanMouOperations.CompleteMouHqRound(id);
        _coreSoleIncomingEffectRepeatOperations.CompleteSoleIncomingExtraRound(id);
        _characterHuangYueyingOlMouOperations.CompleteMouBingcaiExtraEffectRound(id);
        _characterDaQiaoLeOperations.CompleteZixiExtraEffectRound(id);
        _characterSunQuanOlMagicOperations.CompleteMoQuanyuExtraRound(id);
        if (_characterFeiYiOlOperations.TryBeginFeiYiExtraEffectRound(id, afterAttack))
            return true;
        if (_characterDaQiaoLeOperations.TryBeginZixiExtraEffectRound(id, afterAttack))
            return true;
        if (_characterSunQuanOlMagicOperations.TryBeginMoQuanyuExtraRound(id, afterAttack))
            return true;
        if (_characterHuangYueyingOlMouOperations.TryBeginMouBingcaiExtraEffectRound(id, afterAttack))
            return true;
        if (_coreSoleIncomingEffectRepeatOperations.TryBeginSoleIncomingExtraRound(id, afterAttack))
            return true;
        if (_characterHuangQuanMouOperations.TryBeginMouHqRound(id, afterAttack))
            return true;
        if (_characterJiangJiMouOperations.TryBeginMouJjEffect(id, afterAttack))
            return true;
        if (_coreShanheSchoolTrickOperations.TryBeginShanheSchoolTrick(id, afterAttack))
            return true;
        if (!HasUnstartedExtraCardEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ExtraCardEffect: { } r, Action: { } action } use)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("An extra effect cannot pass an unfinished child.");
        if (r.Trick is not null && _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || r.Simple is not null && _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (r.Source.SkillId == "guandu:battle-event" && !_coreGuanduExtraEffectsOperations.GuanduRepeatTargetsRemainLegal(use))
            return false;
        // The source/targets were already accepted, finalized and paid by this
        // use. Rechecking distance, target legality, skill ownership or quota
        // here would turn an extra effect into another card use.
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { ExtraCardEffect = r with { WholeGroupRepeated = true }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, action.ActionId, 0, use.TargetSeats.FirstOrDefault(use.SourceSeat), use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var first = r.OriginalAttacks.OrderBy(s => s.TargetIndex).FirstOrDefault(s => _coreMatchParticipantsPlayersAccess._players[s.Attack.TargetSeat].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, s.Attack.TargetSeat));
            if (first is null)
                return false;
            var prepared = use.PreparedTargetAttacks?.Select((state, index) => r.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == index)?.Attack ?? state).ToArray();
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { TargetIndex = first.TargetIndex, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            if (r.Fangtian is { } fang)
            {
                _coreCardAttackStateOperations.UpdateCardContinuations(id, state => state with { FangtianHalberd = fang with { Active = true, CurrentAttackOwnerId = id } });
                _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id);
            }

            _coreCardAttackStateOperations.UpdateCardAttackState(id, _ => ResetExtraCardEffectAttack(first.Attack));
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
            return true;
        }

        if (r.Trick is { } trick)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
            _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, trick.ActionKind, targetCardId: null, trick.RequiredCardKind, use.CardKind);
            return true;
        }

        if (r.Simple is { } simple)
        {
            if (use.TargetSeats.Count > 1 && simple.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { AdjustedSimpleContinuation = simple });
            _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, simple);
            return true;
        }

        throw new InvalidOperationException("An issued extra effect lacks its original native effect continuation.");
    }

    internal static CardAttackState ResetExtraCardEffectAttack(CardAttackState original) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ExtraCardEffectRounds.ResetExtraCardEffectAttack(original);
    internal bool ContinueExtraCardEffectFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_characterHuangQuanMouOperations.ContinueMouHqFangtian(attack))
            return true;
        if (_coreSoleIncomingEffectRepeatOperations.ContinueSoleIncomingFangtian(attack))
            return true;
        if (_characterHuangYueyingOlMouOperations.ContinueMouBingcaiExtraFangtian(attack))
            return true;
        if (_characterSunQuanOlMagicOperations.ContinueMoQuanyuFangtian(attack))
            return true;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ExtraCardEffect: { WholeGroupRepeated: true } r } use)
            return false;
        var seed = r.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == use.TargetIndex);
        if (seed is null)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return true;
        }

        _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, _ => ResetExtraCardEffectAttack(seed.Attack));
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
        return true;
    }

    internal bool TryBeginExtraAttackRound(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        // All chain propagation and all native target cursors must have returned
        // before FinishAttack calls this bridge. The accepted use stays live.
        return TryBeginExtraCardEffectRound(attack.ResolutionId, afterAttack: true);
    }

    internal void AssertExtraCardEffectReceipts()
    {
        _coreShanheSchoolTrickOperations.AssertShanheSchoolTrickRounds();
        _characterJiangJiMouOperations.AssertMouJjEffects();
        _characterHuangQuanMouOperations.AssertMouHqRounds();
        _characterDaQiaoLeOperations.AssertZixiExtraEffectRounds();
        foreach (var incoming in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
            _coreSoleIncomingRangeDrawOperations.AssertSoleIncomingEffectRounds(incoming);
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.ExtraCardEffect is not null))
        {
            var r = use.ExtraCardEffect!;
            if (use.Action is not { Type: CardActionType.Use } action || action.ActionId != r.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(use.CardKind) || (r.AutomaticGameplayHash is { } automaticHash ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AutomaticExtraCardEffectIssuedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.ProgramFrameId == r.ProgramFrameId && e.Source == r.Source && e.GameplayHash == automaticHash && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == automaticHash) : _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ExtraCardEffectIssuedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.ProgramFrameId == r.ProgramFrameId && e.Source == r.Source && e.PaidCardId == r.PaidCardId)) != 1 || r.OriginalAttacks.GroupBy(s => s.TargetIndex).Any(g => g.Count() != 1) || r.OriginalAttacks.Any(s => s.TargetIndex < 0 || s.TargetIndex >= use.TargetSeats.Count) || r.WholeGroupRepeated && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ExtraCardEffectStartedEvent>().Any(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId))
                throw new InvalidOperationException("An extra effect lost its one real use, paid source or native target seeds.");
        }
    }

    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 20)]
    internal bool FinishExtraAttackRound(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = TryBeginExtraAttackRound(attack);
        return paused;
    }
}

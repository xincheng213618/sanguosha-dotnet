#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangXiuMouRuntime
{
    internal bool CanMouZhangXiuAnniversarySlash(ProgramSkillFrame f) => f.MouZhangXiuAnniversary is { Stage: MouZhangXiuAnniversaryStage.SlashIssuing } r && ValidMouZhangXiuAnniversary(f) && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, CardKind.Slash, CardActionType.Use) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[r.Target]) && _coreProgramTargetRulesOperations.TargetRules.AllowsSlash(new(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[r.Target], CardKind.Slash, r.Source, [])) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(f.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, r.Target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[r.Target], CardKind.Slash, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(f.OwnerSeat, r.Target, Suit.None) && !_characterCuiYanOlOperations.IsCuiYanNextUseForbidden(f.OwnerSeat, [], Suit.None);
    internal CardActionContext PrepareMouZhangXiuAnniversarySlashAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuAnniversary: { Stage: MouZhangXiuAnniversaryStage.SlashIssuing } r } f)
            return action;
        if (!ValidMouZhangXiuAnniversary(f) || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.RequesterSeat is not null || action.EffectiveKind != CardKind.Slash || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != r.Source || !action.TargetSeats.SequenceEqual([r.Target]) || r.Index >= r.Count || r.UseId != 0)
            throw new InvalidOperationException("Fuxi cannot declare another source, target or uncounted Slash.");
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, Suit.None, 0, null, action.FactionOrigin);
    }

    internal void IssueMouZhangXiuAnniversarySlash(ProgramSkillFrame f)
    {
        if (!CanMouZhangXiuAnniversarySlash(f))
            throw new InvalidOperationException("Fuxi Slash lost its exact accepted forced target.");
        var r = f.MouZhangXiuAnniversary!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, [r.Target], CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), physicalCardIds: [], conversionSource: r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.MouZhangXiuAnniversarySlash is null)
            throw new InvalidOperationException("Fuxi native Slash must bind before its real declaration.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, r.Target, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, r.Target, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void BindMouZhangXiuAnniversarySlashUse(CardUseFrame supplied)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == supplied.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { MouZhangXiuAnniversary: { Stage: MouZhangXiuAnniversaryStage.SlashIssuing } r } parent)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        if (!ValidMouZhangXiuAnniversary(parent) || r.UseId != 0 || r.Index >= r.Count || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActorSeat != parent.OwnerSeat || a.ProviderSeat != parent.OwnerSeat || a.RequesterSeat is not null || a.ConversionChain is not [var source] || source != r.Source || !a.TargetSeats.SequenceEqual([r.Target]) || !use.TargetSeats.SequenceEqual([r.Target]) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null)
            throw new InvalidOperationException("Fuxi cannot bind a different native counted Slash.");
        var origin = new MouZhangXiuAnniversarySlashOrigin(parent.Id, r.Source, r.GameplayHash, r.Index, r.Target, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouZhangXiuAnniversary = r with { Stage = MouZhangXiuAnniversaryStage.NativeSlash, UseId = use.Id, ActionId = a.ActionId, Returned = false } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouZhangXiuAnniversarySlash = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuAnniversarySlashIssuedEvent(origin));
    }

    internal bool IsIssuedMouZhangXiuAnniversaryUse(CardUseFrame use)
    {
        if (use.MouZhangXiuAnniversarySlash is not { } o || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } current || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.UnlimitedUse != true || o.UseId != use.Id || o.ActionId != current.ActionId || current.ProviderSeat != o.Source.OwnerSeat || current.RequesterSeat is not null || _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash != o.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuAnniversarySlashIssuedEvent>().Count(e => e.Origin == o) != 1 || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.FrameId)is not ProgramSkillFrame { MouZhangXiuAnniversary: { Stage: MouZhangXiuAnniversaryStage.NativeSlash, Returned: false } r } parent || !ValidMouZhangXiuAnniversary(parent) || r.UseId != use.Id || r.ActionId != current.ActionId || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.Index != o.Index || r.Target != o.Target)
            return false;
        var original = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            original = fire.OriginalAction;
        }
        else if (use.CardKind != CardKind.Slash)
            return false;
        if (original.EffectiveKind != CardKind.Slash || original.EffectiveSuit != Suit.None || original.EffectiveRank != 0 || original.EffectiveIsRed is not null || original.ConversionChain is not [var source] || source != o.Source)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Source.OwnerSeat, o.Source.OwnerSeat) : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && use.SourceSeat == o.Source.OwnerSeat && current.ActorSeat == o.Source.OwnerSeat;
    }

    internal bool IsMouZhangXiuAnniversaryUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedMouZhangXiuAnniversaryUse(use);
    internal Card MouZhangXiuAnniversaryRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedMouZhangXiuAnniversaryUse(use) ? new(0, use.CardKind, Suit.None, 0) : throw new InvalidOperationException("Fuxi requires its own native zero-material representation.");
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 36600)]
    internal bool FinishMouZhangXiuAnniversaryAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsMouZhangXiuAnniversaryUse(attack.ResolutionId))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(36600)]
    internal void ValidateMouZhangXiuAnniversaryNative(CardUseFrame use)
    {
        if (use.MouZhangXiuAnniversarySlash is not null && (!IsIssuedMouZhangXiuAnniversaryUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Fuxi cannot retire an incomplete or unrelated native use.");
    }

    [NativeUseReturn(36600)]
    internal void ReturnMouZhangXiuAnniversaryNative(CardUseFrame use)
    {
        if (use.MouZhangXiuAnniversarySlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouZhangXiuAnniversary!;
        if (r.Stage != MouZhangXiuAnniversaryStage.NativeSlash || r.Returned || r.UseId != use.Id || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Fuxi lost its unique typed whole-use return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZhangXiuAnniversary = r with { Returned = true } });
        if (use.CardAttack is not { Active: true })
            SettleMouZhangXiuAnniversaryNative(o);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 36600)]
    internal bool ReturnMouZhangXiuAnniversaryAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhangXiuAnniversary: { Returned: true, Stage: MouZhangXiuAnniversaryStage.NativeSlash } r } f || c.ResolutionId != r.UseId)
            return false;
        if (c.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Fuxi attack returned to another owner.");
        SettleMouZhangXiuAnniversaryNative(_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuAnniversarySlashIssuedEvent>().Single(e => e.Origin.FrameId == f.Id && e.Origin.Index == r.Index).Origin);
        return true;
    }

    internal void SettleMouZhangXiuAnniversaryNative(MouZhangXiuAnniversarySlashOrigin o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouZhangXiuAnniversary!;
        if (!r.Returned || r.UseId != o.UseId || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhangXiuAnniversarySlashReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index))
            throw new InvalidOperationException("Fuxi cannot settle a native Slash twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZhangXiuAnniversary = r with { Index = r.Index + 1, Stage = MouZhangXiuAnniversaryStage.SlashChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhangXiuAnniversarySlashReturnedEvent(f.Id, r.Index, o.UseId, o.ActionId));
        ContinueMouZhangXiuAnniversary(f.Id);
    }
}

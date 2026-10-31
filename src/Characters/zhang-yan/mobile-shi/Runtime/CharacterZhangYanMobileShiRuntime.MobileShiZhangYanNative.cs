#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYanMobileShiRuntime
{
    internal void IssueShiZyVirtualDuel(ProgramSkillFrame f)
    {
        var r = f.ShiZhangYan!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(r.Target, f.OwnerSeat))
        {
            FinishShiZy(f);
            return;
        }

        var original = FrozenShiZyCompletedSlash(f) ?? throw new InvalidOperationException("Xiaoge lost its exact completed Slash before suspending its native attack.");
        if (original.CardAttack!.Active != r.OriginalAttackActive)
            throw new InvalidOperationException("Xiaoge cannot suspend a modified original native Slash cursor.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(original.Id, use => use with { CardAttack = use.CardAttack!with { Active = false } });
        var origin = new ProgramVirtualOrdinaryTrickOrigin(f.Id, f.InstructionIndex, r.WindowId, f.OwnerSeat, f.OwnerSeat, r.Target, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, CardKind.Duel, SkillProgramConditionKind.Always, null, null);
        var id = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangYan = r with { NativeUseId = id, Duel = origin, Stage = ShiZyStage.DuelIssued } });
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        if (_coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [r.Target], CardKind.Duel, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin) != id)
            throw new InvalidOperationException("Xiaoge lost the reserved exact zero-entity native Duel.");
        var native = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        origin = native.VirtualOrdinaryTrickOrigin!;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == f.Id)is not ProgramSkillFrame parent || parent != f || _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id) != _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id) + 1)
            throw new InvalidOperationException("Xiaoge's newly issued native Duel lost its frozen original parent.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShiZhangYan = f.ShiZhangYan!with { Duel = origin } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZyDuelIssuedEvent(id, origin));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, native.TargetSeats, LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    internal bool ValidShiZyVirtualDuelUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ShiZyXiaoge, OutputKind: CardKind.Duel } o || use.CardId != 0 || use.CardKind != CardKind.Duel || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActionId != o.CardActionId || a.ProviderSeat != o.InitialActorSeat || !a.TargetSeats.SequenceEqual(use.TargetSeats) || o.ConditionKind != SkillProgramConditionKind.Always || o.PindianBind is not null || o.FrozenSourceWon is not null || o.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ParentProgramFrameId)is not ProgramSkillFrame { ShiZhangYan: { Stage: ShiZyStage.DuelIssued } r } f || r.Op != SkillProgramEffectOp.ResolveMobileShiZhangYanXiaogeDuel || f.SkillId != ShiZyXiaoge || f.TriggerId != "sole-slash-duel" || f.InstructionIndex != o.InstructionIndex || f.OwnerSeat != o.OwnerSeat || f.SkillInstanceId != o.SkillInstanceId || f.GameplayHash != o.GameplayHash || r.Source != ShiZySource(f) || r.Hash != o.GameplayHash || r.WindowId != o.LifecycleFrameId || f.WindowContext?.ParentFrameId != r.WindowId || r.NativeUseId != use.Id || r.Duel is not { } issued || (issued with
        {
            CardActionId = 0
        }

        ) != (o with
        {
            CardActionId = 0
        }

        ) || o.InitialActorSeat != f.OwnerSeat || o.InitialTargetSeat != r.Target || !ValidShiZy(f) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Step: ResolutionFrameStep.Completed, Action: { } slash } original || !CoreNativeCardClassificationsRuntime.IsSlashCard(original.CardKind) || slash.ActionId != r.ActionId || slash.ActorSeat != f.OwnerSeat || original.SourceSeat != f.OwnerSeat || original.TargetIndex != r.OriginalTargetIndex || original.CardAttack?.Active != false || r.OriginalAttackActive is null || slash.EffectiveDesignatedTargetSeats is not [var target] || target != r.Target)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == f.Id && _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, a.ActorSeat, o.InitialActorSeat);
    }

    internal bool TryReturnShiZyVirtualDuel(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ShiZyXiaoge } origin)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhangYan: { Stage: ShiZyStage.DuelIssued } r } f || f.Id != origin.ParentProgramFrameId || f.InstructionIndex != origin.InstructionIndex || r.NativeUseId != use.Id || r.Duel != origin || use.Step != ResolutionFrameStep.Completed || !ValidShiZy(f))
            throw new InvalidOperationException("Xiaoge's native Duel lost its exact original completed-Slash program return.");
        if (use.CardAttack is { Active: true })
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == use.Id))
                throw new InvalidOperationException("Xiaoge Duel cannot return the same native attack twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramVirtualOrdinaryAttackReturnedEvent(use.Id, origin));
            return true;
        }

        CompleteShiZyDuelReturn(f, use.Id, origin);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 50500)]
    internal bool ResumeShiZyNativeDuel()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhangYan: { Stage: ShiZyStage.DuelIssued, Duel: { } origin } r } f || r.Op != SkillProgramEffectOp.ResolveMobileShiZhangYanXiaogeDuel || _resolutionStackOperations._resolutionStack.Any(x => x.Id == r.NativeUseId) || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == r.NativeUseId && e.Origin == origin))
            return false;
        CompleteShiZyDuelReturn(f, r.NativeUseId, origin);
        return true;
    }

    internal void CompleteShiZyDuelReturn(ProgramSkillFrame f, long useId, ProgramVirtualOrdinaryTrickOrigin origin)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyDuelReturnedEvent>().Any(e => e.UseId == useId))
            throw new InvalidOperationException("Xiaoge's issued Duel already returned.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangYan = f.ShiZhangYan!with { Stage = ShiZyStage.DuelChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZyDuelReturnedEvent(useId, origin));
        ContinueShiZy(f.Id);
    }

    internal CardUseFrame? FrozenShiZyCompletedSlash(ProgramSkillFrame f) => f.ShiZhangYan is { Op: SkillProgramEffectOp.ResolveMobileShiZhangYanXiaogeDuel, OriginalAttackActive: not null, OriginalTargetIndex: { } cursor } r && ValidShiZy(f) && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } action, CardAttack: not null } use && use.SourceSeat == f.OwnerSeat && action.ActorSeat == f.OwnerSeat && use.TargetIndex == cursor && action.ActionId == r.ActionId && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && action.EffectiveDesignatedTargetSeats is [var target] && target == r.Target && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == r.WindowId)is ProgramCardTriggerWindowFrame window && window.ParentFrameId == use.Id && window.AttackOwnerFrameId == use.Id && window.Continuation == ProgramCardContinuation.CompletedSlash && window.CompletedResponseReturn is null && CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(window.Action, action) && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardKind == use.CardKind) == 1 ? use : null;
    internal void RestoreShiZyCompletedSlash(ProgramSkillFrame f)
    {
        var r = f.ShiZhangYan!;
        if (r.Stage != ShiZyStage.DuelChildren || r.Duel is not { } origin || r.OriginalAttackActive is not { } active || FrozenShiZyCompletedSlash(f)is not { CardAttack.Active: false } original || _resolutionStackOperations._resolutionStack.Any(frame => frame.Id == r.NativeUseId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyDuelReturnedEvent>().Count(e => e.UseId == r.NativeUseId && e.Origin == origin) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZyAttackRestoredEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Xiaoge cannot restore a different, unfinished or already restored original Slash attack.");
        if (active)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(original.Id, use => use with { CardAttack = use.CardAttack!with { Active = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZyAttackRestoredEvent(f.Id, original.Id, r.ActionId, original.TargetIndex, active));
    }
}

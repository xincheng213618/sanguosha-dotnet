#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiOlMagicRuntime
{
    internal bool CanMoSimaYiRetaliationSlash(int actor, int target) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: [], ignoreDistance: true, countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33900)]
    internal void BindMoSimaYiSlash(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MoSimaYi: { Stage: MoSimaYiStage.IssuingSlash, Operation: SkillProgramEffectOp.ResolveMoSimaYiRetaliation } r } f)
            return;
        var actor = r.Targets[r.Index];
        if (!MoSimaYi.Valid(f) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != actor || a.ProviderSeat != actor || a.RequesterSeat is not null || !a.TargetSeats.SequenceEqual([f.OwnerSeat]))
            throw new InvalidOperationException("Zhouxi must issue the frozen character's actual neutral forced Slash.");
        var origin = new MoSimaYiSlashOrigin(f.Id, r.Source, r.GameplayHash, r.Index, use.Id, a.ActionId, actor, f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MoSimaYi = r with { Stage = MoSimaYiStage.NativeSlash, NativeUseId = use.Id, NativeActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MoSimaYiSlash = origin });
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoSimaYiSlashIssuedEvent(origin));
    }

    internal bool IsMoSimaYiRetaliation(CardUseFrame use)
    {
        if (use.MoSimaYiSlash is not { } o || use.Id != o.UseId || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActionId != o.ActionId || a.ProviderSeat != o.Actor || a.RequesterSeat is not null || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is not ProgramSkillFrame { MoSimaYi: { Stage: MoSimaYiStage.NativeSlash } r } f || !MoSimaYi.Valid(f) || o.Source != r.Source || o.GameplayHash != f.GameplayHash || o.Index != r.Index || r.NativeUseId != use.Id || r.NativeActionId != a.ActionId || o.Actor != r.Targets[r.Index] || o.Target != f.OwnerSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoSimaYiSlashIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        return _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Actor, o.Actor);
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 33900)]
    internal bool FinishMoSimaYiSlash(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MoSimaYiSlash: not null } use || !IsMoSimaYiRetaliation(use))
            return false;
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

    [NativeUseValidation(33900)]
    internal void ValidateMoSimaYiUse(CardUseFrame use)
    {
        if (use.MoSimaYiSlash is not null && (!IsMoSimaYiRetaliation(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Zhouxi requires its exact whole native use to finish.");
    }

    [NativeUseReturn(33900)]
    internal void ReturnMoSimaYiUse(CardUseFrame use)
    {
        if (use.MoSimaYiSlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MoSimaYi!;
        if (r.NativeUseId != use.Id || r.NativeActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Zhouxi cannot return a repeated or unrelated native Slash.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MoSimaYi = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            MoSimaYi.SlashReturned(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 33900)]
    internal bool ReturnMoSimaYiAttack(GameEngine.AttackCompletionReceipt receipt)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoSimaYi: { Stage: MoSimaYiStage.NativeSlash, NativeReturned: true } r } f || r.NativeUseId != receipt.ResolutionId)
            return false;
        if (receipt.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Zhouxi's attack returned to another producer.");
        MoSimaYi.SlashReturned(f.Id);
        return true;
    }
}

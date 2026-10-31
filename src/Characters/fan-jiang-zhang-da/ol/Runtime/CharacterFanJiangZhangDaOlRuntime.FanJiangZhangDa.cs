#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFanJiangZhangDaOlRuntime
{
    internal bool HasFanJiangZhangDaCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveJueshengLimitedDuel);

    internal bool JueshengSpent(int seat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JueshengSpentEvent>().Any(e => e.OwnerSeat == seat && e.SkillId == "ol:juesheng");
    internal bool CanIssueJueshengDuel(int actor, int target) => _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor);
    SkillProgramStepOutcome global::CardGame.Core.IFanJiangZhangDaHost.Resolve(ProgramSkillFrame f) => BeginJuesheng(f);
    bool global::CardGame.Core.IJueshengTargetQuery.Allows(int actor, int target) => CanIssueJueshengDuel(actor, target);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27350, SkillProgramEffectOp.ResolveJueshengLimitedDuel)]
    internal bool CanActivateJuesheng(CharacterState owner) => !JueshengSpent(owner.Seat) && _coreMatchParticipantsPlayersAccess._players.Any(p => CanIssueJueshengDuel(owner.Seat, p.Seat));
    internal SkillProgramStepOutcome BeginJuesheng(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Juesheng is not null || f.SkillId != "ol:juesheng" || f.ActivationId != "limited-duel" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || JueshengSpent(f.OwnerSeat) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanIssueJueshengDuel(f.OwnerSeat, target) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Juesheng requires its unspent owner, exact actual Play activation and legal Duel target.");
        var origin = new JueshengDuelOrigin(f.Id, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.OwnerSeat, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Juesheng = new(origin, f.InstructionIndex, JueshengStage.NativeUse, [], []) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengSpentEvent(f.Id, f.OwnerSeat, f.SkillId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengDuelIssuedEvent(origin, f.InstructionIndex));
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [target], CardKind.Duel, physicalCardIds: [], jueshengDuel: origin);
        if (id != origin.UseId)
            throw new InvalidOperationException("Juesheng changed its reserved native Use identity.");
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int PrepareJueshengDuelDamage(long useId, int targetSeat)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { JueshengDuel: { } origin } use)
            return 1;
        if (!IsJueshengDuelUse(useId) || !use.TargetSeats.Contains(targetSeat))
            throw new InvalidOperationException("Juesheng damage belongs to one actual finalized native Duel opponent.");
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == origin.ProgramFrameId);
        var r = f.Juesheng!;
        if (r.Targets.SingleOrDefault(t => t.TargetSeat == targetSeat)is { } frozen)
            return frozen.Amount;
        var count = _coreLifetimeSlashUsesOperations.LifetimeSlashUseCount(targetSeat);
        var entry = new JueshengTargetDamage(targetSeat, count, Math.Max(1, count));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Juesheng = r with { Targets = r.Targets.Append(entry).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengTargetDamageFrozenEvent(f.Id, useId, r.Targets.Count, targetSeat, entry.SlashCount, entry.Amount));
        return entry.Amount;
    }

    internal bool JueshengActorMatches(CardUseFrame use, JueshengDuelOrigin origin)
    {
        if (use.Action is not { } action || action.ProviderSeat != origin.ActorSeat)
            return false;
        var actor = origin.ActorSeat;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
        {
            if (e.PreviousActorSeat != actor || e.OwnerSeat != actor || e.ProviderSeat != origin.ActorSeat)
                return false;
            actor = e.ActorSeat;
        }

        return actor == use.SourceSeat && actor == action.ActorSeat;
    }

    internal bool IsJueshengDuelUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { JueshengDuel: { } origin, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: null, EffectiveRank: null or 0, EffectiveIsRed: null } } use || origin.UseId != id || !JueshengActorMatches(use, origin) || _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengDuelIssuedEvent>().Count(e => e.Origin == origin && e.InstructionIndex == 1) != 1)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1] is ProgramSkillFrame { Juesheng: { Stage: JueshengStage.NativeUse, NativeCompleted: false } r } f && f.Id == origin.ProgramFrameId && r.Origin == origin && f.SkillId == origin.Source.SkillId && f.SkillInstanceId == origin.Source.SkillInstanceId && f.GameplayHash == origin.GameplayHash && r.InstructionIndex == f.InstructionIndex;
    }

    internal long? JueshengDuelProgramParent(long id) => IsJueshengDuelUse(id) ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.JueshengDuel!.ProgramFrameId : null;
    [NativeUseValidation(27350)]
    internal void ValidateJueshengNative(CardUseFrame use)
    {
        if (use.JueshengDuel is not null && !IsJueshengDuelUse(use.Id))
            throw new InvalidOperationException("Juesheng lost its zero-entity native Duel, final actor chain or owning paid operation.");
    }

    [NativeUseReturn(27350)]
    internal void ReturnJueshengNative(CardUseFrame use)
    {
        if (use.JueshengDuel is not { } origin)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ProgramFrameId);
        var r = f.Juesheng;
        if (r is not { Stage: JueshengStage.NativeUse, NativeCompleted: false } || r.Origin != origin || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardKind == CardKind.Duel && e.CardId == 0) != 1)
            throw new InvalidOperationException("Juesheng must return once after the entire native Duel, every target and extra effect round.");
        var targets = use.TargetSeats.Distinct().ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Juesheng = r with { NativeCompleted = true, FinalTargets = targets } });
        for (var i = 0; i < targets.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengFinalTargetEvent(f.Id, use.Id, i, targets[i]));
        if (use.CardAttack is { Active: true })
            return;
        SettleJueshengNative(origin);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27350)]
    internal bool ReturnJueshengAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Juesheng: { Stage: JueshengStage.NativeUse, NativeCompleted: true } r } || completion.ResolutionId != r.Origin.UseId)
            return false;
        if (completion.ProgramFrameId is { } parent && parent != r.Origin.ProgramFrameId)
            throw new InvalidOperationException("Juesheng's complete native Duel returned to another program.");
        SettleJueshengNative(r.Origin);
        return true;
    }

    internal void SettleJueshengNative(JueshengDuelOrigin origin)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ProgramFrameId);
        var r = f.Juesheng!;
        if (!r.NativeCompleted || r.Origin != origin || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<JueshengDuelReturnedEvent>().Any(e => e.UseId == origin.UseId))
            throw new InvalidOperationException("Juesheng cannot issue the then-clause before or twice after its whole native Use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Juesheng = r with { Stage = JueshengStage.GrantChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengDuelReturnedEvent(f.Id, origin.UseId, r.FinalTargets.Count));
        ContinueJuesheng(f.Id);
    }

    internal void ContinueJuesheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Juesheng: { } r } f || f.Id != id)
            return;
        if (r.Stage != JueshengStage.GrantChildren)
            return;
        while (true)
        {
            if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.Juesheng!;
            if (r.GrantCursor == r.FinalTargets.Count)
                break;
            var index = r.GrantCursor;
            var seat = r.FinalTargets[index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Juesheng = r with { GrantCursor = index + 1 } });
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengGrantSkippedEvent(f.Id, r.Origin.UseId, index, seat));
                continue;
            }

            var source = $"juesheng:{(f.Id)}:{(r.Origin.UseId)}:{(index)}:{(seat)}";
            var grantId = source + ":ol:juesheng";
            var instance = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ActiveGrants.Where(g => g.SkillId == "ol:juesheng").OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId ?? $"juesheng-shared:{(seat)}";
            var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
            _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grant(new(grantId, "ol:juesheng", instance, source));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengSkillGrantedEvent(f.Id, r.Origin.UseId, index, seat, grantId, instance, source, cutoff));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengCompletedEvent(f.Id, r.Origin.UseId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Juesheng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27350)]
    internal void ExpireJueshengSkills(ActualTurnCursorReturnedEvent returned)
    {
        if (!HasFanJiangZhangDaCapability)
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        foreach (var issued in history.OfType<JueshengSkillGrantedEvent>().Where(e => e.RecipientSeat == returned.OwnerSeat))
        {
            if (history.OfType<JueshengSkillExpiredEvent>().Any(e => e.GrantId == issued.GrantId))
                continue;
            var start = history.Skip(issued.HistoryCutoff).OfType<TurnStartedEvent>().FirstOrDefault(e => e.ActorSeat == issued.RecipientSeat);
            if (start?.TurnNumber != returned.ActualTurn)
                continue;
            var grant = _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].SkillGrants.Grants.SingleOrDefault(g => g.GrantId == issued.GrantId);
            if (grant is not null && (grant.SkillId != "ol:juesheng" || grant.SourceId != issued.SourceId || grant.SkillInstanceId != issued.SkillInstanceId))
                throw new InvalidOperationException("Juesheng may expire only its original independently issued grant.");
            if (grant is not null)
                _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].SkillGrants.RemoveGrant(grant.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JueshengSkillExpiredEvent(issued.RecipientSeat, issued.GrantId, issued.SkillInstanceId, issued.SourceId, returned.ActualTurn));
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27350)]
    internal bool ResumeJuesheng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Juesheng: not null } f || f.Id != id)
            return false;
        ContinueJuesheng(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27350)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27350)]
    internal bool ContinuePaidJuesheng(ProgramSkillFrame f)
    {
        if (f.Juesheng is null)
            return false;
        return true;
    }
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangJiMouRuntime
{
    internal MouJiangJiRuntime MouJiangJi => _mouJiangJiOperations.Get();

    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 36300)]
    internal IEnumerable<LegalAction> MouJjForeignActions(CharacterState actor) => MouJiangJi.ForeignActions(actor.Seat);
    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 36300)]
    internal bool SubmitMouJjForeign(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (command.SkillId != MouJiangJiRuntime.Shiju || command.ActivationId != "give-and-use" || command.SkillOwnerSeat is not { } owner)
            return false;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        if (command.CardIds.Count != 0 || command.TargetSeats.Count != 0 || !MouJiangJi.ForeignActions(command.ActorSeat).Any(a => a.ProgramSkillOwnerSeat == owner))
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "Shiju requires its current published donor, owner and actual Play.");
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            IssueMouJjForeign(command.ActorSeat, owner);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 36300)]
    internal bool ExecuteMouJjForeign(CharacterState actor, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (action.ProgramSkillId != MouJiangJiRuntime.Shiju || action.ProgramActivationId != "give-and-use" || action.ProgramSkillOwnerSeat is not { } owner)
            return false;
        if (cards.Count != 0 || targets.Count != 0 || !MouJiangJi.ForeignActions(actor.Seat).Any(a => a.ProgramSkillOwnerSeat == owner))
            throw new InvalidOperationException("Shiju changed its published foreign owner or actual giver.");
        IssueMouJjForeign(actor.Seat, owner);
        return true;
    }

    internal void IssueMouJjForeign(int actor, int owner)
    {
        var program = _coreSkillProgramsOperations.GetEnabledSkillProgram(_coreMatchParticipantsPlayersAccess._players[owner], MouJiangJiRuntime.Shiju);
        var f = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner, program.Id, "give-and-use", program.GameplayHash, 0, [], [])
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], program.Id),
            ActivatingSeat = actor,
            ActivationPhase = new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, actor, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
        };
        _coreRuntimeOperations.PushRuntimeFrame(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSharedActivationStartedEvent(f.Id, new(f.SkillId, f.ActivationId, owner, f.SkillInstanceId), actor, f.ActivationPhase));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(f.Id, owner, f.SkillId, f.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 36300)]
    internal bool MouJjFinalized(ProgramTargetCardWindowQuery q) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouJiangJiYingshi) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(q.Kind);
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 36300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 36300)]
    internal bool MouJjImplicit(ProgramTargetCardWindowQuery q) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouJiangJiYingshi);
    int global::CardGame.Core.IMouJiangJiFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMouJiangJiFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long global::CardGame.Core.IMouJiangJiFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    long global::CardGame.Core.IMouJiangJiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouJiangJiFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouJiangJiFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouJiangJiFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string global::CardGame.Core.IMouJiangJiFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    bool global::CardGame.Core.IMouJiangJiFacts.ActualPlay(int s) => ((global::CardGame.Core.IMouJiangJiFacts)this).Alive(s) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == s && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && ((global::CardGame.Core.IMouJiangJiFacts)this).PhaseId > 0;
    int global::CardGame.Core.IMouJiangJiFacts.EquipmentCount(int s) => ((global::CardGame.Core.IMouJiangJiFacts)this).Alive(s) ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count : 0;
    SkillProgram? global::CardGame.Core.IMouJiangJiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouJiangJiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<int> global::CardGame.Core.IMouJiangJiFacts.ShijuOwners(int actor) => !((global::CardGame.Core.IMouJiangJiFacts)this).ActualPlay(actor) || ((global::CardGame.Core.IMouJiangJiFacts)this).Cards(actor, false).Count == 0 ? [] : Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreSkillProgramsOperations.EnabledSkillPrograms(p).Any(s => s.Id == MouJiangJiRuntime.Shiju)).Select(p => p.Seat).ToArray());
    IReadOnlyList<MouJiangJiCard> global::CardGame.Core.IMouJiangJiFacts.Cards(int seat, bool discard) => !((global::CardGame.Core.IMouJiangJiFacts)this).Alive(seat) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Transfer) && (!discard || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, new(z, seat), OwnedCardMoveIntent.Discard))).Select(c => new MouJiangJiCard(c.Id, new(z, seat), c.Kind, c.DisplayName))).ToArray());
    bool global::CardGame.Core.IMouJiangJiFacts.Disabled(int seat, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState(seat, MouJiangJiRuntime.Yingshi, instance, "disabled");
    bool global::CardGame.Core.IMouJiangJiFacts.Parent(ProgramSkillFrame f)
    {
        if (f.SkillId == MouJiangJiRuntime.Shiju)
        {
            if (f.TriggerId is not null || f.ActivationId != "give-and-use" || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || ((global::CardGame.Core.IMouJiangJiFacts)this).History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.OwnerSeat == f.OwnerSeat && p.SkillId == f.SkillId && p.ActivationId == f.ActivationId) != 1)
                return false;
            return f.ActivatingSeat is not { } actor ? f.ActivationPhase is null : f.ActivationPhase is { } phase && ((global::CardGame.Core.IMouJiangJiFacts)this).History<ProgramSharedActivationStartedEvent>().Count(p => p.FrameId == f.Id && p.ActorSeat == actor && p.Source == MouJiangJiRuntime.Source(f) && p.Phase == phase) == 1;
        }

        if (f.TriggerId != "ordinary-trick-target" || f.WindowContext is not { } w || f.ActivationId != f.TriggerId)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return at > 0 && _resolutionStackOperations._resolutionStack[at - 1].Id == w.ParentFrameId && ((global::CardGame.Core.IMouJiangJiFacts)this).Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, w.OccurrenceIndex), w)is not null;
    }

    MouJiangJiUse? global::CardGame.Core.IMouJiangJiFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouJiangJiRuntime.Yingshi || w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != card.ParentCardUseFrameId || window.CompletedResponseReturn is not null || window.Continuation != ProgramCardContinuation.FinalizedTrick || window.TrickContinuation is null || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActorSeat != c.OwnerSeat || a.ActionId != card.CardActionId || window.Action.ActionId != a.ActionId || use.SourceSeat != c.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var now = window.Candidates[window.CandidateIndex];
        if (now.OwnerSeat != c.OwnerSeat || now.SkillId != c.SkillId || now.TriggerId != c.BindingId || now.SkillInstanceId != c.SkillInstanceId || now.GameplayHash != c.GameplayHash)
            return null;
        var targets = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : use.TargetSeats.ToArray();
        return new(use.Id, a.ActionId, window.Id, use.CardKind)
        {
            Targets = targets
        };
    }

    IReadOnlyList<LegalAction> global::CardGame.Core.IMouJiangJiFacts.UseOptions(ProgramSkillFrame f) => MouJjUseOptions(f);
    CardKind global::CardGame.Core.IMouJiangJiFacts.UseKind(int actor, LegalAction action) => action.PlayedCardKind ?? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[actor], _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(action.CardId!.Value), action.ConversionSource));
    void global::CardGame.Core.IMouJiangJiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouJiangJiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouJiangJiNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouJiangJiNative.Give(ProgramSkillFrame f)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.GiveChildren, Card: { } card } r || _cardZonesOperations._cardZones.GetLocation(card.Id) != card.From)
            throw new InvalidOperationException("Shiju requires its own committed actual HE entity.");
        if (r.Giver == f.OwnerSeat && card.From == CardLocation.Hand(r.Giver))
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiangJi = r with { Delivered = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangJiSelfCardCommittedEvent(f.Id, card.Id));
            return;
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(MouJiangJiRuntime.GiveReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangJi!;
            if (!MouJiangJi.Valid(parent) || moves is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Shiju lost its primary true gift receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJiangJi = paid with { Delivered = m.To == CardLocation.Hand(f.OwnerSeat), After = m.Sequence } });
            if (m.To == CardLocation.Hand(f.OwnerSeat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangJiGivenEvent(f.Id, r.Giver, f.OwnerSeat, card.Id, m.Sequence));
            _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, r.Giver, f.OwnerSeat, [card.Id], r.Before);
        });
    }

    void global::CardGame.Core.IMouJiangJiNative.Use(ProgramSkillFrame f) => IssueMouJjUse(f);
    void global::CardGame.Core.IMouJiangJiNative.Repeat(ProgramSkillFrame f) => IssueMouJjRepeat(f);
    void global::CardGame.Core.IMouJiangJiNative.Discard(ProgramSkillFrame f)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.DiscardChildren } r || r.Selected.Any(id => !_characterJiangJiMouOperations.CreateMouJiangJiFacts().Cards(r.Target, true).Any(c => c.Id == id)))
            throw new InvalidOperationException("Yingshi requires its actual target's selected payable HE.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, r.Selected, new(MouJiangJiRuntime.DiscardReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouJiangJi!;
            if (!MouJiangJi.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From.OwnerSeat != r.Target || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Yingshi lost its actual native logical discard.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouJiangJi = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangJiDiscardPaidEvent(f.Id, r.Target, m.CardId, m.Sequence));
        });
    }

    void global::CardGame.Core.IMouJiangJiNative.Disable(ProgramSkillFrame f)
    {
        if (!MouJiangJi.Valid(f) || f.MouJiangJi is not { Stage: MouJiangJiStage.DiscardChildren } r)
            throw new InvalidOperationException("Yingshi cannot disable before its issued discard branch returns.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, MouJiangJiRuntime.Yingshi, f.SkillInstanceId, "disabled", true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangJiDisabledEvent(f.Id, r.Source, r.Turn));
    }

    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 36300)]
    internal bool MouJjDamage(global::CardGame.Core.GameEngine.IDamageAttempt a, long id)
    {
        if (!a.DamageWasApplied || a is not global::CardGame.Core.NativeOwners.CardAttackHandle || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is not DamageFrame { Amount: > 0 })
            return false;
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MouJiangJi is { Stage: MouJiangJiStage.NativeUse, Native: { } n } && n.UseId == a.ResolutionId).ToArray())
            MouJiangJi.Damage(f);
        return false;
    }
}

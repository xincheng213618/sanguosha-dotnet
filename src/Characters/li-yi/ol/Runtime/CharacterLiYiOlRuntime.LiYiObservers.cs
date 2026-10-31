#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiYiOlRuntime
{
    internal static bool ChanshuangChildren(ChanshuangStage s) => global::CardGame.Core.RuntimeSkills.RuntimeRules_li_yi_ol_LiYiObservers.ChanshuangChildren(s);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27480)]
    internal void AssertLiYi(ProgramSkillFrame f)
    {
        if (f.Chanshuang is not { } r)
            return;
        if (f.InstructionIndex != 1 || r.Source != ChanshuangSource(f) || r.GameplayHash != f.GameplayHash || f.SkillId != "ol:chanshuang" || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || r.Operation is not (SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices or SkillProgramEffectOp.ResolveChanshuangEnding) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.ActualTurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || r.Cursor < 0 || r.Cursor > ChanshuangItemCount(r) || _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangStartedEvent>().Count(e => e == new ChanshuangStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ActualTurn, r.ActualTurnOwner, r.PeerSeat, r.FrozenMask)) != 1)
            throw new InvalidOperationException("Li Yi lost his real registered instruction, current actual turn, immutable issuance or execution cursor.");
        var ending = r.Operation == SkillProgramEffectOp.ResolveChanshuangEnding;
        if (ending ? (f.TriggerId != "ending-first-items" || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } c || c.SourceSeat != f.OwnerSeat || r.PeerSeat != f.OwnerSeat || r.FrozenMask is < 1 or > 7 || r.Votes.Count != 0) : (f.TriggerId is not null || f.SelectedTargetSeats is not [var peer] || peer != r.PeerSeat || peer == f.OwnerSeat || r.FrozenMask != 0 || r.Votes.Count > 2 || r.Votes.Where((v, i) => v.Seat != (i == 0 ? f.OwnerSeat : r.PeerSeat) || !Enum.IsDefined(v.Option)).Any() || r.Stage != ChanshuangStage.Votes && r.Votes.Count != 2))
            throw new InvalidOperationException("Chanshuang replaced its sealed independent choices or frozen Ending first-X obligation.");
        if (!ending && r.Stage != ChanshuangStage.Votes)
        {
            var votes = _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangVoteRevealedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
            if (votes.Length != 2 || votes.Where((e, i) => e.Index != i || e.Seat != r.Votes[i].Seat || e.Option != r.Votes[i].Option).Any())
                throw new InvalidOperationException("Chanshuang must reveal both actual votes, including duplicate options, exactly once.");
        }

        var completed = _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangItemCompletedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Cursor).ToArray();
        if (completed.Length != r.Cursor || completed.Where((e, i) => e.Cursor != i || e.ActorSeat != (ending ? f.OwnerSeat : r.Votes[i].Seat) || e.Option != (ending ? (ChanshuangOption)(i + 1) : r.Votes[i].Option)).Any())
            throw new InvalidOperationException("Chanshuang cannot skip, repeat or reorder an issued participant execution.");
        if (r.Selected.Distinct().Count() != r.Selected.Count || r.Materials.Select(c => c.CardId).Distinct().Count() != r.Materials.Count || r.Selected.Count > 2)
            throw new InvalidOperationException("Chanshuang cannot replace or duplicate its real primary HE selections.");
        var frozenGrain = _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangGrainFrozenEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).OrderBy(e => e.Index).ToArray();
        if (frozenGrain.Length != r.Grain.Count || frozenGrain.Where((e, i) => e.Index != i || e.CardId != r.Grain[i]).Any())
            throw new InvalidOperationException("Chanshuang replaced its original Wooden Ox spill entities.");
        if (ChanshuangChildren(r.Stage))
        {
            if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } movement || movement.SubjectSeat != f.OwnerSeat)
                throw new InvalidOperationException("Chanshuang lost its typed once-paid movement continuation.");
            if (r.Stage != ChanshuangStage.NativeChildren)
            {
                var item = ChanshuangItem(f);
                var cost = _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangCostIssuedEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).ToArray();
                if (cost is not [var fact] || fact.ActorSeat != item.Seat || fact.Option != item.Option || fact.Count != r.Materials.Count || r.Materials.Count is < 1 or > 2 || item.Option == ChanshuangOption.Recast && r.Materials.Count != 1 || r.Materials.Any(c => c.From.OwnerSeat != item.Seat || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
                    throw new InvalidOperationException("Chanshuang lost its actual complete recast/discard primary group.");
                var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangMaterialPaidEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).OrderBy(e => e.Index).ToArray();
                if (paid.Length != r.Materials.Count || paid.Where((e, i) => e.Index != i || e.ActorSeat != item.Seat || e.CardId != r.Materials[i].CardId || e.Kind != r.Materials[i].Kind || e.From != r.Materials[i].From || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason == (item.Option == ChanshuangOption.Recast ? CardMoveReasons.RecastDiscard : new CardMoveReason(ChanshuangDiscardReason)))).Any())
                    throw new InvalidOperationException("Chanshuang changed an exact paid material or its native movement proof.");
                if (r.Stage == ChanshuangStage.CostChildren && (fact.Before != r.Before || fact.After != r.After) || r.Stage == ChanshuangStage.DrawChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.ActorSeat == item.Seat && e.Count == r.Drawn && e.Before == r.Before && e.After == r.After) != 1)
                    throw new InvalidOperationException("Chanshuang lost its frozen cost/draw interval.");
                if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => !IsLiYiPaidMovement(f, m)))
                    throw new InvalidOperationException("Chanshuang's paid interval contains an unrelated card movement.");
            }
            else if (r.Native is not { } origin || !r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.UseId == origin.UseId && e.ActionId == origin.ActionId) != 1)
                throw new InvalidOperationException("Chanshuang lost its whole native Slash return before settlement.");
        }
        else if (f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Unpaid Chanshuang cannot claim a movement continuation.");
        if (r.Stage is ChanshuangStage.SlashIssuing or ChanshuangStage.NativeUse or ChanshuangStage.NativeChildren)
        {
            if (r.Selection is not { } selection || _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangSlashPreparedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.ActorSeat == selection.ActorSeat && e.ProviderSeat == selection.ProviderSeat && e.TargetSeat == selection.TargetSeat && e.Kind == selection.Kind && e.SelectionHash == ChanshuangHash(selection)) != 1)
                throw new InvalidOperationException("Chanshuang lost its real supplier's exact prepared selection.");
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && !ChanshuangChildren(r.Stage) && r.Stage is not (ChanshuangStage.NativeUse or ChanshuangStage.SlashIssuing) && !(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } faction && ValidateChanshuangFactionSlashParent(faction, f)))
        {
            PendingDecision Native(PendingDecision p) => _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramNativeDecision(_characterMaXiuMaTieOlOperations.KenshangProgramNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime.WeijieNativeDecision(p))))));
            var native = Native(prompt);
            var expected = Native(prompt with { Choices = ChanshuangChoices(f) });
            if (native.Kind != DecisionKind.ProgramTrigger || native.PlayerSeat != ChanshuangChooser(f) || !native.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(native.Choices, expected.Choices))
                throw new InvalidOperationException("Chanshuang changed its real private participant prompt.");
        }
    }

    internal bool IsLiYiPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Chanshuang!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        var actor = ChanshuangItem(f).Seat;
        if (r.Stage == ChanshuangStage.DrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(actor) && m.Reason == CardMoveReasons.RecastDraw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangMaterialPaidEvent>().Any(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To))
            return true;
        return r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(actor)) && r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(actor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 430)]
    internal bool IsLiYiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Chanshuang is { } r && ChanshuangChildren(r.Stage) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && effect?.Op == r.Operation;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 180)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 170)]
    internal bool IsLiYiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Chanshuang is { Stage: ChanshuangStage.CostChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == ChanshuangItem(f).Seat && recovery.Attempt.TargetSeat == ChanshuangItem(f).Seat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(recovery.Attempt.TargetSeat)) && recovery.Attempt.Completion.MoveReason == (ChanshuangItem(f).Option == ChanshuangOption.Recast ? CardMoveReasons.RecastDiscard : new CardMoveReason(ChanshuangDiscardReason));
    internal bool LiYiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Chanshuang is not { } r)
            return false;
        AssertLiYi(f);
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is ProgramSkillFrame { HuanShuJun: not null } huanpei)
            return _characterHuanShuJunOlOperations.HuanpeiDraftRidesOn(huanpei, f);
        if (child is CardUseFrame use)
            return r.Stage == ChanshuangStage.NativeUse && use.ChanshuangSlash == r.Native && IsIssuedChanshuangUse(use);
        if (child is DrawFundedDistinctBasicFrame funded)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && funded.Payment.ParentFrameId == f.Id && funded.Payment.RequestFrameId == f.Id && funded.Payment.Cursor == f.InstructionIndex && funded.Payment.Source.OwnerSeat == actor && funded.Payment.TargetSeat is { } t && targets.Contains(t) && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(funded.Payment);
        if (child is ChainedStateBasicFrame chained)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && chained.Payment.ParentFrameId == f.Id && chained.Payment.RequestFrameId == f.Id && chained.Payment.Cursor == f.InstructionIndex && chained.Payment.Source.OwnerSeat == actor && chained.Payment.TargetSeat is { } t && targets.Contains(t) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chained.Payment);
        if (child is ProgramSkillFrame { Weijie: { } wj } draft)
            return IsChanshuangSlashRequest(f) && TryGetChanshuangSlashRequest(f, out var actor, out var targets) && wj.Need.OwnerFrameId == f.Id && wj.Need.RequestFrameId == f.Id && wj.Need.ProviderSeat == actor && wj.Need.Targets.SequenceEqual(targets) && _characterGuoTuOlOperations.WeijieParent(draft);
        if (!ChanshuangChildren(r.Stage))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsLiYiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return r.Stage == ChanshuangStage.NativeChildren ? moved.ResumeProgramFrameId == f.Id && IsChanshuangNativeSettlementBatch(f, moved.Batch) : global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsLiYiPaidMovement(f, m));
    }

    internal ProgramSkillFrame? LiYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiYiOlOperations.LiYiFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 940)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1050)]
    internal bool HasLiYiDamageObserver(long id) => LiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1080)]
    internal bool HasLiYiBeforeDamageObserver(long id) => LiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 910)]
    internal bool IsLiYiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && LiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27480)]
    internal bool AdvanceLiYiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}

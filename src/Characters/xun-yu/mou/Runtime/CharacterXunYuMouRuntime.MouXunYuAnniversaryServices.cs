#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunYuMouRuntime
{
    internal MouXunYuAnniversaryRuntime MouXyAnniversary => _mouXunYuAnniversaryOperations.Get();

    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 37200)]
    internal bool MouXyDrawTwoTargets(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouXunYuAnniversaryShimou);
    int IMouXunYuAnniversaryFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    long IMouXunYuAnniversaryFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouXunYuAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouXunYuAnniversaryFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouXunYuAnniversaryFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouXunYuAnniversaryFacts.OwnPlay(int s) => ((IMouXunYuAnniversaryFacts)this).Alive(s) && s == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play;
    int IMouXunYuAnniversaryFacts.HandCount(int s) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count;
    int IMouXunYuAnniversaryFacts.MaximumHp(int s) => _coreMatchParticipantsPlayersAccess._players[s].MaxHp;
    int IMouXunYuAnniversaryFacts.Hp(int s) => _coreMatchParticipantsPlayersAccess._players[s].Hp;
    string IMouXunYuAnniversaryFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    string IMouXunYuAnniversaryFacts.Instance(int s, string skill) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[s], skill);
    bool IMouXunYuAnniversaryFacts.Yin(int owner, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner, MouXunYuAnniversaryRuntime.Shimou, instance, "yin");
    SkillProgram? IMouXunYuAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    SkillPromptPresentation IMouXunYuAnniversaryFacts.Presentation(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    IEnumerable<T> IMouXunYuAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IReadOnlyList<int> IMouXunYuAnniversaryFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<MouXyCard> IMouXunYuAnniversaryFacts.Hand(int s) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(s, c, CardLocation.Hand(s), OwnedCardMoveIntent.Discard)).Select(c => new MouXyCard(c.Id, c.Kind, c.DisplayName)).ToArray());
    IReadOnlyList<MouXyPlan> IMouXunYuAnniversaryFacts.Plans(ProgramSkillFrame f) => MouXyPlans(f);
    bool IMouXunYuAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouXunYuAnniversaryRuntime.Shimou && f.ActivationId == "adjust-and-command" && f.WindowContext is null && ((IMouXunYuAnniversaryFacts)this).History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId) == 1;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        if (w.Window == SkillProgramTriggerWindow.GameStarting)
            return f.SkillId == MouXunYuAnniversaryRuntime.Shimou && f.TriggerId == "opening-state" && _resolutionStackOperations._resolutionStack[at - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } p && p.Id == w.ParentFrameId;
        return f.SkillId == MouXunYuAnniversaryRuntime.Bizuo && w.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack[at - 1] is DeferredTurnEndFrame p2 && p2.Id == w.ParentFrameId && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(p2) && w.SourceSeat == p2.OwnerSeat && w.TargetSeat == p2.OwnerSeat && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p2)is { } c && c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.BindingId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash && (f.TriggerId is not ("allocate-own" or "allocate-otherLiving") || IsIssuedMouXyAllocationCandidate(c, w));
    }

    bool IMouXunYuAnniversaryFacts.SameChoice(PromptChoice a, PromptChoice b) => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([a], [b]);
    bool IMouXunYuAnniversaryFacts.ExtraTurnStarted(TurnStartedEvent started, MouXyBizuoGrant g) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == g.Beneficiary && started.ActorSeat == g.Beneficiary && started.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && g.IssuedTurn < started.TurnNumber && ((IMouXunYuAnniversaryFacts)this).History<MouXyBizuoGrantedEvent>().Count(x => x.Grant == g) == 1 && ((IMouXunYuAnniversaryFacts)this).History<ProgramExtraTurnPendedEvent>().LastOrDefault()is { } last && last.FrameId == g.FrameId && last.SkillId == g.Source.SkillId && last.Seat == g.Beneficiary && g.GameplayHash == ((IMouXunYuAnniversaryFacts)this).Program(g.Source.SkillId)?.GameplayHash && !((IMouXunYuAnniversaryFacts)this).History<MouXyBizuoStartedEvent>().Any(x => x.Grant.FrameId == g.FrameId);
    bool IMouXunYuAnniversaryFacts.NonDiscardEntry(CardMovedEvent move, out int cardId, out long sequence)
    {
        cardId = 0;
        sequence = 0;
        if (move.To != CardLocation.DiscardPile)
            return false;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == move.CardId && m.From == move.From && m.To == move.To && m.Reason == move.Reason);
        if (actual is null || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(actual)is not null)
            return false;
        cardId = actual.CardId;
        sequence = actual.Sequence;
        return true;
    }

    MouXyBizuoGrant? IMouXunYuAnniversaryFacts.PendingLease => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.MouXyBizuoPending;

    MouXyBizuoGrant? IMouXunYuAnniversaryFacts.AllocationLease(ProgramSkillFrame f) => MouXyAllocationLease(f);
    IReadOnlyList<MouXyAllocationCard> IMouXunYuAnniversaryFacts.AllocationCards(ProgramSkillFrame f) => MouXyAllocationCards(f);
    bool IMouXunYuAnniversaryFacts.AllocationDue(int owner) => MouXyAllocationsDue().Any(x => x.Grant.Source.OwnerSeat == owner);
    void IMouXunYuAnniversaryNative.State(ProgramSkillFrame f, bool yin)
    {
        if (!MouXyAnniversary.Valid(f))
            throw new InvalidOperationException("Shimou state requires its own real instruction.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, MouXunYuAnniversaryRuntime.Shimou, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], MouXunYuAnniversaryRuntime.Shimou), "yin", yin);
    }

    void IMouXunYuAnniversaryNative.Discard(ProgramSkillFrame f)
    {
        if (!MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.DiscardChildren } r || r.Selected.Count != r.Requested || r.Selected.Any(id => !_characterXunYuMouOperations.CreateMouXunYuAnniversaryFacts().Hand(r.Actor).Any(c => c.Id == id)))
            throw new InvalidOperationException("Shimou requires the actual actor's selected discardable Hand.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Actor, r.Selected, new("skill-program.mou:shimou.discard"), (_, moves) => MouXyAnniversary.DiscardPaid(f.Id, moves));
    }

    void IMouXunYuAnniversaryNative.Use(ProgramSkillFrame f) => IssueMouXyTrick(f);
    void IMouXunYuAnniversaryNative.Pend(ProgramSkillFrame f, MouXyBizuoGrant grant)
    {
        if (!MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.Beneficiary } || grant.FrameId != f.Id || grant.Source.OwnerSeat != f.OwnerSeat || grant.GameplayHash != f.GameplayHash || !_coreMatchParticipantsPlayersAccess._players[grant.Beneficiary].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXyBizuoGrantedEvent>().Count(x => x.Grant == grant) != 1)
            throw new InvalidOperationException("Bizuo requires its own issued limited extra-turn source.");
        _coreSkillProgramsOperations.PendProgramExtraTurn(f, grant.Beneficiary);
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression with
        {
            MouXyBizuoPending = grant
        };
    }

    void IMouXunYuAnniversaryNative.Suppress(MouXyBizuoGrant grant, int actualTurn, IReadOnlyList<int> targets)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != grant.Beneficiary || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber != actualTurn || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXyBizuoStartedEvent>().Count(s => s.Grant == grant && s.ActualTurn == actualTurn) != 1 || targets.Any(s => s == grant.Source.OwnerSeat || s == grant.Beneficiary || !_coreMatchParticipantsPlayersAccess._players[s].IsAlive))
            throw new InvalidOperationException("Bizuo suppression requires the exact claimed native extra actual turn.");
        foreach (var target in targets)
        {
            var fact = new CurrentTurnNonLockedSkillSuppression(actualTurn, grant.Beneficiary, grant.FrameId, 0, new(grant.Source.SkillId, grant.Source.BindingId, grant.Source.OwnerSeat, grant.Source.SkillInstanceId), target);
            if (_coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.Contains(fact))
                throw new InvalidOperationException("Bizuo cannot issue the same suppression twice.");
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressions.Add(fact);
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess._currentTurnSkillSuppressionRevision++;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnNonLockedSkillSuppressionIssuedEvent(fact));
        }
    }

    void IMouXunYuAnniversaryNative.Allocate(ProgramSkillFrame f)
    {
        if (!MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.AllocationChildren, AllocationCard: { } card } r || !_coreMatchParticipantsPlayersAccess._players[r.AllocationRecipient].IsAlive || !MouXyAllocationCards(f).Contains(card))
            throw new InvalidOperationException("Bizuo requires the original extra-turn entry still in its allowed current region and a living recipient.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(r.AllocationRecipient), new("skill-program.mou:bizuo.allocate"), (_, moves) => MouXyAnniversary.AllocationPaid(f.Id, moves), disclosure: CardMovementDisclosure.FaceUp);
    }

    bool IMouXunYuAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouXunYuAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouXunYuAnniversaryNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    internal bool MouXyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouXunYuAnniversary is { Stage: MouXyStage.DiscardChildren or MouXyStage.DrawChildren or MouXyStage.AllocationChildren } r && MouXyAnniversary.Valid(f) && effect?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == (r.Stage == MouXyStage.AllocationChildren ? r.AllocationRecipient : r.Actor) && p.BeforeCount == 0 && p.CoverageResultBind is null;
}

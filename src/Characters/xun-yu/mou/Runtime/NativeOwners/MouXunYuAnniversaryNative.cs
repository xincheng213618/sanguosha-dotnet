#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXunYuAnniversaryNative(Func<RuntimeEntries.CharacterXunYuMouEntries> getCharacterXunYuMouRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, MouXunYuAnniversaryFactsFactory createMouXunYuAnniversaryFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreGameEngineCurrentTurnSkillSuppressionsState gameEngineCurrentTurnSkillSuppressionsState, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouXunYuAnniversaryNative
{
    public void State(ProgramSkillFrame f, bool yin)
    {
        if (!getCharacterXunYuMouRuntime().MouXyAnniversary.Valid(f))
            throw new InvalidOperationException("Shimou state requires its own real instruction.");
        getCoreGameEngineProgramPersistentStateRuntime().SetProgramBooleanState(f.OwnerSeat, MouXunYuAnniversaryRuntime.Shimou, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], MouXunYuAnniversaryRuntime.Shimou), "yin", yin);
    }

    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterXunYuMouRuntime().MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.DiscardChildren } r || r.Selected.Count != r.Requested || r.Selected.Any(id => !createMouXunYuAnniversaryFacts().Hand(r.Actor).Any(c => c.Id == id)))
            throw new InvalidOperationException("Shimou requires the actual actor's selected discardable Hand.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Actor, r.Selected, new("skill-program.mou:shimou.discard"), (_, moves) => getCharacterXunYuMouRuntime().MouXyAnniversary.DiscardPaid(f.Id, moves));
    }

    public void Use(ProgramSkillFrame f) => getCharacterXunYuMouRuntime().IssueMouXyTrick(f);
    public void Allocate(ProgramSkillFrame f)
    {
        if (!getCharacterXunYuMouRuntime().MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.AllocationChildren, AllocationCard: { } card } r || !matchParticipantsState.ReferenceToPlayers[r.AllocationRecipient].IsAlive || !getCharacterXunYuMouRuntime().MouXyAllocationCards(f).Contains(card))
            throw new InvalidOperationException("Bizuo requires the original extra-turn entry still in its allowed current region and a living recipient.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(r.AllocationRecipient), new("skill-program.mou:bizuo.allocate"), (_, moves) => getCharacterXunYuMouRuntime().MouXyAnniversary.AllocationPaid(f.Id, moves), disclosure: CardMovementDisclosure.FaceUp);
    }

    public void Pend(ProgramSkillFrame f, MouXyBizuoGrant grant)
    {
        if (!getCharacterXunYuMouRuntime().MouXyAnniversary.Valid(f) || f.MouXunYuAnniversary is not { Stage: MouXyStage.Beneficiary } || grant.FrameId != f.Id || grant.Source.OwnerSeat != f.OwnerSeat || grant.GameplayHash != f.GameplayHash || !matchParticipantsState.ReferenceToPlayers[grant.Beneficiary].IsAlive || getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<MouXyBizuoGrantedEvent>().Count(x => x.Grant == grant) != 1)
            throw new InvalidOperationException("Bizuo requires its own issued limited extra-turn source.");
        getCoreGameEngineSkillProgramsRuntime().PendProgramExtraTurn(f, grant.Beneficiary);
        actualTurnProgressState.ReferenceToTurnProgression = actualTurnProgressState.ReferenceToTurnProgression with
        {
            MouXyBizuoPending = grant
        };
    }

    public void Suppress(MouXyBizuoGrant grant, int actualTurn, IReadOnlyList<int> targets)
    {
        if (actualTurnProgressState.ReferenceToTurnProgression.Kind != ActualTurnKind.Extra || actualTurnProgressState.ReferenceToCurrentSeat != grant.Beneficiary || actualTurnProgressState.ReferenceToTurnNumber != actualTurn || getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<MouXyBizuoStartedEvent>().Count(s => s.Grant == grant && s.ActualTurn == actualTurn) != 1 || targets.Any(s => s == grant.Source.OwnerSeat || s == grant.Beneficiary || !matchParticipantsState.ReferenceToPlayers[s].IsAlive))
            throw new InvalidOperationException("Bizuo suppression requires the exact claimed native extra actual turn.");
        foreach (var target in targets)
        {
            var fact = new CurrentTurnNonLockedSkillSuppression(actualTurn, grant.Beneficiary, grant.FrameId, 0, new(grant.Source.SkillId, grant.Source.BindingId, grant.Source.OwnerSeat, grant.Source.SkillInstanceId), target);
            if (gameEngineCurrentTurnSkillSuppressionsState.ReferenceToCurrentTurnSkillSuppressions.Contains(fact))
                throw new InvalidOperationException("Bizuo cannot issue the same suppression twice.");
            gameEngineCurrentTurnSkillSuppressionsState.ReferenceToCurrentTurnSkillSuppressions.Add(fact);
            gameEngineCurrentTurnSkillSuppressionsState.ReferenceToCurrentTurnSkillSuppressionRevision++;
            advanceEventRulesAndQueueFact(new CurrentTurnNonLockedSkillSuppressionIssuedEvent(fact));
        }
    }

    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
}

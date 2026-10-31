#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouJuShouNative(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, Func<RuntimeEntries.CharacterJuShouMouEntries> getCharacterJuShouMouRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, MouJuShouFactsFactory createMouJuShouFacts, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime) : IMouJuShouNative
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, f.MouJuShou?.Stage == MouJuShouStage.HealthChildren ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public SkillProgramStepOutcome LoseHp(ProgramSkillFrame f) => getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(f.Id, f.SkillId, f.MouJuShou!.Actor, 1);
    public void Use(ProgramSkillFrame f) => getCharacterJuShouMouRuntime().IssueMouJsUse(f);
    public void SetGrants(int seat, IReadOnlyList<MouJsZuoJunGrant> grants) => matchParticipantsState.ReferenceToPlayers[seat].MouJsZuoJunGrants = Array.AsReadOnly(grants.ToArray());
    public void SetLosses(int seat, IReadOnlyList<MouJsFirstLoss> losses) => matchParticipantsState.ReferenceToPlayers[seat].MouJsFirstLosses = Array.AsReadOnly(losses.ToArray());
    public void Discard(ProgramSkillFrame f)
    {
        if (!getCharacterJuShouMouRuntime().MouJuShou.Valid(f) || f.MouJuShou is not { Stage: MouJuShouStage.DiscardChildren or MouJuShouStage.PenaltyChildren } r || r.Discard.Any(id => !createMouJuShouFacts().Discardable(r.Actor).Contains(id)))
            throw new InvalidOperationException("Mou Ju Shou requires actual payable bound leftovers or a selected penalty entity.");
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveProgramLogicalDiscard(f, r.Actor, r.Discard, new("skill-program." + f.SkillId + ".discard"), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            var paid = parent.MouJuShou!;
            if (!getCharacterJuShouMouRuntime().MouJuShou.Valid(parent) || moves.Count != r.Discard.Count || moves.Any(m => !r.Discard.Contains(m.CardId) || m.From.OwnerSeat != r.Actor || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Ju Shou lost his actual native logical-discard receipt.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJuShou = paid with { After = moves.Count > 0 ? moves.Max(m => m.Sequence) : r.Before } });
            foreach (var m in moves)
                advanceEventRulesAndQueueFact(new MouJsMovedEvent(f.Id, m.CardId, m.From, m.To, m.Sequence));
        });
    }

    public void Recover(ProgramSkillFrame f)
    {
        if (!getCharacterJuShouMouRuntime().MouJuShou.Valid(f) || f.MouJuShou is not { Stage: MouJuShouStage.RecoverChildren, Recover: true, CardId: > 0 } r || createMouJuShouFacts().Losses(r.Actor).SingleOrDefault(l => l.Source == r.Source && l.Turn == r.Turn && l.CardId == r.CardId && l.FirstSequence == r.LossSequence)is not { } loss || !createMouJuShouFacts().ReadyToReclaim(loss))
            throw new InvalidOperationException("Muwang requires the original tracked entity actually in Discard.");
        getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([r.CardId], CardLocation.Hand(r.Actor), new("skill-program.mou:muwang.return"), (_, moves) =>
        {
            var parent = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(f.Id);
            if (moves is not [var m] || m.CardId != r.CardId || m.From != CardLocation.DiscardPile || m.Sequence <= r.Before || m.To != CardLocation.Hand(r.Actor) && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Muwang cannot invent a returned entity.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent with { MouJuShou = parent.MouJuShou!with { After = m.Sequence } });
            if (m.To == CardLocation.Hand(r.Actor))
                getCharacterJuShouMouRuntime().MouJuShou.Recovered(f.Id, m);
            advanceEventRulesAndQueueFact(new MouJsMovedEvent(f.Id, m.CardId, m.From, m.To, m.Sequence));
        });
    }
}

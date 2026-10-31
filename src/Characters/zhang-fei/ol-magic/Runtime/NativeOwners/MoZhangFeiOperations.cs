#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MoZhangFeiOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries> getCoreGameEngineCurrentTurnSkillSuppressionsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::System.Collections.Generic.Dictionary<int, long>> getDictionary, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSharedGameEngineOlMoRumoEntries> getCharacterSharedGameEngineOlMoRumoRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime) : IMoZhangFeiOperations
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Recover(ProgramSkillFrame f, int amount) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, amount, null, null);
    public void LoseHp(ProgramSkillFrame f, int seat, int amount) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, seat, amount);
    public void Suppress(ProgramSkillFrame f, int seat) => getCoreGameEngineCurrentTurnSkillSuppressionsRuntime().IssueCurrentTurnNonLockedSkillSuppression(f, seat);
    public void ObserveSkillRevisions()
    {
        foreach (var player in matchParticipantsState.ReferenceToPlayers)
            getDictionary().TryAdd(player.Seat, player.SkillGrants.Revision);
    }

    public bool RemoveSkillGrant(int owner, string id) => matchParticipantsState.ReferenceToPlayers[owner].SkillGrants.RemoveGrant(id);
    public void GrantModifiedZhuohun(ProgramSkillFrame f) => getCoreGameEngineProgramSkillChangesRuntime().GrantProgramSkills(f, [MoZhangFeiRuntime.ModifiedZhuohun]);
    public void EnterRumo(ProgramSkillFrame f) => getCharacterSharedGameEngineOlMoRumoRuntime().EnterOlMoRumo(f);
    public bool BeginRecovery(long id) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    public bool BeginCharacterState(long id) => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    public bool BeginHpChanged(long id) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    public bool BeginCardsMoved(long id) => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(id);
    public bool BeginSkillsChanged(long id) => getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(id);
    public void GainSlash(ProgramSkillFrame f, Card card, int recipientSeat) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(recipientSeat), new(MoZhangFeiRuntime.GreenReason), m => advanceEventRulesAndQueueFact(new MoZhangFeiGreenMaterialEvent(f.Id, f.MoZhangFei!.DrawAttemptId!.Value, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
    public void CompleteDrawPileReplacement(ProgramSkillFrame f, IReadOnlyList<int> ids, CardMoveReason reason, long before) => getCoreGameEngineNativeDrawAttemptsRuntime().NativeDrawAttempts.CompleteDrawPileSlashReplacement(f, ids, reason, before);
}

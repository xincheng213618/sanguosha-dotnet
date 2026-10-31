#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXiaoQiaoOlOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::CardGame.Core.ProgramCapabilityRegistry> getProgramCapabilityRegistry, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.NativeOwners.ProgramSkillHostFactory createProgramSkillHost, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.NativeOwners.GetNativeTryResumeTarget getReturnRevealWindowTarget, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeFireAttackEntries> getCoreNativeFireAttackRuntime, global::CardGame.Core.NativeOwners.GetNativeReturnRevealWindowTarget getReturnRevealWindowTarget2, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : IMouXiaoQiaoOlOperations
{
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Reveal(ProgramSkillFrame f, IReadOnlyList<int> ids) => advanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, "mou-xiao-qiao-public-hand", Array.AsReadOnly(ids.Select(id => getProgramCapabilityRegistry().Get<IHandCardViewQueries>().Project(matchParticipantsState.ReferenceToPlayers[f.OwnerSeat], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id))).ToArray())));
    public bool BeginRevealChildren(long id) => getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow(id, PublicHandRevealReturnKind.Program);
    public void Gift(ProgramSkillFrame f, int id, int target) => getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), getCardZoneStore().GetLocation(id), CardLocation.Hand(target), new(MouXiaoQiaoOlRuntime.GiveReason), getCharacterHuangQuanMouRuntime().NativeGiveCompletion(f.Id, f.OwnerSeat, target, [id], cardMovementJournalState.ReferenceToMovementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
    public void Recover(ProgramSkillFrame f, int target) => createProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
    public void LoseHp(ProgramSkillFrame f, int target) => createProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1);
    public bool BeginRecovery(long id) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    public bool BeginCharacterState(long id) => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    public bool BeginHpChanged(long id) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    public bool BeginCardsMoved(long id) => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(id);
    public bool BeginSkillsChanged(long id) => getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(id);
    public void IssueNativeUse(ProgramSkillFrame f, MouXiaoQiaoOlUsePlan p) => getCharacterXiaoQiaoMouOlRuntime().IssueMouXqNativeUse(f, p);
    public void SignNativeBatches(ProgramSkillFrame f, CardUseFrame use) => getCharacterXiaoQiaoMouOlRuntime().SignMouXqNativeBatches(f, use);
    public bool BeginResponse(PromptChoice choice) => getCharacterXiaoQiaoMouOlRuntime().BeginMouXqJueyanResponse(choice);
    public void ReturnResponse(ProgramSkillFrame f) => getCharacterXiaoQiaoMouOlRuntime().ReturnMouXqJueyanResponse(f);
    public void BeginRevealTrigger(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => getCoreGameEngineProgramLifecycleRuntime().BeginProgramBinding(c, context);
    public void ReturnRevealWindow(PublicHandRevealWindowFrame w)
    {
        if (w.ReturnKind == PublicHandRevealReturnKind.Program)
        {
            if (!getReturnRevealWindowTarget()(w.ParentFrameId))
                getCoreGameEngineRuntimeRuntime().AdvanceRuntimeProgram(w.ParentFrameId);
        }
        else if (w.ReturnKind == PublicHandRevealReturnKind.FireAttack)
        {
            var use = getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(w.ParentFrameId) ?? throw new InvalidOperationException("Public reveal lost its actual FireAttack.");
            var id = use.FireAttackSelection?.RevealedCardId ?? throw new InvalidOperationException("FireAttack cannot replace its frozen revealed entity.");
            getCoreNativeFireAttackRuntime().BeginFireAttackDiscard(use, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id));
        }
        else if (w.ReturnKind == PublicHandRevealReturnKind.Registered)
            getReturnRevealWindowTarget2()(w.ReturnKey!, getFrameStore().Single(x => x.Id == w.ParentFrameId));
        else
            getCoreGameEngineRuntimeRuntime().AdvanceRuntimeFrame(w.ParentFrameId);
    }
}

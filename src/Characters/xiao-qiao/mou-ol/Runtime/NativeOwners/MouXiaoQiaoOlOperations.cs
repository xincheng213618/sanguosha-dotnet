#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouXiaoQiaoOlOperations(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<ProgramCapabilityRegistry> getProgramCapabilityRegistry, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, ProgramSkillHostFactory createProgramSkillHost, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, GetNativeTryResumeTarget getReturnRevealWindowTarget, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<RuntimeEntries.CoreNativeFireAttackEntries> getCoreNativeFireAttackRuntime, GetNativeReturnRevealWindowTarget getReturnRevealWindowTarget2, Func<FrameStore> getFrameStore) : IMouXiaoQiaoOlOperations
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

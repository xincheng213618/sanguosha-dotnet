#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouKongRongNative(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterKongRongMouOlRuntime>> getCharacterKongRongMouOlRuntime, Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, Func<RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries> getCoreGameEngineDamagePreventionProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouKongRongNative
{
    public int ChangeXian(ProgramSkillFrame f, int seat, int delta)
    {
        var old = matchParticipantsState.ReferenceToPlayers[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MouKongRongXian);
        var next = Math.Clamp(old + delta, 0, 5);
        if (old != next)
            getCoreGameEngineStrategicProgramsRuntime().SetStrategicMarker(seat, PlayerMarkerKind.MouKongRongXian, seat, next, f);
        return next - old;
    }

    public void IssueUse(ProgramSkillFrame f, int actor, LegalAction a, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => getCharacterKongRongMouOlRuntime().Value.IssueMouKrUse(f, actor, a, cards, targets);
    public SkillProgramStepOutcome LoseHp(ProgramSkillFrame f, int seat, int amount) => getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(f.Id, f.SkillId, seat, amount);
    public void PreventDamage(ProgramSkillFrame f) => getCoreGameEngineDamagePreventionProgramsRuntime().PreventProgramCurrentDamage(f);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
}

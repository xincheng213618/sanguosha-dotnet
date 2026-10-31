#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouKongRongNative(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterKongRongMouOlEntries> getCharacterKongRongMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries> getCoreGameEngineDamagePreventionProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState) : IMouKongRongNative
{
    public int ChangeXian(ProgramSkillFrame f, int seat, int delta)
    {
        var old = matchParticipantsState.ReferenceToPlayers[seat].Markers.GetValueOrDefault(PlayerMarkerKind.MouKongRongXian);
        var next = Math.Clamp(old + delta, 0, 5);
        if (old != next)
            getCoreGameEngineStrategicProgramsRuntime().SetStrategicMarker(seat, PlayerMarkerKind.MouKongRongXian, seat, next, f);
        return next - old;
    }

    public void IssueUse(ProgramSkillFrame f, int actor, LegalAction a, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => getCharacterKongRongMouOlRuntime().IssueMouKrUse(f, actor, a, cards, targets);
    public SkillProgramStepOutcome LoseHp(ProgramSkillFrame f, int seat, int amount) => getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(f.Id, f.SkillId, seat, amount);
    public void PreventDamage(ProgramSkillFrame f) => getCoreGameEngineDamagePreventionProgramsRuntime().PreventProgramCurrentDamage(f);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public void Publish() => advanceRulesAndPublishState();
}

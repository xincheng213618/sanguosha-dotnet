#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseNativeReturns(Func<RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime, Func<RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries> getCoreGameEngineGrantedEntityPhasesRuntime, Func<RuntimeEntries.CharacterZhangYiOlEntries> getCharacterZhangYiOlRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries> getCoreGameEnginePlayPhaseSkillGrantsRuntime, Func<RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries> getCoreGameEngineActualTurnSkillGrantsRuntime) : IProgramPhaseNativeReturns
{
    public bool TryResumeZhangShiPingHongjiPhase() => getCharacterZhangShiPingOlRuntime().TryResumeZhangShiPingHongjiPhase();
    public bool TryCancelZhangShiPingHongjiPhase() => getCharacterZhangShiPingOlRuntime().TryCancelZhangShiPingHongjiPhase();
    public ProgramSkillFrame FreezeGrantedEntityPhaseEnd(ProgramSkillFrame frame) => getCoreGameEngineGrantedEntityPhasesRuntime().FreezeGrantedEntityPhaseEnd(frame);
    public void ResumeZhangYiDianjunExtraPlay(long frameId) => getCharacterZhangYiOlRuntime().ResumeZhangYiDianjunExtraPlay(frameId);
    public void CompleteDetachedProgramBinding(ProgramSkillFrame frame, bool completed) => getCoreGameEngineProgramLifecycleRuntime().CompleteDetachedProgramBinding(frame, completed);
    public void ExpireActualTurnEndEffects(int seat) => getCoreActualTurnsRuntime().ExpireActualTurnEndEffects(matchParticipantsState.ReferenceToPlayers[seat]);
    public void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => getCoreGameEnginePlayPhaseSkillGrantsRuntime().ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
    public void ExpireActualTurnSkillGrants(int turnNumber, int seat) => getCoreGameEngineActualTurnSkillGrantsRuntime().ExpireActualTurnSkillGrants(turnNumber, seat);
}

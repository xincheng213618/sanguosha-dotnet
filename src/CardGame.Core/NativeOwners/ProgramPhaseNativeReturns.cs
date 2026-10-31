#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseNativeReturns(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries> getCoreGameEngineGrantedEntityPhasesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangYiOlEntries> getCharacterZhangYiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries> getCoreGameEnginePlayPhaseSkillGrantsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries> getCoreGameEngineActualTurnSkillGrantsRuntime) : IProgramPhaseNativeReturns
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

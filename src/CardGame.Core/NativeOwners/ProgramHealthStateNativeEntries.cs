#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthStateNativeEntries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryFramesEntries> getCoreNativeRecoveryFramesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries> getCoreGameEngineCharacterTurnedOverProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IProgramHealthStateNativeEntries
{
    public bool TryHandleHappyQiaoRecoverTo(long frameId, int sourceSeat, int targetSeat, int targetHp) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().TryHandleHappyQiaoRecoverTo(frameId, sourceSeat, targetSeat, targetHp);
    public bool TryQueueRecoveryReplacement(long frameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion) => getCoreRecoveryReplacementRuntime().TryQueueRecoveryReplacement(frameId, sourceSeat, targetSeat, amount, completion);
    public long BeginRecovery(long frameId, int sourceSeat, int targetSeat, int amount) => getCoreNativeRecoveryFramesRuntime().BeginRecovery(frameId, sourceSeat, targetSeat, amount);
    public bool TryApplyHappyQiaoMaximumHp(long frameId, int targetSeat, int maximumHp, string sourceSkillId) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().TryApplyHappyQiaoMaximumHp(frameId, targetSeat, maximumHp, sourceSkillId);
    public void RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window) => getCoreGameEngineCharacterStateProgramsRuntime().RecordCharacterStateChange(targetSeat, window);
    public void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => getCoreGameEngineCharacterTurnedOverProgramsRuntime().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
    public void AddLog(string category, string message, int ownerSeat, int targetSeat) => getCoreMatchLogRuntime().AddLog(category, message, ownerSeat, targetSeat);
}

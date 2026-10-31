#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthStateNativeEntries(Func<RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreNativeRecoveryFramesEntries> getCoreNativeRecoveryFramesRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries> getCoreGameEngineCharacterTurnedOverProgramsRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IProgramHealthStateNativeEntries
{
    public bool TryHandleHappyQiaoRecoverTo(long frameId, int sourceSeat, int targetSeat, int targetHp) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().TryHandleHappyQiaoRecoverTo(frameId, sourceSeat, targetSeat, targetHp);
    public bool TryQueueRecoveryReplacement(long frameId, int sourceSeat, int targetSeat, int amount, RecoveryAttemptCompletion completion) => getCoreRecoveryReplacementRuntime().TryQueueRecoveryReplacement(frameId, sourceSeat, targetSeat, amount, completion);
    public long BeginRecovery(long frameId, int sourceSeat, int targetSeat, int amount) => getCoreNativeRecoveryFramesRuntime().BeginRecovery(frameId, sourceSeat, targetSeat, amount);
    public bool TryApplyHappyQiaoMaximumHp(long frameId, int targetSeat, int maximumHp, string sourceSkillId) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().TryApplyHappyQiaoMaximumHp(frameId, targetSeat, maximumHp, sourceSkillId);
    public void RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window) => getCoreGameEngineCharacterStateProgramsRuntime().RecordCharacterStateChange(targetSeat, window);
    public void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => getCoreGameEngineCharacterTurnedOverProgramsRuntime().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
    public void AddLog(string category, string message, int ownerSeat, int targetSeat) => getCoreMatchLogRuntime().AddLog(category, message, ownerSeat, targetSeat);
}

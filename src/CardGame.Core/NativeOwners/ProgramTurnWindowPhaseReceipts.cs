#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowPhaseReceipts(Func<RuntimeEntries.CharacterWangKuangOlEntries> getCharacterWangKuangOlRuntime, Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, Func<RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime, Func<RuntimeEntries.CoreGameEngineZhuijiProgramsEntries> getCoreGameEngineZhuijiProgramsRuntime, Func<RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState) : IProgramTurnWindowPhaseReceipts
{
    public WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => getCharacterWangKuangOlRuntime().CaptureWangKuangRenxiaEndingPhase(owner);
    public WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner) => getCharacterLvQianOlRuntime().CaptureWeiluEndingPhase(owner);
    public QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().CaptureQingzhongEndingPhase(owner);
    public ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner) => getCoreGameEngineZhuijiProgramsRuntime().CaptureZhuijiEndingPhase(owner);
    public bool TracksPhaseNamePredictions => getCoreGameEnginePhaseNamePredictionRuntime().TracksPhaseNamePredictions;
    public int PhaseInstanceId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
}

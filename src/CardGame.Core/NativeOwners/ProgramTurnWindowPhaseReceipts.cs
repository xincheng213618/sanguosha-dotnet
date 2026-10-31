#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowPhaseReceipts(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangKuangOlEntries> getCharacterWangKuangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries> getCharacterLuZhiWeiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries> getCoreGameEngineZhuijiProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime, global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState) : IProgramTurnWindowPhaseReceipts
{
    public WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => getCharacterWangKuangOlRuntime().CaptureWangKuangRenxiaEndingPhase(owner);
    public WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner) => getCharacterLvQianOlRuntime().CaptureWeiluEndingPhase(owner);
    public QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().CaptureQingzhongEndingPhase(owner);
    public ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner) => getCoreGameEngineZhuijiProgramsRuntime().CaptureZhuijiEndingPhase(owner);
    public bool TracksPhaseNamePredictions => getCoreGameEnginePhaseNamePredictionRuntime().TracksPhaseNamePredictions;
    public int PhaseInstanceId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
}

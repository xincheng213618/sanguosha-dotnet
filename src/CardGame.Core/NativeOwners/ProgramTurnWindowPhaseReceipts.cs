#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramTurnWindowPhaseReceipts(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWangKuangOlRuntime>> getCharacterWangKuangOlRuntime, Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuZhiWeiOlRuntime>> getCharacterLuZhiWeiOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineZhuijiProgramsRuntime>> getCoreGameEngineZhuijiProgramsRuntime, Func<RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries> getCoreGameEnginePhaseNamePredictionRuntime, EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState) : IProgramTurnWindowPhaseReceipts
{
    public WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => getCharacterWangKuangOlRuntime().Value.CaptureWangKuangRenxiaEndingPhase(owner);
    public WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner) => getCharacterLvQianOlRuntime().CaptureWeiluEndingPhase(owner);
    public QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner) => getCharacterLuZhiWeiOlRuntime().Value.CaptureQingzhongEndingPhase(owner);
    public ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner) => getCoreGameEngineZhuijiProgramsRuntime().Value.CaptureZhuijiEndingPhase(owner);
    public bool TracksPhaseNamePredictions => getCoreGameEnginePhaseNamePredictionRuntime().TracksPhaseNamePredictions;
    public int PhaseInstanceId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
}

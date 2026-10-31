#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseBoundaryProtocols(Func<RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries> getCoreGameEngineEarnedActualEndingBenefitsRuntime, Func<RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries> getCoreGameEnginePrepDiscardEndingPromisesRuntime, Func<RuntimeEntries.CoreGameEngineConvertingGiftsEntries> getCoreGameEngineConvertingGiftsRuntime, Func<RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries> getCoreGameEngineFixedDistanceOneDebtRuntime, Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, Func<RuntimeEntries.CharacterWenYangOlEntries> getCharacterWenYangOlRuntime, Func<RuntimeEntries.CharacterLiuBaOlEntries> getCharacterLiuBaOlRuntime, Func<RuntimeEntries.CharacterRuanYuOlEntries> getCharacterRuanYuOlRuntime, Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime, Func<RuntimeEntries.CharacterLiWanOlEntries> getCharacterLiWanOlRuntime, Func<ProgramPipelineHooks> getProgramPipelineHooks, KernelCompleteGameCall completeGame, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime) : IProgramPhaseBoundaryProtocols
{
    public void AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().AppendEarnedActualEndingItems(items);
    public void AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().AppendPrepDiscardEndingItems(items);
    public void AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineConvertingGiftsRuntime().AppendGiftRetentionEndingItems(items);
    public void AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineFixedDistanceOneDebtRuntime().AppendFixedDistanceOneEndingItems(items);
    public void AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterYanPuOlRuntime().AppendYanPuEndingItems(items);
    public void AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterWenYangOlRuntime().AppendWenYangEndingItems(items);
    public void AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterLiuBaOlRuntime().AppendLiuBaCoinEndingItems(items);
    public void AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterRuanYuOlRuntime().AppendRuanYuXingzuoEndingItems(items);
    public void AppendQushiEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterGuoTuOlRuntime().AppendQushiEndingItems(items);
    public void AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterLiWanOlRuntime().AppendLianjuEndingItems(items);
    public void AppendPipelineEndingItems(List<TurnEndingBoundaryItem> items) => getProgramPipelineHooks().AppendTurnEndingItems(items);
    public bool ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item) => getCoreGameEngineConvertingGiftsRuntime().ConsumeGiftRetentionEndingItem(frame, item);
    public void ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().ConsumeSkippedEarnedActualEnding(frame);
    public void ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().ConsumeSkippedPrepDiscardEnding(frame);
    public void ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame frame) => getCoreGameEngineFixedDistanceOneDebtRuntime().ConsumeSkippedFixedDistanceDebt(frame);
    public void CompleteGame() => completeGame();
    public void PublishState() => advanceRulesAndPublishState();
    public void FinalizeEndTurn(CharacterState owner) => getCoreActualTurnsRuntime().FinalizeEndTurn(owner);
}

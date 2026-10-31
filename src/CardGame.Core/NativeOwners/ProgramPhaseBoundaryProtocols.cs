#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseBoundaryProtocols(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime>> getCoreGameEngineEarnedActualEndingBenefitsRuntime, Func<RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries> getCoreGameEnginePrepDiscardEndingPromisesRuntime, Func<RuntimeEntries.CoreGameEngineConvertingGiftsEntries> getCoreGameEngineConvertingGiftsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime>> getCoreGameEngineFixedDistanceOneDebtRuntime, Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWenYangOlRuntime>> getCharacterWenYangOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuBaOlRuntime>> getCharacterLiuBaOlRuntime, Func<RuntimeEntries.CharacterRuanYuOlEntries> getCharacterRuanYuOlRuntime, Func<RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiWanOlRuntime>> getCharacterLiWanOlRuntime, Func<ProgramPipelineHooks> getProgramPipelineHooks, KernelCompleteGameCall completeGame, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime) : IProgramPhaseBoundaryProtocols
{
    public void AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().Value.AppendEarnedActualEndingItems(items);
    public void AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().AppendPrepDiscardEndingItems(items);
    public void AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineConvertingGiftsRuntime().AppendGiftRetentionEndingItems(items);
    public void AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.AppendFixedDistanceOneEndingItems(items);
    public void AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterYanPuOlRuntime().AppendYanPuEndingItems(items);
    public void AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterWenYangOlRuntime().Value.AppendWenYangEndingItems(items);
    public void AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterLiuBaOlRuntime().Value.AppendLiuBaCoinEndingItems(items);
    public void AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterRuanYuOlRuntime().AppendRuanYuXingzuoEndingItems(items);
    public void AppendQushiEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterGuoTuOlRuntime().AppendQushiEndingItems(items);
    public void AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items) => getCharacterLiWanOlRuntime().Value.AppendLianjuEndingItems(items);
    public void AppendPipelineEndingItems(List<TurnEndingBoundaryItem> items) => getProgramPipelineHooks().AppendTurnEndingItems(items);
    public bool ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item) => getCoreGameEngineConvertingGiftsRuntime().ConsumeGiftRetentionEndingItem(frame, item);
    public void ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().Value.ConsumeSkippedEarnedActualEnding(frame);
    public void ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame) => getCoreGameEnginePrepDiscardEndingPromisesRuntime().ConsumeSkippedPrepDiscardEnding(frame);
    public void ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame frame) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.ConsumeSkippedFixedDistanceDebt(frame);
    public void CompleteGame() => completeGame();
    public void PublishState() => advanceRulesAndPublishState();
    public void FinalizeEndTurn(CharacterState owner) => getCoreActualTurnsRuntime().FinalizeEndTurn(owner);
}

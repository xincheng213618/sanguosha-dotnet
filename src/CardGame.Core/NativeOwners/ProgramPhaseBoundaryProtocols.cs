#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPhaseBoundaryProtocols(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries> getCoreGameEngineEarnedActualEndingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries> getCoreGameEnginePrepDiscardEndingPromisesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries> getCoreGameEngineConvertingGiftsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries> getCoreGameEngineFixedDistanceOneDebtRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries> getCharacterWenYangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries> getCharacterLiuBaOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries> getCharacterRuanYuOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiWanOlEntries> getCharacterLiWanOlRuntime, global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks, global::CardGame.Core.NativeOwners.KernelCompleteGameCall completeGame, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime) : IProgramPhaseBoundaryProtocols
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

#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseBoundaryRuntimePortsRuntime
{
    Winner IProgramPhaseBoundaryFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus IProgramPhaseBoundaryFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int IProgramPhaseBoundaryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    int IProgramPhaseBoundaryFacts.PreviousLivingSeatFor(int owner) => _coreNeighborDiscardOpportunitiesOperations.PreviousLivingSeatFor(owner);
    bool IProgramPhaseBoundaryFacts.HasWenheDeadOwnerEnding(int seat) => _coreWenheActivityProgramsOperations.HasWenheDeadOwnerEnding(seat);
    bool IProgramPhaseBoundaryFacts.HasAnniversaryTianjieDeadOwnerEnding(int seat) => _characterZhangJiaoAnniversaryGodOperations.HasAnniversaryTianjieDeadOwnerEnding(seat);
    bool IProgramPhaseBoundaryFacts.IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => _coreWenheActivityProgramsOperations.IsWenheDeadOwnerEndingCandidate(candidate, seat);
    bool IProgramPhaseBoundaryFacts.IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => _characterZhangJiaoAnniversaryGodOperations.IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, seat);
    bool IProgramPhaseBoundaryFacts.UsesTreatedWounded => _coreTreatedWoundedOperations.UsesTreatedWounded;

    bool IProgramPhaseBoundaryFacts.IsRuleWounded(CharacterState player) => _coreTreatedWoundedOperations.IsRuleWounded(player);
    QingzhongPlayPhaseKey? IProgramPhaseBoundaryFacts.CaptureQingzhongStartingPhase(CharacterState owner) => _characterLuZhiWeiOlOperations.CaptureQingzhongStartingPhase(owner);
    WeiluPhaseKey? IProgramPhaseBoundaryFacts.CaptureWeiluStartingPhase(CharacterState owner) => _characterLvQianOlOperations.CaptureWeiluStartingPhase(owner);
    ProgramSkillWindowContext IProgramPhaseBoundaryFacts.QushiEndingContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterGuoTuOlOperations.CreateQushiEndingContext(frame, candidate, context);
    void IProgramPhaseBoundaryProtocols.AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items) => _coreEarnedActualEndingBenefitsOperations.AppendEarnedActualEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items) => _corePrepDiscardEndingPromisesOperations.AppendPrepDiscardEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items) => _coreConvertingGiftsOperations.AppendGiftRetentionEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items) => _coreFixedDistanceOneDebtOperations.AppendFixedDistanceOneEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items) => _characterYanPuOlOperations.AppendYanPuEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items) => _characterWenYangOlOperations.AppendWenYangEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items) => _characterLiuBaOlOperations.AppendLiuBaCoinEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items) => _characterRuanYuOlOperations.AppendRuanYuXingzuoEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendQushiEndingItems(List<TurnEndingBoundaryItem> items) => _characterGuoTuOlOperations.AppendQushiEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items) => _characterLiWanOlOperations.AppendLianjuEndingItems(items);
    void IProgramPhaseBoundaryProtocols.AppendPipelineEndingItems(List<TurnEndingBoundaryItem> items) => _programPipelineHooksOperations._programPipelineHooks.AppendTurnEndingItems(items);
    bool IProgramPhaseBoundaryProtocols.ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item) => _coreConvertingGiftsOperations.ConsumeGiftRetentionEndingItem(frame, item);
    void IProgramPhaseBoundaryProtocols.ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame) => _coreEarnedActualEndingBenefitsOperations.ConsumeSkippedEarnedActualEnding(frame);
    void IProgramPhaseBoundaryProtocols.ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame) => _corePrepDiscardEndingPromisesOperations.ConsumeSkippedPrepDiscardEnding(frame);
    void IProgramPhaseBoundaryProtocols.ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame frame) => _coreFixedDistanceOneDebtOperations.ConsumeSkippedFixedDistanceDebt(frame);
    void IProgramPhaseBoundaryProtocols.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void IProgramPhaseBoundaryProtocols.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IProgramPhaseBoundaryProtocols.FinalizeEndTurn(CharacterState owner) => _coreActualTurnsOperations.FinalizeEndTurn(owner);
}

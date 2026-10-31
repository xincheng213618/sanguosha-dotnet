#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseBoundaryRuntimePortsRuntime
{
    Winner global::CardGame.Core.IProgramPhaseBoundaryFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.IProgramPhaseBoundaryFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int global::CardGame.Core.IProgramPhaseBoundaryFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    int global::CardGame.Core.IProgramPhaseBoundaryFacts.PreviousLivingSeatFor(int owner) => _coreNeighborDiscardOpportunitiesOperations.PreviousLivingSeatFor(owner);
    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.HasWenheDeadOwnerEnding(int seat) => _coreWenheActivityProgramsOperations.HasWenheDeadOwnerEnding(seat);
    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.HasAnniversaryTianjieDeadOwnerEnding(int seat) => _characterZhangJiaoAnniversaryGodOperations.HasAnniversaryTianjieDeadOwnerEnding(seat);
    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => _coreWenheActivityProgramsOperations.IsWenheDeadOwnerEndingCandidate(candidate, seat);
    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat) => _characterZhangJiaoAnniversaryGodOperations.IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, seat);
    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.UsesTreatedWounded => _coreTreatedWoundedOperations.UsesTreatedWounded;

    bool global::CardGame.Core.IProgramPhaseBoundaryFacts.IsRuleWounded(CharacterState player) => _coreTreatedWoundedOperations.IsRuleWounded(player);
    QingzhongPlayPhaseKey? global::CardGame.Core.IProgramPhaseBoundaryFacts.CaptureQingzhongStartingPhase(CharacterState owner) => _characterLuZhiWeiOlOperations.CaptureQingzhongStartingPhase(owner);
    WeiluPhaseKey? global::CardGame.Core.IProgramPhaseBoundaryFacts.CaptureWeiluStartingPhase(CharacterState owner) => _characterLvQianOlOperations.CaptureWeiluStartingPhase(owner);
    ProgramSkillWindowContext global::CardGame.Core.IProgramPhaseBoundaryFacts.QushiEndingContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterGuoTuOlOperations.CreateQushiEndingContext(frame, candidate, context);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items) => _coreEarnedActualEndingBenefitsOperations.AppendEarnedActualEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items) => _corePrepDiscardEndingPromisesOperations.AppendPrepDiscardEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items) => _coreConvertingGiftsOperations.AppendGiftRetentionEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items) => _coreFixedDistanceOneDebtOperations.AppendFixedDistanceOneEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items) => _characterYanPuOlOperations.AppendYanPuEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items) => _characterWenYangOlOperations.AppendWenYangEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items) => _characterLiuBaOlOperations.AppendLiuBaCoinEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items) => _characterRuanYuOlOperations.AppendRuanYuXingzuoEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendQushiEndingItems(List<TurnEndingBoundaryItem> items) => _characterGuoTuOlOperations.AppendQushiEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items) => _characterLiWanOlOperations.AppendLianjuEndingItems(items);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.AppendPipelineEndingItems(List<TurnEndingBoundaryItem> items) => _programPipelineHooksOperations._programPipelineHooks.AppendTurnEndingItems(items);
    bool global::CardGame.Core.IProgramPhaseBoundaryProtocols.ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item) => _coreConvertingGiftsOperations.ConsumeGiftRetentionEndingItem(frame, item);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame) => _coreEarnedActualEndingBenefitsOperations.ConsumeSkippedEarnedActualEnding(frame);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame) => _corePrepDiscardEndingPromisesOperations.ConsumeSkippedPrepDiscardEnding(frame);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame frame) => _coreFixedDistanceOneDebtOperations.ConsumeSkippedFixedDistanceDebt(frame);
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IProgramPhaseBoundaryProtocols.FinalizeEndTurn(CharacterState owner) => _coreActualTurnsOperations.FinalizeEndTurn(owner);
}

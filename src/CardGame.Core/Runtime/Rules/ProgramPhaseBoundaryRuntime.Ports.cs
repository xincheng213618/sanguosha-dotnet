namespace CardGame.Core;

internal interface IProgramPhaseBoundaryFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    EngineStatus Status { get; }
    int RelativeTableOrder(int from, int target);
    int PreviousLivingSeatFor(int owner);
    bool HasWenheDeadOwnerEnding(int seat);
    bool HasAnniversaryTianjieDeadOwnerEnding(int seat);
    bool IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat);
    bool IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int seat);
    bool UsesTreatedWounded { get; }
    bool IsRuleWounded(CharacterState player);
    QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(CharacterState owner);
    WeiluPhaseKey? CaptureWeiluStartingPhase(CharacterState owner);
    ProgramSkillWindowContext QushiEndingContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
}

// Each operation belongs to an existing native obligation or phase producer.
// Candidate admission, ordering, prompting and parent returns live in the runtime.
internal interface IProgramPhaseBoundaryProtocols : IProgramRuntimeOperations
{
    void AppendEarnedActualEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendYanPuEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendLiuBaCoinEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendQushiEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendLianjuEndingItems(List<TurnEndingBoundaryItem> items);
    void AppendPipelineEndingItems(List<TurnEndingBoundaryItem> items);
    bool ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item);
    void ConsumeSkippedEarnedActualEnding(TurnEndingBoundaryFrame frame);
    void ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame);
    void ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame frame);
    void CompleteGame();
    void PublishState();
    void FinalizeEndTurn(CharacterState owner);
}

public sealed partial class GameEngine
{
}

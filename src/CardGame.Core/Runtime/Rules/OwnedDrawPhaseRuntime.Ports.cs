namespace CardGame.Core;

// Qualification and receipts remain with their native domain owners. These
// adapters expose no frame store or alternate pending Draw-phase state.
internal interface IOwnedDrawPhaseFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    int SkipDrawPhaseMask { get; }
    bool HasAttemptCapability { get; }
    bool TracksNativeDrawThresholds { get; }
    bool TracksShanheS5PhaseSkips { get; }
    bool TracksBoundaryZhuHuanPhaseSkips { get; }
    bool HasIssuedExtraDrawMarker(int seat);
    int RelativeTableOrder(int from, int target);
}

internal interface IOwnedDrawPhaseNativeOperations : IProgramRuntimeOperations
{
    bool BeginRecoveryReplacement(long frameId);
    bool BeginHpChangedWindow(long frameId);
    void ConsumeDuFuRenDrawPhase(int seat);
    void ConsumeChengGongYingDraw(int seat);
    int FreezeNativeDrawBase(CharacterState owner, bool normalDrawReplaced);
    ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(DrawPhaseObligationFrame frame);
    void AdvanceRulesAndPublishState();
    void EndTurn();
    bool CompleteShanheSourceChunlaoJudgmentDraw(DrawPhaseObligationFrame frame);
    void CompleteTurnStartAfterDraw(CharacterState owner, int delayedEffects);
}

public sealed partial class GameEngine
{
}

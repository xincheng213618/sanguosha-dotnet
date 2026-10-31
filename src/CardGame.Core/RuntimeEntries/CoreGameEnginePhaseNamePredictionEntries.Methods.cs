#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseNamePredictionEntries
{
    internal bool AllowsSettlePhaseNamePredictionTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsSettlePhaseNamePredictionTrigger(gate);
    internal void AppendPhaseNamePredictionCandidates(global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().AppendPhaseNamePredictionCandidates(candidates);
    internal void AssertPhaseNamePrediction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPhaseNamePrediction(f);
    internal global::CardGame.Core.ProgramSkillWindowContext AttachPhaseNamePredictionContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AttachPhaseNamePredictionContext(c, context);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPhaseNamePrediction(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().BeginPhaseNamePrediction(supplied, stateId);
    internal bool CanContinuePhaseNamePrediction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePhaseNamePrediction(f);
    internal void ContinuePredictionAfterCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePredictionAfterCost(f);
    internal bool DrainPredictionChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainPredictionChildren(f);
    internal bool HasIssuedPhaseNamePrediction(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedPhaseNamePrediction(c, context);
    internal bool IsPhaseNamePredictionMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPhaseNamePredictionMovement(f, e, pending);
    internal void ObservePhaseNamePredictionUse(global::CardGame.Core.CardActionContext action, bool qualifiedActualUse = false) => _resolve().ObservePhaseNamePredictionUse(action, qualifiedActualUse);
    internal void ObservePredictionUse(global::CardGame.Core.PhaseNamePredictionPolicy p, global::CardGame.Core.PhaseNamePredictionActualUseEvent use) => _resolve().ObservePredictionUse(p, use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PredictionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PredictionChoices(f);
    internal bool PredictionConsumed(global::CardGame.Core.PhaseNamePredictionPolicy p) => _resolve().PredictionConsumed(p);
    internal global::CardGame.Core.PhaseNamePredictionMaterial[] PredictionMaterials(long frameId, int seat) => _resolve().PredictionMaterials(frameId, seat);
    internal void PublishPredictionChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPredictionChoice(f);
    internal void RecordPredictionActualUse(long? actionId, long? nativeFrameId, int actorSeat, global::CardGame.Core.CardKind kind) => _resolve().RecordPredictionActualUse(actionId, nativeFrameId, actorSeat, kind);
    internal void RecordPredictionNativeCardUse(long frameId, int actorSeat, global::CardGame.Core.CardKind kind) => _resolve().RecordPredictionNativeCardUse(frameId, actorSeat, kind);
    internal void ResolvePhaseNamePredictionChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePhaseNamePredictionChoice(choice);
    internal bool ResumePhaseNamePrediction(long id) => _resolve().ResumePhaseNamePrediction(id);
    internal bool ReturnPhaseNamePredictionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPhaseNamePredictionMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettlePhaseNamePrediction(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().SettlePhaseNamePrediction(supplied, stateId);
    internal int CurrentPredictionPhaseInstanceId { get => _resolve().CurrentPredictionPhaseInstanceId; }
    internal bool TracksPhaseNamePredictions { get => _resolve().TracksPhaseNamePredictions; }
}

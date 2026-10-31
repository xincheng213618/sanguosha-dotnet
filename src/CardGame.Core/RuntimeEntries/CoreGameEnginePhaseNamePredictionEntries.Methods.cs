#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseNamePredictionEntries
{
    internal bool AllowsSettlePhaseNamePredictionTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsSettlePhaseNamePredictionTrigger(gate);
    internal void AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates) => _resolve().AppendPhaseNamePredictionCandidates(candidates);
    internal ProgramSkillWindowContext AttachPhaseNamePredictionContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().AttachPhaseNamePredictionContext(c, context);
    internal SkillProgramStepOutcome BeginPhaseNamePrediction(ProgramSkillFrame supplied, string stateId) => _resolve().BeginPhaseNamePrediction(supplied, stateId);
    internal bool CanContinuePhaseNamePrediction(ProgramSkillFrame f) => _resolve().CanContinuePhaseNamePrediction(f);
    internal void ContinuePredictionAfterCost(ProgramSkillFrame f) => _resolve().ContinuePredictionAfterCost(f);
    internal bool DrainPredictionChildren(ProgramSkillFrame f) => _resolve().DrainPredictionChildren(f);
    internal bool HasIssuedPhaseNamePrediction(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().HasIssuedPhaseNamePrediction(c, context);
    internal void ObservePhaseNamePredictionUse(CardActionContext action, bool qualifiedActualUse = false) => _resolve().ObservePhaseNamePredictionUse(action, qualifiedActualUse);
    internal void ObservePredictionUse(PhaseNamePredictionPolicy p, PhaseNamePredictionActualUseEvent use) => _resolve().ObservePredictionUse(p, use);
    internal IReadOnlyList<PromptChoice> PredictionChoices(ProgramSkillFrame f) => _resolve().PredictionChoices(f);
    internal bool PredictionConsumed(PhaseNamePredictionPolicy p) => _resolve().PredictionConsumed(p);
    internal PhaseNamePredictionMaterial[] PredictionMaterials(long frameId, int seat) => _resolve().PredictionMaterials(frameId, seat);
    internal void PublishPredictionChoice(ProgramSkillFrame f) => _resolve().PublishPredictionChoice(f);
    internal void RecordPredictionActualUse(long? actionId, long? nativeFrameId, int actorSeat, CardKind kind) => _resolve().RecordPredictionActualUse(actionId, nativeFrameId, actorSeat, kind);
    internal void RecordPredictionNativeCardUse(long frameId, int actorSeat, CardKind kind) => _resolve().RecordPredictionNativeCardUse(frameId, actorSeat, kind);
    internal void ResolvePhaseNamePredictionChoice(PromptChoice choice) => _resolve().ResolvePhaseNamePredictionChoice(choice);
    internal bool ResumePhaseNamePrediction(long id) => _resolve().ResumePhaseNamePrediction(id);
    internal bool ReturnPhaseNamePredictionMovement(ProgramSkillFrame f) => _resolve().ReturnPhaseNamePredictionMovement(f);
    internal SkillProgramStepOutcome SettlePhaseNamePrediction(ProgramSkillFrame supplied, string stateId) => _resolve().SettlePhaseNamePrediction(supplied, stateId);
    internal int CurrentPredictionPhaseInstanceId { get => _resolve().CurrentPredictionPhaseInstanceId; }
    internal bool TracksPhaseNamePredictions { get => _resolve().TracksPhaseNamePredictions; }
}

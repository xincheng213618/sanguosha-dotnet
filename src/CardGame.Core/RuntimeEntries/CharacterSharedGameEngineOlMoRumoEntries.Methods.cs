#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoEntries
{
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendOlMoRumoMarker(int ownerSeat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => _resolve().AppendOlMoRumoMarker(ownerSeat, ordinary);
    internal void CompleteOlMoRumo(ProgramSkillFrame frame) => _resolve().CompleteOlMoRumo(frame);
    internal bool ContinueIssuedOlMoRumo(ProgramSkillFrame frame) => _resolve().ContinueIssuedOlMoRumo(frame);
    internal void ContinueOlMoRumo(long frameId) => _resolve().ContinueOlMoRumo(frameId);
    internal bool DrainOlMoRumo(ProgramSkillFrame frame) => _resolve().DrainOlMoRumo(frame);
    internal void EnterOlMoRumo(ProgramSkillFrame supplied) => _resolve().EnterOlMoRumo(supplied);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeOlMoRumoRoundCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeOlMoRumoRoundCandidates(owner, window, ordinary);
    internal bool IsIssuedOlMoRumoCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedOlMoRumoCandidate(candidate, context);
    internal bool IsOlMoRumoEntered(int ownerSeat) => _resolve().IsOlMoRumoEntered(ownerSeat);
    internal void ObserveOlMoRumoLoss(IGameEvent payload) => _resolve().ObserveOlMoRumoLoss(payload);
    internal PlayerMarkerSnapshot? OlMoRumoMarker(int ownerSeat) => _resolve().OlMoRumoMarker(ownerSeat);
    internal SkillProgramStepOutcome ResolveOlMoRumoRoundPenalty(ProgramSkillFrame supplied) => _resolve().ResolveOlMoRumoRoundPenalty(supplied);
    internal bool ResumeOlMoRumo(long frameId) => _resolve().ResumeOlMoRumo(frameId);
    internal bool HasOlMoRumoCapability { get => _resolve().HasOlMoRumoCapability; }
}

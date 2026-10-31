#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendOlMoRumoMarker(int ownerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? ordinary) => _resolve().AppendOlMoRumoMarker(ownerSeat, ordinary);
    internal void CompleteOlMoRumo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteOlMoRumo(frame);
    internal bool ContinueIssuedOlMoRumo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueIssuedOlMoRumo(frame);
    internal void ContinueOlMoRumo(long frameId) => _resolve().ContinueOlMoRumo(frameId);
    internal bool DrainOlMoRumo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainOlMoRumo(frame);
    internal void EnterOlMoRumo(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().EnterOlMoRumo(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeOlMoRumoRoundCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeOlMoRumoRoundCandidates(owner, window, ordinary);
    internal bool IsIssuedOlMoRumoCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedOlMoRumoCandidate(candidate, context);
    internal bool IsOlMoRumoEntered(int ownerSeat) => _resolve().IsOlMoRumoEntered(ownerSeat);
    internal void ObserveOlMoRumoLoss(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveOlMoRumoLoss(payload);
    internal global::CardGame.Core.PlayerMarkerSnapshot? OlMoRumoMarker(int ownerSeat) => _resolve().OlMoRumoMarker(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveOlMoRumoRoundPenalty(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveOlMoRumoRoundPenalty(supplied);
    internal bool ResumeOlMoRumo(long frameId) => _resolve().ResumeOlMoRumo(frameId);
    internal bool HasOlMoRumoCapability { get => _resolve().HasOlMoRumoCapability; }
}

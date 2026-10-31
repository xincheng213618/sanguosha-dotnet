#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMatchingRecast(global::CardGame.Core.ProgramSkillFrame supplied, int peerSeat, string ledgerId) => _resolve().BeginMatchingRecast(supplied, peerSeat, ledgerId);
    internal bool CanContinueMatchingRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueMatchingRecast(f);
    internal bool DrainMatchingRecastChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMatchingRecastChildren(f);
    internal void FinishMatchingRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMatchingRecast(f);
    internal void IssueMatchingRecastCost(global::CardGame.Core.ProgramSkillFrame f, bool peer) => _resolve().IssueMatchingRecastCost(f, peer);
    internal void IssueMatchingRecastDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMatchingRecastDraw(f);
    internal global::CardGame.Core.PromptChoice MatchingRecastChoice(global::CardGame.Core.ProgramSkillFrame f, string step, string label, int? card = default) => _resolve().MatchingRecastChoice(f, step, label, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MatchingRecastChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchingRecastChoices(f);
    internal global::CardGame.Core.MatchingRecastMaterial[] MatchingRecastMaterials(int seat) => _resolve().MatchingRecastMaterials(seat);
    internal bool MatchingRecastMaterialsStillOwned(global::CardGame.Core.MatchingRecastParticipantReceipt p) => _resolve().MatchingRecastMaterialsStillOwned(p);
    internal void PublishMatchingRecastChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMatchingRecastChoice(f);
    internal void ResolveMatchingRecastChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveMatchingRecastChoice(choice);
    internal bool ResumeMatchingRecast(long id) => _resolve().ResumeMatchingRecast(id);
    internal bool ReturnMatchingRecastMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMatchingRecastMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiMatchingRecast(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMatchingRecast(decision, f);
    internal bool ValidMatchingRecastSelection(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().ValidMatchingRecastSelection(seat, ids);
    internal long MatchingRecastSequence { get => _resolve().MatchingRecastSequence; }
}

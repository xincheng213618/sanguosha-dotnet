#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastsEntries
{
    internal SkillProgramStepOutcome BeginMatchingRecast(ProgramSkillFrame supplied, int peerSeat, string ledgerId) => _resolve().BeginMatchingRecast(supplied, peerSeat, ledgerId);
    internal bool CanContinueMatchingRecast(ProgramSkillFrame f) => _resolve().CanContinueMatchingRecast(f);
    internal bool DrainMatchingRecastChildren(ProgramSkillFrame f) => _resolve().DrainMatchingRecastChildren(f);
    internal void FinishMatchingRecast(ProgramSkillFrame f) => _resolve().FinishMatchingRecast(f);
    internal void IssueMatchingRecastCost(ProgramSkillFrame f, bool peer) => _resolve().IssueMatchingRecastCost(f, peer);
    internal void IssueMatchingRecastDraw(ProgramSkillFrame f) => _resolve().IssueMatchingRecastDraw(f);
    internal PromptChoice MatchingRecastChoice(ProgramSkillFrame f, string step, string label, int? card = default) => _resolve().MatchingRecastChoice(f, step, label, card);
    internal IReadOnlyList<PromptChoice> MatchingRecastChoices(ProgramSkillFrame f) => _resolve().MatchingRecastChoices(f);
    internal MatchingRecastMaterial[] MatchingRecastMaterials(int seat) => _resolve().MatchingRecastMaterials(seat);
    internal bool MatchingRecastMaterialsStillOwned(MatchingRecastParticipantReceipt p) => _resolve().MatchingRecastMaterialsStillOwned(p);
    internal void PublishMatchingRecastChoice(ProgramSkillFrame f) => _resolve().PublishMatchingRecastChoice(f);
    internal void ResolveMatchingRecastChoice(PromptChoice choice) => _resolve().ResolveMatchingRecastChoice(choice);
    internal bool ResumeMatchingRecast(long id) => _resolve().ResumeMatchingRecast(id);
    internal bool ReturnMatchingRecastMovement(ProgramSkillFrame f) => _resolve().ReturnMatchingRecastMovement(f);
    internal PromptChoice SelectAiMatchingRecast(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiMatchingRecast(decision, f);
    internal bool ValidMatchingRecastSelection(int seat, IReadOnlyList<int> ids) => _resolve().ValidMatchingRecastSelection(seat, ids);
    internal long MatchingRecastSequence { get => _resolve().MatchingRecastSequence; }
}

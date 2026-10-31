#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianSelectionsEntries
{
    internal bool CanOfferSimultaneousTop(ProgramSkillFrame f, int seat) => _resolve().CanOfferSimultaneousTop(f, seat);
    internal void CommitSimultaneousSelection(ProgramSkillFrame f, int seat, PromptChoice choice) => _resolve().CommitSimultaneousSelection(f, seat, choice);
    internal void ContinueSimultaneousRandomPolicies(ProgramSkillFrame f) => _resolve().ContinueSimultaneousRandomPolicies(f);
    internal void ContinueSimultaneousSelection(ProgramSkillFrame f) => _resolve().ContinueSimultaneousSelection(f);
    internal bool IsSimultaneousPrivateTopCard(int id) => _resolve().IsSimultaneousPrivateTopCard(id);
    internal void ResolveSimultaneousRandomPolicy(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveSimultaneousRandomPolicy(f, choice);
    internal void RevealSimultaneousCommittedGroup(ProgramSkillFrame f) => _resolve().RevealSimultaneousCommittedGroup(f);
    internal CardLocation SimultaneousOriginalLocation(SimultaneousPindianReceipt r, PindianRevealedCard card) => _resolve().SimultaneousOriginalLocation(r, card);
}

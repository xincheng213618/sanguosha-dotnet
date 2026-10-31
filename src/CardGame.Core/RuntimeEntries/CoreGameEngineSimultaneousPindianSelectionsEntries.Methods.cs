#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianSelectionsEntries
{
    internal bool CanOfferSimultaneousTop(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().CanOfferSimultaneousTop(f, seat);
    internal void CommitSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.PromptChoice choice) => _resolve().CommitSimultaneousSelection(f, seat, choice);
    internal void ContinueSimultaneousRandomPolicies(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSimultaneousRandomPolicies(f);
    internal void ContinueSimultaneousSelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSimultaneousSelection(f);
    internal bool IsSimultaneousPrivateTopCard(int id) => _resolve().IsSimultaneousPrivateTopCard(id);
    internal void ResolveSimultaneousRandomPolicy(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSimultaneousRandomPolicy(f, choice);
    internal void RevealSimultaneousCommittedGroup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RevealSimultaneousCommittedGroup(f);
    internal global::CardGame.Core.CardLocation SimultaneousOriginalLocation(global::CardGame.Core.SimultaneousPindianReceipt r, global::CardGame.Core.PindianRevealedCard card) => _resolve().SimultaneousOriginalLocation(r, card);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoSanNiangOlEntries
{
    internal void AdvanceBaoOutcomes(ProgramSkillFrame f) => _resolve().AdvanceBaoOutcomes(f);
    internal int[] BaoAssaultLegalTargets(ProgramSkillFrame f) => _resolve().BaoAssaultLegalTargets(f);
    internal PromptChoice BaoChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().BaoChoice(f, branch, label, cards, targets);
    internal IReadOnlyList<PromptChoice> BaoChoices(ProgramSkillFrame f) => _resolve().BaoChoices(f);
    internal IReadOnlyList<Card> BaoCostCards(CharacterState owner) => _resolve().BaoCostCards(owner);
    internal bool BaoFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().BaoFirstChild(root, child);
    internal bool BaoHasSpentFriendlyResetSkill(ProgramSkillFrame f) => _resolve().BaoHasSpentFriendlyResetSkill(f);
    internal ProgramSkillFrame? BaoObserverRoot() => _resolve().BaoObserverRoot();
    internal CardConversionSource BaoSource(ProgramSkillFrame f) => _resolve().BaoSource(f);
    internal bool BaoSourceCurrent(ProgramSkillFrame f) => _resolve().BaoSourceCurrent(f);
    internal SkillProgramStepOutcome BeginBao2025(ProgramSkillFrame input, SkillProgramEffect e) => _resolve().BeginBao2025(input, e);
    internal bool CanContinueBao2025(ProgramSkillFrame f) => _resolve().CanContinueBao2025(f);
    internal bool CanOfferBao2025(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferBao2025(candidate, trigger, c);
    internal bool DrainBaoChildren(ProgramSkillFrame f) => _resolve().DrainBaoChildren(f);
    internal void FinishBao2025(ProgramSkillFrame f, bool completed) => _resolve().FinishBao2025(f, completed);
    internal bool HasBaoSilverLionPayment(ProgramSkillFrame f, int seat) => _resolve().HasBaoSilverLionPayment(f, seat);
    internal bool IsChosenAssaultUse(long id) => _resolve().IsChosenAssaultUse(id);
    internal void IssueBaoAssault(ProgramSkillFrame f) => _resolve().IssueBaoAssault(f);
    internal bool MatchesChosenAssaultUseAction(long id, CardActionContext action, int cardId) => _resolve().MatchesChosenAssaultUseAction(id, action, cardId);
    internal void ObserveChosenAssaultDamage(IGameEvent payload) => _resolve().ObserveChosenAssaultDamage(payload);
    internal void PayBaoAssault(ProgramSkillFrame f) => _resolve().PayBaoAssault(f);
    internal void PayBaoGain(ProgramSkillFrame f, PromptChoice choice) => _resolve().PayBaoGain(f, choice);
    internal void PublishBaoChoice(ProgramSkillFrame f) => _resolve().PublishBaoChoice(f);
    internal void RequireBao2025(ProgramSkillFrame f) => _resolve().RequireBao2025(f);
    internal void ResetBaoLimitedInstances(ProgramSkillFrame f) => _resolve().ResetBaoLimitedInstances(f);
    internal void ResolveBao2025Choice(PromptChoice choice) => _resolve().ResolveBao2025Choice(choice);
    internal bool ResumeBao2025(long id) => _resolve().ResumeBao2025(id);
    internal void ReturnBaoAssault(CardUseFrame use) => _resolve().ReturnBaoAssault(use);
    internal bool ReturnBaoMovement(ProgramSkillFrame f) => _resolve().ReturnBaoMovement(f);
    internal PromptChoice SelectAiBaoChoice(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiBaoChoice(decision, f);
    internal bool TryAdvanceBaoSubtree() => _resolve().TryAdvanceBaoSubtree();
    internal bool ValidBao2025(ProgramSkillFrame f) => _resolve().ValidBao2025(f);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaoSanNiangOlEntries
{
    internal void AdvanceBaoOutcomes(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AdvanceBaoOutcomes(f);
    internal void AssertChosenAssaults() => _resolve().AssertChosenAssaults();
    internal void AssertRegisteredBao2025(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredBao2025(frame);
    internal int[] BaoAssaultLegalTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoAssaultLegalTargets(f);
    internal global::CardGame.Core.PromptChoice BaoChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().BaoChoice(f, branch, label, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> BaoCostCards(global::CardGame.Core.CharacterState owner) => _resolve().BaoCostCards(owner);
    internal bool BaoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().BaoFirstChild(root, child);
    internal bool BaoHasSpentFriendlyResetSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoHasSpentFriendlyResetSkill(f);
    internal global::CardGame.Core.ProgramSkillFrame? BaoObserverRoot() => _resolve().BaoObserverRoot();
    internal global::CardGame.Core.CardConversionSource BaoSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoSource(f);
    internal bool BaoSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BaoSourceCurrent(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBao2025(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginBao2025(input, e);
    internal bool CanContinueBao2025(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueBao2025(f);
    internal bool CanOfferBao2025(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferBao2025(candidate, trigger, c);
    internal bool DrainBaoChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainBaoChildren(f);
    internal void FinishBao2025(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().FinishBao2025(f, completed);
    internal bool HasBaoDamageObserver(long id) => _resolve().HasBaoDamageObserver(id);
    internal bool HasBaoSilverLionPayment(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().HasBaoSilverLionPayment(f, seat);
    internal bool IsBaoDying() => _resolve().IsBaoDying();
    internal bool IsBaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsBaoMovement(f, e, p);
    internal bool IsChosenAssaultAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsChosenAssaultAttackConsistent(attack, processing);
    internal bool IsChosenAssaultUse(long id) => _resolve().IsChosenAssaultUse(id);
    internal void IssueBaoAssault(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueBaoAssault(f);
    internal bool MatchesChosenAssaultUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesChosenAssaultUseAction(id, action, cardId);
    internal void ObserveChosenAssaultDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveChosenAssaultDamage(payload);
    internal void PayBaoAssault(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayBaoAssault(f);
    internal void PayBaoGain(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().PayBaoGain(f, choice);
    internal void PublishBaoChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBaoChoice(f);
    internal void RequireBao2025(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireBao2025(f);
    internal void ResetBaoLimitedInstances(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResetBaoLimitedInstances(f);
    internal void ResolveBao2025Choice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveBao2025Choice(choice);
    internal bool ResumeBao2025(long id) => _resolve().ResumeBao2025(id);
    internal void ReturnBaoAssault(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnBaoAssault(use);
    internal bool ReturnBaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBaoMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiBaoChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiBaoChoice(decision, f);
    internal bool TryAdvanceBaoSubtree() => _resolve().TryAdvanceBaoSubtree();
    internal bool ValidBao2025(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBao2025(f);
}

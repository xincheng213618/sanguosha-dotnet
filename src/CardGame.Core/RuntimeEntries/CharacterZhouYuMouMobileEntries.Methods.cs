#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMouMobileEntries
{
    internal bool AdvanceMouZyMobileChildren() => _resolve().AdvanceMouZyMobileChildren();
    internal global::CardGame.Core.PromptChoice AiMouZyMobile(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouZyMobile(p, f);
    internal void AnswerMouZyMobile(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouZyMobile(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouZyMobile(global::CardGame.Core.ProgramSkillFrame supplied, bool yingzi) => _resolve().BeginMouZyMobile(supplied, yingzi);
    internal bool CanStartMouZyMobile(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouZyMobile(owner);
    internal void ContinueMouZyMobile(long id) => _resolve().ContinueMouZyMobile(id);
    internal void DisableMouZyMobile(global::CardGame.Core.ProgramSkillFrame f, string reason) => _resolve().DisableMouZyMobile(f, reason);
    internal bool DrainMouZyMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouZyMobile(f);
    internal void FinishMouZyMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouZyMobile(f);
    internal bool HasMouZyMobileBeforeDamageObserver(long id) => _resolve().HasMouZyMobileBeforeDamageObserver(id);
    internal bool HasMouZyMobileDamageObserver(long id) => _resolve().HasMouZyMobileDamageObserver(id);
    internal bool IsMouZyMobileDying() => _resolve().IsMouZyMobileDying();
    internal bool IsMouZyMobileMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouZyMobileMovement(f, e, p);
    internal bool IsMouZyMobileRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouZyMobileRecoveryParent(f, recovery);
    internal bool KeepMouZyMobilePaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMouZyMobilePaid(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> MouZhouYuMobilePublicCards() => _resolve().MouZhouYuMobilePublicCards();
    internal global::CardGame.Core.Card[] MouZyMobileCards(int owner) => _resolve().MouZyMobileCards(owner);
    internal global::CardGame.Core.PromptChoice MouZyMobileChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, int? card = default, global::CardGame.Core.Suit? declared = default) => _resolve().MouZyMobileChoice(f, branch, text, card, declared);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouZyMobileChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouZyMobileChoices(f);
    internal bool MouZyMobileDisabled(int owner, global::CardGame.Core.MouZhouYuMobileTurn turn) => _resolve().MouZyMobileDisabled(owner, turn);
    internal bool MouZyMobileFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZyMobileFirstChild(f, child);
    internal int MouZyMobileHandLimit(int owner) => _resolve().MouZyMobileHandLimit(owner);
    internal global::CardGame.Core.ProgramSkillFrame? MouZyMobileObserverRoot() => _resolve().MouZyMobileObserverRoot();
    internal bool MouZyMobileObserverSuffix(int index) => _resolve().MouZyMobileObserverSuffix(index);
    internal bool MouZyMobileSuitSpent(int owner, global::CardGame.Core.MouZhouYuMobileTurn turn, global::CardGame.Core.Suit suit) => _resolve().MouZyMobileSuitSpent(owner, turn, suit);
    internal global::CardGame.Core.MouZhouYuMobileTurn? MouZyMobileTurn() => _resolve().MouZyMobileTurn();
    internal void PublishMouZyMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouZyMobile(f);
    internal bool ResumeMouZyMobile(long id) => _resolve().ResumeMouZyMobile(id);
    internal bool ReturnMouZyMobileMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouZyMobileMovement(f);
    internal void SealMouZyMobile(long id, long before) => _resolve().SealMouZyMobile(id, before);
    internal bool ValidMouZyMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouZyMobile(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> WithoutMouZhouYuMobileCovered(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().WithoutMouZhouYuMobileCovered(cards);
    internal bool HasMouZhouYuMobileCapability { get => _resolve().HasMouZhouYuMobileCapability; }
}

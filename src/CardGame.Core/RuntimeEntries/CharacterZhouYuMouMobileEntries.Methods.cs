#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMouMobileEntries
{
    internal bool AdvanceMouZyMobileChildren() => _resolve().AdvanceMouZyMobileChildren();
    internal PromptChoice AiMouZyMobile(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouZyMobile(p, f);
    internal void AnswerMouZyMobile(PromptChoice selected) => _resolve().AnswerMouZyMobile(selected);
    internal SkillProgramStepOutcome BeginMouZyMobile(ProgramSkillFrame supplied, bool yingzi) => _resolve().BeginMouZyMobile(supplied, yingzi);
    internal bool CanStartMouZyMobile(CharacterState owner) => _resolve().CanStartMouZyMobile(owner);
    internal void ContinueMouZyMobile(long id) => _resolve().ContinueMouZyMobile(id);
    internal void DisableMouZyMobile(ProgramSkillFrame f, string reason) => _resolve().DisableMouZyMobile(f, reason);
    internal bool DrainMouZyMobile(ProgramSkillFrame f) => _resolve().DrainMouZyMobile(f);
    internal void FinishMouZyMobile(ProgramSkillFrame f) => _resolve().FinishMouZyMobile(f);
    internal bool IsMouZyMobileDying() => _resolve().IsMouZyMobileDying();
    internal bool IsMouZyMobileRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouZyMobileRecoveryParent(f, recovery);
    internal bool KeepMouZyMobilePaid(ProgramSkillFrame f) => _resolve().KeepMouZyMobilePaid(f);
    internal IReadOnlyList<CardSnapshot> MouZhouYuMobilePublicCards() => _resolve().MouZhouYuMobilePublicCards();
    internal Card[] MouZyMobileCards(int owner) => _resolve().MouZyMobileCards(owner);
    internal PromptChoice MouZyMobileChoice(ProgramSkillFrame f, string branch, string text, int? card = default, Suit? declared = default) => _resolve().MouZyMobileChoice(f, branch, text, card, declared);
    internal IReadOnlyList<PromptChoice> MouZyMobileChoices(ProgramSkillFrame f) => _resolve().MouZyMobileChoices(f);
    internal bool MouZyMobileDisabled(int owner, MouZhouYuMobileTurn turn) => _resolve().MouZyMobileDisabled(owner, turn);
    internal bool MouZyMobileFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZyMobileFirstChild(f, child);
    internal int MouZyMobileHandLimit(int owner) => _resolve().MouZyMobileHandLimit(owner);
    internal ProgramSkillFrame? MouZyMobileObserverRoot() => _resolve().MouZyMobileObserverRoot();
    internal bool MouZyMobileObserverSuffix(int index) => _resolve().MouZyMobileObserverSuffix(index);
    internal bool MouZyMobileSuitSpent(int owner, MouZhouYuMobileTurn turn, Suit suit) => _resolve().MouZyMobileSuitSpent(owner, turn, suit);
    internal MouZhouYuMobileTurn? MouZyMobileTurn() => _resolve().MouZyMobileTurn();
    internal void PublishMouZyMobile(ProgramSkillFrame f) => _resolve().PublishMouZyMobile(f);
    internal bool ResumeMouZyMobile(long id) => _resolve().ResumeMouZyMobile(id);
    internal bool ReturnMouZyMobileMovement(ProgramSkillFrame f) => _resolve().ReturnMouZyMobileMovement(f);
    internal void SealMouZyMobile(long id, long before) => _resolve().SealMouZyMobile(id, before);
    internal bool ValidMouZyMobile(ProgramSkillFrame f) => _resolve().ValidMouZyMobile(f);
    internal IReadOnlyList<Card> WithoutMouZhouYuMobileCovered(IReadOnlyList<Card> cards) => _resolve().WithoutMouZhouYuMobileCovered(cards);
    internal bool HasMouZhouYuMobileCapability { get => _resolve().HasMouZhouYuMobileCapability; }
}

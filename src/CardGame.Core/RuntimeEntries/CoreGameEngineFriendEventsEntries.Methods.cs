#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendEventsEntries
{
    internal int AscendingHandRank(int seat, string skillId) => _resolve().AscendingHandRank(seat, skillId);
    internal SkillProgramStepOutcome BeginFriendEvent(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginFriendEvent(supplied, operation);
    internal bool CanOfferFriendEvent(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferFriendEvent(candidate, trigger, context);
    internal void CaptureFriendMinimumHandUse(CardActionContext? action) => _resolve().CaptureFriendMinimumHandUse(action);
    internal void CompleteFriendRecastDuelAttackReturn(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteFriendRecastDuelAttackReturn(completion);
    internal void ContinueFriendEventMovement(long id) => _resolve().ContinueFriendEventMovement(id);
    internal void FinishFriendEvent(ProgramSkillFrame f) => _resolve().FinishFriendEvent(f);
    internal IReadOnlyList<PromptChoice> FriendConfigurationChoices(ProgramSkillFrame f) => _resolve().FriendConfigurationChoices(f);
    internal bool FriendDuelCategoryAllowed(int seat, CardKind kind) => _resolve().FriendDuelCategoryAllowed(seat, kind);
    internal bool FriendHandDiscardAllowed(int seat, Card card) => _resolve().FriendHandDiscardAllowed(seat, card);
    internal bool FriendSuitAllowed(int seat, Suit suit) => _resolve().FriendSuitAllowed(seat, suit);
    internal int FriendSuitUseCount(int seat, string skillId) => _resolve().FriendSuitUseCount(seat, skillId);
    internal bool IsFriendRecastDuelUse(long id) => _resolve().IsFriendRecastDuelUse(id);
    internal void PublishFriendConfiguration(ProgramSkillFrame f) => _resolve().PublishFriendConfiguration(f);
    internal void ResolveFriendConfiguration(PromptChoice selected) => _resolve().ResolveFriendConfiguration(selected);
    internal bool ResumeFriendEvent(long id) => _resolve().ResumeFriendEvent(id);
    internal bool ReturnFriendEventMovement(ProgramSkillFrame f) => _resolve().ReturnFriendEventMovement(f);
    internal void ReturnFriendRecastDuel(CardUseFrame use) => _resolve().ReturnFriendRecastDuel(use);
    internal bool ReturnRegisteredCompleteFriendRecastDuelAttackReturn(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteFriendRecastDuelAttackReturn(completion);
    internal Card? UniqueMaximumHandCard(int seat) => _resolve().UniqueMaximumHandCard(seat);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendEventsEntries
{
    internal int AscendingHandRank(int seat, string skillId) => _resolve().AscendingHandRank(seat, skillId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFriendEvent(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginFriendEvent(supplied, operation);
    internal bool CanOfferFriendEvent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferFriendEvent(candidate, trigger, context);
    internal void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action) => _resolve().CaptureFriendMinimumHandUse(action);
    internal void CompleteFriendRecastDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteFriendRecastDuelAttackReturn(completion);
    internal void ContinueFriendEventMovement(long id) => _resolve().ContinueFriendEventMovement(id);
    internal void FinishFriendEvent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFriendEvent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendConfigurationChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FriendConfigurationChoices(f);
    internal bool FriendDuelCategoryAllowed(int seat, global::CardGame.Core.CardKind kind) => _resolve().FriendDuelCategoryAllowed(seat, kind);
    internal bool FriendHandDiscardAllowed(int seat, global::CardGame.Core.Card card) => _resolve().FriendHandDiscardAllowed(seat, card);
    internal bool FriendSuitAllowed(int seat, global::CardGame.Core.Suit suit) => _resolve().FriendSuitAllowed(seat, suit);
    internal int FriendSuitUseCount(int seat, string skillId) => _resolve().FriendSuitUseCount(seat, skillId);
    internal bool IsFriendRecastDuelUse(long id) => _resolve().IsFriendRecastDuelUse(id);
    internal void PublishFriendConfiguration(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFriendConfiguration(f);
    internal void ResolveFriendConfiguration(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFriendConfiguration(selected);
    internal bool ResumeFriendEvent(long id) => _resolve().ResumeFriendEvent(id);
    internal bool ReturnFriendEventMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFriendEventMovement(f);
    internal void ReturnFriendRecastDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnFriendRecastDuel(use);
    internal bool ReturnRegisteredCompleteFriendRecastDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteFriendRecastDuelAttackReturn(completion);
    internal global::CardGame.Core.Card? UniqueMaximumHandCard(int seat) => _resolve().UniqueMaximumHandCard(seat);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendTrioEntries
{
    internal void AssertFriendTrio(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertFriendTrio(f);
    internal void BeginFriendXuanjianUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginFriendXuanjianUse(f);
    internal bool CanOfferFriendTrio(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferFriendTrio(candidate, trigger, context);
    internal void ContinueFriendTrio(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueFriendTrio(f);
    internal bool DrainFriendTrioChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainFriendTrioChildren(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteFriendTrio(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteFriendTrio(supplied, op);
    internal void FinishFriendTrio(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFriendTrio(f);
    internal global::CardGame.Core.CardActionContext? FriendCardAction(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().FriendCardAction(context);
    internal global::CardGame.Core.PlayerMarkerKind FriendCategoryMarker(global::CardGame.Core.CardKind kind) => _resolve().FriendCategoryMarker(kind);
    internal string FriendCategoryValue(global::CardGame.Core.CardKind kind) => _resolve().FriendCategoryValue(kind);
    internal int FriendPredictionCount(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FriendPredictionCount(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendTrioChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FriendTrioChoices(f);
    internal void FriendTrioMutation(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.FriendTrioStage next, global::System.Action action) => _resolve().FriendTrioMutation(f, next, action);
    internal bool FriendXuanjianFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().FriendXuanjianFrameRidesOn(child, parent);
    internal bool FriendXuanjianIgnoresDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => _resolve().FriendXuanjianIgnoresDistance(owner, source);
    internal bool FriendXuanjianIgnoresDistanceQuery(global::CardGame.Core.ProgramSlashConversionDistanceQuery q) => _resolve().FriendXuanjianIgnoresDistanceQuery(q);
    internal void GainFriendMatchingDeckCards(global::CardGame.Core.ProgramSkillFrame f, string mode, string value, int count) => _resolve().GainFriendMatchingDeckCards(f, mode, value, count);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.FriendPredictionView>? GetFriendPredictionViews(int? viewerSeat) => _resolve().GetFriendPredictionViews(viewerSeat);
    internal bool HasFriendTrioAlly(global::CardGame.Core.CharacterState owner, string id) => _resolve().HasFriendTrioAlly(owner, id);
    internal bool IsFriendActualCardUse(global::CardGame.Core.CardActionContext? action) => _resolve().IsFriendActualCardUse(action);
    internal bool IsFriendTrioMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsFriendTrioMovement(f, e, pending);
    internal void PublishFriendTrioChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFriendTrioChoice(f);
    internal void ResolveFriendTrioChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveFriendTrioChoice(choice);
    internal bool ResumeFriendTrio(long id) => _resolve().ResumeFriendTrio(id);
    internal bool ReturnFriendTrioMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFriendTrioMovement(f);
    internal void ReturnFriendXuanjianUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnFriendXuanjianUse(use);
    internal global::CardGame.Core.PromptChoice SelectAiFriendTrioChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiFriendTrioChoice(decision, frame);
    internal bool ValidWholeHandSuitConversion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource source) => _resolve().ValidWholeHandSuitConversion(owner, cards, source);
}

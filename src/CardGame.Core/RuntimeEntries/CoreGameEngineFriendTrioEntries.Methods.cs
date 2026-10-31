#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFriendTrioEntries
{
    internal void BeginFriendXuanjianUse(ProgramSkillFrame f) => _resolve().BeginFriendXuanjianUse(f);
    internal bool CanOfferFriendTrio(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferFriendTrio(candidate, trigger, context);
    internal void ContinueFriendTrio(ProgramSkillFrame f) => _resolve().ContinueFriendTrio(f);
    internal bool DrainFriendTrioChildren(ProgramSkillFrame f) => _resolve().DrainFriendTrioChildren(f);
    internal SkillProgramStepOutcome ExecuteFriendTrio(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteFriendTrio(supplied, op);
    internal void FinishFriendTrio(ProgramSkillFrame f) => _resolve().FinishFriendTrio(f);
    internal CardActionContext? FriendCardAction(ProgramSkillWindowContext context) => _resolve().FriendCardAction(context);
    internal PlayerMarkerKind FriendCategoryMarker(CardKind kind) => _resolve().FriendCategoryMarker(kind);
    internal string FriendCategoryValue(CardKind kind) => _resolve().FriendCategoryValue(kind);
    internal int FriendPredictionCount(ProgramSkillFrame f) => _resolve().FriendPredictionCount(f);
    internal IReadOnlyList<PromptChoice> FriendTrioChoices(ProgramSkillFrame f) => _resolve().FriendTrioChoices(f);
    internal void FriendTrioMutation(ProgramSkillFrame f, FriendTrioStage next, Action action) => _resolve().FriendTrioMutation(f, next, action);
    internal bool FriendXuanjianFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().FriendXuanjianFrameRidesOn(child, parent);
    internal bool FriendXuanjianIgnoresDistance(CharacterState owner, CardConversionSource? source) => _resolve().FriendXuanjianIgnoresDistance(owner, source);
    internal bool FriendXuanjianIgnoresDistanceQuery(ProgramSlashConversionDistanceQuery q) => _resolve().FriendXuanjianIgnoresDistanceQuery(q);
    internal void GainFriendMatchingDeckCards(ProgramSkillFrame f, string mode, string value, int count) => _resolve().GainFriendMatchingDeckCards(f, mode, value, count);
    internal IReadOnlyList<FriendPredictionView>? GetFriendPredictionViews(int? viewerSeat) => _resolve().GetFriendPredictionViews(viewerSeat);
    internal bool HasFriendTrioAlly(CharacterState owner, string id) => _resolve().HasFriendTrioAlly(owner, id);
    internal bool IsFriendActualCardUse(CardActionContext? action) => _resolve().IsFriendActualCardUse(action);
    internal void PublishFriendTrioChoice(ProgramSkillFrame f) => _resolve().PublishFriendTrioChoice(f);
    internal void ResolveFriendTrioChoice(PromptChoice choice) => _resolve().ResolveFriendTrioChoice(choice);
    internal bool ResumeFriendTrio(long id) => _resolve().ResumeFriendTrio(id);
    internal bool ReturnFriendTrioMovement(ProgramSkillFrame f) => _resolve().ReturnFriendTrioMovement(f);
    internal void ReturnFriendXuanjianUse(CardUseFrame use) => _resolve().ReturnFriendXuanjianUse(use);
    internal PromptChoice SelectAiFriendTrioChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiFriendTrioChoice(decision, frame);
    internal bool ValidWholeHandSuitConversion(CharacterState owner, IReadOnlyList<Card> cards, CardConversionSource source) => _resolve().ValidWholeHandSuitConversion(owner, cards, source);
}

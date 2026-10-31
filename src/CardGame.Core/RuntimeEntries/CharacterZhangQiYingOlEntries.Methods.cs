#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangQiYingOlEntries
{
    internal void AssertZhangQiYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertZhangQiYing(f);
    internal void AssertZhenyiJudgment(global::CardGame.Core.JudgmentFrame frame) => _resolve().AssertZhenyiJudgment(frame);
    internal bool CanContinueZhangQiYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueZhangQiYing(f);
    internal bool CanOfferZhangQiYing(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhangQiYing(candidate, trigger, context);
    internal bool CanPayZhenyiHoutu(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source) => _resolve().CanPayZhenyiHoutu(owner, source);
    internal int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback) => _resolve().EffectiveZhenyiJudgmentRank(frame, fallback);
    internal global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback) => _resolve().EffectiveZhenyiJudgmentSuit(frame, fallback);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteZhangQiYing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteZhangQiYing(supplied, op);
    internal global::CardGame.Core.CardLocation? FaluDiscardOwnedSource(global::CardGame.Core.CardMovementRecord movement) => _resolve().FaluDiscardOwnedSource(movement);
    internal int FaluTokenCount(int owner) => _resolve().FaluTokenCount(owner);
    internal bool HasZhangQiYingBeforeDamageObserver(long id) => _resolve().HasZhangQiYingBeforeDamageObserver(id);
    internal bool HasZhangQiYingDamageObserver(long id) => _resolve().HasZhangQiYingDamageObserver(id);
    internal bool IsZhangQiYingMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhangQiYingMovement(f, e, p);
    internal bool IsZhenyiHoutu(global::CardGame.Core.CardConversionSource source) => _resolve().IsZhenyiHoutu(source);
    internal void ObserveAcceptedZhenyiHoutu(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedZhenyiHoutu(accepted);
    internal void PayZhenyiHoutu(global::CardGame.Core.CardActionContext action) => _resolve().PayZhenyiHoutu(action);
    internal void PublishZhenyiJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhenyiJudgment(f);
    internal void ResolveZhenyiJudgment(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveZhenyiJudgment(choice);
    internal bool ResumeZhangQiYing(long id) => _resolve().ResumeZhangQiYing(id);
    internal bool ReturnZhangQiYingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangQiYingMovement(f);
    internal void SetFaluToken(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PlayerMarkerKind marker, int value, string reason) => _resolve().SetFaluToken(f, marker, value, reason);
    internal bool TryAdvanceZhangQiYingSubtree() => _resolve().TryAdvanceZhangQiYingSubtree();
    internal bool ZhangQiYingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangQiYingFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangQiYingObserverRoot() => _resolve().ZhangQiYingObserverRoot();
    internal global::CardGame.Core.PromptChoice ZhenyiAiJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhenyiAiJudgment(f);
    internal bool ZhenyiHoutuConversionUnavailable(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().ZhenyiHoutuConversionUnavailable(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhenyiJudgmentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhenyiJudgmentChoices(f);
    internal bool HasZhangQiYingCapability { get => _resolve().HasZhangQiYingCapability; }
}

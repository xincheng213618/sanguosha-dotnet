#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangQiYingOlEntries
{
    internal void AssertZhangQiYing(ProgramSkillFrame f) => _resolve().AssertZhangQiYing(f);
    internal void AssertZhenyiJudgment(JudgmentFrame frame) => _resolve().AssertZhenyiJudgment(frame);
    internal bool CanContinueZhangQiYing(ProgramSkillFrame f) => _resolve().CanContinueZhangQiYing(f);
    internal bool CanOfferZhangQiYing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhangQiYing(candidate, trigger, context);
    internal bool CanPayZhenyiHoutu(CharacterState owner, CardConversionSource source) => _resolve().CanPayZhenyiHoutu(owner, source);
    internal int EffectiveZhenyiJudgmentRank(JudgmentFrame frame, int fallback) => _resolve().EffectiveZhenyiJudgmentRank(frame, fallback);
    internal Suit EffectiveZhenyiJudgmentSuit(JudgmentFrame frame, Suit fallback) => _resolve().EffectiveZhenyiJudgmentSuit(frame, fallback);
    internal SkillProgramStepOutcome ExecuteZhangQiYing(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteZhangQiYing(supplied, op);
    internal CardLocation? FaluDiscardOwnedSource(CardMovementRecord movement) => _resolve().FaluDiscardOwnedSource(movement);
    internal int FaluTokenCount(int owner) => _resolve().FaluTokenCount(owner);
    internal bool IsZhenyiHoutu(CardConversionSource source) => _resolve().IsZhenyiHoutu(source);
    internal void ObserveAcceptedZhenyiHoutu(CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedZhenyiHoutu(accepted);
    internal void PayZhenyiHoutu(CardActionContext action) => _resolve().PayZhenyiHoutu(action);
    internal void PublishZhenyiJudgment(ProgramSkillFrame f) => _resolve().PublishZhenyiJudgment(f);
    internal void ResolveZhenyiJudgment(PromptChoice choice) => _resolve().ResolveZhenyiJudgment(choice);
    internal bool ResumeZhangQiYing(long id) => _resolve().ResumeZhangQiYing(id);
    internal bool ReturnZhangQiYingMovement(ProgramSkillFrame f) => _resolve().ReturnZhangQiYingMovement(f);
    internal void SetFaluToken(ProgramSkillFrame f, PlayerMarkerKind marker, int value, string reason) => _resolve().SetFaluToken(f, marker, value, reason);
    internal bool TryAdvanceZhangQiYingSubtree() => _resolve().TryAdvanceZhangQiYingSubtree();
    internal bool ZhangQiYingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangQiYingFirstChild(f, child);
    internal ProgramSkillFrame? ZhangQiYingObserverRoot() => _resolve().ZhangQiYingObserverRoot();
    internal PromptChoice ZhenyiAiJudgment(ProgramSkillFrame f) => _resolve().ZhenyiAiJudgment(f);
    internal bool ZhenyiHoutuConversionUnavailable(ProgramViewAsSourceQuery query) => _resolve().ZhenyiHoutuConversionUnavailable(query);
    internal IReadOnlyList<PromptChoice> ZhenyiJudgmentChoices(ProgramSkillFrame f) => _resolve().ZhenyiJudgmentChoices(f);
    internal bool HasZhangQiYingCapability { get => _resolve().HasZhangQiYingCapability; }
}

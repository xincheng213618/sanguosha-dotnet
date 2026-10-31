#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoOlSpEntries
{
    internal PromptChoice AiSpZhangLiao(PendingDecision d, ProgramSkillFrame f) => _resolve().AiSpZhangLiao(d, f);
    internal void AnswerSpZhangLiao(PromptChoice choice) => _resolve().AnswerSpZhangLiao(choice);
    internal SkillProgramStepOutcome BeginSpZhangLiao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginSpZhangLiao(supplied, op);
    internal bool CanOfferSpZhangLiao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferSpZhangLiao(c, t, context);
    internal void CleanupSpZhangLiao(ProgramSkillFrame f) => _resolve().CleanupSpZhangLiao(f);
    internal bool ContinueIssuedSpZhangLiao(ProgramSkillFrame f) => _resolve().ContinueIssuedSpZhangLiao(f);
    internal void ContinueSpZhangLiao(long id) => _resolve().ContinueSpZhangLiao(id);
    internal bool DrainSpZhangLiaoChildren(long id) => _resolve().DrainSpZhangLiaoChildren(id);
    internal bool HasDiaolingAwakened(int seat) => _resolve().HasDiaolingAwakened(seat);
    internal void MoveSpZhangLiaoCards(long id, IReadOnlyList<int> cards, CardLocation destination, string reason) => _resolve().MoveSpZhangLiaoCards(id, cards, destination, reason);
    internal IReadOnlyList<int> MubingAvailableGifts(ProgramSkillFrame f) => _resolve().MubingAvailableGifts(f);
    internal IReadOnlyList<int> MubingAvailableReveal(ProgramSkillFrame f) => _resolve().MubingAvailableReveal(f);
    internal int MubingMilitaryCount(int seat) => _resolve().MubingMilitaryCount(seat);
    internal void PublishSpZhangLiao(ProgramSkillFrame f) => _resolve().PublishSpZhangLiao(f);
    internal bool ResumeSpZhangLiao(long id) => _resolve().ResumeSpZhangLiao(id);
    internal IReadOnlyList<PromptChoice> SpZhangLiaoChoices(ProgramSkillFrame f) => _resolve().SpZhangLiaoChoices(f);
    internal bool SpZhangLiaoFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SpZhangLiaoFirstChild(root, child);
    internal string SpZhangLiaoMovementReason(ProgramSkillFrame f) => _resolve().SpZhangLiaoMovementReason(f);
    internal ProgramSkillFrame? SpZhangLiaoObserverRoot() => _resolve().SpZhangLiaoObserverRoot();
    internal CardConversionSource SpZhangLiaoSource(ProgramSkillFrame f) => _resolve().SpZhangLiaoSource(f);
    internal bool TryAdvanceSpZhangLiaoSubtree() => _resolve().TryAdvanceSpZhangLiaoSubtree();
}

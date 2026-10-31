#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoOlSpEntries
{
    internal global::CardGame.Core.PromptChoice AiSpZhangLiao(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiSpZhangLiao(d, f);
    internal void AnswerSpZhangLiao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSpZhangLiao(choice);
    internal void AssertSpZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSpZhangLiao(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSpZhangLiao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginSpZhangLiao(supplied, op);
    internal bool CanOfferSpZhangLiao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSpZhangLiao(c, t, context);
    internal void CleanupSpZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupSpZhangLiao(f);
    internal bool ContinueIssuedSpZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedSpZhangLiao(f);
    internal void ContinueSpZhangLiao(long id) => _resolve().ContinueSpZhangLiao(id);
    internal bool DrainSpZhangLiaoChildren(long id) => _resolve().DrainSpZhangLiaoChildren(id);
    internal bool HasDiaolingAwakened(int seat) => _resolve().HasDiaolingAwakened(seat);
    internal bool HasSpZhangLiaoDamageObserver(long id) => _resolve().HasSpZhangLiaoDamageObserver(id);
    internal bool IsSpZhangLiaoDying() => _resolve().IsSpZhangLiaoDying();
    internal void MoveSpZhangLiaoCards(long id, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.CardLocation destination, string reason) => _resolve().MoveSpZhangLiaoCards(id, cards, destination, reason);
    internal global::System.Collections.Generic.IReadOnlyList<int> MubingAvailableGifts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MubingAvailableGifts(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> MubingAvailableReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MubingAvailableReveal(f);
    internal int MubingMilitaryCount(int seat) => _resolve().MubingMilitaryCount(seat);
    internal void PublishSpZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSpZhangLiao(f);
    internal bool ResumeSpZhangLiao(long id) => _resolve().ResumeSpZhangLiao(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SpZhangLiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SpZhangLiaoChoices(f);
    internal bool SpZhangLiaoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SpZhangLiaoFirstChild(root, child);
    internal string SpZhangLiaoMovementReason(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SpZhangLiaoMovementReason(f);
    internal global::CardGame.Core.ProgramSkillFrame? SpZhangLiaoObserverRoot() => _resolve().SpZhangLiaoObserverRoot();
    internal global::CardGame.Core.CardConversionSource SpZhangLiaoSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SpZhangLiaoSource(f);
    internal bool TryAdvanceSpZhangLiaoSubtree() => _resolve().TryAdvanceSpZhangLiaoSubtree();
}

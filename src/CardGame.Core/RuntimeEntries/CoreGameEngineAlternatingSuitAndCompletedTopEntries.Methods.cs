#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopEntries
{
    internal int[] AvailableCompletedTopCosts(CardActionContext a, long parentFrameId) => _resolve().AvailableCompletedTopCosts(a, parentFrameId);
    internal SkillProgramStepOutcome BeginAlternatingSuitDrawDiscard(ProgramSkillFrame f) => _resolve().BeginAlternatingSuitDrawDiscard(f);
    internal SkillProgramStepOutcome BeginFirstCategoryCompletedTop(ProgramSkillFrame f) => _resolve().BeginFirstCategoryCompletedTop(f);
    internal void CommitAlternatingSuitTop(ProgramSkillFrame f) => _resolve().CommitAlternatingSuitTop(f);
    internal bool DistanceBorrowedSlash150HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash150HasPhaseSuitAllowance(q);
    internal bool DistanceCardUseExemption120HasPhaseSuitAllowance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption120HasPhaseSuitAllowance(q);
    internal bool DistancePhysicalSlash190HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash190HasPhaseSuitAllowance(q);
    internal bool DistanceVirtualSlash90HasPhaseSuitAllowance(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash90HasPhaseSuitAllowance(q);
    internal IReadOnlyList<IssuedPlayPhaseSuitUseAllowance>? GetIssuedPlayPhaseSuitUseAllowances(int seat) => _resolve().GetIssuedPlayPhaseSuitUseAllowances(seat);
    internal bool HasPhaseSuitAllowance(CharacterState owner, Card card) => _resolve().HasPhaseSuitAllowance(owner, card);
    internal bool HasPhaseSuitAllowance2(int seat, Suit? suit) => _resolve().HasPhaseSuitAllowance(seat, suit);
    internal bool IsFirstCategoryUse(int owner, string skill, CardActionContext action) => _resolve().IsFirstCategoryUse(owner, skill, action);
    internal bool IsLiveSuitAllowance(PlayPhaseSuitAllowanceGrantedEvent e, int seat) => _resolve().IsLiveSuitAllowance(e, seat);
    internal void ObserveFirstTurnCategoryUse(IGameEvent payload) => _resolve().ObserveFirstTurnCategoryUse(payload);
    internal Suit? PhysicalGroupSuit(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().PhysicalGroupSuit(owner, cards);
    internal void PublishAlternatingSuitTop(ProgramSkillFrame f) => _resolve().PublishAlternatingSuitTop(f);
    internal void RecordFirstTurnCategoryUse(CardActionContext action) => _resolve().RecordFirstTurnCategoryUse(action);
    internal void ResolveAlternatingSuitTopChoice(PromptChoice c) => _resolve().ResolveAlternatingSuitTopChoice(c);
    internal bool ResumeAlternatingSuitTop(long id) => _resolve().ResumeAlternatingSuitTop(id);
    internal SkillProgramStepOutcome ResumeAlternatingSuitTopImmediately(long id) => _resolve().ResumeAlternatingSuitTopImmediately(id);
}

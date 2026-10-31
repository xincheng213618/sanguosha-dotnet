#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingSuitAndCompletedTopEntries
{
    internal int[] AvailableCompletedTopCosts(global::CardGame.Core.CardActionContext a, long parentFrameId) => _resolve().AvailableCompletedTopCosts(a, parentFrameId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAlternatingSuitDrawDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginAlternatingSuitDrawDiscard(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFirstCategoryCompletedTop(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginFirstCategoryCompletedTop(f);
    internal void CommitAlternatingSuitTop(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CommitAlternatingSuitTop(f);
    internal bool DistanceBorrowedSlash150HasPhaseSuitAllowance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash150HasPhaseSuitAllowance(q);
    internal bool DistanceCardUseExemption120HasPhaseSuitAllowance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption120HasPhaseSuitAllowance(q);
    internal bool DistancePhysicalSlash190HasPhaseSuitAllowance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash190HasPhaseSuitAllowance(q);
    internal bool DistanceVirtualSlash90HasPhaseSuitAllowance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash90HasPhaseSuitAllowance(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IssuedPlayPhaseSuitUseAllowance>? GetIssuedPlayPhaseSuitUseAllowances(int seat) => _resolve().GetIssuedPlayPhaseSuitUseAllowances(seat);
    internal bool HasPhaseSuitAllowance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().HasPhaseSuitAllowance(owner, card);
    internal bool HasPhaseSuitAllowance2(int seat, global::CardGame.Core.Suit? suit) => _resolve().HasPhaseSuitAllowance(seat, suit);
    internal bool IsFirstCategoryUse(int owner, string skill, global::CardGame.Core.CardActionContext action) => _resolve().IsFirstCategoryUse(owner, skill, action);
    internal bool IsLiveSuitAllowance(global::CardGame.Core.PlayPhaseSuitAllowanceGrantedEvent e, int seat) => _resolve().IsLiveSuitAllowance(e, seat);
    internal void ObserveFirstTurnCategoryUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveFirstTurnCategoryUse(payload);
    internal global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().PhysicalGroupSuit(owner, cards);
    internal void PublishAlternatingSuitTop(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAlternatingSuitTop(f);
    internal void RecordFirstTurnCategoryUse(global::CardGame.Core.CardActionContext action) => _resolve().RecordFirstTurnCategoryUse(action);
    internal void ResolveAlternatingSuitTopChoice(global::CardGame.Core.PromptChoice c) => _resolve().ResolveAlternatingSuitTopChoice(c);
    internal bool ResumeAlternatingSuitTop(long id) => _resolve().ResumeAlternatingSuitTop(id);
    internal global::CardGame.Core.SkillProgramStepOutcome ResumeAlternatingSuitTopImmediately(long id) => _resolve().ResumeAlternatingSuitTopImmediately(id);
}

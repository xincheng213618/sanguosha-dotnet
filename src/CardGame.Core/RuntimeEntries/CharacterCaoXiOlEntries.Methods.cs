#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiOlEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> ActiveCaoXiGangshuUnits() => _resolve().ActiveCaoXiGangshuUnits();
    internal void AddCaoXiGangshuBonusAttackRangeContribution(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddCaoXiGangshuBonusAttackRangeContribution(player, contributions);
    internal bool AdvanceCaoXiSubtree() => _resolve().AdvanceCaoXiSubtree();
    internal void AnswerCaoXi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoXi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoXi(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginCaoXi(input, op);
    internal bool CanOfferCaoXi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCaoXi(c, t, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoXiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoXiChoices(f);
    internal bool CaoXiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoXiFirstChild(f, child);
    internal int CaoXiGangshuBonus(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().CaoXiGangshuBonus(player, query);
    internal global::CardGame.Core.ProgramSkillFrame? CaoXiObserverRoot() => _resolve().CaoXiObserverRoot();
    internal void CaptureCaoXiGangshuDrawUse(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> frozen) => _resolve().CaptureCaoXiGangshuDrawUse(player, frozen);
    internal void CaptureCaoXiGangshuSlashOffset(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureCaoXiGangshuSlashOffset(attack);
    internal void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated) => _resolve().CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated);
    internal global::CardGame.Core.PromptChoice ChooseCaoXiAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseCaoXiAi(p, f);
    internal void CompleteCaoXiNativeCounterspellBeforeUseCompleted(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().CompleteCaoXiNativeCounterspellBeforeUseCompleted(action, continuation);
    internal bool ContinuePaidCaoXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCaoXi(f);
    internal global::CardGame.Core.CaoXiNumbers CurrentCaoXiNumbers(int seat) => _resolve().CurrentCaoXiNumbers(seat);
    internal bool DrainCaoXiChildren(long id) => _resolve().DrainCaoXiChildren(id);
    internal void ExpireCaoXiGangshuUnits(int seat, string reason, long nativeId, global::System.Func<global::CardGame.Core.CaoXiGangshuIncreasedEvent, bool>? select = default) => _resolve().ExpireCaoXiGangshuUnits(seat, reason, nativeId, select);
    internal int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false) => _resolve().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> FreezeCaoXiGangshuDrawUse(global::CardGame.Core.CharacterState player) => _resolve().FreezeCaoXiGangshuDrawUse(player);
    internal bool HasCaoXiDamageObserver(long id) => _resolve().HasCaoXiDamageObserver(id);
    internal bool IsCaoXiDrawUnitEligible(global::CardGame.Core.CaoXiGangshuIncreasedEvent unit, global::CardGame.Core.ActualDiscardRecoveryPhaseKey? phase) => _resolve().IsCaoXiDrawUnitEligible(unit, phase);
    internal bool IsCaoXiDying() => _resolve().IsCaoXiDying();
    internal void IssueCaoXiDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueCaoXiDraw(f);
    internal void ObserveCaoXiGangshuExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveCaoXiGangshuExpiry(payload);
    internal void PublishCaoXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoXi(f);
    internal void RequireCaoXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireCaoXi(f);
    internal bool ResumeCaoXi(long id) => _resolve().ResumeCaoXi(id);
    internal bool ValidCaoXiGangshu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaoXiGangshu(f);
    internal bool ValidCaoXiJianxuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaoXiJianxuan(f);
    internal bool HasCaoXiCapability { get => _resolve().HasCaoXiCapability; }
    internal bool HasCaoXiGangshuCapability { get => _resolve().HasCaoXiGangshuCapability; }
}

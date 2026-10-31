#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiOlEntries
{
    internal IReadOnlyList<CaoXiGangshuIncreasedEvent> ActiveCaoXiGangshuUnits() => _resolve().ActiveCaoXiGangshuUnits();
    internal void AddCaoXiGangshuBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddCaoXiGangshuBonusAttackRangeContribution(player, contributions);
    internal bool AdvanceCaoXiSubtree() => _resolve().AdvanceCaoXiSubtree();
    internal void AnswerCaoXi(PromptChoice choice) => _resolve().AnswerCaoXi(choice);
    internal SkillProgramStepOutcome BeginCaoXi(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginCaoXi(input, op);
    internal bool CanOfferCaoXi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferCaoXi(c, t, context);
    internal IReadOnlyList<PromptChoice> CaoXiChoices(ProgramSkillFrame f) => _resolve().CaoXiChoices(f);
    internal bool CaoXiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaoXiFirstChild(f, child);
    internal int CaoXiGangshuBonus(CharacterState player, SkillRuleQuery query) => _resolve().CaoXiGangshuBonus(player, query);
    internal ProgramSkillFrame? CaoXiObserverRoot() => _resolve().CaoXiObserverRoot();
    internal void CaptureCaoXiGangshuDrawUse(CharacterState player, IReadOnlyList<CaoXiGangshuIncreasedEvent> frozen) => _resolve().CaptureCaoXiGangshuDrawUse(player, frozen);
    internal void CaptureCaoXiGangshuSlashOffset(NativeOwners.CardAttackHandle attack) => _resolve().CaptureCaoXiGangshuSlashOffset(attack);
    internal void CaptureCaoXiNativeCounterspellOffset(NullificationWindowFrame pending, CardActionContext action, bool invalidated) => _resolve().CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated);
    internal PromptChoice ChooseCaoXiAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseCaoXiAi(p, f);
    internal void CompleteCaoXiNativeCounterspellBeforeUseCompleted(CardActionContext action, ProgramCardContinuation continuation) => _resolve().CompleteCaoXiNativeCounterspellBeforeUseCompleted(action, continuation);
    internal bool ContinuePaidCaoXi(ProgramSkillFrame f) => _resolve().ContinuePaidCaoXi(f);
    internal CaoXiNumbers CurrentCaoXiNumbers(int seat) => _resolve().CurrentCaoXiNumbers(seat);
    internal bool DrainCaoXiChildren(long id) => _resolve().DrainCaoXiChildren(id);
    internal void ExpireCaoXiGangshuUnits(int seat, string reason, long nativeId, Func<CaoXiGangshuIncreasedEvent, bool>? select = default) => _resolve().ExpireCaoXiGangshuUnits(seat, reason, nativeId, select);
    internal int FreezeAndCaptureCaoXiNativeDrawBase(CharacterState player, bool replaced = false) => _resolve().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
    internal IReadOnlyList<CaoXiGangshuIncreasedEvent> FreezeCaoXiGangshuDrawUse(CharacterState player) => _resolve().FreezeCaoXiGangshuDrawUse(player);
    internal bool IsCaoXiDrawUnitEligible(CaoXiGangshuIncreasedEvent unit, ActualDiscardRecoveryPhaseKey? phase) => _resolve().IsCaoXiDrawUnitEligible(unit, phase);
    internal void IssueCaoXiDraw(ProgramSkillFrame f) => _resolve().IssueCaoXiDraw(f);
    internal void ObserveCaoXiGangshuExpiry(IGameEvent payload) => _resolve().ObserveCaoXiGangshuExpiry(payload);
    internal void PublishCaoXi(ProgramSkillFrame f) => _resolve().PublishCaoXi(f);
    internal bool ResumeCaoXi(long id) => _resolve().ResumeCaoXi(id);
    internal bool ValidCaoXiJianxuan(ProgramSkillFrame f) => _resolve().ValidCaoXiJianxuan(f);
    internal bool HasCaoXiCapability { get => _resolve().HasCaoXiCapability; }
    internal bool HasCaoXiGangshuCapability { get => _resolve().HasCaoXiGangshuCapability; }
}

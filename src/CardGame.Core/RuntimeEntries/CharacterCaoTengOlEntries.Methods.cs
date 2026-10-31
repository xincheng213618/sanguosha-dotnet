#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoTengOlEntries
{
    internal bool AdvanceCaoTengChildren() => _resolve().AdvanceCaoTengChildren();
    internal void AnswerCaoTeng(PromptChoice choice) => _resolve().AnswerCaoTeng(choice);
    internal void ApplyCaoTengFaction(ProgramSkillFrame f, string faction, bool setup) => _resolve().ApplyCaoTengFaction(f, faction, setup);
    internal SkillProgramStepOutcome BeginCaoTeng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginCaoTeng(supplied, op);
    internal bool CanOfferCaoTeng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferCaoTeng(c, t, context);
    internal IReadOnlyList<PromptChoice> CaoTengChoices(ProgramSkillFrame f) => _resolve().CaoTengChoices(f);
    internal int CaoTengChooser(ProgramSkillFrame f) => _resolve().CaoTengChooser(f);
    internal bool CaoTengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaoTengFirstChild(f, child);
    internal ProgramSkillFrame? CaoTengObserverRoot() => _resolve().CaoTengObserverRoot();
    internal bool CaoTengParentMatches(ProgramSkillFrame f) => _resolve().CaoTengParentMatches(f);
    internal int CaoTengYongzuHandLimitBonus(int seat) => _resolve().CaoTengYongzuHandLimitBonus(seat);
    internal PromptChoice ChooseCaoTengAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseCaoTengAi(p, f);
    internal void ContinueCaoTeng(long id) => _resolve().ContinueCaoTeng(id);
    internal bool ContinuePaidCaoTeng(ProgramSkillFrame f) => _resolve().ContinuePaidCaoTeng(f);
    internal bool DrainCaoTeng(long id) => _resolve().DrainCaoTeng(id);
    internal void ExpireCaoTengNextOwnTurn(TurnStartedEvent started) => _resolve().ExpireCaoTengNextOwnTurn(started);
    internal void FinishCaoTeng(ProgramSkillFrame f) => _resolve().FinishCaoTeng(f);
    internal void GrantCaoTengSkill(ProgramSkillFrame f, int recipient) => _resolve().GrantCaoTengSkill(f, recipient);
    internal bool IsCaoTengFactionChange(ProgramSkillFrame f, string faction) => _resolve().IsCaoTengFactionChange(f, faction);
    internal bool IsCaoTengQingliuDyingExitContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsCaoTengQingliuDyingExitContext(candidate, context);
    internal bool IsCaoTengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsCaoTengRecoveryParent(f, recovery);
    internal void PublishCaoTeng(ProgramSkillFrame f) => _resolve().PublishCaoTeng(f);
    internal bool ResumeCaoTeng(long id) => _resolve().ResumeCaoTeng(id);
    internal bool ValidCaoTeng(ProgramSkillFrame f) => _resolve().ValidCaoTeng(f);
    internal bool HasCaoTengCapability { get => _resolve().HasCaoTengCapability; }
}

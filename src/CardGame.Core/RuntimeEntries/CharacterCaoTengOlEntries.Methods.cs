#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoTengOlEntries
{
    internal bool AdvanceCaoTengChildren() => _resolve().AdvanceCaoTengChildren();
    internal void AnswerCaoTeng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoTeng(choice);
    internal void ApplyCaoTengFaction(global::CardGame.Core.ProgramSkillFrame f, string faction, bool setup) => _resolve().ApplyCaoTengFaction(f, faction, setup);
    internal void AssertCaoTeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertCaoTeng(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoTeng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginCaoTeng(supplied, op);
    internal bool CanOfferCaoTeng(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCaoTeng(c, t, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoTengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoTengChoices(f);
    internal int CaoTengChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoTengChooser(f);
    internal bool CaoTengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoTengFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? CaoTengObserverRoot() => _resolve().CaoTengObserverRoot();
    internal bool CaoTengParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoTengParentMatches(f);
    internal int CaoTengYongzuHandLimitBonus(int seat) => _resolve().CaoTengYongzuHandLimitBonus(seat);
    internal global::CardGame.Core.PromptChoice ChooseCaoTengAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseCaoTengAi(p, f);
    internal void ContinueCaoTeng(long id) => _resolve().ContinueCaoTeng(id);
    internal bool ContinuePaidCaoTeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCaoTeng(f);
    internal bool DrainCaoTeng(long id) => _resolve().DrainCaoTeng(id);
    internal void ExpireCaoTengNextOwnTurn(global::CardGame.Core.TurnStartedEvent started) => _resolve().ExpireCaoTengNextOwnTurn(started);
    internal void FinishCaoTeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaoTeng(f);
    internal void GrantCaoTengSkill(global::CardGame.Core.ProgramSkillFrame f, int recipient) => _resolve().GrantCaoTengSkill(f, recipient);
    internal bool HasCaoTengBeforeDamageObserver(long id) => _resolve().HasCaoTengBeforeDamageObserver(id);
    internal bool HasCaoTengDamageObserver(long id) => _resolve().HasCaoTengDamageObserver(id);
    internal bool IsCaoTengDying() => _resolve().IsCaoTengDying();
    internal bool IsCaoTengFactionChange(global::CardGame.Core.ProgramSkillFrame f, string faction) => _resolve().IsCaoTengFactionChange(f, faction);
    internal bool IsCaoTengQingliuDyingExitContext(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsCaoTengQingliuDyingExitContext(candidate, context);
    internal bool IsCaoTengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsCaoTengRecoveryParent(f, recovery);
    internal void PublishCaoTeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoTeng(f);
    internal bool ResumeCaoTeng(long id) => _resolve().ResumeCaoTeng(id);
    internal bool ValidCaoTeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaoTeng(f);
    internal bool HasCaoTengCapability { get => _resolve().HasCaoTengCapability; }
}

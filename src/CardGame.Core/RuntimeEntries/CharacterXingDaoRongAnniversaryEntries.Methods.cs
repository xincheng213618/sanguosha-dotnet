#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXingDaoRongAnniversaryEntries
{
    internal bool AdvanceXingDaoRongObserver() => _resolve().AdvanceXingDaoRongObserver();
    internal void AnswerXingDaoRong(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerXingDaoRong(answer);
    internal void AssertXingDaoRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertXingDaoRong(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXingDaoRong(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginXingDaoRong(supplied);
    internal bool ContinueXingDaoRong(long id) => _resolve().ContinueXingDaoRong(id);
    internal bool DrainXingDaoRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainXingDaoRong(f);
    internal bool HasAnniversaryXingDaoRongPrompt() => _resolve().HasAnniversaryXingDaoRongPrompt();
    internal bool KeepPaidXingDaoRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidXingDaoRong(f);
    internal void PublishXingDaoRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXingDaoRong(f);
    internal bool QualifyXingDaoRong(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().QualifyXingDaoRong(g);
    internal void ReturnXingDaoRongDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnXingDaoRongDraw(supplied, outcome);
    internal bool ReturnXingDaoRongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXingDaoRongMovement(f);
    internal bool ValidXingDaoRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidXingDaoRong(f);
    internal global::CardGame.Core.PromptChoice XingDaoRongAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().XingDaoRongAi(p, f);
    internal bool XingDaoRongBeforeDamageObserver(long id) => _resolve().XingDaoRongBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XingDaoRongCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XingDaoRongCards(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XingDaoRongChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XingDaoRongChoices(f);
    internal bool XingDaoRongDamageObserver(long id) => _resolve().XingDaoRongDamageObserver(id);
    internal bool XingDaoRongDyingObserver() => _resolve().XingDaoRongDyingObserver();
    internal bool XingDaoRongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().XingDaoRongFirstChild(f, child);
    internal bool XingDaoRongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().XingDaoRongMovement(f, e, p);
    internal int[] XingDaoRongNearby(int owner) => _resolve().XingDaoRongNearby(owner);
    internal global::CardGame.Core.ProgramSkillFrame? XingDaoRongObserverRoot() => _resolve().XingDaoRongObserverRoot();
    internal bool XingDaoRongObserverSuffix(int i) => _resolve().XingDaoRongObserverSuffix(i);
    internal bool XingDaoRongOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XingDaoRongOriginal(f);
    internal bool XingDaoRongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame r) => _resolve().XingDaoRongRecoveryParent(f, r);
}

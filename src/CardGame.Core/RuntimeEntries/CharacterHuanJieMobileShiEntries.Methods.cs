#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuanJieMobileShiEntries
{
    internal bool AdvanceShiHjChildren() => _resolve().AdvanceShiHjChildren();
    internal void AnswerShiHj(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiHj(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiHj(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiHj(input, op);
    internal bool CanOfferShiHj(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().CanOfferShiHj(q);
    internal global::CardGame.Core.PromptChoice ChooseShiHjAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiHjAi(p, f);
    internal void ContinueShiHj(long id) => _resolve().ContinueShiHj(id);
    internal bool DrainShiHj(long id) => _resolve().DrainShiHj(id);
    internal void FinishShiHj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiHj(f);
    internal void GrantShiHjPaidTurnSkills(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GrantShiHjPaidTurnSkills(f);
    internal bool KeepPaidShiHj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShiHj(f);
    internal void PublishShiHj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiHj(f);
    internal void RequireShiHj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShiHj(f);
    internal bool ResumeShiHj(long id) => _resolve().ResumeShiHj(id);
    internal void ReturnShiHjDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiHjDraw(f, outcome);
    internal bool ReturnShiHjMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiHjMovement(f);
    internal void SealShiHj(long id) => _resolve().SealShiHj(id);
    internal bool ShiHjBeforeDamageChild(long id) => _resolve().ShiHjBeforeDamageChild(id);
    internal bool ShiHjChildEdge(int i) => _resolve().ShiHjChildEdge(i);
    internal bool ShiHjChildSuffix(int n) => _resolve().ShiHjChildSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiHjChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiHjChoices(f);
    internal bool ShiHjDamageChild(long id) => _resolve().ShiHjDamageChild(id);
    internal bool ShiHjDyingChild() => _resolve().ShiHjDyingChild();
    internal bool ShiHjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiHjFirstChild(f, child);
    internal bool ShiHjMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiHjMovementProof(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiHjObserverRoot() => _resolve().ShiHjObserverRoot();
    internal bool ShiHjRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiHjRecoveryParent(f, recovery);
}

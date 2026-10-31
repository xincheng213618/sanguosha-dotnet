#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinXianYingMobileShiEntries
{
    internal bool AdvanceShiXxyChildren() => _resolve().AdvanceShiXxyChildren();
    internal void AnswerShiXxy(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiXxy(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiXxy(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op, int actor) => _resolve().BeginShiXxy(supplied, op, actor);
    internal void CompleteShiXxyJiejie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteShiXxyJiejie(f);
    internal void ContinueShiXxy(long id) => _resolve().ContinueShiXxy(id);
    internal bool DrainShiXxy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiXxy(f);
    internal void FinishShiXxy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiXxy(f);
    internal bool HasShiXxySuitAllowance(int actor, global::CardGame.Core.Suit? suit) => _resolve().HasShiXxySuitAllowance(actor, suit);
    internal void IssueShiXxyQingshi(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().IssueShiXxyQingshi(f, target);
    internal bool PaidShiXxy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaidShiXxy(f);
    internal void PublishShiXxy(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PublishShiXxy(supplied);
    internal bool ResumeShiXxy(long id) => _resolve().ResumeShiXxy(id);
    internal void ReturnShiXxyDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiXxyDraw(f, outcome);
    internal bool ReturnShiXxyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiXxyMovement(f);
    internal void SealShiXxy(long id) => _resolve().SealShiXxy(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiXinXianYingPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().ShiXinXianYingPublicStates(owner, skillId);
    internal global::CardGame.Core.PromptChoice ShiXxyAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXxyAi(p, f);
    internal bool ShiXxyAlcoholDebit(long id, int actor) => _resolve().ShiXxyAlcoholDebit(id, actor);
    internal bool ShiXxyBeforeDamageChild(long id) => _resolve().ShiXxyBeforeDamageChild(id);
    internal bool ShiXxyChildEdge(int i) => _resolve().ShiXxyChildEdge(i);
    internal bool ShiXxyChildSuffix(int n) => _resolve().ShiXxyChildSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiXxyChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiXxyChoices(f);
    internal bool ShiXxyDamageChild(long id) => _resolve().ShiXxyDamageChild(id);
    internal bool ShiXxyDyingChild() => _resolve().ShiXxyDyingChild();
    internal bool ShiXxyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiXxyFirstChild(f, child);
    internal bool ShiXxyMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiXxyMovementProof(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiXxyObserverRoot() => _resolve().ShiXxyObserverRoot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> ShiXxyPrivateCards(int viewer) => _resolve().ShiXxyPrivateCards(viewer);
    internal bool ShiXxyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiXxyRecoveryParent(f, recovery);
    internal int ShiXxyRoundMask(int owner, int actor) => _resolve().ShiXxyRoundMask(owner, actor);
    internal int ShiXxyRoundUsed(int owner) => _resolve().ShiXxyRoundUsed(owner);
    internal bool ShiXxySameCamp(int owner, int target) => _resolve().ShiXxySameCamp(owner, target);
    internal bool ShiXxySlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().ShiXxySlashQuota(q);
}

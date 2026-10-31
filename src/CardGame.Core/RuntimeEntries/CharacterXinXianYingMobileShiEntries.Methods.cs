#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXinXianYingMobileShiEntries
{
    internal bool AdvanceShiXxyChildren() => _resolve().AdvanceShiXxyChildren();
    internal void AnswerShiXxy(PromptChoice choice) => _resolve().AnswerShiXxy(choice);
    internal SkillProgramStepOutcome BeginShiXxy(ProgramSkillFrame supplied, SkillProgramEffectOp op, int actor) => _resolve().BeginShiXxy(supplied, op, actor);
    internal void CompleteShiXxyJiejie(ProgramSkillFrame f) => _resolve().CompleteShiXxyJiejie(f);
    internal void ContinueShiXxy(long id) => _resolve().ContinueShiXxy(id);
    internal bool DrainShiXxy(ProgramSkillFrame f) => _resolve().DrainShiXxy(f);
    internal void FinishShiXxy(ProgramSkillFrame f) => _resolve().FinishShiXxy(f);
    internal bool HasShiXxySuitAllowance(int actor, Suit? suit) => _resolve().HasShiXxySuitAllowance(actor, suit);
    internal void IssueShiXxyQingshi(ProgramSkillFrame f, int target) => _resolve().IssueShiXxyQingshi(f, target);
    internal bool PaidShiXxy(ProgramSkillFrame f) => _resolve().PaidShiXxy(f);
    internal void PublishShiXxy(ProgramSkillFrame supplied) => _resolve().PublishShiXxy(supplied);
    internal bool ResumeShiXxy(long id) => _resolve().ResumeShiXxy(id);
    internal void ReturnShiXxyDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiXxyDraw(f, outcome);
    internal bool ReturnShiXxyMovement(ProgramSkillFrame f) => _resolve().ReturnShiXxyMovement(f);
    internal void SealShiXxy(long id) => _resolve().SealShiXxy(id);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> ShiXinXianYingPublicStates(CharacterState owner, string skillId) => _resolve().ShiXinXianYingPublicStates(owner, skillId);
    internal PromptChoice ShiXxyAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ShiXxyAi(p, f);
    internal bool ShiXxyAlcoholDebit(long id, int actor) => _resolve().ShiXxyAlcoholDebit(id, actor);
    internal bool ShiXxyChildEdge(int i) => _resolve().ShiXxyChildEdge(i);
    internal bool ShiXxyChildSuffix(int n) => _resolve().ShiXxyChildSuffix(n);
    internal IReadOnlyList<PromptChoice> ShiXxyChoices(ProgramSkillFrame f) => _resolve().ShiXxyChoices(f);
    internal bool ShiXxyDyingChild() => _resolve().ShiXxyDyingChild();
    internal bool ShiXxyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiXxyFirstChild(f, child);
    internal bool ShiXxyMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().ShiXxyMovementProof(f, effect, p);
    internal ProgramSkillFrame? ShiXxyObserverRoot() => _resolve().ShiXxyObserverRoot();
    internal IEnumerable<CardSnapshot> ShiXxyPrivateCards(int viewer) => _resolve().ShiXxyPrivateCards(viewer);
    internal bool ShiXxyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiXxyRecoveryParent(f, recovery);
    internal int ShiXxyRoundMask(int owner, int actor) => _resolve().ShiXxyRoundMask(owner, actor);
    internal int ShiXxyRoundUsed(int owner) => _resolve().ShiXxyRoundUsed(owner);
    internal bool ShiXxySameCamp(int owner, int target) => _resolve().ShiXxySameCamp(owner, target);
    internal bool ShiXxySlashQuota(ProgramSlashQuotaQuery q) => _resolve().ShiXxySlashQuota(q);
}

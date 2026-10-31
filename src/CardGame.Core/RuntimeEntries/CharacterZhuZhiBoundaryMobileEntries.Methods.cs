#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuZhiBoundaryMobileEntries
{
    internal void AnswerBoundaryAnguo(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryAnguo(choice);
    internal void ApplyBoundaryAnguoPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ApplyBoundaryAnguoPayment(frame);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyBoundaryZhuZhiHandLimit(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ApplyBoundaryZhuZhiHandLimit(player, ordinary);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryAnguo(global::CardGame.Core.ProgramSkillFrame supplied, bool rescue) => _resolve().BeginBoundaryAnguo(supplied, rescue);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryAnguoPayment(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryAnguoPayment(supplied);
    internal global::CardGame.Core.PromptChoice BoundaryAnguoAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryAnguoAi(decision, frame);
    internal global::CardGame.Core.PromptChoice BoundaryAnguoChoice(global::CardGame.Core.ProgramSkillFrame frame, string branch, int previous, int target, string label) => _resolve().BoundaryAnguoChoice(frame, branch, previous, target, label);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryAnguoChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryAnguoChoices(frame);
    internal int[] BoundaryAnguoNewRecipients(int owner) => _resolve().BoundaryAnguoNewRecipients(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> BoundaryZhuZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().BoundaryZhuZhiPublicStates(owner, skillId);
    internal bool CanOfferBoundaryAnguo(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryAnguo(gate);
    internal bool ContinueBoundaryAnguoPayment(long id) => _resolve().ContinueBoundaryAnguoPayment(id);
    internal bool HadBoundaryAnguoMark(int seat) => _resolve().HadBoundaryAnguoMark(seat);
    internal bool HasBoundaryAnguoMark(int seat) => _resolve().HasBoundaryAnguoMark(seat);
    internal bool HasExactBoundaryAnguoRescueTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactBoundaryAnguoRescueTarget(frame);
    internal bool KeepIssuedBoundaryAnguo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryAnguo(frame);
    internal void PublishBoundaryAnguo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryAnguo(frame);
    internal void RecordBoundaryAnguoPayment(long id) => _resolve().RecordBoundaryAnguoPayment(id);
    internal bool ReturnBoundaryAnguoPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryAnguoPayment(frame);
    internal bool ValidBoundaryAnguo(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryAnguo(frame);
}

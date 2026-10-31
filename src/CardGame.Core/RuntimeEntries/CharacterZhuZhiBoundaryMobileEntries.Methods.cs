#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuZhiBoundaryMobileEntries
{
    internal void AnswerBoundaryAnguo(PromptChoice choice) => _resolve().AnswerBoundaryAnguo(choice);
    internal void ApplyBoundaryAnguoPayment(ProgramSkillFrame frame) => _resolve().ApplyBoundaryAnguoPayment(frame);
    internal RuleQueryEvaluation ApplyBoundaryZhuZhiHandLimit(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ApplyBoundaryZhuZhiHandLimit(player, ordinary);
    internal SkillProgramStepOutcome BeginBoundaryAnguo(ProgramSkillFrame supplied, bool rescue) => _resolve().BeginBoundaryAnguo(supplied, rescue);
    internal SkillProgramStepOutcome BeginBoundaryAnguoPayment(ProgramSkillFrame supplied) => _resolve().BeginBoundaryAnguoPayment(supplied);
    internal PromptChoice BoundaryAnguoAi(PendingDecision decision, ProgramSkillFrame frame) => _resolve().BoundaryAnguoAi(decision, frame);
    internal PromptChoice BoundaryAnguoChoice(ProgramSkillFrame frame, string branch, int previous, int target, string label) => _resolve().BoundaryAnguoChoice(frame, branch, previous, target, label);
    internal IReadOnlyList<PromptChoice> BoundaryAnguoChoices(ProgramSkillFrame frame) => _resolve().BoundaryAnguoChoices(frame);
    internal int[] BoundaryAnguoNewRecipients(int owner) => _resolve().BoundaryAnguoNewRecipients(owner);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryZhuZhiPublicStates(CharacterState owner, string skillId) => _resolve().BoundaryZhuZhiPublicStates(owner, skillId);
    internal bool CanOfferBoundaryAnguo(ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryAnguo(gate);
    internal bool ContinueBoundaryAnguoPayment(long id) => _resolve().ContinueBoundaryAnguoPayment(id);
    internal bool HadBoundaryAnguoMark(int seat) => _resolve().HadBoundaryAnguoMark(seat);
    internal bool HasBoundaryAnguoMark(int seat) => _resolve().HasBoundaryAnguoMark(seat);
    internal bool HasExactBoundaryAnguoRescueTarget(ProgramSkillFrame frame) => _resolve().HasExactBoundaryAnguoRescueTarget(frame);
    internal bool KeepIssuedBoundaryAnguo(ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryAnguo(frame);
    internal void PublishBoundaryAnguo(ProgramSkillFrame frame) => _resolve().PublishBoundaryAnguo(frame);
    internal void RecordBoundaryAnguoPayment(long id) => _resolve().RecordBoundaryAnguoPayment(id);
    internal bool ReturnBoundaryAnguoPayment(ProgramSkillFrame frame) => _resolve().ReturnBoundaryAnguoPayment(frame);
    internal bool ValidBoundaryAnguo(ProgramSkillFrame frame) => _resolve().ValidBoundaryAnguo(frame);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhaoMobileShiEntries
{
    internal void AnswerShiDz(PromptChoice choice) => _resolve().AnswerShiDz(choice);
    internal SkillProgramStepOutcome BeginShiDz(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginShiDz(input, op);
    internal bool CanOfferShiDz(ProgramTriggerLegalityContext q) => _resolve().CanOfferShiDz(q);
    internal PromptChoice ChooseShiDzAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiDzAi(p, f);
    internal void ContinueShiDz(long id) => _resolve().ContinueShiDz(id);
    internal void FinishShiDz(ProgramSkillFrame f) => _resolve().FinishShiDz(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiDzExtra(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiDzExtra(owner, window, ordinary);
    internal bool IsIssuedShiDzExtra(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedShiDzExtra(c, w);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiDongZhaoPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiDongZhaoPublicStates(owner, skillId);
    internal bool PaidShiDzCanReturn(ProgramSkillFrame f) => _resolve().PaidShiDzCanReturn(f);
    internal void PublishShiDz(ProgramSkillFrame f) => _resolve().PublishShiDz(f);
    internal bool ResumeShiDz(long id) => _resolve().ResumeShiDz(id);
    internal bool ReturnShiDz(ProgramSkillFrame f) => _resolve().ReturnShiDz(f);
    internal void SealShiDz(long id) => _resolve().SealShiDz(id);
    internal IReadOnlyList<PromptChoice> ShiDzChoices(ProgramSkillFrame f) => _resolve().ShiDzChoices(f);
    internal IReadOnlyList<PromptChoice> ShiDzPayments(ProgramSkillFrame f) => _resolve().ShiDzPayments(f);
    internal MobileShiDongZhaoExtraGrantedEvent? ShiDzPendingExtra(int beneficiary) => _resolve().ShiDzPendingExtra(beneficiary);
    internal int ShiDzSameNameMaximum(int seat) => _resolve().ShiDzSameNameMaximum(seat);
}

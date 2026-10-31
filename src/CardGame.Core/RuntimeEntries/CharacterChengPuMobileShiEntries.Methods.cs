#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengPuMobileShiEntries
{
    internal void AdvanceShiCpBihanReduction(ProgramSkillFrame f) => _resolve().AdvanceShiCpBihanReduction(f);
    internal void AnswerMobileShiBihan(PromptChoice selected) => _resolve().AnswerMobileShiBihan(selected);
    internal void AnswerMobileShiDuzuo(PromptChoice selected) => _resolve().AnswerMobileShiDuzuo(selected);
    internal SkillProgramStepOutcome BeginMobileShiBihanGuardProgram(ProgramSkillFrame frame) => _resolve().BeginMobileShiBihanGuardProgram(frame);
    internal SkillProgramStepOutcome BeginMobileShiDuzuoGrantProgram(ProgramSkillFrame frame) => _resolve().BeginMobileShiDuzuoGrantProgram(frame);
    internal void BindShiCpBihanDiscarder(ProgramSkillFrame f, int seat) => _resolve().BindShiCpBihanDiscarder(f, seat);
    internal bool CanOfferMobileShiChengPu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiChengPu(candidate, trigger, context);
    internal PromptChoice ChooseShiCpBihan(PendingDecision prompt) => _resolve().ChooseShiCpBihan(prompt);
    internal PromptChoice ChooseShiCpDuzuo(PendingDecision prompt) => _resolve().ChooseShiCpDuzuo(prompt);
    internal void ContinueMobileShiBihan(long id) => _resolve().ContinueMobileShiBihan(id);
    internal void ContinueMobileShiDuzuo(long id) => _resolve().ContinueMobileShiDuzuo(id);
    internal bool ContinuePaidShiCpBihan(ProgramSkillFrame f) => _resolve().ContinuePaidShiCpBihan(f);
    internal bool DrainShiCpBihan(ProgramSkillFrame f) => _resolve().DrainShiCpBihan(f);
    internal int[] MobileShiBihanDiscarderSeats(ProgramSkillFrame f) => _resolve().MobileShiBihanDiscarderSeats(f);
    internal Card? MobileShiChengPuFireSlash() => _resolve().MobileShiChengPuFireSlash();
    internal void PublishShiCpBihanDiscard(ProgramSkillFrame f) => _resolve().PublishShiCpBihanDiscard(f);
    internal void PublishShiCpBihanDiscarder(ProgramSkillFrame f) => _resolve().PublishShiCpBihanDiscarder(f);
    internal void PublishShiCpDuzuoRecipient(ProgramSkillFrame f) => _resolve().PublishShiCpDuzuoRecipient(f);
    internal bool ResumeShiCpChengPu(long id) => _resolve().ResumeShiCpChengPu(id);
    internal bool ReturnShiCpChengPuMovement(ProgramSkillFrame f) => _resolve().ReturnShiCpChengPuMovement(f);
    internal IReadOnlyList<PromptChoice> ShiCpBihanDiscardChoices(ProgramSkillFrame f) => _resolve().ShiCpBihanDiscardChoices(f);
    internal IReadOnlyList<PromptChoice> ShiCpBihanDiscarderChoices(ProgramSkillFrame f) => _resolve().ShiCpBihanDiscarderChoices(f);
    internal IReadOnlyList<PromptChoice> ShiCpDuzuoChoices(ProgramSkillFrame f) => _resolve().ShiCpDuzuoChoices(f);
}

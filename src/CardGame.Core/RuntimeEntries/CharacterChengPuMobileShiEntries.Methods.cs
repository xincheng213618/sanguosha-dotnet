#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengPuMobileShiEntries
{
    internal void AdvanceShiCpBihanReduction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AdvanceShiCpBihanReduction(f);
    internal void AnswerMobileShiBihan(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileShiBihan(selected);
    internal void AnswerMobileShiDuzuo(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileShiDuzuo(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileShiBihanGuardProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginMobileShiBihanGuardProgram(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileShiDuzuoGrantProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginMobileShiDuzuoGrantProgram(frame);
    internal void BindShiCpBihanDiscarder(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().BindShiCpBihanDiscarder(f, seat);
    internal bool CanOfferMobileShiChengPu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiChengPu(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseShiCpBihan(global::CardGame.Core.PendingDecision prompt) => _resolve().ChooseShiCpBihan(prompt);
    internal global::CardGame.Core.PromptChoice ChooseShiCpDuzuo(global::CardGame.Core.PendingDecision prompt) => _resolve().ChooseShiCpDuzuo(prompt);
    internal void ContinueMobileShiBihan(long id) => _resolve().ContinueMobileShiBihan(id);
    internal void ContinueMobileShiDuzuo(long id) => _resolve().ContinueMobileShiDuzuo(id);
    internal bool ContinuePaidShiCpBihan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShiCpBihan(f);
    internal bool DrainShiCpBihan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiCpBihan(f);
    internal int[] MobileShiBihanDiscarderSeats(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiBihanDiscarderSeats(f);
    internal global::CardGame.Core.Card? MobileShiChengPuFireSlash() => _resolve().MobileShiChengPuFireSlash();
    internal void PublishShiCpBihanDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCpBihanDiscard(f);
    internal void PublishShiCpBihanDiscarder(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCpBihanDiscarder(f);
    internal void PublishShiCpDuzuoRecipient(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCpDuzuoRecipient(f);
    internal bool ResumeShiCpChengPu(long id) => _resolve().ResumeShiCpChengPu(id);
    internal bool ReturnShiCpChengPuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiCpChengPuMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCpBihanDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCpBihanDiscardChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCpBihanDiscarderChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCpBihanDiscarderChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCpDuzuoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCpDuzuoChoices(f);
}

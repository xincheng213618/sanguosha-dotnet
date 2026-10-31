#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMobileEntries
{
    internal SkillProgramStepOutcome BeginRecipientRandomTransfer(ProgramSkillFrame frame, int recipient, string bind, Card card) => _resolve().BeginRecipientRandomTransfer(frame, recipient, bind, card);
    internal void ContinueRecipientRandomTransfer(long id) => _resolve().ContinueRecipientRandomTransfer(id);
    internal bool KeepPaidRecipientRandomTransfer(ProgramSkillFrame frame) => _resolve().KeepPaidRecipientRandomTransfer(frame);
    internal SkillProgramRandomTransferRevealPolicy? RandomTransferRevealPolicy(ProgramSkillFrame frame, string bind) => _resolve().RandomTransferRevealPolicy(frame, bind);
    internal bool ResumeRecipientRandomTransfer(long id) => _resolve().ResumeRecipientRandomTransfer(id);
    internal bool ReturnRecipientRandomTransferMovement(ProgramSkillFrame frame) => _resolve().ReturnRecipientRandomTransferMovement(frame);
    internal void ReturnRecipientRandomTransferReveal(ProgramSkillFrame frame) => _resolve().ReturnRecipientRandomTransferReveal(frame);
}

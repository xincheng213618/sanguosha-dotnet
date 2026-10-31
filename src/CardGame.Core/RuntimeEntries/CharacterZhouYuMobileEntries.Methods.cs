#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMobileEntries
{
    internal void AssertRandomOwnedTransfer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRandomOwnedTransfer(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRecipientRandomTransfer(global::CardGame.Core.ProgramSkillFrame frame, int recipient, string bind, global::CardGame.Core.Card card) => _resolve().BeginRecipientRandomTransfer(frame, recipient, bind, card);
    internal void ContinueRecipientRandomTransfer(long id) => _resolve().ContinueRecipientRandomTransfer(id);
    internal bool KeepPaidRecipientRandomTransfer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepPaidRecipientRandomTransfer(frame);
    internal global::CardGame.Core.SkillProgramRandomTransferRevealPolicy? RandomTransferRevealPolicy(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().RandomTransferRevealPolicy(frame, bind);
    internal bool ResumeRecipientRandomTransfer(long id) => _resolve().ResumeRecipientRandomTransfer(id);
    internal bool ReturnRecipientRandomTransferMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnRecipientRandomTransferMovement(frame);
    internal void ReturnRecipientRandomTransferReveal(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnRecipientRandomTransferReveal(frame);
}

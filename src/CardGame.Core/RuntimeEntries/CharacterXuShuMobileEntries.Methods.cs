#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShuMobileEntries
{
    internal void AnswerMobileSpXuShuJianyan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileSpXuShuJianyan(choice);
    internal void AssertMobileSpXuShuJianyan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileSpXuShuJianyan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileSpXuShuJianyan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMobileSpXuShuJianyan(f);
    internal void BeginMobileSpXuShuJianyanCleanup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMobileSpXuShuJianyanCleanup(f);
    internal void CaptureMobileSpXuShuJianyanNativeBatch(long id) => _resolve().CaptureMobileSpXuShuJianyanNativeBatch(id);
    internal bool KeepMobileSpXuShuJianyanIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileSpXuShuJianyanIssued(f);
    internal global::CardGame.Core.PromptChoice MobileSpXuShuJianyanAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileSpXuShuJianyanAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileSpXuShuJianyanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileSpXuShuJianyanChoices(f);
    internal bool MobileSpXuShuJianyanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MobileSpXuShuJianyanMovement(f, e, p);
    internal void PublishMobileSpXuShuJianyan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileSpXuShuJianyan(f);
    internal void PublishOrFinishMobileSpXuShuJianyanRecipient(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOrFinishMobileSpXuShuJianyanRecipient(f);
    internal void RecordMobileSpXuShuJianyanMovement(long id, string kind, long batch, long before, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> records) => _resolve().RecordMobileSpXuShuJianyanMovement(id, kind, batch, before, records);
    internal bool ResumeMobileSpXuShuJianyan(long id) => _resolve().ResumeMobileSpXuShuJianyan(id);
    internal bool ReturnMobileSpXuShuJianyanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileSpXuShuJianyanMovement(f);
    internal bool ValidMobileSpXuShuJianyanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileSpXuShuJianyanMovement(f);
}

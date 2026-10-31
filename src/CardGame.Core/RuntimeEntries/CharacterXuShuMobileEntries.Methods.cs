#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShuMobileEntries
{
    internal void AnswerMobileSpXuShuJianyan(PromptChoice choice) => _resolve().AnswerMobileSpXuShuJianyan(choice);
    internal SkillProgramStepOutcome BeginMobileSpXuShuJianyan(ProgramSkillFrame f) => _resolve().BeginMobileSpXuShuJianyan(f);
    internal void BeginMobileSpXuShuJianyanCleanup(ProgramSkillFrame f) => _resolve().BeginMobileSpXuShuJianyanCleanup(f);
    internal void CaptureMobileSpXuShuJianyanNativeBatch(long id) => _resolve().CaptureMobileSpXuShuJianyanNativeBatch(id);
    internal bool KeepMobileSpXuShuJianyanIssued(ProgramSkillFrame f) => _resolve().KeepMobileSpXuShuJianyanIssued(f);
    internal PromptChoice MobileSpXuShuJianyanAi(PendingDecision p, ProgramSkillFrame f) => _resolve().MobileSpXuShuJianyanAi(p, f);
    internal IReadOnlyList<PromptChoice> MobileSpXuShuJianyanChoices(ProgramSkillFrame f) => _resolve().MobileSpXuShuJianyanChoices(f);
    internal void PublishMobileSpXuShuJianyan(ProgramSkillFrame f) => _resolve().PublishMobileSpXuShuJianyan(f);
    internal void PublishOrFinishMobileSpXuShuJianyanRecipient(ProgramSkillFrame f) => _resolve().PublishOrFinishMobileSpXuShuJianyanRecipient(f);
    internal void RecordMobileSpXuShuJianyanMovement(long id, string kind, long batch, long before, IReadOnlyList<CardMovementRecord> records) => _resolve().RecordMobileSpXuShuJianyanMovement(id, kind, batch, before, records);
    internal bool ResumeMobileSpXuShuJianyan(long id) => _resolve().ResumeMobileSpXuShuJianyan(id);
    internal bool ReturnMobileSpXuShuJianyanMovement(ProgramSkillFrame f) => _resolve().ReturnMobileSpXuShuJianyanMovement(f);
}

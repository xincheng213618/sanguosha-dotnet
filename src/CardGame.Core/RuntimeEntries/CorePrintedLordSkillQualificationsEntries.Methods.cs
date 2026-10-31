#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePrintedLordSkillQualificationsEntries
{
    internal IReadOnlyList<PrintedLordSkillQualification> CapturePrintedLordQualifications(ProgramSkillFrame f, int seat) => _resolve().CapturePrintedLordQualifications(f, seat);
    internal bool IsPrintedLordGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsPrintedLordGrantQualified(owner, grant);
    internal void IssueCapturedPrintedLordQualifications(ProgramSkillFrame f, PrintedLordBenefitReceipt r) => _resolve().IssueCapturedPrintedLordQualifications(f, r);
    internal string? PreferredQualifiedPrintedLordInstance(CharacterState owner, string skillId) => _resolve().PreferredQualifiedPrintedLordInstance(owner, skillId);
}

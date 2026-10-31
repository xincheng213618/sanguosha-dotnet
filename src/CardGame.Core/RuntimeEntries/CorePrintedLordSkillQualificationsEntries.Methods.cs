#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePrintedLordSkillQualificationsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrintedLordSkillQualification> CapturePrintedLordQualifications(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().CapturePrintedLordQualifications(f, seat);
    internal bool IsPrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsPrintedLordGrantQualified(owner, grant);
    internal void IssueCapturedPrintedLordQualifications(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PrintedLordBenefitReceipt r) => _resolve().IssueCapturedPrintedLordQualifications(f, r);
    internal string? PreferredQualifiedPrintedLordInstance(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().PreferredQualifiedPrintedLordInstance(owner, skillId);
}

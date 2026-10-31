#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineScenarioPrintedLordQualificationsEntries
{
    internal bool IsShanhePrintedLordGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsShanhePrintedLordGrantQualified(owner, grant);
    internal bool HasShanhePrintedLordQualifications { get => _resolve().HasShanhePrintedLordQualifications; }
}

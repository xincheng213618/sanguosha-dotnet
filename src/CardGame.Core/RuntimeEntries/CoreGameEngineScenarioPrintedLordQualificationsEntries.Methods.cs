#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineScenarioPrintedLordQualificationsEntries
{
    internal bool IsShanhePrintedLordGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsShanhePrintedLordGrantQualified(owner, grant);
    internal bool HasShanhePrintedLordQualifications { get => _resolve().HasShanhePrintedLordQualifications; }
}

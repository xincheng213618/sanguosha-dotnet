#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed partial class CorePrintedLordSkillQualificationsEntries
{
    private readonly Func<EngineOwners.CorePrintedLordSkillQualificationsRuntime> _resolve;
    private CorePrintedLordSkillQualificationsEntries(Func<EngineOwners.CorePrintedLordSkillQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePrintedLordSkillQualificationsEntries Create(Func<EngineOwners.CorePrintedLordSkillQualificationsRuntime> resolve) => new(resolve);
}

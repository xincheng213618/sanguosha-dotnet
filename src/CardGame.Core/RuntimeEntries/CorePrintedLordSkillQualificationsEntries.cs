#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime))]
internal sealed partial class CorePrintedLordSkillQualificationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> _resolve;
    private CorePrintedLordSkillQualificationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePrintedLordSkillQualificationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePrintedLordSkillQualificationsRuntime> resolve) => new(resolve);
}

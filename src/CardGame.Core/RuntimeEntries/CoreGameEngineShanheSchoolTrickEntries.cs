#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolTrickEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> _resolve;
    private CoreGameEngineShanheSchoolTrickEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolTrickEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> resolve) => new(resolve);
}

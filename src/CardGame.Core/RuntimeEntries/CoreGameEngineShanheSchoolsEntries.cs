#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime))]
internal sealed partial class CoreGameEngineShanheSchoolsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> _resolve;
    private CoreGameEngineShanheSchoolsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolsRuntime> resolve) => new(resolve);
}

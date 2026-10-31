#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolVitalEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> _resolve;
    private CoreGameEngineShanheSchoolVitalEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolVitalEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> resolve) => new(resolve);
}

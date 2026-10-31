#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSchoolVitalEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> _resolve;
    private CoreGameEngineShanheSchoolVitalEntries(Func<EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSchoolVitalEntries Create(Func<EngineOwners.CoreGameEngineShanheSchoolVitalRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardActionContestsRuntime))]
internal sealed partial class CoreGameEngineCardActionContestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardActionContestsRuntime> _resolve;
    private CoreGameEngineCardActionContestsEntries(Func<EngineOwners.CoreGameEngineCardActionContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardActionContestsEntries Create(Func<EngineOwners.CoreGameEngineCardActionContestsRuntime> resolve) => new(resolve);
}

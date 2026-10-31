#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed partial class CoreGameEngineDelayedNullificationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDelayedNullificationRuntime> _resolve;
    private CoreGameEngineDelayedNullificationEntries(Func<EngineOwners.CoreGameEngineDelayedNullificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelayedNullificationEntries Create(Func<EngineOwners.CoreGameEngineDelayedNullificationRuntime> resolve) => new(resolve);
}

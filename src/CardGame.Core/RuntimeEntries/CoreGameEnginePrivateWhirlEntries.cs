#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateWhirlRuntime))]
internal sealed partial class CoreGameEnginePrivateWhirlEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateWhirlRuntime> _resolve;
    private CoreGameEnginePrivateWhirlEntries(Func<EngineOwners.CoreGameEnginePrivateWhirlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateWhirlEntries Create(Func<EngineOwners.CoreGameEnginePrivateWhirlRuntime> resolve) => new(resolve);
}

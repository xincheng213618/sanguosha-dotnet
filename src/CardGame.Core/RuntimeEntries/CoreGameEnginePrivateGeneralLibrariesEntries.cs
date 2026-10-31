#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime))]
internal sealed partial class CoreGameEnginePrivateGeneralLibrariesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> _resolve;
    private CoreGameEnginePrivateGeneralLibrariesEntries(Func<EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateGeneralLibrariesEntries Create(Func<EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> resolve) => new(resolve);
}

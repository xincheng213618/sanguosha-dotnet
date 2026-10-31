#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime))]
internal sealed partial class CoreGameEngineNativeDrawOrdinalsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> _resolve;
    private CoreGameEngineNativeDrawOrdinalsEntries(Func<EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawOrdinalsEntries Create(Func<EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> resolve) => new(resolve);
}

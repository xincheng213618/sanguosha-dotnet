#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime))]
internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> _resolve;
    private CoreGameEngineNativeUsedDiscardOriginsEntries(Func<EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeUsedDiscardOriginsEntries Create(Func<EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> resolve) => new(resolve);
}

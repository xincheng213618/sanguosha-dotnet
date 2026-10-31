#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed partial class CoreGameEngineNativeLogicalDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _resolve;
    private CoreGameEngineNativeLogicalDiscardsEntries(Func<EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeLogicalDiscardsEntries Create(Func<EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime))]
internal sealed partial class CoreGameEngineNativeRecastInvocationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> _resolve;
    private CoreGameEngineNativeRecastInvocationsEntries(Func<EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeRecastInvocationsEntries Create(Func<EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> resolve) => new(resolve);
}

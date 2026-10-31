#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime))]
internal sealed partial class CoreGameEngineNativeRecastInvocationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> _resolve;
    private CoreGameEngineNativeRecastInvocationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeRecastInvocationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecastInvocationsRuntime> resolve) => new(resolve);
}

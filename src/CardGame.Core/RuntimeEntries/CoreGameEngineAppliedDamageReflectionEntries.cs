#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageReflectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> _resolve;
    private CoreGameEngineAppliedDamageReflectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAppliedDamageReflectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> resolve) => new(resolve);
}

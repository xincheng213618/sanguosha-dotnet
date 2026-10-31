#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageReflectionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> _resolve;
    private CoreGameEngineAppliedDamageReflectionEntries(Func<EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAppliedDamageReflectionEntries Create(Func<EngineOwners.CoreGameEngineAppliedDamageReflectionRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed partial class CoreGameEngineCardAttackStateEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardAttackStateRuntime> _resolve;
    private CoreGameEngineCardAttackStateEntries(Func<EngineOwners.CoreGameEngineCardAttackStateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardAttackStateEntries Create(Func<EngineOwners.CoreGameEngineCardAttackStateRuntime> resolve) => new(resolve);
}

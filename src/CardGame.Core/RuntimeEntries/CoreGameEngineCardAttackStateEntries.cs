#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed partial class CoreGameEngineCardAttackStateEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _resolve;
    private CoreGameEngineCardAttackStateEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardAttackStateEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> resolve) => new(resolve);
}

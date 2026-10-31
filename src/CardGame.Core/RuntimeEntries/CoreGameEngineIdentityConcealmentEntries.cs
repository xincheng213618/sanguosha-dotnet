#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed partial class CoreGameEngineIdentityConcealmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _resolve;
    private CoreGameEngineIdentityConcealmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIdentityConcealmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> resolve) => new(resolve);
}

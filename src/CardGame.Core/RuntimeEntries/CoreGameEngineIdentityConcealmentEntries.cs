#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed partial class CoreGameEngineIdentityConcealmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _resolve;
    private CoreGameEngineIdentityConcealmentEntries(Func<EngineOwners.CoreGameEngineIdentityConcealmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIdentityConcealmentEntries Create(Func<EngineOwners.CoreGameEngineIdentityConcealmentRuntime> resolve) => new(resolve);
}

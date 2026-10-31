#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed partial class CoreGameEngineShenwuCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShenwuCardsRuntime> _resolve;
    private CoreGameEngineShenwuCardsEntries(Func<EngineOwners.CoreGameEngineShenwuCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShenwuCardsEntries Create(Func<EngineOwners.CoreGameEngineShenwuCardsRuntime> resolve) => new(resolve);
}

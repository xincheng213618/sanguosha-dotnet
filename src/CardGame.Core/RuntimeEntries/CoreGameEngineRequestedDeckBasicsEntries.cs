#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _resolve;
    private CoreGameEngineRequestedDeckBasicsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRequestedDeckBasicsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed partial class CoreMatchLogEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _resolve;
    private CoreMatchLogEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchLogEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> resolve) => new(resolve);
}

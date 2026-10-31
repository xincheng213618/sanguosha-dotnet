#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduChariotResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> _resolve;
    private CoreGameEngineGuanduChariotResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduChariotResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> resolve) => new(resolve);
}

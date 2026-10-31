#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime))]
internal sealed partial class CoreGameEngineGuanduBasicConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> _resolve;
    private CoreGameEngineGuanduBasicConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduBasicConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduBasicConversionsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime> _resolve;
    private CoreGameEngineProgramChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> _resolve;
    private CoreGameEngineProgramCompoundCardTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCompoundCardTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve) => new(resolve);
}

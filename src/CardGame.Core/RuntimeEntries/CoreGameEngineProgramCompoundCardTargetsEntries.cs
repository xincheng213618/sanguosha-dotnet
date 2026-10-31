#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime))]
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> _resolve;
    private CoreGameEngineProgramCompoundCardTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCompoundCardTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve) => new(resolve);
}

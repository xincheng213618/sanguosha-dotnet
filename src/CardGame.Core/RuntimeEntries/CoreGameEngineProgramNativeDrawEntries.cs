#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime))]
internal sealed partial class CoreGameEngineProgramNativeDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> _resolve;
    private CoreGameEngineProgramNativeDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramNativeDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> resolve) => new(resolve);
}

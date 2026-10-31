#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime))]
internal sealed partial class CoreGameEngineProgramTriggerWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> _resolve;
    private CoreGameEngineProgramTriggerWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTriggerWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> resolve) => new(resolve);
}

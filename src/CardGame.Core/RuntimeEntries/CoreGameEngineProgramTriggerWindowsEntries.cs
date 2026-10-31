#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime))]
internal sealed partial class CoreGameEngineProgramTriggerWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> _resolve;
    private CoreGameEngineProgramTriggerWindowsEntries(Func<EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTriggerWindowsEntries Create(Func<EngineOwners.CoreGameEngineProgramTriggerWindowsRuntime> resolve) => new(resolve);
}

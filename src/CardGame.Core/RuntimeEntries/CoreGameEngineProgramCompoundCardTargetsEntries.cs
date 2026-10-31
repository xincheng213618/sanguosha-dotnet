#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime))]
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> _resolve;
    private CoreGameEngineProgramCompoundCardTargetsEntries(Func<EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCompoundCardTargetsEntries Create(Func<EngineOwners.CoreGameEngineProgramCompoundCardTargetsRuntime> resolve) => new(resolve);
}

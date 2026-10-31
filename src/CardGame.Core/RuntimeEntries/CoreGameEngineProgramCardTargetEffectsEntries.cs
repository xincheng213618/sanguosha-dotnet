#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed partial class CoreGameEngineProgramCardTargetEffectsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _resolve;
    private CoreGameEngineProgramCardTargetEffectsEntries(Func<EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardTargetEffectsEntries Create(Func<EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> resolve) => new(resolve);
}

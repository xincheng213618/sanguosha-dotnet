#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAttackEffectsRuntime))]
internal sealed partial class CoreGameEngineProgramAttackEffectsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> _resolve;
    private CoreGameEngineProgramAttackEffectsEntries(Func<EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackEffectsEntries Create(Func<EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> resolve) => new(resolve);
}

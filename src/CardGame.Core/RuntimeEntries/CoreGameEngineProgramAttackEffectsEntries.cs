#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime))]
internal sealed partial class CoreGameEngineProgramAttackEffectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> _resolve;
    private CoreGameEngineProgramAttackEffectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackEffectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackEffectsRuntime> resolve) => new(resolve);
}

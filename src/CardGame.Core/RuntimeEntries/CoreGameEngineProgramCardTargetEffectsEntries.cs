#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardTargetEffectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _resolve;
    private CoreGameEngineProgramCardTargetEffectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardTargetEffectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> resolve) => new(resolve);
}

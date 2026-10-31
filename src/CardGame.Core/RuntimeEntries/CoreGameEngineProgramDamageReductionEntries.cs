#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDamageReductionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime> _resolve;
    private CoreGameEngineProgramDamageReductionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDamageReductionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime> resolve) => new(resolve);
}

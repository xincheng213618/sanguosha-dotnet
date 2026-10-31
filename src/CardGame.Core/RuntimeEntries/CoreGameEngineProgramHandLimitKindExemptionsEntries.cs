#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandLimitKindExemptionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> _resolve;
    private CoreGameEngineProgramHandLimitKindExemptionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandLimitKindExemptionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramHandLimitKindExemptionsRuntime> resolve) => new(resolve);
}

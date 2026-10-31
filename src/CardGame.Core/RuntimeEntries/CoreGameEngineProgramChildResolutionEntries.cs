#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramChildResolutionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> _resolve;
    private CoreGameEngineProgramChildResolutionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramChildResolutionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramChildResolutionRuntime> resolve) => new(resolve);
}

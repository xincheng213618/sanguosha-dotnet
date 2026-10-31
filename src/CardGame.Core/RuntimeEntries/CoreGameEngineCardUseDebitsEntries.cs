#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed partial class CoreGameEngineCardUseDebitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> _resolve;
    private CoreGameEngineCardUseDebitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseDebitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseDebitsRuntime> resolve) => new(resolve);
}

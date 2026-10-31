#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineColorFireAttacksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _resolve;
    private CoreGameEngineColorFireAttacksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineColorFireAttacksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> resolve) => new(resolve);
}

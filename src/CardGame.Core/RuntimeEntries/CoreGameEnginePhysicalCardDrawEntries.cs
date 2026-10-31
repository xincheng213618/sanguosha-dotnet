#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime))]
internal sealed partial class CoreGameEnginePhysicalCardDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> _resolve;
    private CoreGameEnginePhysicalCardDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhysicalCardDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhysicalCardDrawRuntime> resolve) => new(resolve);
}

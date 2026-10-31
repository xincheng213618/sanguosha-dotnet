#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime))]
internal sealed partial class CoreGameEngineSourceOwnerGainsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> _resolve;
    private CoreGameEngineSourceOwnerGainsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceOwnerGainsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceOwnerGainsRuntime> resolve) => new(resolve);
}

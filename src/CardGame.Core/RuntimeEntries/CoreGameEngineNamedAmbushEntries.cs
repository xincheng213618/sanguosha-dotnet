#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime))]
internal sealed partial class CoreGameEngineNamedAmbushEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime> _resolve;
    private CoreGameEngineNamedAmbushEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedAmbushEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime> resolve) => new(resolve);
}

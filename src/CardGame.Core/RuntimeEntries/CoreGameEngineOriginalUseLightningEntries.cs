#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed partial class CoreGameEngineOriginalUseLightningEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _resolve;
    private CoreGameEngineOriginalUseLightningEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalUseLightningEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> resolve) => new(resolve);
}

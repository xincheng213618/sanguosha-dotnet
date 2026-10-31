#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed partial class CoreGameEngineOriginalUseLightningEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _resolve;
    private CoreGameEngineOriginalUseLightningEntries(Func<EngineOwners.CoreGameEngineOriginalUseLightningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalUseLightningEntries Create(Func<EngineOwners.CoreGameEngineOriginalUseLightningRuntime> resolve) => new(resolve);
}

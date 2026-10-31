#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPileColorsRuntime))]
internal sealed partial class CoreGameEnginePublicPileColorsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPileColorsRuntime> _resolve;
    private CoreGameEnginePublicPileColorsEntries(Func<EngineOwners.CoreGameEnginePublicPileColorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileColorsEntries Create(Func<EngineOwners.CoreGameEnginePublicPileColorsRuntime> resolve) => new(resolve);
}

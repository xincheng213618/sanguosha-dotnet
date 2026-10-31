#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime))]
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> _resolve;
    private CoreGameEngineCardDamageMaximumHpReplacementEntries(Func<EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDamageMaximumHpReplacementEntries Create(Func<EngineOwners.CoreGameEngineCardDamageMaximumHpReplacementRuntime> resolve) => new(resolve);
}

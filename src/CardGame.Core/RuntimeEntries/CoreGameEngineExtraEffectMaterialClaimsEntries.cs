#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime))]
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> _resolve;
    private CoreGameEngineExtraEffectMaterialClaimsEntries(Func<EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectMaterialClaimsEntries Create(Func<EngineOwners.CoreGameEngineExtraEffectMaterialClaimsRuntime> resolve) => new(resolve);
}

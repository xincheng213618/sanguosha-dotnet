#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardEffectProgramsRuntime))]
internal sealed partial class CoreGameEngineCardEffectProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardEffectProgramsRuntime> _resolve;
    private CoreGameEngineCardEffectProgramsEntries(Func<EngineOwners.CoreGameEngineCardEffectProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardEffectProgramsEntries Create(Func<EngineOwners.CoreGameEngineCardEffectProgramsRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreWushuangCombatProgramsRuntime))]
internal sealed partial class CoreWushuangCombatProgramsEntries
{
    private readonly Func<EngineOwners.CoreWushuangCombatProgramsRuntime> _resolve;
    private CoreWushuangCombatProgramsEntries(Func<EngineOwners.CoreWushuangCombatProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreWushuangCombatProgramsEntries Create(Func<EngineOwners.CoreWushuangCombatProgramsRuntime> resolve) => new(resolve);
}

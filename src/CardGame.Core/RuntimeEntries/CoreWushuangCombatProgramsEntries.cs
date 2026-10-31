#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreWushuangCombatProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> _resolve;
    private CoreWushuangCombatProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreWushuangCombatProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> resolve) => new(resolve);
}

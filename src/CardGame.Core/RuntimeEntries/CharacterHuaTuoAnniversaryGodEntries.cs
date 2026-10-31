#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed partial class CharacterHuaTuoAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _resolve;
    private CharacterHuaTuoAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> resolve) => new(resolve);
}

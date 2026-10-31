#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuaTuoHappyGodRuntime))]
internal sealed partial class CharacterHuaTuoHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterHuaTuoHappyGodRuntime> _resolve;
    private CharacterHuaTuoHappyGodEntries(Func<EngineOwners.CharacterHuaTuoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoHappyGodEntries Create(Func<EngineOwners.CharacterHuaTuoHappyGodRuntime> resolve) => new(resolve);
}

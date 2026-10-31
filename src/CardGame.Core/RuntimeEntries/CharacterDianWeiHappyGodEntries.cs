#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDianWeiHappyGodRuntime))]
internal sealed partial class CharacterDianWeiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterDianWeiHappyGodRuntime> _resolve;
    private CharacterDianWeiHappyGodEntries(Func<EngineOwners.CharacterDianWeiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiHappyGodEntries Create(Func<EngineOwners.CharacterDianWeiHappyGodRuntime> resolve) => new(resolve);
}

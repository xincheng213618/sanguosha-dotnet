#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDiaoChanHappyGodRuntime))]
internal sealed partial class CharacterDiaoChanHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterDiaoChanHappyGodRuntime> _resolve;
    private CharacterDiaoChanHappyGodEntries(Func<EngineOwners.CharacterDiaoChanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDiaoChanHappyGodEntries Create(Func<EngineOwners.CharacterDiaoChanHappyGodRuntime> resolve) => new(resolve);
}

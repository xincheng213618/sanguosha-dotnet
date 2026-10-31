#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDengAiHappyGodRuntime))]
internal sealed partial class CharacterDengAiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterDengAiHappyGodRuntime> _resolve;
    private CharacterDengAiHappyGodEntries(Func<EngineOwners.CharacterDengAiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiHappyGodEntries Create(Func<EngineOwners.CharacterDengAiHappyGodRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoJiaHappyGodRuntime))]
internal sealed partial class CharacterGuoJiaHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterGuoJiaHappyGodRuntime> _resolve;
    private CharacterGuoJiaHappyGodEntries(Func<EngineOwners.CharacterGuoJiaHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaHappyGodEntries Create(Func<EngineOwners.CharacterGuoJiaHappyGodRuntime> resolve) => new(resolve);
}

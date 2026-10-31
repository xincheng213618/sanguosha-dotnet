#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSimaYiHappyGodRuntime))]
internal sealed partial class CharacterSimaYiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterSimaYiHappyGodRuntime> _resolve;
    private CharacterSimaYiHappyGodEntries(Func<EngineOwners.CharacterSimaYiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSimaYiHappyGodEntries Create(Func<EngineOwners.CharacterSimaYiHappyGodRuntime> resolve) => new(resolve);
}

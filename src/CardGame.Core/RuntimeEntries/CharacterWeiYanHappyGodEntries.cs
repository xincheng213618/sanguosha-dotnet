#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed partial class CharacterWeiYanHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterWeiYanHappyGodRuntime> _resolve;
    private CharacterWeiYanHappyGodEntries(Func<EngineOwners.CharacterWeiYanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiYanHappyGodEntries Create(Func<EngineOwners.CharacterWeiYanHappyGodRuntime> resolve) => new(resolve);
}

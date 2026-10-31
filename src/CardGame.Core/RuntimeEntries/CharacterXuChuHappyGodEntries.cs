#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuChuHappyGodRuntime))]
internal sealed partial class CharacterXuChuHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterXuChuHappyGodRuntime> _resolve;
    private CharacterXuChuHappyGodEntries(Func<EngineOwners.CharacterXuChuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuChuHappyGodEntries Create(Func<EngineOwners.CharacterXuChuHappyGodRuntime> resolve) => new(resolve);
}

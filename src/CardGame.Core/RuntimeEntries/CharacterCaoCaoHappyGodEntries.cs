#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterCaoCaoHappyGodRuntime> _resolve;
    private CharacterCaoCaoHappyGodEntries(Func<EngineOwners.CharacterCaoCaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoHappyGodEntries Create(Func<EngineOwners.CharacterCaoCaoHappyGodRuntime> resolve) => new(resolve);
}

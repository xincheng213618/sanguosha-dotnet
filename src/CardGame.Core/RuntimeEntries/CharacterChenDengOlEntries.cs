#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDengOlEntries
{
    private readonly Func<EngineOwners.CharacterChenDengOlRuntime> _resolve;
    private CharacterChenDengOlEntries(Func<EngineOwners.CharacterChenDengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenDengOlEntries Create(Func<EngineOwners.CharacterChenDengOlRuntime> resolve) => new(resolve);
}

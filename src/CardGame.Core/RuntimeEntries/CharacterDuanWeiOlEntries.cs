#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuanWeiOlEntries
{
    private readonly Func<EngineOwners.CharacterDuanWeiOlRuntime> _resolve;
    private CharacterDuanWeiOlEntries(Func<EngineOwners.CharacterDuanWeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuanWeiOlEntries Create(Func<EngineOwners.CharacterDuanWeiOlRuntime> resolve) => new(resolve);
}

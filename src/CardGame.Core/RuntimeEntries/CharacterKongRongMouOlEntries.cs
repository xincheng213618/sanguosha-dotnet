#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterKongRongMouOlRuntime))]
internal sealed partial class CharacterKongRongMouOlEntries
{
    private readonly Func<EngineOwners.CharacterKongRongMouOlRuntime> _resolve;
    private CharacterKongRongMouOlEntries(Func<EngineOwners.CharacterKongRongMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKongRongMouOlEntries Create(Func<EngineOwners.CharacterKongRongMouOlRuntime> resolve) => new(resolve);
}

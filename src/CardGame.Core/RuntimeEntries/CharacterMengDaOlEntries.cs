#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMengDaOlRuntime))]
internal sealed partial class CharacterMengDaOlEntries
{
    private readonly Func<EngineOwners.CharacterMengDaOlRuntime> _resolve;
    private CharacterMengDaOlEntries(Func<EngineOwners.CharacterMengDaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengDaOlEntries Create(Func<EngineOwners.CharacterMengDaOlRuntime> resolve) => new(resolve);
}

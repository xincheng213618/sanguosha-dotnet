#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoTengOlRuntime))]
internal sealed partial class CharacterCaoTengOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoTengOlRuntime> _resolve;
    private CharacterCaoTengOlEntries(Func<EngineOwners.CharacterCaoTengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoTengOlEntries Create(Func<EngineOwners.CharacterCaoTengOlRuntime> resolve) => new(resolve);
}

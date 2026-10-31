#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoShuangOlRuntime))]
internal sealed partial class CharacterCaoShuangOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoShuangOlRuntime> _resolve;
    private CharacterCaoShuangOlEntries(Func<EngineOwners.CharacterCaoShuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangOlEntries Create(Func<EngineOwners.CharacterCaoShuangOlRuntime> resolve) => new(resolve);
}

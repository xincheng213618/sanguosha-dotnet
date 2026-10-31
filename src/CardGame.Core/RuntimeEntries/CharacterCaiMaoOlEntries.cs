#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaiMaoOlRuntime))]
internal sealed partial class CharacterCaiMaoOlEntries
{
    private readonly Func<EngineOwners.CharacterCaiMaoOlRuntime> _resolve;
    private CharacterCaiMaoOlEntries(Func<EngineOwners.CharacterCaiMaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiMaoOlEntries Create(Func<EngineOwners.CharacterCaiMaoOlRuntime> resolve) => new(resolve);
}

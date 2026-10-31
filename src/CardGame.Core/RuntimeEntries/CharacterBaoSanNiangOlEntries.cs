#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBaoSanNiangOlRuntime))]
internal sealed partial class CharacterBaoSanNiangOlEntries
{
    private readonly Func<EngineOwners.CharacterBaoSanNiangOlRuntime> _resolve;
    private CharacterBaoSanNiangOlEntries(Func<EngineOwners.CharacterBaoSanNiangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaoSanNiangOlEntries Create(Func<EngineOwners.CharacterBaoSanNiangOlRuntime> resolve) => new(resolve);
}

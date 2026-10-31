#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuLiAnniversaryRuntime))]
internal sealed partial class CharacterLiuLiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLiuLiAnniversaryRuntime> _resolve;
    private CharacterLiuLiAnniversaryEntries(Func<EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuLiAnniversaryEntries Create(Func<EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve) => new(resolve);
}

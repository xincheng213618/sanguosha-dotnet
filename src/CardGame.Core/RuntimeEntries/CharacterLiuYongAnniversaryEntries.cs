#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuYongAnniversaryRuntime))]
internal sealed partial class CharacterLiuYongAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLiuYongAnniversaryRuntime> _resolve;
    private CharacterLiuYongAnniversaryEntries(Func<EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYongAnniversaryEntries Create(Func<EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuYouAnniversaryRuntime))]
internal sealed partial class CharacterLiuYouAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLiuYouAnniversaryRuntime> _resolve;
    private CharacterLiuYouAnniversaryEntries(Func<EngineOwners.CharacterLiuYouAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYouAnniversaryEntries Create(Func<EngineOwners.CharacterLiuYouAnniversaryRuntime> resolve) => new(resolve);
}

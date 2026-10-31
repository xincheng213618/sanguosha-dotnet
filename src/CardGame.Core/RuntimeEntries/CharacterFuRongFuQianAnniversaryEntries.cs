#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFuRongFuQianAnniversaryRuntime))]
internal sealed partial class CharacterFuRongFuQianAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> _resolve;
    private CharacterFuRongFuQianAnniversaryEntries(Func<EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuRongFuQianAnniversaryEntries Create(Func<EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> resolve) => new(resolve);
}

#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime))]
internal sealed partial class CharacterFuRongFuQianAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> _resolve;
    private CharacterFuRongFuQianAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuRongFuQianAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongFuQianAnniversaryRuntime> resolve) => new(resolve);
}

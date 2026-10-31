#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuZanMobileRuntime))]
internal sealed partial class CharacterLiuZanMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiuZanMobileRuntime> _resolve;
    private CharacterLiuZanMobileEntries(Func<EngineOwners.CharacterLiuZanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuZanMobileEntries Create(Func<EngineOwners.CharacterLiuZanMobileRuntime> resolve) => new(resolve);
}

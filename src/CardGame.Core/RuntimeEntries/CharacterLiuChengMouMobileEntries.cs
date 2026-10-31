#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed partial class CharacterLiuChengMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiuChengMouMobileRuntime> _resolve;
    private CharacterLiuChengMouMobileEntries(Func<EngineOwners.CharacterLiuChengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuChengMouMobileEntries Create(Func<EngineOwners.CharacterLiuChengMouMobileRuntime> resolve) => new(resolve);
}

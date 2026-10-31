#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangFuZhaoLeiMobileRuntime))]
internal sealed partial class CharacterWangFuZhaoLeiMobileEntries
{
    private readonly Func<EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> _resolve;
    private CharacterWangFuZhaoLeiMobileEntries(Func<EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangFuZhaoLeiMobileEntries Create(Func<EngineOwners.CharacterWangFuZhaoLeiMobileRuntime> resolve) => new(resolve);
}

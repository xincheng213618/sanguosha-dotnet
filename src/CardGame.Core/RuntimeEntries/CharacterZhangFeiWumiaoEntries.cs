#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed partial class CharacterZhangFeiWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterZhangFeiWumiaoRuntime> _resolve;
    private CharacterZhangFeiWumiaoEntries(Func<EngineOwners.CharacterZhangFeiWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiWumiaoEntries Create(Func<EngineOwners.CharacterZhangFeiWumiaoRuntime> resolve) => new(resolve);
}

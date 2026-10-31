#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhaoYunMobileShiRuntime))]
internal sealed partial class CharacterZhaoYunMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZhaoYunMobileShiRuntime> _resolve;
    private CharacterZhaoYunMobileShiEntries(Func<EngineOwners.CharacterZhaoYunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoYunMobileShiEntries Create(Func<EngineOwners.CharacterZhaoYunMobileShiRuntime> resolve) => new(resolve);
}

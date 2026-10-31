#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanYuVariantCapability(Func<RuntimeEntries.CharacterGuanYuRuntimeEntries> getCharacterGuanYuRuntimeRuntime) : IGuanYuVariantHost
{
    public SkillProgramStepOutcome ExecuteGuanYuVariant(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterGuanYuRuntimeRuntime().ExecuteGuanYuVariant(frame, op);
}

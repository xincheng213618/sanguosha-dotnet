#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GuanYuVariantCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuRuntimeEntries> getCharacterGuanYuRuntimeRuntime) : IGuanYuVariantHost
{
    public SkillProgramStepOutcome ExecuteGuanYuVariant(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterGuanYuRuntimeRuntime().ExecuteGuanYuVariant(frame, op);
}

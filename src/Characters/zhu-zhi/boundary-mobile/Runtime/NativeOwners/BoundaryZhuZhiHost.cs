#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhuZhiHost(Func<RuntimeEntries.CharacterZhuZhiBoundaryMobileEntries> getCharacterZhuZhiBoundaryMobileRuntime) : IBoundaryZhuZhiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.PayBoundaryZhuZhiAnguo ? getCharacterZhuZhiBoundaryMobileRuntime().BeginBoundaryAnguoPayment(frame) : getCharacterZhuZhiBoundaryMobileRuntime().BeginBoundaryAnguo(frame, operation == SkillProgramEffectOp.BeginBoundaryZhuZhiRescue);
}

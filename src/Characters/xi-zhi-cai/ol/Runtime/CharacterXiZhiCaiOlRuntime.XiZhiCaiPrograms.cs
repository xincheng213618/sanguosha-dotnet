#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{
    internal CardConversionSource XiZhiCaiSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IXiZhiCaiProgramHost.ArmXianfuRelation(ProgramSkillFrame f, int t) => ArmXianfuRelation(f, t);
    SkillProgramStepOutcome IXiZhiCaiProgramHost.SettleXianfuNativeEcho(ProgramSkillFrame f) => SettleXianfuNativeEcho(f);
    SkillProgramStepOutcome IXiZhiCaiProgramHost.StartChouceDamagePointJudgment(ProgramSkillFrame f, SkillProgramEffect e) => StartChouceDamagePointJudgment(f, e);
    SkillProgramStepOutcome IXiZhiCaiProgramHost.ResolveChouceFrozenJudgment(ProgramSkillFrame f, SkillProgramEffect e) => ResolveChouceFrozenJudgment(f, e);
}

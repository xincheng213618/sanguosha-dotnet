namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_yuan_ji_ol_YuanJiLedger
{
    internal static CardConversionSource YuanJiSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);

    internal static bool IsYuanJiReturnOp(SkillProgramEffectOp op)=>op is SkillProgramEffectOp.ResolveJinghuaTurnReturn or SkillProgramEffectOp.ResolveShuiyueRoundReturn;

    internal static bool IsYuanJiMarkDraw(SkillProgramEffectOp op)=>op is SkillProgramEffectOp.ResolveJinghuaRoundDraw or SkillProgramEffectOp.ResolveShuiyueTurnDraw;
}

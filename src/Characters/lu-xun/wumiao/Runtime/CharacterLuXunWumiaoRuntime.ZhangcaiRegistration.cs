#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool HasWuLuXunZhangcaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuXunZhangcai);

    SkillProgramStepOutcome global::CardGame.Core.IWuLuXunZhangcaiHost.Zhangcai(ProgramSkillFrame frame) => BeginWuLuXunZhangcai(frame);
    SkillProgramStepOutcome global::CardGame.Core.IWuLuXunZhangcaiHost.Ruxian(ProgramSkillFrame frame) => ActivateWuLuXunRuxian(frame);
}

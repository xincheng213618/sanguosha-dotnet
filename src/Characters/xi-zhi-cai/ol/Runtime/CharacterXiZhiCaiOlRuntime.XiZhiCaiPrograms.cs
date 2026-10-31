#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{
    internal CardConversionSource XiZhiCaiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IXiZhiCaiProgramHost.ArmXianfuRelation(ProgramSkillFrame f, int t) => ArmXianfuRelation(f, t);
    SkillProgramStepOutcome global::CardGame.Core.IXiZhiCaiProgramHost.SettleXianfuNativeEcho(ProgramSkillFrame f) => SettleXianfuNativeEcho(f);
    SkillProgramStepOutcome global::CardGame.Core.IXiZhiCaiProgramHost.StartChouceDamagePointJudgment(ProgramSkillFrame f, SkillProgramEffect e) => StartChouceDamagePointJudgment(f, e);
    SkillProgramStepOutcome global::CardGame.Core.IXiZhiCaiProgramHost.ResolveChouceFrozenJudgment(ProgramSkillFrame f, SkillProgramEffect e) => ResolveChouceFrozenJudgment(f, e);
}

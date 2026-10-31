#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiAnniversaryGodRuntime
{
    internal bool HasShenJwPaidPingxiang(ProgramSkillFrame f) => f.ShenJiangWei is { Operation: SkillProgramEffectOp.ResolveShenJiangWeiPingxiang } r && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenJiangWeiPingxiangPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.MaximumBefore == r.MaximumBefore && e.MaximumAfter == r.MaximumAfter && e.MaximumBefore > 9 && e.MaximumBefore - e.MaximumAfter == 9) == 1;
}

#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiMouRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 28000, SkillProgramEffectOp.MouCaoPiSongwei)]
    internal bool CanGainMouSongwei(ProgramTriggerLegalityContext c) => c.Owner.Role == Role.Lord && HasMouXingshang(c.Owner);
}

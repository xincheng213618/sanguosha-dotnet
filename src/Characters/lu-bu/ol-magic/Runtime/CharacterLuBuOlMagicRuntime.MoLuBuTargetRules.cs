#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQualification, 10)]
    internal bool MoGangquanSlashTargetQualified(ProgramSlashTargetQuery query) => MoGangquanTargetAllowed(query.Actor, query.Target, query.Kind, query.Source, query.PhysicalCardIds);
}

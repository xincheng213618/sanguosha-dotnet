#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardUseModulesCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId);
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

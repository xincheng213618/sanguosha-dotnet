#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPindianPortsRuntime
{
    bool global::CardGame.Core.IProgramPindianPolicyQueries.ProhibitsPindianTarget(int seat) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ProhibitPindianTarget);
    bool global::CardGame.Core.IProgramPindianPolicyQueries.CanDiscardForPindian(int seat) => _characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IProgramPindianProducer.Begin(long programFrameId, SkillPromptPresentation presentation, int sourceSeat, int opponentSeat, string resultBind, SkillProgramCardSetVisibility visibility) => _corePindianOperations.BeginSharedPindian(programFrameId, presentation, sourceSeat, opponentSeat, programResultBind: resultBind, programResultVisibility: visibility);
}

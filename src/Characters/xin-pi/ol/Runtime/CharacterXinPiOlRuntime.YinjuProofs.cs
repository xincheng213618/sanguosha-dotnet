#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal bool YinjuRelationValid(YinjuTurnRelation r) => r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) && r.Source.OwnerSeat != r.TargetSeat && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == r.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YinjuRelationArmedEvent>().Count(e => e.Relation == r) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillStartedEvent>().Count(e => e.FrameId == r.ActivationFrameId && e.OwnerSeat == r.Source.OwnerSeat && e.SkillId == r.Source.SkillId && e.ActivationId == r.Source.BindingId) == 1 && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.Activations.Any(a => a.Id == r.Source.BindingId && a.Effects is [{ Op: SkillProgramEffectOp.ArmYinjuActualTurnRelation }]);
    internal YinjuTurnRelation? CurrentYinju(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].YinjuRelation is { } r && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && YinjuRelationValid(r) ? r : null;
}

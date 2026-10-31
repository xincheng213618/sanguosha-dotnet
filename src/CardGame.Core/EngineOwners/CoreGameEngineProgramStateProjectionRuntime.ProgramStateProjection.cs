#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramStateProjectionRuntime
{
    internal bool HasProgramPersistentZone(CharacterState owner, CardZoneKind zone)
    {
        var programs = _coreSkillProgramsOperations.EnabledSkillPrograms(owner);
        for (var index = 0; index < programs.Count; index++)
        {
            var program = programs[index];
            for (var activationIndex = 0; activationIndex < program.Activations.Count; activationIndex++)
                if (WritesZone(program.Activations[activationIndex].Effects))
                    return true;
            for (var triggerIndex = 0; triggerIndex < program.Triggers.Count; triggerIndex++)
                if (WritesZone(program.Triggers[triggerIndex].Effects))
                    return true;
        }

        return false;
        bool WritesZone(IReadOnlyList<SkillProgramEffect> effects)
        {
            for (var index = 0; index < effects.Count; index++)
                if (effects[index].Destination == SkillProgramCardDestination.OwnerPersistentZone && effects[index].DestinationZone == zone)
                    return true;
            return false;
        }
    }

    internal bool IsRuntimeAcquiredSkill(CharacterState player, string skillId) => player.AcquiredSkillIds.Contains(skillId) || player.TurnGrantedSkillIds.Contains(skillId, StringComparer.Ordinal) || player.PhaseGrantedSkillIds.Contains(skillId, StringComparer.Ordinal);
    internal SkillRuntimeStateSnapshot CreateProgramAwareSkillStateSnapshot(CharacterState owner, string skillId)
    {
        var snapshot = _skillRuntimeStateOperations._skillRuntimeState.CreateSnapshot(owner.Seat, skillId, IsRuntimeAcquiredSkill(owner, skillId));
        List<ProgramBooleanStateSnapshot>? states = null;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
        {
            if (instance.SkillId != skillId)
                continue;
            foreach (var definition in instance.Program.BooleanStates)
            {
                if (definition.Visibility != SkillProgramStateVisibility.Public)
                    continue;
                var value = _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, skillId, instance.SkillInstanceId, definition.Id);
                var presentation = instance.Definition.ProgramPresentation?.BooleanStates.GetValueOrDefault(definition.Id);
                (states ??= []).Add(new(instance.SkillInstanceId, definition.Id, value, presentation is null ? $"{(definition.Id)}：{((value ? "是" : "否"))}" : value ? presentation.TrueText : presentation.FalseText));
            }
        }

        return snapshot with
        {
            PublicRuleStates = _coreGameActivationProgressOperations.GetSkillPublicRuleStates(owner, skillId),
            BooleanStates = states is null ? global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionDefinitions.EmptyProgramBooleanStateSnapshots : Array.AsReadOnly(states.ToArray()),
            DirectedPolicies = _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies.Count == 0 ? global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionDefinitions.EmptyDirectedPolicySnapshots : CreateNonemptyDirectedPolicySnapshots(owner, skillId),
            ActionProhibitions = _turnCardUseEffectsOperations._turnCardUseEffects.ActionProhibitions.Count == 0 ? global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateProjectionDefinitions.EmptyActionProhibitionSnapshots : CreateNonemptyActionProhibitionSnapshots(owner, skillId)
        };
    }

    internal IReadOnlyList<DirectedTurnCardPolicy> CreateNonemptyDirectedPolicySnapshots(CharacterState owner, string skillId) => Array.AsReadOnly(_coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies.Where(item => item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && item.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && item.Source.OwnerSeat == owner.Seat && item.Source.SkillId == skillId && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, skillId, item.Source.SkillInstanceId)).ToArray());
    internal IReadOnlyList<TurnCardActionProhibition> CreateNonemptyActionProhibitionSnapshots(CharacterState owner, string skillId) => Array.AsReadOnly(_turnCardUseEffectsOperations._turnCardUseEffects.ActionProhibitions.Where(item => item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && item.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && item.Source.OwnerSeat == owner.Seat && item.Source.SkillId == skillId && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, skillId, item.Source.SkillInstanceId)).ToArray());
}

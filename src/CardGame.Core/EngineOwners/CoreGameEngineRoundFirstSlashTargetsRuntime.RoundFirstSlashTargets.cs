#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundFirstSlashTargetsRuntime
{
    internal IEnumerable<RuleQueryContribution> RoundFirstSlashTargetContributions(CharacterState owner, CardKind kind, long? declaredUseFrameId = null)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasRoundFirstSlashTargetPolicy)
            yield break;
        var bindings = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.RoundFirstSlashAdditionalTargets, kind).ToArray();
        if (bindings.Length == 0)
            yield break;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var end = history.Length;
        CurrentTurnCardUseKindsRecordedEvent? current = null;
        if (declaredUseFrameId is { } frameId)
        {
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == frameId)is not CardUseFrame use || use.SourceSeat != owner.Seat || use.CardKind != kind)
                throw new InvalidOperationException("First-Slash targets require their original native use.");
            var issued = history.Select((fact, index) => (fact, index)).Where(item => item.fact is CurrentTurnCardUseKindsRecordedEvent fact && fact.OriginFrameId == frameId && !fact.IsNullificationUse && fact.RoundActionType == CardActionType.Use).ToArray();
            if (issued is not [var entry] || entry.fact is not CurrentTurnCardUseKindsRecordedEvent declaration || declaration.CardActionId != use.Action?.ActionId || declaration.RoundProviderSeat != (use.Action?.ProviderSeat ?? declaration.ActorSeat))
                throw new InvalidOperationException("First-Slash targets lost their exact native declaration receipt.");
            current = declaration;
            end = entry.index + 1;
            if (_coreSlashUseOrdinalsOperations.SlashUseKindAtPrefix(current, history, end, current) != kind)
                throw new InvalidOperationException("First-Slash targets lost their original effective kind.");
        }

        var boundary = -1;
        for (var index = end - 1; index >= 0; index--)
            if (history[index] is RoundStartedEvent)
            {
                boundary = index;
                break;
            }

        if (boundary < 0 || ((RoundStartedEvent)history[boundary]).RoundNumber < 1)
            yield break;
        var used = _coreSlashUseOrdinalsOperations.CountSlashUsePrefix(history, boundary + 1, end, owner.Seat, false, current);
        if (used != (current is null ? 0 : 1))
            yield break;
        foreach (var item in bindings)
            yield return new FiniteRuleQueryContribution($"skill:{(item.Source.SkillId)}:{(item.Source.SkillInstanceId)}:policy:{(item.Policy.Id)}", SkillRuleOperation.Add, item.Policy.Value);
    }
}

#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiAnniversaryGodRuntime
{
    internal IReadOnlyList<KemingBorrowOption> KemingPrintedBorrowOptions(ContentGeneralDefinition general) => Array.AsReadOnly(general.SkillIds.SelectMany(id => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program is { } p ? p.Triggers.Where(t => t.Window == SkillProgramTriggerWindow.AfterDamageApplied && t.Subject == SkillProgramTriggerSubject.Owner).Select(t => new KemingBorrowOption(general.Id, id, t.Id, p.GameplayHash)) : []).OrderBy(o => o.SkillId, StringComparer.Ordinal).ThenBy(o => o.BindingId, StringComparer.Ordinal).ToArray());
    internal IReadOnlyList<KemingBorrowOption> KemingSourceBorrowOptions(int source) => Array.AsReadOnly(KemingPrintedBorrowOptions(_contentRegistryOperations._contentRegistry.Generals[_coreMatchParticipantsPlayersAccess._players[source].General.Id]).Concat(_coreMatchParticipantsPlayersAccess._players[source].SecondaryGeneral is { } second ? KemingPrintedBorrowOptions(_contentRegistryOperations._contentRegistry.Generals[second.Id]) : []).DistinctBy(o => (o.SkillId, o.BindingId)).ToArray());
    internal IReadOnlyList<KemingBorrowOption> KemingBorrowOptions(int borrower, int source)
    {
        var printed = KemingSourceBorrowOptions(source);
        if (printed.Count > 0)
            return printed;
        return Array.AsReadOnly(_contentRegistryOperations._contentRegistry.Generals.Values.Where(g => g.CharacterId?.StartsWith("character:cao-", StringComparison.Ordinal) == true && g.RulesetId is null or "sanguosha-anniversary").SelectMany(KemingPrintedBorrowOptions).DistinctBy(o => (o.SkillId, o.BindingId)).OrderBy(o => o.GeneralId, StringComparer.Ordinal).ThenBy(o => o.SkillId, StringComparer.Ordinal).ThenBy(o => o.BindingId, StringComparer.Ordinal).ToArray());
    }

    internal IReadOnlyList<PromptChoice> KemingBorrowChoices(ProgramSkillFrame f)
    {
        if (f.ShenCaoPi is not { Stage: ShenCaoPiStage.BorrowChoice } r)
            return[];
        return Array.AsReadOnly(r.Options.Select((o, index) => (o, index)).Where(x => r.ForcedBorrow is null || x.o == r.ForcedBorrow).Select(x => new PromptChoice(new($"keming:{(f.Id)}:{(x.index)}"), "触发【" + _contentRegistryOperations._contentRegistry.GetSkill(x.o.SkillId).Name + "】", [], [], new Dictionary<string, string> { ["program-action"] = "keming-borrow", ["option"] = x.index.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    internal void PublishKemingBorrow(ProgramSkillFrame f)
    {
        var choices = KemingBorrowChoices(f);
        if (choices.Count == 0)
        {
            FinishShenCaoPi(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "刻名：触发本次受伤时机的技能。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("keming-borrow")]
    internal void AnswerKemingBorrow(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenCaoPi: { Stage: ShenCaoPiStage.BorrowChoice } r } f || !KemingBorrowChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Keming lost its exact original borrowed-skill choice.");
        var option = r.Options[int.Parse(selected.Parameters["option"], CultureInfo.InvariantCulture)];
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } context || target != f.OwnerSeat || source == target || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not DamageTriggerWindowFrame window || window.Id != r.ParentId || window.ParentFrameId != damageId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _contentRegistryOperations._contentRegistry.GetSkill(option.SkillId).Program is not { } program || program.GameplayHash != option.GameplayHash)
            throw new InvalidOperationException("Keming cannot move its borrowed trigger to another native damage segment.");
        var trigger = program.Triggers.Single(t => t.Id == option.BindingId);
        var count = trigger.DamageOccurrence == SkillProgramDamageOccurrence.PerDamagePoint ? context.Amount : 1;
        var instance = $"keming:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(option.SkillId)}";
        var additions = Enumerable.Range(0, count).Select(i => new DamageTriggerCandidate(f.OwnerSeat,
            $"keming:{(f.Id)}:{(option.SkillId)}:{(option.BindingId)}:{(i)}", trigger.Priority, trigger.Optional,
            option.SkillId, option.BindingId, instance, option.GameplayHash, i))
            .Where(candidate => !window.Candidates.Any(c => c.OwnerSeat == f.OwnerSeat && c.ProgramId == option.SkillId &&
                c.ProgramTriggerId == option.BindingId && c.OccurrenceIndex == candidate.OccurrenceIndex)).ToArray();
        var candidates = window.Candidates.Take(window.CandidateIndex + 1).Concat(additions).Concat(window.Candidates.Skip(window.CandidateIndex + 1)).ToArray();
        var authorizations = additions.Select(candidate => new ProgramInvocationAuthorization(window.Id,
            damageId, context.Window, candidate.ToProgramCandidate()));
        _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with
        {
            Candidates = Array.AsReadOnly(candidates),
            InvocationAuthorizations = (window.InvocationAuthorizations ?? []).Concat(authorizations).ToArray()
        });
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        FinishShenCaoPi(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveKemingDamagedSkill)]
    internal PromptChoice ChooseKemingBorrowAi(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
}

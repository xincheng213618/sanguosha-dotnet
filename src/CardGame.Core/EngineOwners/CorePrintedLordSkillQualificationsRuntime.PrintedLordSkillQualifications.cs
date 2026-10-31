#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePrintedLordSkillQualificationsRuntime
{
    internal bool IsPrintedLordGrantQualified(CharacterState owner, SkillGrant grant) => PrintedLordSkillQualifications.Matches(owner, grant) || _coreGuanduActivitiesOperations.IsGuanduCommanderPrintedLordGrant(owner, grant) || _coreScenarioPrintedLordQualificationsOperations.IsShanhePrintedLordGrantQualified(owner, grant);
    internal string? PreferredQualifiedPrintedLordInstance(CharacterState owner, string skillId)
    {
        if (owner.Role == Role.Lord || !owner.SkillGrants.Grants.Any(g => g.SkillId == skillId && IsPrintedLordGrantQualified(owner, g)))
            return null;
        if (!_contentRegistryOperations._contentRegistry.GetSkill(skillId).Tags.HasFlag(SkillTag.Lord))
            return null;
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(g => g.SkillId == skillId && IsPrintedLordGrantQualified(owner, g)).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal).ThenBy(g => g.GrantId, StringComparer.Ordinal).Select(g => g.SkillInstanceId).FirstOrDefault();
    }

    internal IReadOnlyList<PrintedLordSkillQualification> CapturePrintedLordQualifications(ProgramSkillFrame f, int seat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        var issuer = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        return Array.AsReadOnly(target.SkillGrants.Grants.Where(g => g.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource && g.LordProjection is null && g.GeneralLibraryProjection is null && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord)).Select(g => new PrintedLordSkillQualification(seat, (g.SourceId == CharacterState.PrimarySkillSource ? target.General : target.SecondaryGeneral)!.Id, g.SourceId, g.GrantId, g.SkillId, g.SkillInstanceId, f.Id, issuer, f.GameplayHash)).ToArray());
    }

    internal void IssueCapturedPrintedLordQualifications(ProgramSkillFrame f, PrintedLordBenefitReceipt r)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[r.BeneficiarySeat];
        foreach (var q in r.PrintedQualifications)
        {
            var grant = target.SkillGrants.Grants.SingleOrDefault(g => g.GrantId == q.GrantId && g.SkillId == q.SkillId && g.SourceId == q.TemplateSourceId && g.SkillInstanceId == q.SkillInstanceId);
            if (grant is null || !PrintedLordSkillQualifications.Matches(target, grant with { PrintedLordQualification = q }))
                continue;
            // Preserve an already qualified exact grant's original provenance.
            if (!PrintedLordSkillQualifications.Matches(target, grant))
                target.SkillGrants.SetPrintedLordQualification(grant.GrantId, q);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrintedLordQualificationIssuedEvent(f.Id, r.BeneficiarySeat, r.Source, r.GameplayHash));
    }
}

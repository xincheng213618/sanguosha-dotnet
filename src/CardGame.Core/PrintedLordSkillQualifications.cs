namespace CardGame.Core;

// A qualification belongs to one existing printed grant, never to a role or
// an acquired skill id. The issuing skill need not remain live afterwards.
public sealed record PrintedLordSkillQualification(int BeneficiarySeat, string GeneralId,
    string TemplateSourceId, string GrantId, string SkillId, string SkillInstanceId,
    long ProgramFrameId, CardConversionSource Issuer, string GameplayHash);

internal static class PrintedLordSkillQualifications
{
    internal static bool Matches(CharacterState owner, SkillGrant grant)
    {
        if (grant.PrintedLordQualification is not { } q || q.BeneficiarySeat != owner.Seat ||
            q.GrantId != grant.GrantId || q.SkillId != grant.SkillId || q.SkillInstanceId != grant.SkillInstanceId ||
            q.TemplateSourceId != grant.SourceId || grant.LordProjection is not null || grant.GeneralLibraryProjection is not null ||
            q.ProgramFrameId <= 0 || q.Issuer.OwnerSeat == owner.Seat ||
            string.IsNullOrWhiteSpace(q.Issuer.SkillId) || string.IsNullOrWhiteSpace(q.Issuer.BindingId) ||
            string.IsNullOrWhiteSpace(q.Issuer.SkillInstanceId) || string.IsNullOrWhiteSpace(q.GameplayHash)) return false;
        var general = grant.SourceId switch
        {
            CharacterState.PrimarySkillSource => owner.General,
            CharacterState.SecondarySkillSource => owner.SecondaryGeneral,
            _ => null
        };
        return general?.Id == q.GeneralId && general.Skills.Any(s => s.ContentId == grant.SkillId && s.Tags.HasFlag(SkillTag.Lord));
    }
}

// Hidden printed-general slots stay on the trusted owning receipt/grant. This
// public fact announces qualification without disclosing slot ids or skills.
public sealed record PrintedLordQualificationIssuedEvent(long ProgramFrameId, int BeneficiarySeat,
    CardConversionSource Issuer, string GameplayHash) : IGameEvent;

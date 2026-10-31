using System.Text.Json.Serialization;

namespace CardGame.Core;

// A qualification belongs to one existing printed grant, never to a role or
// an acquired skill id. The issuing skill need not remain live afterwards.
public sealed record PrintedLordSkillQualification(int BeneficiarySeat, string GeneralId,
    string TemplateSourceId, string GrantId, string SkillId, string SkillInstanceId,
    PrintedLordQualificationSource Source);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(PrintedLordQualificationSource.SetupDeclared), "setupDeclared")]
[JsonDerivedType(typeof(PrintedLordQualificationSource.ProgramIssued), "programIssued")]
public abstract record PrintedLordQualificationSource
{
    public sealed record SetupDeclared(string DeclarationId) : PrintedLordQualificationSource;
    public sealed record ProgramIssued(long ProgramFrameId, CardConversionSource Issuer,
        string GameplayHash) : PrintedLordQualificationSource;
}

internal static class PrintedLordSkillQualifications
{
    internal static bool Matches(CharacterState owner, SkillGrant grant)
    {
        if (grant.PrintedLordQualification is not { } q || q.BeneficiarySeat != owner.Seat ||
            q.GrantId != grant.GrantId || q.SkillId != grant.SkillId || q.SkillInstanceId != grant.SkillInstanceId ||
            q.TemplateSourceId != grant.SourceId || grant.LordProjection is not null || grant.GeneralLibraryProjection is not null)
            return false;
        if (!(q.Source switch
        {
            PrintedLordQualificationSource.SetupDeclared declared => !string.IsNullOrWhiteSpace(declared.DeclarationId),
            PrintedLordQualificationSource.ProgramIssued issued => issued.ProgramFrameId > 0 && issued.Issuer.OwnerSeat != owner.Seat &&
                !string.IsNullOrWhiteSpace(issued.Issuer.SkillId) && !string.IsNullOrWhiteSpace(issued.Issuer.BindingId) &&
                !string.IsNullOrWhiteSpace(issued.Issuer.SkillInstanceId) && !string.IsNullOrWhiteSpace(issued.GameplayHash),
            _ => false
        })) return false;
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

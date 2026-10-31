namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_sun_quan_ol_magic_MoSunQuanDeclarationQuota
{
    internal static bool IsMoQuanyuTrueSlashDeclaration(DeclaredCardPayment paid, CardDeclarationParentReturn parent) =>
        RuntimeRules_Core_CommonRules.IsSlashCard(paid.DeclaredKind) && parent.Purpose is
            (CardDeclarationPurpose.Use or CardDeclarationPurpose.BorrowedSword or CardDeclarationPurpose.ProvidedSlash or
             CardDeclarationPurpose.BorrowedSwordProvidedSlash or CardDeclarationPurpose.QinglongProvidedSlash);
}

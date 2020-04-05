using Synuit.Toolkit.AuditLogging.Constants;

namespace Synuit.Toolkit.AuditLogging.Configuration
{
    public class AuditHttpSubjectOptions
    {
        public string SubjectIdentifierClaim { get; set; } = ClaimsConsts.Sub;

        public string SubjectNameClaim { get; set; } = ClaimsConsts.Name;
    }
}
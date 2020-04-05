﻿using System;
using System.Threading.Tasks;
using Synuit.Toolkit.AuditLogging.EntityFramework.Entities;
using Synuit.Toolkit.AuditLogging.EntityFramework.Mapping;
using Synuit.Toolkit.AuditLogging.EntityFramework.Repositories;
using Synuit.Toolkit.AuditLogging.Events;
using Synuit.Toolkit.AuditLogging.Services;

namespace Synuit.Toolkit.AuditLogging.EntityFramework.Services
{
    public class DatabaseAuditEventLoggerSink<TAuditLog> : IAuditEventLoggerSink 
        where TAuditLog : AuditLog, new()
    {
        private readonly IAuditLoggingRepository<TAuditLog> _auditLoggingRepository;

        public DatabaseAuditEventLoggerSink(IAuditLoggingRepository<TAuditLog> auditLoggingRepository)
        {
            _auditLoggingRepository = auditLoggingRepository;
        }

        public virtual async Task PersistAsync(AuditEvent auditEvent)
        {
            if (auditEvent == null) throw new ArgumentNullException(nameof(auditEvent));

            var auditLog = auditEvent.MapToEntity<TAuditLog>();

            await _auditLoggingRepository.SaveAsync(auditLog);
        }
    }
}
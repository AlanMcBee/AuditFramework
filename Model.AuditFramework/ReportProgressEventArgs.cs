using System;

namespace CodeCharm.Model.AuditFramework
{
    public class ReportProgressEventArgs
        : EventArgs
    {
        public ReportProgressEventArgs(string message, float? percentComplete)
        {
            Message = message;
            PercentComplete = percentComplete;
        }

        protected float? PercentComplete
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }
    }
}